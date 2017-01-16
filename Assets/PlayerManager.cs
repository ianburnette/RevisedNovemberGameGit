using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    #region PrivateVariables
    [SerializeField]
    Behaviour[] movementScripts;
    
#endregion

#region PublicProperties

#endregion

#region UnityFunctions
void Start()
{
        SetControls(0);
}
void Update()
{

}
#endregion

#region CustomFunctions
    public void SetControls(int which)
    {
        for (int i = 0; i < movementScripts.Length; i++) {
            if (i == which)
                movementScripts[i].enabled = true;
            else
                movementScripts[i].enabled = false;
        }
    }
#endregion
}
