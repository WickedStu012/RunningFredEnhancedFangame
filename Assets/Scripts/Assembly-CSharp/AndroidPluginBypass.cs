using UnityEngine;
using System;

/// <summary>
/// Safe wrapper for Android plugin calls that may not be available
/// This prevents ClassNotFoundException errors by providing fallback implementations
/// </summary>
public static class AndroidPluginBypass
{
    private static bool _isInitialized = false;
    private static bool _androidPluginsAvailable = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_isInitialized) return;
        
        _isInitialized = true;
        
        // Check if we're on Android and if the plugins are available
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                // Try to access a simple Android class to test if plugins are available
                using (var testClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    _androidPluginsAvailable = testClass != null;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Android plugins not available: {e.Message}");
                _androidPluginsAvailable = false;
            }
        }
        else
        {
            _androidPluginsAvailable = false;
        }
        
        Debug.Log($"AndroidPluginBypass initialized. Android plugins available: {_androidPluginsAvailable}");
    }

    /// <summary>
    /// Safely gets the Android username with fallback
    /// </summary>
    public static string GetAndroidUserName()
    {
        // Check if we should use local mode (non-mobile platforms)
        bool isLocalMode = Application.platform != RuntimePlatform.Android && 
                          Application.platform != RuntimePlatform.IPhonePlayer;
        
        if (isLocalMode)
        {
            return "LocalPlayer"; // Local mode username
        }
        
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return "Player"; // Fallback username
        }

        try
        {
            return AndroidMarketAccount.GetUserName();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to get Android username: {e.Message}");
            return "Player"; // Fallback username
        }
    }

    /// <summary>
    /// Safely initializes Google IAB with error handling
    /// </summary>
    public static void SafeGoogleIABInit(string publicKey)
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Google IAB not available on this platform");
            return;
        }

        try
        {
            GoogleIAB.init(publicKey);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to initialize Google IAB: {e.Message}");
        }
    }

    /// <summary>
    /// Safely initializes Flurry with error handling
    /// </summary>
    public static void SafeFlurryInit(string apiKey)
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Flurry not available on this platform");
            return;
        }

        try
        {
            FlurryAndroid.onStartSession(apiKey);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to initialize Flurry: {e.Message}");
        }
    }

    /// <summary>
    /// Safely logs Flurry events with error handling
    /// </summary>
    public static void SafeFlurryLogEvent(string eventName)
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        try
        {
            FlurryAndroid.logEvent(eventName);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to log Flurry event '{eventName}': {e.Message}");
        }
    }

    /// <summary>
    /// Safely logs Flurry events with parameters
    /// </summary>
    public static void SafeFlurryLogEvent(string eventName, System.Collections.Generic.Dictionary<string, string> parameters)
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        try
        {
            FlurryAndroid.logEvent(eventName, parameters);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to log Flurry event '{eventName}' with parameters: {e.Message}");
        }
    }

    /// <summary>
    /// Safely ends Flurry session
    /// </summary>
    public static void SafeFlurryEndSession()
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        try
        {
            FlurryAndroid.onEndSession();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to end Flurry session: {e.Message}");
        }
    }

    /// <summary>
    /// Safely initializes Twitter with error handling
    /// </summary>
    public static void SafeTwitterInit(string consumerKey, string consumerSecret)
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Twitter not available on this platform");
            return;
        }

        try
        {
            TwitterAndroid.init(consumerKey, consumerSecret);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to initialize Twitter: {e.Message}");
        }
    }

    /// <summary>
    /// Safely checks if Twitter is logged in
    /// </summary>
    public static bool SafeTwitterIsLoggedIn()
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return false;
        }

        try
        {
            return TwitterAndroid.isLoggedIn();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to check Twitter login status: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Safely gets Twitter username
    /// </summary>
    public static string SafeTwitterGetUsername()
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return string.Empty;
        }

        try
        {
            return TwitterAndroid.getUsername();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to get Twitter username: {e.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Safely posts Twitter update
    /// </summary>
    public static void SafeTwitterPostUpdate(string update)
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        try
        {
            TwitterAndroid.postUpdate(update);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to post Twitter update: {e.Message}");
        }
    }

    /// <summary>
    /// Safely shows Twitter login dialog
    /// </summary>
    public static void SafeTwitterShowLoginDialog()
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        try
        {
            TwitterAndroid.showLoginDialog();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to show Twitter login dialog: {e.Message}");
        }
    }

    /// <summary>
    /// Safely logs out from Twitter
    /// </summary>
    public static void SafeTwitterLogout()
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        try
        {
            TwitterAndroid.logout();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to logout from Twitter: {e.Message}");
        }
    }

    /// <summary>
    /// Safely initializes MOGA controller
    /// </summary>
    public static bool SafeMogaInit()
    {
        if (!_androidPluginsAvailable || Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("MOGA not available on this platform");
            return false;
        }

        try
        {
            // This would need to be implemented based on your MOGA integration
            // For now, just return false to indicate MOGA is not available
            Debug.Log("MOGA controller not available");
            return false;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to initialize MOGA: {e.Message}");
            return false;
        }
    }

    /// <summary>
    /// Checks if Android plugins are available
    /// </summary>
    public static bool AreAndroidPluginsAvailable()
    {
        return _androidPluginsAvailable;
    }
}
