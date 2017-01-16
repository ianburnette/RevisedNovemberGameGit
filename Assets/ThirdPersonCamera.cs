using UnityEngine;
using System.Collections;

struct CameraPosition
{
    private Vector3 position;
    private Transform xForm;

    public Vector3 Position { get { return position; } set { position = value; } }
    public Transform XForm { get { return xForm; } set { xForm = value; } }

    public void Init(string camName, Vector3 pos, Transform transform)
    {
        position = pos;
        xForm = transform;
        xForm.name = camName;
     // xForm.parent = parent;
        xForm.localPosition = Vector3.zero;
        xForm.localPosition = position;
    }
}

public class ThirdPersonCamera : MonoBehaviour {

    #region Variables (private)
    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float distanceUpMultiplier = 5f;
    [SerializeField]
    private float distanceAwayModifier = 1.5f;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform followXform;
    [SerializeField]
    private float wallBufferAmt;
    [SerializeField]
    private float targetingTime;
    [SerializeField]
    private PlayerMove follow;
    [SerializeField]
    private float firstPersonLookSpeed = 1.5f;
    [SerializeField]
    private Vector2 firstPersonXAxisClamp = new Vector2(-70f, 90f);
    [SerializeField]
    private Vector3 firstPersonPosition = new Vector3(0, .6f, .6f);
    [SerializeField]
    private GameObject nodeSystem;
    [SerializeField]
    Transform FPPOV;
    [SerializeField]
    private float fpsRotationDegreePerSecond;
    [SerializeField]
    private float fpsDampTime;
    [SerializeField]
    private Vector2 camMinDistFromChar = new Vector2(1f, -0.5f);
    [SerializeField]
    private Vector2 camMaxDistFromChar = new Vector2(7f, 4f);
    [SerializeField]
    private const float freeRotationDegreesPerSond = -5f;
    [SerializeField]
    private Transform parentRig;
    [SerializeField]
    private float rightStickThreshold;
    [SerializeField]
    private LayerMask mask;

   
    [SerializeField]
    private float freeZoomSpeed;

    //private global only
        private Vector3 lookDir;
    [SerializeField] private Vector3 targetPosition;
    private Vector3 targetOffset;
    private int selectedCamera = 1;
    private CamStates camState = CamStates.Behind;
    private CamStates[] camSelectionStates;
    private float xAxisRot = 0.0f;
    private float lookWeight;
    private CameraNodeFollow nodeFollower;
    private DialogueCamera dialogueCam;
    private Vector3 curLookDir;
    private Vector3 savedRigToGoal;
    [SerializeField]
    [Range(0f,1f)]
    private float zoomDistance;
    private Vector2 rightStickPrevFrame = Vector2.zero;
    [SerializeField]
    private float distanceAwayFree;
    [SerializeField]
    private float distanceUpFree;

    //smoothing variables
    
    private Vector3 velocityCamSmooth = Vector3.zero;
    [Header("Smoothing Variables")]
    [SerializeField]
    private float camSmoothDampTime = 0.1f;
    [SerializeField] float camClipDamptTime = 25f;
    private Vector3 velocityLookDir = Vector3.zero;
    [SerializeField]
    private float lookDirDampTime = 0.1f;

    [SerializeField] Vector3 calculatedTargetPosition;


    private CameraPosition firstPersonCamPos;



    #endregion

    #region Properties (public)
    public enum CamStates
    {
        Behind,
        FirstPerson,
        Target,
        Free,
        Nodes
    }
    public CamStates CamState
    {
        get
        {
            return camState;
        }
    }
    #endregion

    public CamStates WhatState()
    {
        return camState;
    }

    // Use this for initialization
    void Start () {
        parentRig = this.transform.parent;
        nodeFollower = GetComponent<CameraNodeFollow>();
        curLookDir = Vector3.forward;
        ToggleNodes(false);
        InitializeCamArray();
        if (!followXform)
            followXform = GameObject.FindGameObjectWithTag("CameraFollow").transform;
        firstPersonCamPos = new CameraPosition();
        //= GameObject.FindGameObjectWithTag("FirstPersonPOV").transform;
        /*
        firstPersonCamPos.Init
            (
                "First Person Camera",
                firstPersonPosition,
                FPPOV.transform
               // followXform.parent
            );*/
    }
	
	// Update is called once per frame
	void LateUpdate () {
        float rightX = Input.GetAxis("CamHorizontal");
        float rightY = Input.GetAxis("CamVertical");

        float mouseX = Input.GetAxis("CamHorizontalMouse");
        float mouseY = Input.GetAxis("CamVerticalMouse");

        float leftX = Input.GetAxis("Horizontal");
        float leftY = Input.GetAxis("Vertical");
        GetCamSelectionInput();

        Vector3 characterOffset = followXform.position + new Vector3(0, distanceUp, 0);
        Vector3 lookAt = characterOffset;
        targetPosition = Vector3.zero;

        if (Input.GetAxis("Target") > 0.01f)
            camState = CamStates.Target;
        else
        {
            CamStates lastCamState = camState;
            camState = camSelectionStates[selectedCamera];
            if (lastCamState != camState && camState == CamStates.FirstPerson)
            {
                xAxisRot = 0f;
                lookWeight = 0f;
            }

        }

        switch (camState) {
            case CamStates.Behind:
                if (follow.SpeedMagnitude > follow.LocomotionThreshold)
                {
                    lookDir = Vector3.Lerp(followXform.right * (leftX < 0 ? 1f : -1f), followXform.forward * (leftY < 0 ? -1f : 1f), Mathf.Abs(Vector3.Dot(this.transform.forward, followXform.forward)));
                    curLookDir = Vector3.Normalize(characterOffset - parentRig.transform.position);
                    curLookDir.y = 0;
                    curLookDir = Vector3.SmoothDamp(curLookDir, lookDir, ref velocityLookDir, lookDirDampTime);
                }
                targetPosition = characterOffset + followXform.up * distanceUp - Vector3.Normalize(curLookDir) * distanceAway;
             //   print("in behind case");
                /*
                lookDir = characterOffset - this.transform.position;
                lookDir.y = 0;
                lookDir.Normalize();
                targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
                */
                break;
            case CamStates.Target:
                lookDir = followXform.forward;
                targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
                break;
            case CamStates.FirstPerson:
                //align to face position/rotation
                targetPosition = FPPOV.position;

                transform.rotation = Quaternion.Lerp(transform.rotation, FPPOV.rotation, fpsDampTime * Time.deltaTime); ;
                //Rotate up and down
                xAxisRot = xAxisRot + (leftY * firstPersonLookSpeed);
                xAxisRot = Mathf.Clamp(xAxisRot, firstPersonXAxisClamp.x, firstPersonXAxisClamp.y);
                FPPOV.localRotation = Quaternion.Euler(xAxisRot, 0, 0);
                //Rotate left and right
                Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, fpsRotationDegreePerSecond * (leftX < 0f ? -1f : 1f), 0f), Mathf.Abs(leftX));
                Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
                follow.transform.rotation = follow.transform.rotation * deltaRotation;

                /* 
                xAxisRot = xAxisRot + (leftY * firstPersonLookSpeed);
                xAxisRot = Mathf.Clamp(xAxisRot, firstPersonXAxisClamp.x, firstPersonXAxisClamp.y);
                firstPersonCamPos.XForm.localRotation = Quaternion.Euler(xAxisRot, 0, 0);
               
                lookDir = firstPersonCamPos.XForm.forward;
                transform.LookAt(firstPersonCamPos.XForm.forward);
                //Quaternion rotationShift = Quaternion.FromToRotation(this.transform.forward, firstPersonCamPos.XForm.forward);
                //transform.rotation = rotationShift;
                //targetPosition = firstPersonCamPos.XForm.position;
                //lookAt = Vector3.Lerp(this.transform.position + this.transform.forward, lookAt, Vector3.Distance(this.transform.position, firstPersonCamPos.XForm.position));
                */
                break;
            case CamStates.Free:
                
                lookWeight = Mathf.Lerp(lookWeight, 0.0f, Time.deltaTime * firstPersonLookSpeed);
                Vector3 rigToGoalDirection = Vector3.Normalize(characterOffset - parentRig.transform.position);

                rigToGoalDirection.y = 0f;

                Vector3 rigToGoal = characterOffset - parentRig.position;
                rigToGoal.y = 0f;
                Debug.DrawRay(parentRig.transform.position, rigToGoal, Color.red);

                if (Mathf.Abs(rightY) > rightStickThreshold)
                {
                    zoomDistance += -rightY * freeZoomSpeed * Time.deltaTime;
                    zoomDistance = Mathf.Clamp(zoomDistance, 0f, 1f);
                }else if (Mathf.Abs(mouseY) > rightStickThreshold)
                {
                    zoomDistance += -mouseY * freeZoomSpeed * Time.deltaTime;
                    zoomDistance = Mathf.Clamp(zoomDistance, 0f, 1f);
                }

                distanceAwayFree = Mathf.Lerp(camMinDistFromChar.x, camMaxDistFromChar.x, zoomDistance);
                distanceUpFree = Mathf.Lerp(camMinDistFromChar.y, camMaxDistFromChar.y, zoomDistance);
                targetPosition = characterOffset + followXform.up * distanceUpFree - rigToGoalDirection * distanceAwayFree;

                Debug.DrawRay(targetPosition, Vector3.up, Color.green);

                if (rightX != 0 || rightY != 0)
                {
                    savedRigToGoal = rigToGoalDirection;
                    parentRig.RotateAround(characterOffset, followXform.up,
                        freeRotationDegreesPerSond * (Mathf.Abs(rightX) > rightStickThreshold ? rightX : 0f));

                }else if (mouseX != 0 || mouseY != 0)
                {
                    savedRigToGoal = rigToGoalDirection;
                    parentRig.RotateAround(characterOffset, followXform.up,
                        freeRotationDegreesPerSond * (Mathf.Abs(mouseX) > rightStickThreshold ? mouseX : 0f));
                }

               if (targetPosition == Vector3.zero) 
                    targetPosition = characterOffset + followXform.up * distanceUpFree - savedRigToGoal * distanceAwayFree;


                SmoothPosition(parentRig.transform.position, targetPosition);
                //transform.LookAt(lookAt);
                
                break;
            case CamStates.Nodes:
              
                break;
        }

        //targetPosition = characterOffset + followXform.up * distanceUp - lookDir * distanceAway;
        if (camState != CamStates.Nodes )
        {
            if (CompensateForWalls(characterOffset, ref targetPosition))
                parentRig.transform.position = Vector3.Lerp(parentRig.transform.position, targetPosition, camClipDamptTime * Time.deltaTime);
            else
                parentRig.transform.position = Vector3.Lerp(parentRig.transform.position, targetPosition, camSmoothDampTime * Time.deltaTime);
            //parentRig.transform.position = targetPosition;
            //SmoothPosition(parentRig.transform.position, targetPosition);
            //parentRig.transform.position = Vector3.Lerp(parentRig.transform.position, targetPosition, camSmoothDampTime * Time.deltaTime);
            if (camState != CamStates.FirstPerson )
                transform.LookAt(followXform);
        }
        rightStickPrevFrame = new Vector2(rightX, rightY);
        
	}

    void OnDrawGizmos()
    {
       // Gizmos.color = Color.yellow;
      //  Gizmos.DrawSphere(calculatedTargetPosition, 1f);
        
    }

    void GetCamSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedCamera = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedCamera = 2;
        }
        if (Input.GetAxisRaw("CamSelectionY") > 0)
        {
            if (selectedCamera == 3)
                ToggleNodes(false);
            selectedCamera = 0;
        }
        else if (Input.GetAxisRaw("CamSelectionY") < 0)
        {
            if (selectedCamera == 3)
                ToggleNodes(false);
            selectedCamera = 1;
        }
        else if (Input.GetAxisRaw("CamSelectionX") < 0 && System.Math.Round(follow.SpeedMagnitude, 2) == 0)
        {
            if (selectedCamera == 3)
                ToggleNodes(false);
            selectedCamera = 2;
            savedRigToGoal = Vector3.zero;
        }
        else if (Input.GetAxisRaw("CamSelectionX") > 0)
        {
            selectedCamera = 3;
            ToggleNodes(true);
        }
    }

    private void ToggleNodes(bool state)
    {
        if (nodeFollower.enabled != state)
        {
            nodeFollower.enabled = state;
            nodeSystem.SetActive(state);
        }
    }

    private void InitializeCamArray()
    {
        camSelectionStates = new CamStates[4];
        camSelectionStates[1] = CamStates.Behind;
        camSelectionStates[0] = CamStates.FirstPerson;
        camSelectionStates[2] = CamStates.Free;
        camSelectionStates[3] = CamStates.Nodes;
    }

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        parentRig.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private bool CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        Debug.DrawLine(fromObject, toTarget, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallHit, mask))
        {
            Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            Debug.DrawRay(wallHit.point, (fromObject - wallHit.point) * wallBufferAmt, Color.yellow);
            calculatedTargetPosition = wallHit.point + ((fromObject - wallHit.point) * wallBufferAmt);
            //calculatedTargetPosition.y = toTarget.y;
            //toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
            toTarget = calculatedTargetPosition;
            return true;
        }
        else
            return false;
    }


}
