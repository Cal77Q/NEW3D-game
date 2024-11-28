using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    Animator myAnim;

    [SerializeField] float movementSpeed = 6f;
    [SerializeField] float jumpForce = 5f;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] AudioSource jumpSound;

    [SerializeField] float mouseSensitivity = 1000f;
    float xRotation = 0f;
    [SerializeField] Transform playerCamera;

    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        myAnim = GetComponentInChildren<Animator>();
        
    }

    void Update()
    {
        // Handle camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, ground);

       

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpSound.Play();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        myAnim.SetTrigger("jumped");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Head"))
        {
            Destroy(collision.transform.parent.gameObject);
            Jump();
        }
    }


    void FixedUpdate()
    {
        // Handle movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        Vector3 targetVelocity = new Vector3(moveDirection.x * movementSpeed, rb.velocity.y, moveDirection.z * movementSpeed);
        rb.velocity = targetVelocity;

        myAnim.SetFloat("Speed", moveDirection.magnitude);
        myAnim.SetBool("isOnGround", isActiveAndEnabled);
       

    }
}