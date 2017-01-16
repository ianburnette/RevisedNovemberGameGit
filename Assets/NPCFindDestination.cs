using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFindDestination : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] float maxNavHitDistance;
    [SerializeField] int wanderMask;
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void Start () {
	
}
void Update () {
	
}
    #endregion
    #region CustomFunctions
    public Vector3 GetNearestNavPos(Vector3 inputVector)
    {
        Vector3 found = Vector3.zero;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(inputVector, out hit, maxNavHitDistance, wanderMask))
        {
        //    print("found something");
            found = hit.position;
        }
        else
        {
         //   print("no position found");
        }
        return found;
    }
    #endregion
}
