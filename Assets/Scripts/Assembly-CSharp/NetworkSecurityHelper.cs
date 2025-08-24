using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Helper class to handle network security issues and provide fallback solutions
/// </summary>
public static class NetworkSecurityHelper
{
    /// <summary>
    /// Safely makes a web request with fallback for insecure connections
    /// </summary>
    public static UnityWebRequest SafeWebRequest(string url, string method = "GET")
    {
        try
        {
            // Try HTTPS first
            string secureUrl = url;
            if (url.StartsWith("http://"))
            {
                secureUrl = url.Replace("http://", "https://");
            }
            
            UnityWebRequest request = new UnityWebRequest(secureUrl, method);
            return request;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to create secure web request for {url}: {e.Message}");
            
            // Fallback to original URL (this might still fail, but we try)
            try
            {
                UnityWebRequest request = new UnityWebRequest(url, method);
                return request;
            }
            catch (Exception fallbackException)
            {
                Debug.LogError($"Failed to create fallback web request for {url}: {fallbackException.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Safely downloads from cache or URL with fallback
    /// </summary>
    public static WWW SafeLoadFromCacheOrDownload(string url, int version)
    {
        try
        {
            // Try HTTPS first
            string secureUrl = url;
            if (url.StartsWith("http://"))
            {
                secureUrl = url.Replace("http://", "https://");
            }
            
            return WWW.LoadFromCacheOrDownload(secureUrl, version);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load from cache with secure URL {url}: {e.Message}");
            
            // Fallback to original URL
            try
            {
                return WWW.LoadFromCacheOrDownload(url, version);
            }
            catch (Exception fallbackException)
            {
                Debug.LogError($"Failed to load from cache with fallback URL {url}: {fallbackException.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Safely downloads from cache or URL with hash
    /// </summary>
    public static WWW SafeLoadFromCacheOrDownload(string url, Hash128 hash, uint crc)
    {
        try
        {
            // Try HTTPS first
            string secureUrl = url;
            if (url.StartsWith("http://"))
            {
                secureUrl = url.Replace("http://", "https://");
            }
            
            return WWW.LoadFromCacheOrDownload(secureUrl, hash, crc);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load from cache with secure URL {url}: {e.Message}");
            
            // Fallback to original URL
            try
            {
                return WWW.LoadFromCacheOrDownload(url, hash, crc);
            }
            catch (Exception fallbackException)
            {
                Debug.LogError($"Failed to load from cache with fallback URL {url}: {fallbackException.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Creates a WWW object with fallback for insecure connections
    /// </summary>
    public static WWW SafeWWW(string url)
    {
        try
        {
            // Try HTTPS first
            string secureUrl = url;
            if (url.StartsWith("http://"))
            {
                secureUrl = url.Replace("http://", "https://");
            }
            
            return new WWW(secureUrl);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to create WWW with secure URL {url}: {e.Message}");
            
            // Fallback to original URL
            try
            {
                return new WWW(url);
            }
            catch (Exception fallbackException)
            {
                Debug.LogError($"Failed to create WWW with fallback URL {url}: {fallbackException.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Creates a WWW object with form data for POST requests
    /// </summary>
    public static WWW SafeWWW(string url, WWWForm form)
    {
        try
        {
            // Try HTTPS first
            string secureUrl = url;
            if (url.StartsWith("http://"))
            {
                secureUrl = url.Replace("http://", "https://");
            }
            
            return new WWW(secureUrl, form);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to create WWW with secure URL {url}: {e.Message}");
            
            // Fallback to original URL
            try
            {
                return new WWW(url, form);
            }
            catch (Exception fallbackException)
            {
                Debug.LogError($"Failed to create WWW with fallback URL {url}: {fallbackException.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Creates a WWW object with byte array data
    /// </summary>
    public static WWW SafeWWW(string url, byte[] postData, Dictionary<string, string> headers)
    {
        try
        {
            // Try HTTPS first
            string secureUrl = url;
            if (url.StartsWith("http://"))
            {
                secureUrl = url.Replace("http://", "https://");
            }
            
            return new WWW(secureUrl, postData, headers);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to create WWW with secure URL {url}: {e.Message}");
            
            // Fallback to original URL
            try
            {
                return new WWW(url, postData, headers);
            }
            catch (Exception fallbackException)
            {
                Debug.LogError($"Failed to create WWW with fallback URL {url}: {fallbackException.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// Checks if a URL is secure (HTTPS)
    /// </summary>
    public static bool IsSecureUrl(string url)
    {
        return url.StartsWith("https://");
    }

    /// <summary>
    /// Converts HTTP URL to HTTPS
    /// </summary>
    public static string MakeSecure(string url)
    {
        if (url.StartsWith("http://"))
        {
            return url.Replace("http://", "https://");
        }
        return url;
    }

    /// <summary>
    /// Logs network security information for debugging
    /// </summary>
    public static void LogNetworkSecurityInfo()
    {
        Debug.Log("=== Network Security Information ===");
        Debug.Log($"Application.platform: {Application.platform}");
        Debug.Log($"Application.internetReachability: {Application.internetReachability}");
        Debug.Log($"Application.targetFrameRate: {Application.targetFrameRate}");
        Debug.Log($"SystemInfo.deviceModel: {SystemInfo.deviceModel}");
        Debug.Log($"SystemInfo.operatingSystem: {SystemInfo.operatingSystem}");
        Debug.Log("=== End Network Security Information ===");
    }
}
