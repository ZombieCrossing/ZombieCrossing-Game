using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float JUMP_FORCE = 5;
    private const float PLAYER_MOVEMENT_SPEED = 2;

    private bool spacePressed;
    private bool isGrounded;
    private bool shiftPressed;
    private bool gotHit;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rb;
    private float turnSmoothVelocity;

    public float turnSmoothTime = 0.1f;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        spacePressed = false;
        isGrounded = false;
        gotHit = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            spacePressed=true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftPressed = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftPressed=false;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded=true;
    }

    void FixedUpdate()
    {
        if(spacePressed && isGrounded)
        {
            rb.AddForce(Vector3.up * JUMP_FORCE, ForceMode.Impulse);
            isGrounded=false;
            spacePressed = false;
        }


        float sprintSpeed = 1f;

        if (shiftPressed)
        {
            sprintSpeed = 2;
        }

        Vector3 direction = new Vector3(horizontalInput, rb.velocity.y / (PLAYER_MOVEMENT_SPEED * sprintSpeed), verticalInput).normalized;

        if (horizontalInput > 0.1f ||horizontalInput < -0.1f || verticalInput > 0.1f || verticalInput < -0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            rb.velocity = moveDirection.normalized * PLAYER_MOVEMENT_SPEED * sprintSpeed;
        }

        if(gotHit)
        {
            KnockBack();
            gotHit = false;
        }
    }

    private void KnockBack()
    {

    }

    public void TakeHit(int damage)
    {
        Debug.Log("Got hit");
        gotHit = true;
    }

}
