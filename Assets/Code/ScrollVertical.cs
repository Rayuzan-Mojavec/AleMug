using UnityEngine;


public class ScrollVertical : MonoBehaviour
{
    public float speed = 2f;

    private float tileHeight;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        tileHeight = GetComponent<Renderer>().bounds.size.y;
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Bottom of camera in world space
        float camBottom =
            cam.transform.position.y
            - cam.orthographicSize;

        // If tile is completely below camera
        if (transform.position.y + tileHeight / 2f < camBottom)
        {
            transform.position += Vector3.up * tileHeight * 2f;
        }
    }
}
