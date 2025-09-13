using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public Transform player;                     // Player reference
    public float orbitDistance = 2f;            // Distance from player
    public float orbitSpeed = 5f;              // Orbiting speed
    public float minSpeed = 2f;               // Minimum speed
    public float maxSpeed = 10f;             // Maximum speed
    public float speedFactor = 5f;          // Speed factor based on distance from player
    public float avoidanceStrength = 1.5f; // Strength enemies push away
    public float avoidanceRadius = 3f;    // How close to get before repelling
    private Rigidbody2D rb;              // Rigidbody2D for physics-based movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        orbitDistance += Random.Range(0f, 3f); // Randomize enemy orbits slightly
    }

    void FixedUpdate()
    {
        if (player == null) return; // Null check if player is missing

        // --- Orbit Behavior ---
        Vector2 directionToPlayer = (Vector2)(transform.position - player.position); // Get vector pointing from player to enemy
        float distance = directionToPlayer.magnitude; // Current distance from player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x); // Calculate angle
        angle = angle * Mathf.Rad2Deg; // Convert to degrees
        Quaternion rotation_to_player = Quaternion.Euler(0f, 0f, angle + 90f);
        this.transform.rotation = rotation_to_player; // Make enemy sprites face player

        //Enemies close rotate faster & enemies further rotate slower
        float scaledOrbitSpeed = orbitSpeed;

        if (distance < 2f)         // if enemy is very close
            scaledOrbitSpeed = maxSpeed;
        else if (distance < 5f)    // if enemy is medium range
            scaledOrbitSpeed = orbitSpeed + (10f / (distance + 0.1f));
        else                       // if enemy is far away
            scaledOrbitSpeed = minSpeed;

        // Keep a fixed orbit distance
        Vector2 targetPosition = (Vector2)player.position + directionToPlayer.normalized * orbitDistance;

        // Find perpendicular direction to orbit around player
        Vector2 orbitDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x).normalized;
        Vector2 moveVector = orbitDirection * scaledOrbitSpeed;

        // Apply movement toward the target orbit radius
        Vector2 correction = (targetPosition - (Vector2)transform.position);
        moveVector += correction; // pushes them outward/inward to keep distance

        // --- Enemy to Enemy Avoidance ---
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject other in allEnemies)
        {
            if (other == this.gameObject) continue;

            Vector2 offset = transform.position - other.transform.position;
            float sqrDist = offset.sqrMagnitude;

            if (sqrDist < avoidanceRadius * avoidanceRadius) // If within avoidance radius, push away
            {
                moveVector += offset.normalized * avoidanceStrength / Mathf.Max(sqrDist, 0.01f);
            }
        }

        // --- Apply movement ---
        rb.velocity = moveVector;


    }
}
