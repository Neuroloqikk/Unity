using UnityEngine;

public class RhynoMovement : MonoBehaviour
{
    [Header("Rhyno Configs")]
    private bool isSeeingPlayer;
    private Animator anim;
    public float speed = 2f;                 // Speed of the enemy
    public float detectionDistance = 0.5f;   // Distance to check for obstacles in front
    public float visionRange = 10f;     // The range within which the enemy can see the player
    private PolygonCollider2D hornCollider;

    [Header("Rhyno Die on hit")]
    public LayerMask wallObstacleLayer;          // Layer to detect obstacles (e.g., walls, ground)
    public LayerMask boxObstacleLayer;          // Layer to detect obstacles (e.g., walls, ground)

    [Header("Parent configs")]
    public Rigidbody2D parentBody;
    public GameObject parentGameObject;

    [Header("Player configs")]
    public Transform player;
    public Animator playerAnimator;
    public LayerMask playerLayer;  // Layer mask for detecting the player (e.g., "Player" layer)

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hornCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        //If the rhyno saw the player once, then just run until it hits a wall/box
        if (isSeeingPlayer)
        {
            // Move the enemy in the current direction
            parentBody.linearVelocity = new Vector2(speed, parentBody.linearVelocity.y);

            // Check for a wall to deactivate
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, detectionDistance, wallObstacleLayer);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.right, detectionDistance, boxObstacleLayer);
            if (hit.collider != null || hit2.collider != null)
            {
                //If goes against box/wall, player the touchingWall animation and deactivate
                anim.SetBool("isSeeingPlayer", false);
                anim.SetBool("isTouchingWall", true);
            }
        }
        else
        {
            if (PlayerInSight())
            {
                //Make the rhyno start running when sees the player
                anim.SetBool("isSeeingPlayer", true);
                isSeeingPlayer = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerAnimator.SetTrigger("isDead");
            parentBody.linearVelocity = Vector2.zero;
            anim.SetBool("isSeeingPlayer", false);
            anim.SetBool("isTouchingWall", true);
        }
    }

    bool PlayerInSight()
    {
        // Determine the direction the enemy is facing
        Vector2 direction = transform.right; // Use transform.up if the enemy is facing up in your game

        // Cast a ray in the forward direction to see if it hits the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, playerLayer);

        // Check if the raycast hit the player
        if (hit.collider != null && hit.collider.transform == player)
        {
            return true; // Player is directly in front of the enemy
        }

        return false; // Player is not in sight
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        parentGameObject.SetActive(false);
    }
}