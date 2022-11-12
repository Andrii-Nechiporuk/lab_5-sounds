using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    private bool isAiming;

    private CharacterController controller;
    private Animator anim;

    public AudioSource woodstepsSound, metalstepsSound;
    private string taglayer = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);
        if (isGrounded) 
        {
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && !isAiming)
            {
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }

            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        Aim();
        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
    }
    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        woodstepsSound.enabled = false;
        metalstepsSound.enabled = false;

    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        if (taglayer == "Wood" && isGrounded == true)
        {
            woodstepsSound.enabled = true;
            metalstepsSound.enabled = false;
        }
        if (taglayer == "Metal" && isGrounded == true)
        {
            woodstepsSound.enabled = false;
            metalstepsSound.enabled = true;
        }
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        if (taglayer == "Wood" && isGrounded==true)
        {
            Debug.Log("fdsfsdfsdfasfsad");
            woodstepsSound.enabled = true;
            metalstepsSound.enabled = false;
        }
        if (taglayer == "Metal" && isGrounded == true)
        {
            woodstepsSound.enabled = false;
            metalstepsSound.enabled = true;
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        woodstepsSound.enabled = false;
        metalstepsSound.enabled = false;

    }
    private void Aim()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.Log("Aim");
            isAiming = !isAiming;
            if(isAiming)
            {
                moveSpeed = walkSpeed;
                anim.SetBool("isAiming", true);
            }
            else
            {
                anim.SetBool("isAiming", false);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wood"))
        {
            Debug.Log("wood");
            taglayer = "Wood";
        }
        if (collision.gameObject.CompareTag("Metal"))
        {
            Debug.Log("metal");
            taglayer = "Metal";
        }

    }
}
