using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class PlayerTalking : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] NPC currentNpcToTalkTo;
    [SerializeField] Behaviour[] toEnableWhileInDialogue, toDisableWhileInDialogue;
    [SerializeField] DialogueCamera dialogueCam;
    [SerializeField] PlayerNavAgent agent;
    bool inDialogue = false;
    #endregion
    #region PublicProperties
    public NPC CurrentNPCtoTalkTo { get { return currentNpcToTalkTo; } set { currentNpcToTalkTo = value; } }
    public bool InDialogue{get{return inDialogue;}set{inDialogue = value;}}
    #endregion
    #region UnityFunctions
    void Start () {
        toDisableWhileInDialogue[1] = Camera.main.GetComponent<ThirdPersonCamera>();
        toEnableWhileInDialogue[0] = Camera.main.GetComponent<DialogueCamera>();
        //toEnableWhileInDialogue[1] = Camera.main.GetComponent<DialogueCamera>();
    }

void Update () {
	if (Input.GetButtonDown("Interact") && currentNpcToTalkTo != null && !inDialogue)
        StartDialogue();
    }

    public void StartDialogue()
    {
        inDialogue = true;
        foreach (Behaviour behav in toDisableWhileInDialogue)
        {
            behav.enabled = !inDialogue;
          //  print("behavior " + behav.name + " set to " + !inDialogue);
        }
        foreach (Behaviour behav in toEnableWhileInDialogue)
            behav.enabled = inDialogue;
        agent.TargetTransform = currentNpcToTalkTo.DestinationNode;
        agent.LookTarget = currentNpcToTalkTo.transform;
        StartCoroutine("WaitUntilReady");
    }

    IEnumerator WaitUntilReady()
    {
        while (!agent.InRange)
        {
            yield return new WaitForEndOfFrame();
        }
        FindObjectOfType<DialogueRunner>().StartDialogue(currentNpcToTalkTo.talkToNode);
        yield return null;
    }

    public void ReEnableBehaviors()
    {
        inDialogue = false;
        foreach (Behaviour behav in toDisableWhileInDialogue)
            behav.enabled = !inDialogue;
        foreach (Behaviour behav in toEnableWhileInDialogue)
            behav.enabled = inDialogue;
    }
#endregion
#region CustomFunctions
    public void SetDialogueNode(DialogueNode node, bool state)
    {
        if (state)
            dialogueCam.CurrentNode = node;
        else
            dialogueCam.CurrentNode = null;
    }
#endregion
}
