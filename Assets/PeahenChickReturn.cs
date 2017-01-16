using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;

public class PeahenChickReturn : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] PlayerTalking player;
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
        if (col.transform.tag == "Chick")
        {
            col.transform.tag = "Untagged";
            col.GetComponent<NPCFollowPlayer>().Player = transform;
            if (!player.InDialogue)
            {
                player.CurrentNPCtoTalkTo = transform.parent.GetComponent<NPC>();
                player.StartDialogue();
            }
        }
        
    }
    #endregion
    #region CustomFunctions

    #endregion
}
