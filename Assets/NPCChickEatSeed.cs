using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCChickEatSeed : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] NavMeshAgent agent;
    [SerializeField] NPCFindDestination pathfinder;
    [SerializeField] NPCFollowPlayer followPlayer;
    [SerializeField] Transform seedTransform;
    [SerializeField]float eatTime;
    #endregion
    #region PublicProperties
    public Transform SeedTransform{get{return seedTransform;}set{seedTransform = value;}}
    #endregion
    #region UnityFunctions
    void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        pathfinder = GetComponent<NPCFindDestination>();
        followPlayer = GetComponent<NPCFollowPlayer>();
        agent.stoppingDistance = 0;
        SetLocation(seedTransform);
    }
    #endregion
    #region CustomFunctions
    void SetLocation(Transform target)
    {
        print("eating seeds");
        Vector3 targetPos = pathfinder.GetNearestNavPos(seedTransform.position);
        agent.destination = targetPos;
        Invoke("ImprintOnPlayer", eatTime);
    }
    void ImprintOnPlayer()
    {
        followPlayer.enabled = true;
        this.enabled = false;
    }
    #endregion
}
