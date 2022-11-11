using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Movement : MonoBehaviour
{
    [Header("movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    [Header("jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;





    public float groundDrag;

    [Header("ground check")]
    public float playerHieght;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("keyBinds")]
    KeyCode JumpKey = KeyCode.Space;
    KeyCode SprintKey = KeyCode.LeftControl;
    KeyCode crouchKey = KeyCode.LeftShift;

    [Header("crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    moveState state;
    public enum moveState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    void stateHandler()
    {

        if (Input.GetKey(crouchKey))
        {
            moveSpeed = crouchSpeed;
            state = moveState.crouching;
        }

        if (grounded && Input.GetKey(SprintKey))
        {
            state = moveState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = moveState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = moveState.air;
        }
    }


    private void FixedUpdate()
    {
        movePlayer();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHieght * 0.5f + 0.2f, whatIsGround);
        myInput();
        speedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;
        stateHandler();
    }
    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(JumpKey) && readyToJump && grounded)
        {
            jump();
            readyToJump = false;
            Invoke(nameof(resetJump), jumpCooldown);
        }
        if (Input.GetKey(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 2f, ForceMode.Force);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

        }

    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier * 10f, ForceMode.Force);
    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limetedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limetedVel.x, rb.velocity.y, limetedVel.z);
        }


    }
    private void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }
    private void resetJump()
    {
        readyToJump = true;
    }
}
