using UnityEngine;

public class FredCameraManager : MonoBehaviour
{
    public FredCamera fredCamera;
    public AlphaFredCamera alphaFredCamera;

    public enum Mode
    {
        NORMAL = 0,
        DIVE = 1,
        GLIDE = 2,
        CLIMB = 3
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fredCamera.enabled = false;
        alphaFredCamera.enabled = true;
    }

    public void SwitchMode(Mode m)
    {
        if (alphaFredCamera.enabled)
        {
            switch(m)
            { 
                case Mode.NORMAL:
                    {
                        alphaFredCamera.SwitchMode(AlphaFredCamera.Mode.NORMAL);
                        break;
                    }
                case Mode.DIVE:
                    {
                        alphaFredCamera.SwitchMode(AlphaFredCamera.Mode.DIVE);
                        break;
                    }
                case Mode.GLIDE:
                    {
                        alphaFredCamera.SwitchMode(AlphaFredCamera.Mode.GLIDE);
                        break;
                    }
                case Mode.CLIMB:
                    {
                        alphaFredCamera.SwitchMode(AlphaFredCamera.Mode.CLIMB);
                        break;
                    }
            }
        }
        else
        {
            switch (m)
            {
                case Mode.NORMAL:
                    {
                        fredCamera.SwitchMode(FredCamera.Mode.NORMAL);
                        break;
                    }
                case Mode.DIVE:
                    {
                        fredCamera.SwitchMode(FredCamera.Mode.DIVE);
                        break;
                    }
                case Mode.GLIDE:
                    {
                        fredCamera.SwitchMode(FredCamera.Mode.GLIDE);
                        break;
                    }
                case Mode.CLIMB:
                    {
                        fredCamera.SwitchMode(FredCamera.Mode.CLIMB);
                        break;
                    }
            }
        }
    }
}
