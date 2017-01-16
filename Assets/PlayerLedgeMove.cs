using UnityEngine;
using System.Collections;

public class PlayerLedgeMove : MonoBehaviour {

    #region PrivateVariables
    [SerializeField]
    CharacterMotor motor;
    [SerializeField]
    float accel, stopDistance;

    Quaternion screenMovementSpace;
    Vector3 screenMovementForward, screenMovementRight, direction, moveDirection;
    float horizontal, vertical;
    Transform mainCam;
    #endregion

    #region PublicProperties

    #endregion

    #region UnityFunctions
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    void Update()
    {
        GetPlayerInput();

    }
    void FixedUpdate()
    {
        motor.MoveTo(moveDirection, accel, stopDistance, false);
    }
    #endregion

    #region CustomFunctions
    void GetPlayerInput()
    {
        screenMovementSpace = Quaternion.Euler(0, mainCam.eulerAngles.y, 0);
        screenMovementForward = screenMovementSpace * Vector3.forward;
        screenMovementRight = screenMovementSpace * Vector3.right;

        //get movement input, set direction to move in
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        float speed = new Vector2(horizontal, vertical).sqrMagnitude;

        StickToWorldSpace(this.transform, mainCam.transform, ref direction, ref speed);
        moveDirection = transform.position + direction;

    }
    public void StickToWorldSpace(Transform root, Transform camera, ref Vector3 directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;
        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        speedOut = stickDirection.sqrMagnitude;

        //Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

        //Convert joystick input into worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 1f, root.position.z), moveDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 1f, root.position.z), axisSign, Color.cyan);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 1f, root.position.z), rootDirection, Color.red);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 1f, root.position.z), stickDirection, Color.blue);
               
        //    rotateSpeed = customRotateSpeed;
    
        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
        angleRootToMove /= 180f;
        directionOut = moveDirection;
    }

    #endregion
}
