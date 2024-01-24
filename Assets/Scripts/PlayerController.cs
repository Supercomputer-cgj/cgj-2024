using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 6f;

    [SerializeField] private float mouseSensitivityX = 10f;
    [SerializeField] private float mouseSensitivityY = 10f;

    [SerializeField] private float jumpForce = 15f;

    private PlayerMotor motor;
    private ConfigurableJoint joint;


    /*Animation*/
    private Animator animator;
    private bool isDead = false;
    private bool doSomething = false;

    private string Idle = "idle";
    private string Walk = "walk";
    private string Run = "running";
    private string idleJump = "idleJump";
    private string runningJump = "runningJump";

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        
        //Gestion Rotation joueur AXE X
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;
        motor.Rotate(rotation);

        //Gestion Rotation camera AXE Y
        float xRot = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRot * mouseSensitivityY;
        motor.RotateCamera(cameraRotationX);
        
        if (!doSomething)
        {
            //Gestion déplacement ZQSD
            float xMove = Input.GetAxisRaw("Horizontal");
            float zMove = Input.GetAxisRaw("Vertical");

            Vector3 moveHorizontal = transform.right * xMove;
            Vector3 moveVertical = transform.forward * zMove;




            Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;
            motor.Move(velocity);


            if (velocity.magnitude > 0.2f)
            {
                //si moouvement
                //tant que Lshift appuyé alors speed = 6f
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.Play(Run);
                    speed = 12f; //on accelere a balle
                }
                else
                {
                    animator.Play(Walk);
                    speed = 6f;
                }
            }
            else animator.Play(Idle);


            //Gestion du jump
            if (Input.GetButtonDown("Jump") &&
                Physics.Raycast(transform.position, Vector3.down, 0.8f)) //Gestion du jump
            {
                /*if (velocity.magnitude > 0.2f) animator.Play(runningJump);                                                //fonctionne pas fuck, a re faire a la fin
                else animator.Play(idleJump);*/
                Jump(jumpForce);
            }
        }
        else animator.Play(Idle);
        
        
        //  Physics.Raycast(transform.position, Vector3.down, 0.8f)
        //Gestion du jump
        if (Input.GetButtonDown("Jump")) //Gestion du jump
        {
            motor.Move(new Vector3());
        }
        

    }

    public void doSomethingTrue()
    {
        doSomething = true;
    }
    
    public void doSomethingFalse()
    {
        doSomething = false;
    }
    
    
    
    private void Jump(float _jumpForce)
    {
        motor.Jump(_jumpForce);
    }
}