using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Animator anim;
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
    public void SetFloat(string name, float value)
    {
        anim.SetFloat(name, value);
    }

    public void SetBool(string name, bool value)
    {
        anim.SetBool(name, value);
    }
    public void SetTrigger(string name)
    {
        anim.SetTrigger(name);
    }
#endregion
}
