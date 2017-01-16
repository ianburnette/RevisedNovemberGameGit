using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Transform cam;
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void Start () {
	    if (cam == null)
            cam = Camera.main.transform;
        
    }
    void Update () {
        transform.LookAt(cam);
    }
    #endregion
    #region CustomFunctions

    #endregion
}
