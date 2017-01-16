using UnityEngine;
using System.Collections;

public class FloorCheckSpacer : MonoBehaviour {

#region PrivateVariables
    [SerializeField]
    private Transform[] floorchecks;
    [SerializeField]
    float distance;
    float currentDistance;
#endregion

#region PublicProperties

#endregion

#region UnityFunctions
void Start()
{

}
void Update()
{
    if (distance != currentDistance)
        {
            SpaceChecks();
            currentDistance = distance;
        }
}
#endregion

#region CustomFunctions
    void SpaceChecks()
    {
        floorchecks[0].position = transform.position + (Vector3.right * distance) + (Vector3.forward * distance);
        floorchecks[1].position = transform.position + (Vector3.forward * distance);
        floorchecks[2].position = transform.position + (Vector3.left * distance) + (Vector3.forward * distance);
        floorchecks[3].position = transform.position + (Vector3.right * distance);
        floorchecks[4].position = transform.position;
        floorchecks[5].position = transform.position + (Vector3.left * distance);
        floorchecks[6].position = transform.position + (Vector3.right * distance) + (Vector3.back * distance);
        floorchecks[7].position = transform.position + (Vector3.back * distance);
        floorchecks[8].position = transform.position + (Vector3.left * distance) + (Vector3.back * distance);
    }
#endregion
}
