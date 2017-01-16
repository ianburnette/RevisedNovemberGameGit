using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;

public class MetaScript : MonoBehaviour {
    private static MetaScript _instance;
    [SerializeField] GameObject bubbleGO, currentItemGO;
    [SerializeField] ExampleDialogueUI dialogueUI;

    public GameObject BubbleGO{get{return bubbleGO;}set{bubbleGO = value;}}
    public GameObject CurrentItemGO { get { return currentItemGO; } set { currentItemGO = value; } }
    public ExampleDialogueUI DialogueUI { get { return dialogueUI; } set { dialogueUI = value; } }

    void Awake()
    {
        _instance = this;
    }

    void Start () {
        GameObject.DontDestroyOnLoad(gameObject);	
	}

    public static MetaScript GetGameMeta()
    {
        return _instance;
    }
}
