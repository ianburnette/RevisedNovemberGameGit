using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IInteractible {
    #region PrivateVariables
    [SerializeField] bool inRange;
    [Tooltip("Assigned automatically to first child")]
    [SerializeField] GameObject UIPrompt;
    [Tooltip("0=can,1=seeds,2=axe,3=clippers")]
    [SerializeField] int objectType;
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void Start () {
        UIPrompt = transform.GetChild(0).gameObject;
    }
    void Update () {
        if (UIPrompt.activeSelf != inRange)
            UIPrompt.SetActive(inRange);
    }
    void OnTriggerEnter(Collider col)
    {
        inRange = true;
        col.GetComponent<PlayerInteract>().InteractibleInRange = this;
    }
    void OnTriggerExit(Collider col)
    {
        inRange = false;
        col.GetComponent<PlayerInteract>().InteractibleInRange = null;
    }
    public void Interact()
    {
        switch (objectType)
        {
            case 0:
                GlobalInventory.staticInventory.HaveCan = true;
                break;
            case 1:
                GlobalInventory.staticInventory.HaveSeeds = true;
                break;
            case 2:
                break;
            case 3:
                break;
        }
       
        gameObject.SetActive(false);
    }
    #endregion
    #region CustomFunctions

    #endregion
}
