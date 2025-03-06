using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;

    private Rigidbody2D fruitRigidbody; // Use Rigidbody2D for 2D physics
    private Collider2D fruitCollider;   // Use Collider2D for 2D collisions
    private ParticleSystem juiceEffect;

    public int points = 1;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody2D>(); // Get the 2D Rigidbody
        fruitCollider = GetComponent<Collider2D>(); // Get the 2D Collider
        juiceEffect = GetComponentInChildren<ParticleSystem>(); // Particle system for juice effect
    }

    private void Slice(Vector2 direction, Vector2 position, float force)
    {
        GameManager.Instance.IncreaseScore(points);

        // Disable the whole fruit
        fruitCollider.enabled = false;
        //whole.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        fruitRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;


        // Enable the sliced fruit
        sliced.SetActive(true);
        juiceEffect.Play();

        // Rotate based on the slice angle in 2D
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Get the Rigidbody2D components of the sliced fruit parts
        Rigidbody2D[] slices = sliced.GetComponentsInChildren<Rigidbody2D>();

        // Add force to each slice based on the blade direction
        foreach (Rigidbody2D slice in slices)
        {
            slice.linearVelocity = fruitRigidbody.linearVelocity; // Carry over the initial velocity

            // Apply a force to the slice in 2D space
            slice.AddForceAtPosition(direction * force, position, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the blade collided with the fruit
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }
}
