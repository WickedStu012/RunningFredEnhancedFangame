using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    private FredCamera fredCamera;
    private AlphaFredCamera alphaFredCamera;

    private void Start()
    {
        fredCamera = GetComponent<FredCamera>();
        alphaFredCamera = GetComponent<AlphaFredCamera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (fredCamera != null && alphaFredCamera != null)
            {
                bool isFredActive = fredCamera.enabled;

                fredCamera.enabled = !isFredActive;
                alphaFredCamera.enabled = isFredActive;
            }
        }
    }
}
