using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCwander : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Vector3 startingPosition, target;
    [SerializeField] float waitTime, waitTimeVariance, speedOverride, distance;
    [SerializeField] bool foundSeeds;
    [SerializeField] NPCFindDestination pathfinder;


    #endregion
    #region PublicProperties
    public bool FoundSeeds{get{return foundSeeds;}set{foundSeeds = value;}}
    #endregion
    #region UnityFunctions
    void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        pathfinder = GetComponent<NPCFindDestination>();
    }

    void Start () {
        startingPosition = pathfinder.GetNearestNavPos(transform.position);
        StartCoroutine("Wander");
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(startingPosition, Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(target, 1f);
    }
    #endregion
    #region CustomFunctions
    IEnumerator Wander()
    {
        yield return new WaitForSeconds(Random.Range(waitTime - waitTimeVariance, waitTime + waitTimeVariance));
      //  print("wandering");
        target = pathfinder.GetNearestNavPos(startingPosition + UnityEngine.Random.insideUnitSphere * distance);
        agent.destination = target;
        if (!foundSeeds)
        {
            StartCoroutine("Wander");
            yield return null;
        }
        else
            yield return null;
    }
  
    #endregion
}
