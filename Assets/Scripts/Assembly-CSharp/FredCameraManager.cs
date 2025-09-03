using UnityEngine;

public class FredCameraManager : MonoBehaviour
{
    [SerializeField] private FredCamera fredCamera;
    [SerializeField] private AlphaFredCamera alphaFredCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fredCamera.enabled = false;
        alphaFredCamera.enabled = true;
    }
}
