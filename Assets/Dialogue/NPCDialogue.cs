using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class NPCDialogue : MonoBehaviour {
    #region PrivateVariables
    Collider dialogueCol;
    [SerializeField] GameObject bubble;
    [SerializeField] DialogueNode node;
    bool promptShowing;
#endregion
#region PublicProperties

#endregion
#region UnityFunctions
    void Start () {
        bubble = MetaScript.GetGameMeta().BubbleGO;
        dialogueCol = GetComponent<Collider>();
        promptShowing = false;
        bubble.SetActive(false);
    }
    void Update () {
	
    }

    [YarnCommand("SetStartNode")]
    public void SetStartNode(string nodeName)
    {
        GetComponent<NPC>().talkToNode = nodeName;
    }

    void OnTriggerEnter(Collider col)
    {
        col.GetComponent<PlayerTalking>().SetDialogueNode(node, true);
        string myName = "";
        bool nameStarted = false;
        foreach (char n in transform.name)
        {
            if (n == '_' && !nameStarted)
                nameStarted = true;
            else if (nameStarted)
                myName += n;
        }
        ExampleDialogueUI.staticDialogueUI.PositionBubble(myName);
        ExampleDialogueUI.staticDialogueUI.SetEllipses();
        promptShowing = true;
        bubble.SetActive(true);
        col.GetComponent<PlayerTalking>().CurrentNPCtoTalkTo = this.transform.GetComponent<NPC>();
    }

    void OnTriggerExit(Collider col)
    {
        col.GetComponent<PlayerTalking>().SetDialogueNode(node, false);
        promptShowing = false;
        bubble.SetActive(false);
        col.GetComponent<PlayerTalking>().CurrentNPCtoTalkTo = null;
        ExampleDialogueUI.staticDialogueUI.SetEllipses();
    }
#endregion
#region CustomFunctions

#endregion
}
