using UnityEngine;
using System;

/// <summary>
/// Local backend bypass system for offline/local gameplay
/// This provides mock responses for all backend services
/// </summary>
public static class LocalBackendBypass
{
    private static bool _isLocalMode = false;
    private static bool _isInitialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_isInitialized) return;
        
        _isInitialized = true;
        
        // Determine if we should use local mode
        // Use local mode for non-mobile platforms or when explicitly configured
        _isLocalMode = Application.platform != RuntimePlatform.Android && 
                      Application.platform != RuntimePlatform.IPhonePlayer;
        
        Debug.Log($"LocalBackendBypass initialized. Local mode: {_isLocalMode}");
    }

    /// <summary>
    /// Checks if local mode is enabled
    /// </summary>
    public static bool IsLocalMode()
    {
        return _isLocalMode;
    }

    /// <summary>
    /// Forces local mode (for testing or configuration)
    /// </summary>
    public static void ForceLocalMode(bool enabled)
    {
        _isLocalMode = enabled;
        Debug.Log($"LocalBackendBypass: Local mode forced to {enabled}");
    }

    /// <summary>
    /// Gets a mock token for backend authentication
    /// </summary>
    public static string GetMockToken()
    {
        return $"local_token_{DateTime.Now.Ticks % 100000}";
    }

    /// <summary>
    /// Gets mock user data for local gameplay
    /// </summary>
    public static string GetMockUserData()
    {
        // Return a simple JSON-like structure with basic user data
        return "{\"username\":\"LocalPlayer\",\"level\":1,\"coins\":1000,\"unlocked\":true}";
    }

    /// <summary>
    /// Gets mock daily offer data
    /// </summary>
    public static string GetMockDailyOffer()
    {
        // Return mock daily offer data
        return "{\"available\":true,\"reward\":100,\"type\":\"coins\"}";
    }

    /// <summary>
    /// Gets mock leaderboard data
    /// </summary>
    public static string GetMockLeaderboardData()
    {
        // Return mock leaderboard data
        return "{\"scores\":[{\"name\":\"LocalPlayer\",\"score\":1000},{\"name\":\"Player2\",\"score\":800}]}";
    }

    /// <summary>
    /// Gets mock achievement data
    /// </summary>
    public static string GetMockAchievementData()
    {
        // Return mock achievement data
        return "{\"achievements\":[{\"id\":\"first_run\",\"unlocked\":true},{\"id\":\"collector\",\"unlocked\":false}]}";
    }

    /// <summary>
    /// Gets mock ad data
    /// </summary>
    public static string GetMockAdData()
    {
        // Return mock ad data
        return "{\"available\":false,\"type\":\"none\",\"reward\":0}";
    }

    /// <summary>
    /// Gets mock startup query response
    /// </summary>
    public static string GetMockStartupQuery()
    {
        // Return mock startup data
        return "{\"version\":\"1.0\",\"maintenance\":false,\"features\":[\"local_mode\"]}";
    }

    /// <summary>
    /// Gets mock cheat code response
    /// </summary>
    public static string GetMockCheatResponse(string cheatCode)
    {
        // Return mock cheat response
        return $"{{\"valid\":true,\"coins\":500,\"level\":5,\"message\":\"Cheat code {cheatCode} activated\"}}";
    }

    /// <summary>
    /// Logs a local event (replaces analytics in local mode)
    /// </summary>
    public static void LogLocalEvent(string eventName, string parameters = "")
    {
        Debug.Log($"LocalBackendBypass: Event logged - {eventName} {parameters}");
    }

    /// <summary>
    /// Logs a local error (replaces error reporting in local mode)
    /// </summary>
    public static void LogLocalError(string error, string context = "")
    {
        Debug.LogWarning($"LocalBackendBypass: Error logged - {error} in {context}");
    }

    /// <summary>
    /// Gets mock player name for local mode
    /// </summary>
    public static string GetLocalPlayerName()
    {
        return "LocalPlayer";
    }

    /// <summary>
    /// Gets mock device ID for local mode
    /// </summary>
    public static string GetLocalDeviceId()
    {
        return $"local_device_{SystemInfo.deviceUniqueIdentifier.GetHashCode():X8}";
    }

    /// <summary>
    /// Checks if network is available (always true in local mode)
    /// </summary>
    public static bool IsNetworkAvailable()
    {
        return _isLocalMode || Application.internetReachability != NetworkReachability.NotReachable;
    }

    /// <summary>
    /// Gets mock server status
    /// </summary>
    public static bool IsServerAvailable()
    {
        return _isLocalMode || true; // Always available in local mode
    }
}
