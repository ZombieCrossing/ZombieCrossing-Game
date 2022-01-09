using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public GameObject player;
    private float targetDistance;
    public float range = 40;
    private float minDistanceFromPlayer = 0.8f;
    // private GameObject zombie;
    private float followSpeed = 0.1f;
    private RaycastHit shot;
    private int walkPointRange = 7;
    private Vector3 walkPoint;
    private bool walkPointSet = false;
    private bool isGrounded;

    public LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Only if a player is in sight the zombie does something
        if(Physics.CheckSphere(transform.position, range, playerMask) && isGrounded)
        {
            WalkToPlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void WalkToPlayer()
    {
        // The zombie looks at the player
        transform.LookAt(player.transform);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot))
        {
            targetDistance = shot.distance;
            // If the player is in range the zombie moves to the player
            if (targetDistance <= range && targetDistance >= minDistanceFromPlayer)
            {
                followSpeed = 0.1f;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, followSpeed);
            }
            else
            {
                  
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void Patrol()
    {
        // Debug.Log("Patroling");
        if(!walkPointSet)
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            walkPointSet = true;
            // Debug.Log("Set WalkPoint");
        }

        transform.position = Vector3.MoveTowards(transform.position, walkPoint, followSpeed);
        transform.LookAt(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude <= 1f)
        {
            // Debug.Log("Reset walkpoint");
            walkPointSet = false;
        }
    }
}
