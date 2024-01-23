using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    [SerializeField] private float mouseSensitivityX = 10f;
    [SerializeField] private float mouseSensitivityY = 10f;

    [SerializeField] private float jumpForce = 15f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
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


        //Gestion du jump
        if (Input.GetButtonDown("Jump"))
        {
            Jump(jumpForce);
        }


    }

    private void Jump(float _jumpForce)
    {
        motor.Jump(_jumpForce);
    }
}