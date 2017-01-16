using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour {
    #region PrivateProperties
    [SerializeField] Vector3 sprayOffset;
    [SerializeField] LayerMask sprayMask;
    [SerializeField] Waterable currentWaterable;
    [SerializeField] float waterFlowAmt;
    [SerializeField] ParticleSystem sprayParticles;
    Ray sprayRay;
    #endregion

    #region UnityFunctions
    private void Update()
    {
        sprayRay = new Ray(transform.position + sprayOffset, Vector3.down);
       
     //   CastRay();
    }
    #endregion
    #region CustomFunctions
    public void CastRay()
    {
        Debug.DrawRay(sprayRay.origin, sprayRay.direction, Color.blue);
        RaycastHit hit;
        if (Physics.Raycast(sprayRay.origin, sprayRay.direction, out hit, sprayMask))
        {
            if (hit.transform.tag == "waterable")
            {
                print("hitting " + hit.transform);
                if (currentWaterable == null)
                    currentWaterable = hit.transform.GetComponent<Waterable>();
                currentWaterable.WateredAmt += waterFlowAmt;
                sprayParticles.subEmitters.collision0.enableEmission = false;
            }
            else if (hit.transform.tag == "watered")
            {
                sprayParticles.subEmitters.collision0.enableEmission = true;
                //.enabled = true;
                currentWaterable = null;
            }
            else{
                sprayParticles.subEmitters.collision0.enableEmission = true;
                currentWaterable = null;
            }
        }
        else
        {
            sprayParticles.subEmitters.collision0.enableEmission = true;
            currentWaterable = null;
        }
            
    }
    #endregion
}
