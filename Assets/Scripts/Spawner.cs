using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Using Collider2D for 2D physics
public class Spawner : MonoBehaviour
{
    private Collider2D spawnArea; // Collider2D instead of Collider

    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    [Range(0f, 1f)]
    public float bombChance = 0.05f;

    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifetime = 5f;

    private void Awake()
    {
        spawnArea = GetComponent<Collider2D>(); // Use Collider2D
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);

        while (enabled)
        {
            GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            // Randomly spawn a bomb
            if (Random.value < bombChance)
            {
                prefab = bombPrefab;
            }

            // Get a random spawn position within the spawn area (2D bounds)
            Vector2 position = new Vector2
            {
                x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
            };

            // Set the rotation on the Z axis (2D game rotates around Z)
            float rotationZ = Random.Range(minAngle, maxAngle);

            // Instantiate the object (fruit or bomb) at the spawn position
            GameObject fruit = Instantiate(prefab, position, Quaternion.Euler(0f, 0f, rotationZ));

            // Destroy the object after a set lifetime
            Destroy(fruit, maxLifetime);

            // Add force to the 2D rigidbody
            float force = Random.Range(minForce, maxForce);
            Rigidbody2D rb = fruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply force along the up direction (Vector2.up in 2D)
                rb.AddForce(rb.transform.up * force, ForceMode2D.Impulse);
            }

            // Wait for a random amount of time before spawning the next item
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}
