using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedTriggerZone : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Transform player;
    [SerializeField] float destroyTime;
    #endregion
    #region PublicProperties
    public Transform Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }
    #endregion
    #region UnityFunctions
    void Start () {
       Invoke("DestroySelf", destroyTime);
}
void Update () {
	
}
    void OnCollisionEnter(Collision col)
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Chick")
        {
            //print("chick enetered");
            ChickSeesSeed(col.transform);
        }
    }
#endregion
#region CustomFunctions
    void ChickSeesSeed(Transform chick)
    {
        chick.GetComponent<NPCwander>().FoundSeeds = true;
        chick.GetComponent<NPCwander>().enabled = false;
        chick.GetComponent<NPCFollowPlayer>().enabled = false;
        chick.GetComponent<NPCFollowPlayer>().Player = player;
        chick.GetComponent<NPCChickEatSeed>().SeedTransform = this.transform;
        chick.GetComponent<NPCChickEatSeed>().enabled = true;
        GetComponent<Collider>().enabled = false;
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }

#endregion
}
