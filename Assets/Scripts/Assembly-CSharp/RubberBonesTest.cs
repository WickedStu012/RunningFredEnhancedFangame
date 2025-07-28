using UnityEngine;

public class RubberBonesTest : MonoBehaviour
{
    void Update()
    {
        // Test RubberBones status
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CharProps props = CharHelper.GetProps();
            Debug.Log("=== RUBBERBONES TEST ===");
            Debug.Log("RubberBones enabled: " + props.RubberBones);
            Debug.Log("minHeightToRoll: " + props.minHeightToRoll);
            Debug.Log("minHeightToTrip: " + props.minHeightToTrip);
            Debug.Log("minHeightToDie: " + props.minHeightToDie);
            Debug.Log("minHeightToExplode: " + props.minHeightToExplode);
            
            if (props.RubberBones)
            {
                Debug.Log("With RubberBones:");
                Debug.Log("Effective minHeightToDie: " + (props.minHeightToDie * 1.2f));
                Debug.Log("Effective minHeightToExplode: " + (props.minHeightToExplode * 1.2f));
            }
        }
        
        // Force enable RubberBones
        if (Input.GetKeyDown(KeyCode.F6))
        {
            CharProps props = CharHelper.GetProps();
            props.RubberBones = true;
            Debug.Log("RubberBones FORCED ENABLED");
        }
        
        // Force disable RubberBones
        if (Input.GetKeyDown(KeyCode.F7))
        {
            CharProps props = CharHelper.GetProps();
            props.RubberBones = false;
            Debug.Log("RubberBones FORCED DISABLED");
        }
        
        // Force fall death
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Debug.Log("FORCING FALL DEATH");
            GameEventDispatcher.Dispatch(this, new PlayerDieFalling());
        }
        
        // Test explode death
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Debug.Log("FORCING EXPLODE DEATH");
            CharHelper.GetCharStateMachine().SwitchTo(ActionCode.EXPLODE);
        }
    }
} 