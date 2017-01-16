using UnityEngine;
using System.Collections;

public class AvoidFalls : MonoBehaviour {

#region PrivateVariables
    [SerializeField]
    private LayerMask wallProtectionLayer;
    [SerializeField]
    private float correctionAmt, checkDistance;
    [SerializeField]
    private Rigidbody rb;
#endregion

#region PublicProperties

#endregion

#region UnityFunctions
void Start()
{

}
void FixedUpdate()
{
        Debug.DrawRay(transform.position, rb.velocity, Color.red);
        RaycastHit protectionHit;
        //print("rigidbody velocity is " + rb.velocity + " and rb.vel * checkDist is " + rb.velocity * checkDistance);
        if (Physics.Raycast(transform.position, rb.velocity, out protectionHit, wallProtectionLayer)){
            
            Vector3 reflection = Vector3.Reflect(rb.velocity, protectionHit.normal);
            Debug.DrawRay(protectionHit.point, reflection, Color.yellow);
            if (Vector3.Distance(transform.position, protectionHit.point) > checkDistance)
            {
                Debug.DrawRay(transform.position, rb.velocity + reflection, Color.magenta);
                rb.AddForce(reflection * correctionAmt);
            }
        }
}
#endregion

#region CustomFunctions

#endregion
}
