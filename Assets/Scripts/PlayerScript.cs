using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float JUMP_FORCE = 5;

    private bool spacePressed;
    private bool isGrounded;
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
    }

}
