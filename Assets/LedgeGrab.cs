using UnityEngine;
using System.Collections;

public class LedgeGrab : MonoBehaviour {

    #region PrivateVariables
    [SerializeField]
    float forwardCheckDist, ledgePastWallMargin, maxLedgeHeight, checkDownDist;
    [SerializeField]
    LayerMask wallMask;

    bool nearWall, nearLedge;
    RaycastHit wallHit, ledgeHit;
    #endregion

    #region PublicProperties

    #endregion

    #region UnityFunctions
    void Start()
{

}
void Update()
{
    
}

    void OnDrawGizmos()
    {
        if (nearWall)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(wallHit.point, .2f);
            if (nearLedge)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(new Vector3(wallHit.point.x, ledgeHit.point.y, wallHit.point.z), .3f);
            }
        }
    }
#endregion

#region CustomFunctions
    void ForwardCheck()
    {

        if (Physics.Raycast(transform.position, transform.forward * forwardCheckDist, out wallHit, wallMask))
        {
            nearWall = true;
            LedgeCheck();
        }
        else
        {
            nearWall = nearLedge = false;
            //nearLedge = false;
        }
    }
    void LedgeCheck()
    {
        Ray ledgeRay = new Ray(transform.position + (Vector3.up * maxLedgeHeight) + (transform.forward * forwardCheckDist * ledgePastWallMargin), -transform.up * checkDownDist);
        Debug.DrawRay(ledgeRay.origin, ledgeRay.direction, Color.green);
        if (Physics.Raycast(ledgeRay, out ledgeHit, wallMask))
        {
            nearLedge = true;
        }
        else
        {
            nearLedge = false;
        }
    }
    void DrawRays()
    {
        /*
        foreach(float height in heightChecks)
        {
            foreach(Vector3 dir in checkDirections)
            {
                Vector3 pos = transform.position + Vector3.up * (height * heightMultiplier);
               // Debug.DrawRay(pos, transform.forward + (dir * checkMagnitude), Color.red);
            }
        }*/
    }
#endregion
}
