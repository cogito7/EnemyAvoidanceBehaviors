using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //drag enemy prefab in inspector
    public Transform player;       //reference to the Player
    public int enemyCount = 5;     //number of enemies to spawn
    //public float spawnRadius = 4f; //distance from player where enemies appear
    public float minSpawnRadius = 3f; // closest enemies can spawn
    public float maxSpawnRadius = 7f; // farthest enemies can spawn
    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            //randomize location of spawns
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);

            // Evenly space enemies around a circle
            float angle = i * Mathf.PI * 2f / enemyCount; // divide full circle by number of enemies
            Vector2 spawnPos = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;


            // Create enemy
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // Assign the player reference
            EnemyBehavior eb = newEnemy.GetComponent<EnemyBehavior>();
            if (eb != null)
            {
                eb.player = player;
            }
        }
    }
}
