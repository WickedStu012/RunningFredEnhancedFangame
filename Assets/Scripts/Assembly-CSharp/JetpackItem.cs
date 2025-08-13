using UnityEngine;

public class JetpackItem : MonoBehaviour
{
    public static JetpackItem Instance;

    public GameObject Shine;

    public GameObject jetpackPickup;

    private bool disappear;

    private float accumTime;

    private bool picked;

    private void Start()
    {
        if (!CharHelper.GetProps().HasJetpack)
        {
            picked = false;
            disappear = false;
            if (Shine != null)
            {
                Shine.SetActive(false);
            }
        }
        else
        {
            base.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Instance = this;
        if (picked)
        {
            base.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (disappear)
        {
            accumTime += Time.deltaTime;
            if (accumTime > 0.1f)
            {
                disappear = false;
                base.gameObject.SetActive(false);
            }
        }
        else if (!picked)
        {
            jetpackPickup.transform.Rotate(Vector3.forward, Time.deltaTime * 50f);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (!picked && CharHelper.IsColliderFromPlayer(c))
        {
            CharHelper.GetProps().HasJetpack = true;
            CharHelper.GetCharStateMachine().ShowJetpack();
            SoundManager.PlaySound(base.transform.position, 73);
            if (Shine != null)
            {
                Shine.SetActive(true);
            }
            disappear = true;
            picked = true;
        }
    }
}
