using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Vector3 relativePosition, rotation;

    public Vector3 RelativePosition{get{return relativePosition;}set{relativePosition = value;}}
    public Vector3 Rotation{get{return rotation;}set{rotation = value;}}
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void Start () {
	
    }
    void Update () {
	
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + relativePosition, .5f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position + relativePosition, )
            
    }
    #endregion
    #region CustomFunctions

    #endregion
}
