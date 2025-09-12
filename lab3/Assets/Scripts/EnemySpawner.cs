using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; //drag enemy prefab in inspector
    public Transform player;       //reference to the Player
    public int enemyCount = 5;     //number of enemies to spawn
    public float spawnRadius = 1f; //distance from player where enemies appear

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            //Pick an angle around the player
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 spawnPos = (Vector2)player.position + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;

            //Create enemy
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            //Assign the player reference
            EnemyBehavior eb = newEnemy.GetComponent<EnemyBehavior>();
            if (eb != null)
            {
                eb.player = player;
            }
        }
    }
}
