using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCFollowPlayer : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NPCFindDestination pathfinder;
    [SerializeField] NPCChickEatSeed seedScript;
    [SerializeField] float checkForPlayerTime, distance;
    [SerializeField] Transform player;
    [SerializeField] Vector3 dest;
    [SerializeField] float stoppingDistance;
    [SerializeField] ExampleVariableStorage variables;
    [SerializeField] string variableName;
    #endregion
    #region PublicProperties
    public Transform Player{get{return player;}set{player = value;}}
    #endregion
    #region UnityFunctions
    void OnEnable() { 
        agent = GetComponent<NavMeshAgent>();
        pathfinder = GetComponent<NPCFindDestination>();
        agent.stoppingDistance = stoppingDistance;
        StartCoroutine("Follow");
        SetVariable();
    }
    void Update () {
	
    }
    #endregion
    #region CustomFunctions
    void SetVariable()
    {
        foreach (ExampleVariableStorage.DefaultVariable var in variables.defaultVariables)
        {
            if (var.name == variableName)
            {
                object value = true;
                var v = new Yarn.Value(value);
                variables.SetValue("$" + var.name, v);
            }

            //print("comparing " + var.name + " to " + variableName);
        }
    }
    IEnumerator Follow()
    {
        if (player != null && !seedScript.enabled)
        {
            dest = pathfinder.GetNearestNavPos(player.position + UnityEngine.Random.insideUnitSphere * distance);
            agent.destination = dest;
        }
        yield return new WaitForSeconds(checkForPlayerTime);
        StartCoroutine("Follow");
    }
    #endregion
}