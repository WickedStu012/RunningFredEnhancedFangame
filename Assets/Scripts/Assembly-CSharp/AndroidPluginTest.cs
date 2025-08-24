using UnityEngine;

/// <summary>
/// Test script to verify Android plugin bypass functionality
/// </summary>
public class AndroidPluginTest : MonoBehaviour
{
    [Header("Test Settings")]
    public bool runTestsOnStart = true;
    public bool logDetailedInfo = true;

    private void Start()
    {
        if (runTestsOnStart)
        {
            RunTests();
        }
    }

    [ContextMenu("Run Android Plugin Tests")]
    public void RunTests()
    {
        Debug.Log("=== Android Plugin Bypass Tests ===");
        
        // Test 1: Check if Android plugins are available
        bool pluginsAvailable = AndroidPluginBypass.AreAndroidPluginsAvailable();
        Debug.Log($"Test 1 - Android plugins available: {pluginsAvailable}");
        
        // Test 2: Test username retrieval
        string username = AndroidPluginBypass.GetAndroidUserName();
        Debug.Log($"Test 2 - Username retrieved: {username}");
        
        // Test 3: Test Flurry initialization (with dummy key)
        Debug.Log("Test 3 - Testing Flurry initialization...");
        AndroidPluginBypass.SafeFlurryInit("test_api_key");
        
        // Test 4: Test Flurry event logging
        Debug.Log("Test 4 - Testing Flurry event logging...");
        AndroidPluginBypass.SafeFlurryLogEvent("test_event");
        
        // Test 5: Test Google IAB initialization (with dummy key)
        Debug.Log("Test 5 - Testing Google IAB initialization...");
        AndroidPluginBypass.SafeGoogleIABInit("test_public_key");
        
        // Test 6: Test Flurry session end
        Debug.Log("Test 6 - Testing Flurry session end...");
        AndroidPluginBypass.SafeFlurryEndSession();
        
        // Test 7: Test Twitter functionality
        Debug.Log("Test 7 - Testing Twitter initialization...");
        AndroidPluginBypass.SafeTwitterInit("test_consumer_key", "test_consumer_secret");
        
        // Test 8: Test Twitter login status
        Debug.Log("Test 8 - Testing Twitter login status...");
        bool twitterLoggedIn = AndroidPluginBypass.SafeTwitterIsLoggedIn();
        Debug.Log($"Twitter logged in: {twitterLoggedIn}");
        
        // Test 9: Test Twitter username
        Debug.Log("Test 9 - Testing Twitter username...");
        string twitterUsername = AndroidPluginBypass.SafeTwitterGetUsername();
        Debug.Log($"Twitter username: {twitterUsername}");
        
        // Test 10: Test MOGA initialization
        Debug.Log("Test 10 - Testing MOGA initialization...");
        bool mogaAvailable = AndroidPluginBypass.SafeMogaInit();
        Debug.Log($"MOGA available: {mogaAvailable}");
        
        // Test 11: Test network security helper
        Debug.Log("Test 11 - Testing network security helper...");
        NetworkSecurityHelper.LogNetworkSecurityInfo();
        
        // Test 12: Test local mode detection
        Debug.Log("Test 12 - Testing local mode detection...");
        bool localMode = Application.platform != RuntimePlatform.Android && 
                        Application.platform != RuntimePlatform.IPhonePlayer;
        Debug.Log($"Local mode enabled: {localMode}");
        
        if (localMode)
        {
            Debug.Log("Running in local mode - backend services will use fallbacks");
            
            // Test if LocalBackendBypass is available (might not be due to compilation order)
            try
            {
                var localBypassType = System.Type.GetType("LocalBackendBypass");
                if (localBypassType != null)
                {
                    var isLocalModeMethod = localBypassType.GetMethod("IsLocalMode");
                    var getMockTokenMethod = localBypassType.GetMethod("GetMockToken");
                    var getLocalPlayerNameMethod = localBypassType.GetMethod("GetLocalPlayerName");
                    
                    if (isLocalModeMethod != null && getMockTokenMethod != null && getLocalPlayerNameMethod != null)
                    {
                        bool bypassLocalMode = (bool)isLocalModeMethod.Invoke(null, null);
                        string mockToken = (string)getMockTokenMethod.Invoke(null, null);
                        string localPlayerName = (string)getLocalPlayerNameMethod.Invoke(null, null);
                        
                        Debug.Log($"LocalBackendBypass available - Local mode: {bypassLocalMode}");
                        Debug.Log($"Mock token: {mockToken}");
                        Debug.Log($"Local player name: {localPlayerName}");
                    }
                }
                else
                {
                    Debug.Log("LocalBackendBypass not available (compilation order issue)");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error accessing LocalBackendBypass: {e.Message}");
            }
        }
        
        Debug.Log("=== Android Plugin Bypass Tests Complete ===");
        
        if (logDetailedInfo)
        {
            LogDetailedInfo();
        }
    }

    private void LogDetailedInfo()
    {
        Debug.Log("=== Detailed System Information ===");
        Debug.Log($"Platform: {Application.platform}");
        Debug.Log($"Unity Version: {Application.unityVersion}");
        Debug.Log($"Device Model: {SystemInfo.deviceModel}");
        Debug.Log($"Operating System: {SystemInfo.operatingSystem}");
        Debug.Log($"Device Type: {SystemInfo.deviceType}");
        Debug.Log($"Graphics Device: {SystemInfo.graphicsDeviceName}");
        Debug.Log($"Graphics API: {SystemInfo.graphicsDeviceType}");
        Debug.Log("=== End Detailed Information ===");
    }

    private void OnGUI()
    {
        if (!runTestsOnStart)
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            
            if (GUILayout.Button("Run Android Plugin Tests"))
            {
                RunTests();
            }
            
            if (GUILayout.Button("Test Username"))
            {
                string username = AndroidPluginBypass.GetAndroidUserName();
                Debug.Log($"Username test result: {username}");
            }
            
            if (GUILayout.Button("Test Flurry"))
            {
                AndroidPluginBypass.SafeFlurryInit("test_key");
                AndroidPluginBypass.SafeFlurryLogEvent("test_event");
                AndroidPluginBypass.SafeFlurryEndSession();
            }
            
            if (GUILayout.Button("Test Google IAB"))
            {
                AndroidPluginBypass.SafeGoogleIABInit("test_key");
            }
            
            if (GUILayout.Button("Test Twitter"))
            {
                AndroidPluginBypass.SafeTwitterInit("test_key", "test_secret");
                bool loggedIn = AndroidPluginBypass.SafeTwitterIsLoggedIn();
                Debug.Log($"Twitter test - Logged in: {loggedIn}");
            }
            
            if (GUILayout.Button("Test Network Security"))
            {
                NetworkSecurityHelper.LogNetworkSecurityInfo();
            }
            
            GUILayout.EndArea();
        }
    }
}
