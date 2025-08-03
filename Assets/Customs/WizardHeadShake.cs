using UnityEngine;

public class WizardHeadShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeFrequency = 5f;  // How fast it shakes
    public float shakeAmplitude = 0.05f;  // How much it moves

    private Vector3 startPos;

    void Start()
    {
        // Store the original local position of the head
        startPos = transform.localPosition;
    }

    void Update()
    {
        // Calculate shaking offset
        float shakeOffset = Mathf.Sin(Time.time * shakeFrequency) * shakeAmplitude;

        // Apply the shake to the local position (up and down)
        transform.localPosition = startPos + new Vector3(0f, shakeOffset, 0f);
    }
}
