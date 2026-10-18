using Unity.Cinemachine;
using UnityEngine;

public class DashFeedback : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] TrailRenderer trail;
    [SerializeField] CinemachineCamera cinemachineCamera;

    [SerializeField] float zoomOutAmount = 1f;
    [SerializeField] float zoomOutSpeed = 8f;
    [SerializeField] float zoomReturnSpeed = 3f;

    float originalLensSize, targetLensSize;
    bool wasDashing = false;

    void Awake()
    {
        trail.emitting = false;
        originalLensSize = cinemachineCamera.Lens.OrthographicSize;
        targetLensSize = originalLensSize;
    }

    void LateUpdate()
    {
        UpdateZoom();
    }


    void Update()
    {
        if (player.IsDashing && !wasDashing)
        {
            OnDashStart();
        }
        else if (!player.IsDashing && wasDashing)
        {
            OnDashEnd();
        }
    }

    void OnDashStart()
    {
        wasDashing = true;
        trail.emitting = true;
        targetLensSize = originalLensSize + zoomOutAmount;
    }

    void OnDashEnd()
    {
        wasDashing = false;
        trail.emitting = false;
        targetLensSize = originalLensSize;
    }

    // Zooms out and in the camera when dashing for a speeding effect
    void UpdateZoom()
    {
        var lens = cinemachineCamera.Lens;

        // Speed according to movement's direction
        float speed = targetLensSize > lens.OrthographicSize ? zoomOutSpeed : zoomReturnSpeed;
        lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, targetLensSize, speed * Time.deltaTime);

        cinemachineCamera.Lens = lens;
    }

}
