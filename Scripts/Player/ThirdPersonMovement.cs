using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    [Header("Movement")]
    public float moveSpeed;
    [Header ("Rotation")]
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [Header("Jumping")]
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool grounded;
    public float jumpHeight;
    [Header("keybinds")]
    KeyCode jump = KeyCode.Space;


     void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f , vertical).normalized;
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
       
        if (grounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }
        if (Input.GetKeyDown(jump) && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity );
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            
        }
        
        
    }
}
