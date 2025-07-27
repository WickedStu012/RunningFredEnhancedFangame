using UnityEngine;

public class JetpackPowerBar : MonoBehaviour
{
	public GUI3DProgressBar JetpackProgressBar;

	private CharProps charProps;

	private bool listenerAdded;

	private void OnEnable()
	{
		if (charProps == null && CharHelper.GetProps() != null)
		{
			charProps = CharHelper.GetProps();
			JetpackProgressBar.Progress = (int)Mathf.Ceil(charProps.JetPackFuelLeft / charProps.MaxJetPackFuel * 100f);
			GameEventDispatcher.AddListener("CharChangeState", OnStateChange);
			listenerAdded = true;
			base.enabled = false;
		}
	}

	private void OnDisable()
	{
		if (listenerAdded)
		{
			GameEventDispatcher.RemoveListener("CharChangeState", OnStateChange);
		}
	}

	private void FixedUpdate()
	{
		if (charProps == null)
		{
			if (CharHelper.GetProps() != null)
			{
				charProps = CharHelper.GetProps();
				base.enabled = false;
				GameEventDispatcher.AddListener("CharChangeState", OnStateChange);
			}
		}
		else
		{
			JetpackProgressBar.Progress = (int)Mathf.Ceil(charProps.JetPackFuelLeft / charProps.MaxJetPackFuel * 100f);
		}
	}

	private void OnStateChange(object sender, GameEvent e)
	{
		CharChangeState charChangeState = (CharChangeState)e;
		if (charChangeState != null)
		{
			if (charChangeState.CurrentState.GetState() == ActionCode.JETPACK_SPRINT || charChangeState.CurrentState.GetState() == ActionCode.JETPACK)
			{
				base.enabled = true;
			}
			else
			{
				base.enabled = false;
			}
		}
		else
		{
			base.enabled = false;
		}
	}
}
