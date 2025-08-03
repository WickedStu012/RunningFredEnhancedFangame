using UnityEngine;

[RequireComponent(typeof(FredCamera))]
public class SmootherFollow : MonoBehaviour
{
    [Header("Smooth Movement Settings")]
    [Tooltip("How quickly the camera position follows the target (lower = smoother & slower)")]
    [Range(0.01f, 1f)] public float positionSmoothness = 0.1f;

    [Tooltip("How quickly the camera rotation follows the target (lower = smoother & slower)")]
    [Range(0.01f, 1f)] public float rotationSmoothness = 0.1f;

    private FredCamera fredCamera;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        fredCamera = GetComponent<FredCamera>();
    }

    private void LateUpdate()
    {
        // Smooth position
        transform.position = Vector3.Lerp(
            transform.position,
            fredCamera.transform.position,
            positionSmoothness
        );

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            fredCamera.transform.rotation,
            rotationSmoothness
        );
    }
}
