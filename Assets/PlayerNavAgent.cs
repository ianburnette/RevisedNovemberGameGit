using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavAgent : MonoBehaviour {
    #region PrivateVariables
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerAnimation animator;
    [SerializeField] UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] Transform targetTransform, lookTarget;
    [SerializeField] Vector3 targetVector;
    [SerializeField] float stoppingMargin, rotationSpeed;
    [SerializeField] bool inRange;

    public Transform TargetTransform{get{return targetTransform;}set{targetTransform = value;}}
    public Transform LookTarget { get { return lookTarget; } set { lookTarget = value; } }

    public bool InRange{get{return inRange;}set{inRange = value;}}
    #endregion
    #region PublicProperties

    #endregion
    #region UnityFunctions
    void OnEnable () {
        agent.enabled = true;
        rb.isKinematic = true;
}
    void OnDisable()
    {
        inRange = false;
        agent.enabled = false;
        rb.isKinematic = false;
    }
void Update () {
        SetDestination();
        SetAnimation();
        CheckRotation();
}
#endregion
#region CustomFunctions
    void SetDestination()
    {
        if (targetTransform)
            agent.destination = targetTransform.position;
        else
            agent.destination = targetVector;
    }
    void SetAnimation()
    {
        animator.SetFloat("speed", agent.desiredVelocity.magnitude);
        animator.SetFloat("DistanceToTarget", agent.velocity.magnitude);
        // animator.SetFloat("Turn", moveDirection.x);
        animator.SetBool("OnGround", true);
        animator.SetBool("Grounded", true);
        animator.SetFloat("YVelocity", 0);
    }
    void CheckRotation()
    {
        if (Vector3.Distance(transform.position, TargetTransform.position) <= agent.stoppingDistance - stoppingMargin)
        {
            inRange = true;
            RotateToward(lookTarget);
        }
    }
    private void RotateToward(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    #endregion
}
