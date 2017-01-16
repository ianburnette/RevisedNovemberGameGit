using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisappearMesh : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] List<MeshRenderer> currentMeshes;
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void Start () {
    }
    void Update () {
	
    }
    void OnTriggerEnter(Collider col)
    {
        MeshRenderer mesh = col.transform.GetComponent<MeshRenderer>();
        currentMeshes.Add(mesh);
        mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        mesh = null;

    }
    void OnTriggerExit(Collider col)
    {
        MeshRenderer mesh = col.transform.GetComponent<MeshRenderer>();
        currentMeshes.Remove(col.transform.GetComponent<MeshRenderer>());
        //mesh.enabled = true;
        mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        mesh = null;
    }
    #endregion
    #region CustomFunctions

    #endregion
}
