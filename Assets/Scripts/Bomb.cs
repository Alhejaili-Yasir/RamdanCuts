using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager for scene switching

public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Lose");
        }
    }
}
