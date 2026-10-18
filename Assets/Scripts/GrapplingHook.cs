using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    Rigidbody2D rb;
    LineRenderer lr;
    DistanceJoint2D dj;
    public LayerMask grappleLayer;

    public bool isGrappling = false;
    Vector3 point;

    [SerializeField] float maxGrappleDistance = 8f;
    [SerializeField] float reelSpeed = 10f;
    [SerializeField] float minRopeLength = 1.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();
        dj.enabled = false;
        lr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0f;

            float dist = Vector2.Distance(transform.position, point);

            bool hit = Physics2D.OverlapCircle(point, 0.1f, grappleLayer);
            Debug.Log($"dist: {dist}, max: {maxGrappleDistance}, hit: {hit}");
            if (dist <= maxGrappleDistance && hit)
            {
                isGrappling = true;
                dj.connectedAnchor = point;
                dj.distance = dist;
                dj.enabled = true;
                Debug.Log("Im grapplin it");
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isGrappling)
            {
                rb.linearVelocity *= 1.5f;
            }

            isGrappling = false;
            lr.enabled = false;
            dj.enabled = false;
        }

        if (isGrappling)
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, point);
        }
    }

    void FixedUpdate()
    {
        if (isGrappling && dj.distance > minRopeLength)
        {
            dj.distance -= reelSpeed * Time.fixedDeltaTime;
        }
    }

    public void ReleaseGrapple()
    {
        isGrappling = false;
        lr.enabled = false;
        dj.enabled = false;
    }
}
