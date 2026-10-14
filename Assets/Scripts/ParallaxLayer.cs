using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField, Range(0f, 1f)] float parallaxFactor = 0.5f;

    float startX;
    float camStartX;
    float width;
    float shift;

    void Start()
    {
        if (cam == null) cam = Camera.main.transform;
        startX = transform.position.x;
        camStartX = cam.position.x;
        width = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float dx = cam.position.x - camStartX;
        float travelled = dx * (1 - parallaxFactor);

        if (travelled > shift + width) shift += width;
        else if (travelled < shift - width) shift -= width;

        transform.position = new Vector3(startX + shift + dx * parallaxFactor,
                                            transform.position.y,
                                            transform.position.z);
    }
}