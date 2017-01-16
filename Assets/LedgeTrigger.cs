using UnityEngine;
using System.Collections;

public class LedgeTrigger : MonoBehaviour {

    #region PrivateVariables
    [SerializeField]
    LayerMask playerMask;
#endregion

#region PublicProperties

#endregion

#region UnityFunctions
void Start()
{

}
void FixedUpdate()
{

}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player") 
            col.GetComponent<LedgeHandling>().CurrentLedge = transform;
    }
#endregion

#region CustomFunctions

#endregion
}
