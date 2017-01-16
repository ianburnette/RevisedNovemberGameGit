using UnityEngine;
using System.Collections;

public class PlayerMoveCat : PlayerMove
{
    #region PrivateVariables
    [SerializeField]
    bool freeRunUp, freeRunDown;
    [SerializeField]
    float jumpHeightLow, jumpHeightMed, jumpHeightHigh;
    [SerializeField]
    float avoidWallDistance, avoidLedgeDistance;



    #endregion

    #region Public Properties

    #endregion

    #region UnityFunctions

    void FixedUpdate()
    {
        //(moveDirection, accelCurve.Evaluate(direction.magnitude), stopDistance, true);
        PerformAnimation();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, (moveDirection - transform.position));

    }

    #endregion

    #region CustomFunctions
    void PerformAnimation()
    {
        animator.SetFloat("Speed",direction.magnitude);
 
    }
    #endregion

}