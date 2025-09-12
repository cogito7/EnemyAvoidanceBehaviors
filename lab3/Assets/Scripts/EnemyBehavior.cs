using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;          //player reference
    public float orbitDistance = 3f;  //distance from player
    public float orbitSpeed = 5f;     //orbiting speed
    public float avoidanceStrength = 1.5f; //strength enemies push away
    public float avoidanceRadius = 3f;     //how close to get before repelling

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        //Orbit Behavior
        Vector2 directionToPlayer = (Vector2)(transform.position - player.position);
        float distance = directionToPlayer.magnitude;

        //Orbit offset
        Vector2 orbitDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x).normalized;

        //Speed scales with distance (closer = slower, farther = faster)
        float adjustedSpeed = orbitSpeed * (distance / orbitDistance);

        //Target movement
        Vector2 moveVector = orbitDirection * adjustedSpeed;

        //Enemy to Enemy Avoidance
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject other in allEnemies)
        {
            if (other == this.gameObject) continue; // skip self

            Vector2 offset = transform.position - other.transform.position;
            float sqrDist = offset.sqrMagnitude;

            if (sqrDist < avoidanceRadius * avoidanceRadius)
            {
                moveVector += offset.normalized * avoidanceStrength / Mathf.Max(sqrDist, 0.01f);
            }
        }

        //Apply movement
        rb.velocity = moveVector;

        //1st Attempt to Face Player..
        Vector2 lookDir = (Vector2)player.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 0f;
        rb.rotation = angle;



    }
}
