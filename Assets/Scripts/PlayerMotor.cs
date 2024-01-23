using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    
    private Vector3 velocity;
    private Vector3 rotation;
    private float jumpForce;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    [SerializeField] private float cameraRotationLimit = 85f;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // SETTEUR
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void RotateCamera(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }
    public void Jump(float _jumpForce)
    {
        jumpForce = _jumpForce;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    

    // CALCUL
    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    
    //rb pour le player movement x y z
    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        
    }
    
    //gestion rotation
    private void PerformRotation()
    {   // RB pour axe Y (tous le player moov)
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        
        //gestion axe X cam
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}