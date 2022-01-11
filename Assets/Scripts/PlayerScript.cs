using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private const float JUMP_FORCE = 5;
    private const float PLAYER_MOVEMENT_SPEED = 2;
    public const float KNOCKBACK_FORCE = 2;

    private bool spacePressed;
    private bool isGrounded;
    private bool shiftPressed;
    private bool gotHit;
    private bool leftMouseButtonClicked;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody rb;
    private EnemyRadar radar;
    private float turnSmoothVelocity;
    private Transform enemyPosition;

    public float turnSmoothTime = 0.1f;
    public Transform cam;
    public float attackRange = 10;

    // Start is called before the first frame update
    void Start()
    {
        spacePressed = false;
        isGrounded = false;
        gotHit = false;
        leftMouseButtonClicked = false;
        rb = GetComponent<Rigidbody>();
        radar = GetComponentInChildren<EnemyRadar>();
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
        if(Input.GetMouseButtonDown(0))
        {
            leftMouseButtonClicked=true;
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

        if (leftMouseButtonClicked)
        {
            Attack();
            leftMouseButtonClicked = false;
        }

        if (gotHit)
        {
            KnockBack();
            gotHit = false;
        }
    }

    private void Attack()
    {
        // TODO: Play Attack Animation
        Transform[] enemiesInRange = radar.GetEnemiesInRange(attackRange);
        if (enemiesInRange == null || enemiesInRange.Length == 0)
        {
            return;
        }

        // TODO: Check if player is looking at an enemy and hit that enemy
    }

    private void KnockBack()
    {
        Vector3 direction = transform.position - enemyPosition.position + (Vector3.up/2);
        direction = direction.normalized;

        rb.AddForce(direction * KNOCKBACK_FORCE, ForceMode.Impulse);
    }

    public void TakeHit(int damage, Transform enemyPosition)
    {
        Debug.Log("Got hit");
        gotHit = true;
        this.enemyPosition = enemyPosition;
    }

}
