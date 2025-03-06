using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gomain : MonoBehaviour
{
    public Button yourButton;  // Reference to your button

    void Start()
    {
        // Ensure the button is not null and add the listener to the button
        if (yourButton != null)
        {
            yourButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        // Destroy all objects in the current scene except the camera before loading the new scene
        DestroyAllObjects();

        // Load the "Game" scene after destroying objects
        SceneManager.LoadScene("MainMenu");
    }

    void DestroyAllObjects()
    {
        // Get all game objects in the current scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Loop through and destroy each object
        foreach (GameObject obj in allObjects)
        {
            // Check if the object is not the camera or the SceneLoader object itself
            if (obj != this.gameObject && obj.tag != "MainCamera")
            {
                Destroy(obj);
            }
        }
    }
}
