using UnityEngine;
using System.Collections;

public class TangentTest : MonoBehaviour {

    [SerializeField]
    private Vector3 normal, tangent, cross;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.DrawRay(transform.position, normal, Color.magenta);
        tangent = new Vector3(-normal.z, 0, normal.x);
        Debug.DrawRay(transform.position, tangent, Color.yellow);
        cross = Vector3.Cross(tangent, normal );
        Debug.DrawRay(transform.position, cross, Color.red);
    }
}


/*
n=  .3,1 ,  0
t=   0,0 ,-.3

n=  0,1,.3
t=  -.3,0,0

n=  .3,1,.3
t=  -.3,0,.3

n=  1,1,1
t=  -1,0,1

n=  -1,1,1
t=  -1,0,-1

n=  .3,1,.6
t=  -.6,0,.3

n=  -.3,1,.6
t=  -.6,0,-.3

n=-.3,1,-.6
t=.6,0,-.3





    */
