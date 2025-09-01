using UnityEngine;

public class TorchFlicker : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light torchLight;
    [SerializeField] private float baseIntensity = 1.5f;
    [SerializeField] private float flickerIntensity = 0.5f;
    
    [Header("Flicker Settings")]
    [SerializeField] private float flickerSpeed = 2.0f;
    [SerializeField] private float flickerVariation = 0.3f;
    [SerializeField] private float windEffect = 0.1f;
    
    [Header("Color Settings")]
    [SerializeField] private Color baseColor = new Color(1.0f, 0.6f, 0.3f); // Warm orange
    [SerializeField] private Color flickerColor = new Color(1.0f, 0.4f, 0.1f); // Deeper orange
    [SerializeField] private float colorVariation = 0.2f;
    
    [Header("Advanced Settings")]
    [SerializeField] private bool useNoise = true;
    [SerializeField] private float noiseScale = 1.0f;
    [SerializeField] private float secondaryFlicker = 0.5f;
    
    private float timeOffset;
    private Vector3 originalPosition;
    private float originalIntensity;
    private Color originalColor;
    
    void Start()
    {
        // Auto-find light component if not assigned
        if (torchLight == null)
        {
            torchLight = GetComponent<Light>();
            if (torchLight == null)
            {
                torchLight = GetComponentInChildren<Light>();
            }
        }
        
        if (torchLight == null)
        {
            Debug.LogError("No Light component found! Please assign a light or ensure this script is on the same GameObject as a Light component.");
            enabled = false;
            return;
        }
        
        // Store original values
        originalPosition = transform.position;
        originalIntensity = torchLight.intensity;
        originalColor = torchLight.color;
        
        // Random time offset for variation between multiple torches
        timeOffset = Random.Range(0f, 1000f);
        
        // Set initial light properties
        torchLight.color = baseColor;
        torchLight.intensity = baseIntensity;
    }
    
    void Update()
    {
        if (torchLight == null) return;
        
        float time = Time.time + timeOffset;
        
        // Primary flicker using sine waves
        float primaryFlicker = Mathf.Sin(time * flickerSpeed) * flickerIntensity;
        
        // Secondary flicker for more realistic effect
        float secondaryFlickerValue = Mathf.Sin(time * flickerSpeed * 2.3f) * secondaryFlicker;
        
        // Noise-based flicker for organic movement
        float noiseFlicker = 0f;
        if (useNoise)
        {
            noiseFlicker = Mathf.PerlinNoise(time * noiseScale, 0f) * flickerVariation;
        }
        
        // Wind effect - subtle position movement
        if (windEffect > 0)
        {
            Vector3 windOffset = new Vector3(
                Mathf.Sin(time * 0.8f) * windEffect,
                Mathf.Cos(time * 1.2f) * windEffect * 0.5f,
                Mathf.Sin(time * 0.6f) * windEffect
            );
            transform.position = originalPosition + windOffset;
        }
        
        // Combine all flicker effects
        float totalFlicker = primaryFlicker + secondaryFlickerValue + noiseFlicker;
        float finalIntensity = baseIntensity + totalFlicker;
        
        // Clamp intensity to prevent negative values
        finalIntensity = Mathf.Max(0.1f, finalIntensity);
        
        // Apply intensity
        torchLight.intensity = finalIntensity;
        
        // Color variation
        Color finalColor = Color.Lerp(baseColor, flickerColor, 
            Mathf.Abs(Mathf.Sin(time * flickerSpeed * 0.7f)) * colorVariation);
        
        torchLight.color = finalColor;
    }
    
    // Public methods for external control
    public void SetFlickerIntensity(float intensity)
    {
        flickerIntensity = intensity;
    }
    
    public void SetFlickerSpeed(float speed)
    {
        flickerSpeed = speed;
    }
    
    public void SetBaseColor(Color color)
    {
        baseColor = color;
    }
    
    public void SetFlickerColor(Color color)
    {
        flickerColor = color;
    }
    
    // Reset to original values
    public void ResetToOriginal()
    {
        if (torchLight != null)
        {
            torchLight.intensity = originalIntensity;
            torchLight.color = originalColor;
        }
        transform.position = originalPosition;
    }
    
    // Emergency stop flickering
    public void StopFlickering()
    {
        if (torchLight != null)
        {
            torchLight.intensity = baseIntensity;
            torchLight.color = baseColor;
        }
    }
    
    // Visual debugging in editor
    void OnDrawGizmosSelected()
    {
        if (torchLight != null)
        {
            Gizmos.color = torchLight.color;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            
            // Draw light range
            Gizmos.color = new Color(torchLight.color.r, torchLight.color.g, torchLight.color.b, 0.1f);
            Gizmos.DrawWireSphere(transform.position, torchLight.range);
        }
    }
}
