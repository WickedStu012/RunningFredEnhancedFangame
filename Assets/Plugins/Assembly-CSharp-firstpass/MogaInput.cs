using UnityEngine;

public class MogaInput : MonoBehaviour
{
    private MogaController controller;
    private bool focused;

    // Button state tracking
    private int lastStateButtonA;
    private int lastStateButtonB;
    private int lastStateButtonStart;
    private int lastStateButtonSelect;

    // Button events
    private bool buttonADown;
    private bool buttonBDown;
    private bool buttonStartDown;
    private bool buttonSelectDown;
    private bool buttonAUp;
    private bool buttonBUp;
    private bool buttonStartUp;
    private bool buttonSelectUp;

    private static MogaInput instance;
    private static readonly object lockObject = new object();

    public static MogaInput Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MogaInput>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("MogaInput");
                        instance = obj.AddComponent<MogaInput>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return instance;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            Destroy();
            instance = null;
        }
    }

    protected void Initialize()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try 
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject mogaObject = MogaController.getInstance(activity);
            controller = new MogaController(mogaObject);
            controller.init();
            
            if (focused)
            {
                resumeController();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("MOGA Initialization failed: " + e.Message);
            controller = null;
        }
#else
        controller = null;
#endif
    }

    private void OnApplicationFocus(bool focus)
    {
        focused = focus;
        if (focused)
        {
            resumeController();
        }
        else
        {
            pauseController();
        }
    }

    protected void Destroy()
    {
        if (controller != null)
        {
            controller.exit();
            controller = null;
        }
    }

    private void resumeController()
    {
        if (controller != null)
        {
            controller.onResume();
        }
    }

    private void pauseController()
    {
        if (controller != null)
        {
            controller.onPause();
        }
    }

    public bool IsConnected()
    {
        return controller != null && controller.getState(1) == 1;
    }

    // D-Pad controls
    public bool GetDPadLeft()
    {
        return controller != null && controller.getKeyCode(21) == 0;
    }

    public bool GetDPadRight()
    {
        return controller != null && controller.getKeyCode(22) == 0;
    }

    public bool GetDPadUp()
    {
        return controller != null && controller.getKeyCode(19) == 0;
    }

    public bool GetDPadDown()
    {
        return controller != null && controller.getKeyCode(20) == 0;
    }

    // Analog sticks
    public float GetAxisX()
    {
        if (controller != null)
        {
            return controller.getAxisValue(0);
        }
        return 0f;
    }

    public float GetAxisY()
    {
        if (controller != null)
        {
            return -controller.getAxisValue(1);
        }
        return 0f;
    }
    public float GetAxisZ()
    {
        if (controller != null)
        {
            return controller.getAxisValue(11);
        }
        return 0f;
    }
    public float GetAxisRZ()
    {
        if (controller != null)
        {
            return controller.getAxisValue(14);
        }
        return 0f;
    }

    // Face buttons
    public bool GetButtonA()
    {
        return controller != null && controller.getKeyCode(96) == 0;
    }

    public bool GetButtonB()
    {
        return controller != null && controller.getKeyCode(97) == 0;
    }

    public bool GetButtonStart()
    {
        return controller != null && controller.getKeyCode(108) == 0;
    }

    public bool GetButtonSelect()
    {
        return controller != null && controller.getKeyCode(109) == 0;
    }

    // Shoulder buttons
    public bool GetButtonL1()
    {
        return controller != null && controller.getKeyCode(102) == 0;
    }

    public bool GetButtonR1()
    {
        return controller != null && controller.getKeyCode(103) == 0;
    }

    // Trigger buttons
    public bool GetButtonL2()
    {
        if (controller == null) return false;

        if (controller.getState(4) == 1) // Pro controller
        {
            return controller.getKeyCode(104) == 0;
        }
        return controller.getKeyCode(102) == 0; // Standard controller
    }

    public bool GetButtonR2()
    {
        if (controller == null) return false;

        if (controller.getState(4) == 1) // Pro controller
        {
            return controller.getKeyCode(105) == 0;
        }
        return controller.getKeyCode(103) == 0; // Standard controller
    }

    public bool IsProVersion()
    {
        return controller != null && controller.getState(4) == 1;
    }

    // Button down/up events
    public bool GetButtonADown()
    {
        return buttonADown;
    }

    public bool GetButtonAUp()
    {
        return buttonAUp;
    }

    public bool GetButtonBDown()
    {
        return buttonBDown;
    }

    public bool GetButtonBUp()
    {
        return buttonBUp;
    }

    public bool GetButtonStartDown()
    {
        return buttonStartDown;
    }

    public bool GetButtonStartUp()
    {
        return buttonStartUp;
    }
    public bool GetButtonSelectDown()
    {
        return buttonSelectDown;
    }

    public bool GetButtonSelectUp()
    {
        return buttonSelectUp;
    }

    private void Update()
    {
        if (controller == null) return;

        // Update button states
        buttonADown = CheckButtonDown(96, ref lastStateButtonA);
        buttonAUp = CheckButtonUp(96, ref lastStateButtonA);
        buttonBDown = CheckButtonDown(97, ref lastStateButtonB);
        buttonBUp = CheckButtonUp(97, ref lastStateButtonB);
        buttonStartDown = CheckButtonDown(108, ref lastStateButtonStart);
        buttonStartUp = CheckButtonUp(108, ref lastStateButtonStart);
        buttonSelectDown = CheckButtonDown(109, ref lastStateButtonSelect);
        buttonSelectUp = CheckButtonUp(109, ref lastStateButtonSelect);
    }

    private bool CheckButtonDown(int keyCode, ref int lastState)
    {
        int currentState = controller.getKeyCode(keyCode);
        bool result = currentState == 0 && currentState != lastState;
        lastState = currentState;
        return result;
    }

    private bool CheckButtonUp(int keyCode, ref int lastState)
    {
        int currentState = controller.getKeyCode(keyCode);
        bool result = currentState == 1 && currentState != lastState;
        lastState = currentState;
        return result;
    }
}