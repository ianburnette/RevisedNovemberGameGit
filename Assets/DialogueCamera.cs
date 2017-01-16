using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Transform parentRig;
    [SerializeField] DialogueNode currentNode;
    [SerializeField] float moveSpeed, rotationSpeed;

    public DialogueNode CurrentNode{get{return currentNode;}set{currentNode = value;}}
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void OnEnable () {
	    
    }
    void Update () {
        parentRig.transform.position = Vector3.Lerp(parentRig.transform.position, currentNode.transform.position + currentNode.RelativePosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(currentNode.Rotation), rotationSpeed * Time.deltaTime);
    }
    #endregion
    #region CustomFunctions

    #endregion
}
