using UnityEngine;

public class FallTriggerDebug : MonoBehaviour
{
    private FallTrigger fallTrigger;
    private CharStateMachine charStateMachine;
    
    void Start()
    {
        fallTrigger = GetComponent<FallTrigger>();
        charStateMachine = CharHelper.GetCharStateMachine();
    }
    
    void Update()
    {
        // Debug info
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("=== FALL TRIGGER DEBUG INFO ===");
            Debug.Log("FallTrigger component: " + (fallTrigger != null ? "Found" : "Missing"));
            Debug.Log("CharStateMachine: " + (charStateMachine != null ? "Found" : "Missing"));
            Debug.Log("Player position: " + CharHelper.GetPlayerTransform().position);
            Debug.Log("Is Fred Dead: " + GameManager.IsFredDead());
            
            if (fallTrigger != null)
            {
                // Use reflection to access private fields
                var checkField = typeof(FallTrigger).GetField("check", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var checkUpdateCountField = typeof(FallTrigger).GetField("checkUpdateCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                var collideField = typeof(FallTrigger).GetField("collide", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                
                if (checkField != null)
                    Debug.Log("check: " + checkField.GetValue(fallTrigger));
                if (checkUpdateCountField != null)
                    Debug.Log("checkUpdateCount: " + checkUpdateCountField.GetValue(null));
                if (collideField != null)
                    Debug.Log("collide: " + collideField.GetValue(null));
            }
            
            if (charStateMachine != null)
            {
                Debug.Log("Current State: " + charStateMachine.GetCurrentState());
                Debug.Log("Is Going Up: " + charStateMachine.IsGoingUp);
            }
        }
        
        // Force reset fall trigger
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Forcing FallTrigger reset...");
            FallTrigger.Reset();
        }
        
        // Force enable trigger check
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Debug.Log("Forcing enable trigger check...");
            if (fallTrigger != null)
            {
                fallTrigger.EnableTriggerCheck();
            }
        }
        
        // Test fall death manually
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("Manually triggering fall death...");
            GameEventDispatcher.Dispatch(this, new PlayerDieFalling());
        }
    }
} 