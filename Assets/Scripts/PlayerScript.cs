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
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        spacePressed = false;
        isGrounded = false;
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
            Debug.Log("isSprinting");
            sprintSpeed = 2;
        }

        rb.velocity = new Vector3(horizontalInput, rb.velocity.y / PLAYER_MOVEMENT_SPEED, verticalInput) * PLAYER_MOVEMENT_SPEED * sprintSpeed;
    }

}
