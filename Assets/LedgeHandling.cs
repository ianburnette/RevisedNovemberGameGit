using UnityEngine;
using System.Collections;
using DG.Tweening;


public class LedgeHandling : MonoBehaviour {

    #region PrivateVariables
    [SerializeField] Transform currentLedge;
    [SerializeField] float ledgeCheckHeight, handHeight, handDistance, bodyHeight;
    [SerializeField] float ledgeCheckDistance, hangOffset, resetTime, updateHeightTime, ledgeHeight, climbTime, climbUpHeight, climbForwardLength;
    [SerializeField] LayerMask ledgeMask;
    [SerializeField] Vector3 ledgeHitPosition, verticalPlaneHit, verticalPlaneNormal;
    [SerializeField] bool onLedge, resetting;
    [SerializeField] Vector3[] forwardRays;

    [SerializeField] PlayerAnimation anim;

    [SerializeField] public UnityEngine.UI.Text debugText;

    [SerializeField] Behaviour[] toDisable;

    #endregion

    #region PublicProperties
        public Transform CurrentLedge{get {return currentLedge;}set{currentLedge = value;}}
        public float HangOffset { get { return hangOffset; } set { hangOffset = value; } }
        public Vector3 LedgeHitPosition{get{return ledgeHitPosition;}set{ledgeHitPosition = value;}}
        public bool OnLedge{get{return onLedge;}set{onLedge = value;}}
        public Vector3 VerticalPlaneHit{get{return verticalPlaneHit;}set{verticalPlaneHit = value;}}
        public Vector3 VerticalPlaneNormal { get { return verticalPlaneNormal; } set { verticalPlaneNormal = value; } }
        public float LedgeHeight { get { return ledgeHeight; } set { ledgeHeight = value; } }
    #endregion

    #region UnityFunctions
    void Start()
    {
        forwardRays = new Vector3[9];
        anim = GetComponent<PlayerAnimation>();
    }
    void Update()
    {
        if (!resetting)
        {
            if (CastLedgeCheckRay())
            {
                CastLedgeGrabRay();
                if (onLedge)
                    CastDirectionRay();
            }
        }
        if (onLedge)
        {
         
            GetLedgeInput();
        }
        
       
    }
    #endregion

    #region CustomFunctions
    #region Logic
    void GetLedgeInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            ClimbUp();
        }else if (Input.GetButtonDown("Cancel"))
        {
            Drop();
        }
    }

    void ClimbUp()
    {
        print("climbing up");
        anim.SetTrigger("ClimbFromLedge");
        transform.DOMove((transform.forward * climbForwardLength) + (transform.up * climbUpHeight), climbTime).SetRelative();
    }
    void Drop()
    {
        onLedge = false;
        anim.SetTrigger("DropFromLedge");
        ToggleBehaviors(!onLedge);
        resetting = true;
        Invoke("Reset", resetTime);
    }
    void Reset()
    {
        resetting = false;
    }
    #endregion
    #region Raycasts
    bool CastLedgeCheckRay()
    {
        RaycastHit hit;
        Ray ledgeRay = new Ray((transform.position + transform.forward) + (Vector3.up * ledgeCheckHeight), Vector3.down * ledgeCheckDistance);
        if (Physics.Raycast(ledgeRay.origin, ledgeRay.direction, out hit, ledgeCheckDistance, ledgeMask))
        {
            Debug.DrawRay(ledgeRay.origin, ledgeRay.direction, Color.yellow);
            if (onLedge)
            {
                currentLedge = hit.transform;
                ledgeHitPosition = hit.point;
                InvokeRepeating("UpdateHeight", 0, updateHeightTime);
            }
            return true;
        }
        else
        {
            CancelInvoke();
            currentLedge = null;
            onLedge = false;
           // ToggleBehaviors(!onLedge);
            Debug.DrawRay(ledgeRay.origin, ledgeRay.direction, Color.red);
            return false;
        }
    }
    void UpdateHeight()
    {
        ledgeHeight = ledgeHitPosition.y;
    }
    void CastLedgeGrabRay()
    {
        RaycastHit hit;
        Ray handRay = new Ray(transform.position + Vector3.up * handHeight, transform.forward * handDistance);
        if (Physics.Raycast(handRay.origin, handRay.direction, out hit, handDistance, ledgeMask))
        {
            onLedge = false;
            ToggleBehaviors(!onLedge);
            Debug.DrawRay(transform.position + Vector3.up * handHeight, transform.forward * handDistance, Color.yellow);
        }
        else
        {
            if (!onLedge && !resetting)
            {
                anim.SetTrigger("GrabLedge");
                print("calling grabledge");
            }
            onLedge = true;
            ToggleBehaviors(!onLedge);
            Debug.DrawRay(transform.position + Vector3.up * handHeight, transform.forward * handDistance, Color.green);
        }
    }
    void ToggleBehaviors(bool state)
    {
        foreach (Behaviour behav in toDisable)
        {
            behav.enabled = state;
        }
    }
    void CastDirectionRay()
    {
        RaycastHit hit;
        Vector3 closestDirection = transform.forward;
        Vector3 rayOrigin = transform.position + Vector3.up * bodyHeight;
      /*  foreach (Vector3 dir in forwardRays)
        {
            Debug.DrawRay(rayOrigin, dir, Color.white);
            if(Physics.Raycast(transform.position + Vector3.up * bodyHeight, dir, out hit, handDistance, ledgeMask))
            {
                if (Vector3.Distance(rayOrigin, hit.point) < Vector3.Distance(rayOrigin, closestDirection))
                    closestDirection = dir;
            }

        }*/
        Ray bodyRay = new Ray(rayOrigin, closestDirection * handDistance);
        if (Physics.Raycast(bodyRay.origin, bodyRay.direction, out hit, handDistance, ledgeMask))
        {
            // onLedge = false;
            verticalPlaneHit = hit.point;
            verticalPlaneNormal = hit.normal;
            Debug.DrawRay(transform.position + Vector3.up * bodyHeight, transform.forward * handDistance, Color.cyan);
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up * bodyHeight, transform.forward * handDistance, Color.blue);
        }
    }
 
    #endregion
    #region Movement
    public bool LedgeMovementFilter(ref Vector3 movementVector)
    {
        if (onLedge && currentLedge != null)
        {
           // Vector3 convertedNormal = new Vector3(verticalPlaneNormal.z, 0, verticalPlaneNormal.x);
            movementVector = Vector3.ProjectOnPlane(movementVector, verticalPlaneNormal);
            movementVector += transform.forward / 2f;
            //movementVector += //Vector3.Reflect(movementVector, verticalPlaneNormal);
           // print("planeNormal is " + verticalPlaneNormal + " and returning " + movementVector);
            return true;
        }
        else
            return false;
    }
    #endregion

    #endregion
}
