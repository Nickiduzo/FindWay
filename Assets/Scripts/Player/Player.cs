using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action TakeKey;
    
    public static event Action WinGame;
    public static event Action GameOver;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private float mouseSensetivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    private Rigidbody rb;
    private bool isGrounded;
    private float verticalLookRotation = 0f;

    private float timer;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = 0.25f;
    }
    private void Update()
    {
        MovePlayer();
        RotatePlayer();
        MakeAudioStep();
    }

    // play audio step
    private void MakeAudioStep()
    {
        if (IsMoving() && timer <= 0)
        {
            AudioManager.instance.PlayStep();
            timer = 0.25f;
        }
        else timer -= Time.deltaTime;
    }
    // player move
    private void MovePlayer()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;


        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * speed * Time.deltaTime); ;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            AudioManager.instance.Play("Jump");
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
    }

    //rotate player 
    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -lookLimit, lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    // check on key, trap, end position
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            TakeKey?.Invoke();
            AudioManager.instance.Play("Collect");
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("End")) WinGame?.Invoke();

        if(other.gameObject.CompareTag("Trap")) GameOver?.Invoke();
    }

    // check on player is moving or not
    private bool IsMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }
}
