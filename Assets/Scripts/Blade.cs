using UnityEngine;

public class Blade : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;

    private Camera mainCamera;
    private Collider2D sliceCollider;
    private TrailRenderer sliceTrail;

    public Vector2 direction { get; private set; }
    public bool slicing { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider2D>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlice();
    }

    private void OnDisable()
    {
        StopSlice();
    }

    private void Update()
    {
        if (Input.touchCount > 0) // Check if there's an active touch
        {
            Touch touch = Input.GetTouch(0); // Get the first touch

            if (touch.phase == TouchPhase.Began)
            {
                StartSlice(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                ContinueSlice(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                StopSlice();
            }
        }
    }

    private void StartSlice(Vector2 touchPosition)
    {
        Vector2 position = mainCamera.ScreenToWorldPoint(touchPosition);
        transform.position = position;

        slicing = true;
        sliceCollider.enabled = true;
        sliceTrail.enabled = true;
        sliceTrail.Clear();
    }

    private void StopSlice()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    private void ContinueSlice(Vector2 touchPosition)
    {
        Vector2 newPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        direction = newPosition - (Vector2)transform.position;

        float velocity = direction.magnitude / Time.deltaTime;
        sliceCollider.enabled = velocity > minSliceVelocity;

        transform.position = newPosition;
    }
}
