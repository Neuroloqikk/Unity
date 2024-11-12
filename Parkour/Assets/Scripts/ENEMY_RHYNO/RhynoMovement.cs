using TMPro.EditorUtilities;
using UnityEngine;

public class RhynoMovement : MonoBehaviour
{

    private Animator anim;
    public Transform player;
    public float visionRange = 10f;     // The range within which the enemy can see the player
    public LayerMask detectionLayer;  // Layer mask for detecting the player (e.g., "Player" layer)

    public float speed = 2f;                 // Speed of the enemy
    public float detectionDistance = 0.5f;   // Distance to check for obstacles in front
    public LayerMask obstacleLayer;          // Layer to detect obstacles (e.g., walls, ground)
    private int direction = -1;

    private bool isSeeingPlayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (isSeeingPlayer)
        {
            // Move the enemy in the current direction
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            // Check for a wall to deactivate
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction, detectionDistance, obstacleLayer);
            if (hit.collider != null)
            {
               print("Wall"); // Change direction
            }
        }
        else
        {
            if (PlayerInSight())
            {
                anim.SetBool("isSeeingPlayer", true);
                isSeeingPlayer = true;
            }
        }
    }

    bool PlayerInSight()
    {
        // Determine the direction the enemy is facing
        Vector2 direction = transform.right; // Use transform.up if the enemy is facing up in your game

        // Cast a ray in the forward direction to see if it hits the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, detectionLayer);

        // Check if the raycast hit the player
        if (hit.collider != null && hit.collider.transform == player)
        {
            return true; // Player is directly in front of the enemy
        }

        return false; // Player is not in sight
    }

    // Visualize the raycast in the editor
    private void OnDrawGizmosSelected()
    {
        // Set the color for the vision line
        Gizmos.color = Color.red;
        Vector2 direction = transform.right; // Use transform.up if the enemy faces up
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * visionRange);
    }
}