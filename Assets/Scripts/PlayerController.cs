using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    [SerializeField] private float mouseSensitivityX = 10f;
    [SerializeField] private float mouseSensitivityY = 10f;
    [SerializeField] private float thrusterForce = 1000f;

    [Header("Joint Options")] [SerializeField]
    private float jointSpring = 15;

    [SerializeField] private float jointMaxForce = 30;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        setJoinSettings(jointSpring);
    }

    private void Update()
    {
        //Gestion déplacement ZQSD
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
        motor.Move(velocity);


        //Gestion Rotation joueur AXE X
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;
        motor.Rotate(rotation);


        //Gestion Rotation camera AXE Y
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * mouseSensitivityY;
        motor.RotateCamera(cameraRotationX);


        //Gestion du thruster
        Vector3 thrusterVelocity = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            thrusterVelocity = Vector3.up * thrusterForce;
            setJoinSettings(0f);
        }
        else
        {
            setJoinSettings(jointSpring);
        }
        motor.ApplyThruster(thrusterVelocity);
        
    }

    private void setJoinSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive {maximumForce = jointMaxForce, positionSpring = _jointSpring};
    }
}