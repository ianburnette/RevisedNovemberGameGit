using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Yarn.Unity.Example;

[System.Serializable]
public class item
{
    public bool have;
    public string name;
}

public class PlayerInventory : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Text itemText;
    [SerializeField] item[] tools;
    [SerializeField] item[] items;
    [SerializeField] int currentItem;
    int maxItems = 4;

    //animation
    [SerializeField] FullBodyBipedIK rigScript;
    [SerializeField] Vector3[] posWrotWbendG;
    [SerializeField] Transform[] mainTargets, bendTargets, modelTransforms;
    [SerializeField] bool[] leftHanded;
    [SerializeField] Transform[] twoHanded;
    [SerializeField] RootMotion.Demos.TwoHandedProp twoHandedScript;

    //Seeds
    [SerializeField] Vector2 seedCountRange;
    [SerializeField] ParticleSystem seedBagParticleSystem;
    [SerializeField] GameObject seedTrigger;
    [SerializeField] float seedThrowForce;

    //Bracelet
    [SerializeField] bool haveBracelet;
    [SerializeField] LedgeHandling ledgeScript;

    //keys
    [SerializeField] bool haveKeys;

    //watering can
    [SerializeField] WateringCan wateringCanScript;
    [SerializeField] ParticleSystem wateringCanParticleSystem;
    [SerializeField] float wateringCanEmissionRate = 500f;
    bool currentlyWatering = false;
    
#endregion
    #region PublicProperties
        public bool HaveBracelet { get { return haveBracelet; } set { GetBracelet(); }}
        public bool HaveKeys { get { return haveKeys; } set { GetKeys(); } }
    #endregion
    #region UnityFunctions
    void Start () {
        itemText = MetaScript.GetGameMeta().CurrentItemGO.GetComponent<Text>();
        GlobalInventory.staticInventory.PlayerInven = this;
    }
    void Update () {
        GetInteractInput();
        GetItemChangeInput();
        ShowUI();
        EnactConstantEffects();
    }
    #endregion
    #region CustomFunctions
    #region GettingItmes
    public void GetTool(int itemIndex)
    {
        tools[itemIndex].have = true;
        ChangeItem(itemIndex);
    }
    public void GetKeys()
    {
        haveKeys = true;
        items[0].have = true;
    }
    void GetBracelet()
    {
        haveBracelet = true;
        ledgeScript.enabled = true;
    }
    #endregion
    #region ChangingItems
    void GetItemChangeInput()
    {
        int newItem = currentItem;
        if (Input.GetButtonDown("ItemToggle"))
        {
            print("button pressed");
            if (newItem < maxItems)
                if (HaveItem(newItem + 1))
                    newItem++;
                else if (HaveItem(newItem + 2))
                    newItem += 2;
                else
                    newItem = 0;
            else if (newItem == maxItems)
                newItem = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            newItem = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            newItem = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            newItem = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            newItem = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            newItem = 4;

        if (HaveItem(newItem))
            ChangeItem(newItem);
    }
    bool HaveItem (int itemToCheck)
    {
        if (tools[itemToCheck].have == true)
            return true;
        else
            return false;
    }
    void ChangeItem (int itemToChangeTo)
    {
        currentItem = itemToChangeTo;
        SwitchToItem(itemToChangeTo);
    }
    void ShowUI()
    {
        itemText.text = "Current Item: " + tools[currentItem].name;
    }
    void SwitchToItem(int newItem)
    {
        Vector3 newWeightValues = posWrotWbendG[newItem];
        if (leftHanded[newItem])
        {
            rigScript.solver.rightHandEffector.target = null;
            rigScript.solver.rightHandEffector.positionWeight = 0f;
            rigScript.solver.rightHandEffector.rotationWeight = 0f;
            rigScript.solver.rightArmChain.bendConstraint.bendGoal = null;
            rigScript.solver.rightArmChain.bendConstraint.weight = 0f;

            rigScript.solver.leftHandEffector.target = mainTargets[newItem];
            rigScript.solver.leftArmChain.bendConstraint.bendGoal = bendTargets[newItem];
            rigScript.solver.leftHandEffector.positionWeight = newWeightValues.x;
            rigScript.solver.leftHandEffector.rotationWeight = newWeightValues.y;
            rigScript.solver.leftArmChain.bendConstraint.weight = newWeightValues.z;
        }else{
            rigScript.solver.leftHandEffector.target = null;
            rigScript.solver.leftHandEffector.positionWeight = 0f;
            rigScript.solver.leftHandEffector.rotationWeight = 0f;
            rigScript.solver.leftArmChain.bendConstraint.bendGoal = null;
            rigScript.solver.leftArmChain.bendConstraint.weight = 0f;

            rigScript.solver.rightHandEffector.target = mainTargets[newItem];
            rigScript.solver.rightArmChain.bendConstraint.bendGoal = bendTargets[newItem];
            rigScript.solver.rightHandEffector.positionWeight = newWeightValues.x;
            rigScript.solver.rightHandEffector.rotationWeight = newWeightValues.y;
            rigScript.solver.rightArmChain.bendConstraint.weight = newWeightValues.z;
        }
        if (twoHanded[newItem] != null)
        {
            twoHandedScript.enabled = true;
            twoHandedScript.leftHandTarget = twoHanded[newItem];
        }
        else
        {
          //  twoHandedScript.enabled = false;
          //  twoHandedScript.leftHandTarget = null;
        }
        foreach (Transform tran in modelTransforms)
            if (tran != null)
                tran.gameObject.SetActive(false);
        if (modelTransforms[newItem] != null)
            modelTransforms[newItem].gameObject.SetActive(true);
    }
    #endregion
    #region UsingItems
    void GetInteractInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            UseItem(currentItem);
        }if (Input.GetButtonUp("Interact"))
        {
            UseItemUp(currentItem);
        }
    }
    void UseItem(int itemToUse)
    {
        switch (itemToUse)
        {
            case 0:
                print("use nothing");
                break;
            case 1:
                print("use watering can");
                ToggleWateringCan(true);
                break;
            case 2:
                ThrowSeeds();
                break;
            case 3:
                print("use axe");
                break;
            case 4:
                print("use shears");
                break;
        }

    }
    void UseItemUp(int itemToStop)
    {
        switch (itemToStop)
        {
            case 0:
              //  print("use nothing");
                break;
            case 1:
                // print("use watering can");
                ToggleWateringCan(false);
                break;
            case 2:
              //  ThrowSeeds();
                break;
            case 3:
               // print("use axe");
                break;
            case 4:
              //  print("use shears");
                break;
        }

    }
    void EnactConstantEffects()
    {
        if (currentlyWatering)
             wateringCanScript.CastRay();
    
    }

    [YarnCommand("GetItem")]
    public void GetItem (string itemName)
    {
       // if (itemName == "seeds")
       //     GlobalInventory.staticInventory.HaveSeeds = true;
        if (itemName == "bracelet")
            GlobalInventory.staticInventory.Bracelet = true;
        
    }



    void ThrowSeeds()
    {
        GameObject trigger = GameObject.Instantiate(seedTrigger);
        seedTrigger.GetComponent<SeedTriggerZone>().Player = transform;
        trigger.transform.position = transform.position;
        trigger.GetComponent<Rigidbody>().AddForce(seedThrowForce * transform.forward);
        // - (transform.forward * seedTriggerCreateDistance);
        seedBagParticleSystem.Emit(Mathf.RoundToInt(Random.Range(seedCountRange.x, seedCountRange.y)));
      
    }

    void ToggleWateringCan(bool state)
    {
        if (state)
        {
            wateringCanParticleSystem.emissionRate = wateringCanEmissionRate;
            currentlyWatering = true;
        }
        else
        {
            wateringCanParticleSystem.emissionRate = 0;
            currentlyWatering = false;
        }
    }
    #endregion
    #endregion
}
