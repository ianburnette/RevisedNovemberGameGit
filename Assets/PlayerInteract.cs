using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] IInteractible interactibleInRange;
    #endregion
    #region PublicProperties
    public IInteractible InteractibleInRange { get { return interactibleInRange; } set { interactibleInRange = value; } }
    #endregion
    #region UnityFunctions
    void Start () {
	
}
void Update () {
	if (Input.GetButtonDown("Interact") && interactibleInRange != null)
        {
            interactibleInRange.Interact();
            interactibleInRange = null;
        }
}
#endregion
#region CustomFunctions

#endregion
}
