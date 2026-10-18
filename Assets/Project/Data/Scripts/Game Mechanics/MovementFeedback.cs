using Unity.Cinemachine;
using UnityEngine;

public class MovementFeedback : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GrapplingHook grapple;
    [SerializeField] TrailRenderer dashTrail;
    [SerializeField] TrailRenderer grappleTrail;
    [SerializeField] CinemachineCamera cinemachineCamera;
    [SerializeField] float zoomOutAmount = 1f;
    [SerializeField] float zoomOutSpeed = 8f;
    [SerializeField] float zoomReturnSpeed = 3f;

    float originalLensSize, targetLensSize;
    bool wasDashing = false;
    bool wasGrappling = false;

    void Awake()
    {
        dashTrail.emitting = false;
        grappleTrail.emitting = false;
        originalLensSize = cinemachineCamera.Lens.OrthographicSize;
        targetLensSize = originalLensSize;
    }

    void LateUpdate()
    {
        UpdateZoom();
    }

    void Update()
    {
        if (player.IsDashing != wasDashing)
        {
            OnConfigureDash(player.IsDashing, player.IsDashing ? originalLensSize + zoomOutAmount : originalLensSize);
        }

        if (grapple.IsGrappling != wasGrappling)
        {
            OnConfigureGrapple(grapple.IsGrappling, grapple.IsGrappling ? originalLensSize + zoomOutAmount : originalLensSize);
        }
    }

    void OnConfigureDash(bool activated, float size)
    {
        wasDashing = activated;
        dashTrail.emitting = activated;
        targetLensSize = size;
    }

    void OnConfigureGrapple(bool activated, float size)
    {
        wasGrappling = activated;
        grappleTrail.emitting = activated;
        targetLensSize = size;
    }

    // Zooms out and in the camera when dashing or grappling for a speeding effect
    void UpdateZoom()
    {
        var lens = cinemachineCamera.Lens;

        // Speed according to movement's direction
        float speed = targetLensSize > lens.OrthographicSize ? zoomOutSpeed : zoomReturnSpeed;
        lens.OrthographicSize = Mathf.Lerp(lens.OrthographicSize, targetLensSize, speed * Time.deltaTime);

        cinemachineCamera.Lens = lens;
    }

}
