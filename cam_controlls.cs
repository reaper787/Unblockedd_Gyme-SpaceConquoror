using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour
{
    [Header("refrences")]
    public Transform orientation;
    public Transform player;
    public Transform PlayerObj;
    public Rigidbody rb;

    public float roatationSpeed;
   
   
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        Vector3 veiwDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = veiwDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(inputDir != Vector3.zero)
        {
            PlayerObj.forward = Vector3.Slerp(PlayerObj.forward, inputDir.normalized, Time.deltaTime * roatationSpeed);
        }

    }
}
