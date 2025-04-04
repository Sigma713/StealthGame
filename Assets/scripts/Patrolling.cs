using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Patrolling : MonoBehaviour
{
    public Transform[] points; // Array of patrol points
    int current; // Current patrol point index
    int last; // Total number of patrol points
    bool increasing; // Determines if NPC is moving forward or backward through points
    public float speed; // NPC movement speed
    private Animator animator; // Animator component for NPC animations
    public Transform player; // Reference to the player
    public float detectionRange = 5f; // NPC's vision range
    public float fieldOfViewAngle = 80f; // NPC's field of view angle
    private bool playerInSight = false; // Track if player is visible
    public LayerMask obstacleMask; // Layer mask to detect obstacles like walls

    private float timeSeenPlayer = 0f; // Timer for how long the player is seen
    public float timeToGameOver = 5f; // Time required to trigger Game Over
    private AudioSource alertSound; //reference to audiosource
    private bool hasPlayedSound = false; //Prevent looping alert sound

    void Start()
    {
        //save last level 
        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save(); // Make sure it’s saved
        //last level stuff
        current = 0;
        last = points.Length;
        increasing = true;
        animator = GetComponent<Animator>();
        alertSound = GetComponent<AudioSource>(); //get audio source component
    }

    void Update()
    {
        CheckForPlayer(); // Check if NPC can see the player

        if (playerInSight)
        {
            //play sound if not already played
            if (!hasPlayedSound)
            {
                alertSound.Play();
                hasPlayedSound = true;
            }

            // Turn to face the player
            Vector3 playerDirection = (player.position - transform.position).normalized;
            if (playerDirection != Vector3.zero)
            {
                transform.forward = playerDirection; // Rotate NPC to look at the player
            }

            animator.SetFloat("Speed", 0); // Stop moving animation

            // Increase timer while the player is in sight
            timeSeenPlayer += Time.deltaTime;
            if (timeSeenPlayer >= timeToGameOver)
            {
                SceneManager.LoadScene("GameOver"); // Load Game Over scene
            }
        }
        else
        {
            hasPlayedSound = false; //reset so it plays when sees player again
            timeSeenPlayer = 0f; // Reset timer when the player is not in sight

            if (transform.position == points[current].position)
            {
                TargetInt(); // Select the next patrol point
            }

            // Move towards the next patrol point
            Vector3 direction = (points[current].position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.forward = direction; // Face the direction of movement
            }

            transform.position = Vector3.MoveTowards(transform.position, points[current].position, speed * Time.deltaTime);

            // Set walking animation
            animator.SetFloat("Speed", 0.5f);
        }
    }

    void TargetInt()
    {
        // Determines the next patrol point in sequence
        if (increasing)
        {
            current++;
            if (current == last)
            {
                increasing = false; // Reverse direction at the last point
                current -= 1;
            }
        }
        else
        {
            current--;
            if (current == -1)
            {
                increasing = true; // Start moving forward again at the first point
                current = 0;
            }
        }
    }

    void CheckForPlayer()
    {
        // Get direction to the player
        Vector3 toPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        // Check if player is within vision range and within field of view
        if (Vector3.Distance(transform.position, player.position) <= detectionRange && angleToPlayer <= fieldOfViewAngle / 2)
        {
            // Perform a raycast to check if there are obstacles between NPC and player
            if (!Physics.Raycast(transform.position, toPlayer, Vector3.Distance(transform.position, player.position), obstacleMask))
            {
                playerInSight = true; // Player is detected and no obstacles in between
                return;
            }
        }

        playerInSight = false; // Player is out of sight or behind an obstacle
    }
}
