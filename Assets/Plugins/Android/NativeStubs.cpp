// Native stub implementations for Android builds
// This file provides empty implementations for iOS-specific functions that are causing linker errors

#include <jni.h>

// Twitter function stubs
extern "C" {
    void _twitterInit(const char* consumerKey, const char* consumerSecret) {}
    bool _twitterIsLoggedIn() { return false; }
    const char* _twitterLoggedInUsername() { return ""; }
    void _twitterLogin(const char* username, const char* password) {}
    void _twitterShowOauthLoginDialog() {}
    void _twitterLogout() {}
    void _twitterPostStatusUpdate(const char* status) {}
    void _twitterPostStatusUpdateWithImage(const char* status, const char* imagePath) {}
    void _twitterGetHomeTimeline() {}
    void _twitterPerformRequest(const char* methodType, const char* path, const char* parameters) {}
    bool _twitterIsTweetSheetSupported() { return false; }
    bool _twitterCanUserTweet() { return false; }
    void _twitterShowTweetComposer(const char* status, const char* imagePath) {}
    
    // iCade function stubs
    void _iCadeSetActive(bool active) {}
    void iCadeUpdateState(void* state) {}
    
    // Tapjoy function stubs - Basic functions
    void Tapjoy_SetUnityVersion(const char* version) {}
    void Tapjoy_Connect(const char* appId, const char* secretKey) {}
    void Tapjoy_SetKeyToDictionaryRefValueInDictionary(const char* key, const char* value) {}
    void Tapjoy_SetKeyToValueInDictionary(const char* key, const char* value) {}
    void Tapjoy_ActionComplete(const char* actionID) {}
    
    // Additional Tapjoy function stubs
    const char* Tapjoy_GetSDKVersion() { return ""; }
    void Tapjoy_SetDebugEnabled(bool enabled) {}
    void Tapjoy_SetAppDataVersion(const char* version) {}
    void Tapjoy_ShowOffers() {}
    void Tapjoy_ShowOffersWithCurrencyID(const char* currencyID) {}
    void Tapjoy_GetCurrencyBalance() {}
    void Tapjoy_SpendCurrency(int amount) {}
    void Tapjoy_AwardCurrency(int amount) {}
    float Tapjoy_GetCurrencyMultiplier() { return 1.0f; }
    void Tapjoy_SetCurrencyMultiplier(float multiplier) {}
    void Tapjoy_ShowDefaultEarnedCurrencyAlert() {}
    
    // Tapjoy Placement function stubs
    void* Tapjoy_CreatePlacement(const char* placementName) { return nullptr; }
    void Tapjoy_RequestPlacementContent(void* placement) {}
    void Tapjoy_ShowPlacementContent(void* placement) {}
    bool Tapjoy_IsPlacementContentAvailable(void* placement) { return false; }
    bool Tapjoy_IsPlacementContentReady(void* placement) { return false; }
    void Tapjoy_ActionRequestCompleted(void* actionRequest) {}
    void Tapjoy_ActionRequestCancelled(void* actionRequest) {}
    void Tapjoy_RemovePlacement(void* placement) {}
    void Tapjoy_RemoveActionRequest(void* actionRequest) {}
    
    // Tapjoy Session and User function stubs
    void Tapjoy_StartSession() {}
    void Tapjoy_EndSession() {}
    void Tapjoy_SetUserID(const char* userID) {}
    void Tapjoy_SetUserLevel(int userLevel) {}
    void Tapjoy_SetUserFriendCount(int friendCount) {}
    void Tapjoy_SetUserCohortVariable(int variableIndex, const char* value) {}
    
    // Tapjoy Event Tracking function stubs
    void Tapjoy_TrackEvent(const char* eventName) {}
    void Tapjoy_TrackPurchase(const char* productID, const char* currencyCode, double price, const char* campaignID) {}
}
