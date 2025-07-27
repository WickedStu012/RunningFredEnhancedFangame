public class EvnJetpackSprint : IEvent
{
	private CharProps props;

	public EvnJetpackSprint()
	{
		code = EventCode.EVN_JETPACK_SPRINT;
	}

	public override bool Check()
	{
		if (props == null)
		{
			props = CharHelper.GetProps();
		}
		return props.HasJetpack && props.JetPackFuelLeft > 0f && InputManager.GetSuperSprint();
	}
}
