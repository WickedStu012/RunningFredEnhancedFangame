public class GUI3DOption : GUI3DObject
{
	public delegate void OnOptionChangedEvent(GUI3DOnOptionChangedEvent evt);

	private GUI3DCheckbox[] checkboxes;

	private GUI3DCheckbox activeCheckbox;

	private GUI3DOnOptionChangedEvent onOptionChangedEvent = new GUI3DOnOptionChangedEvent();

	public event OnOptionChangedEvent OptionChangedEvent;

	protected override void Awake()
	{
		base.Awake();
		checkboxes = GetComponentsInChildren<GUI3DCheckbox>();
		if (checkboxes == null)
		{
			return;
		}
		GUI3DCheckbox[] array = checkboxes;
		foreach (GUI3DCheckbox gUI3DCheckbox in array)
		{
			if (gUI3DCheckbox.Checked && activeCheckbox == null)
			{
				activeCheckbox = gUI3DCheckbox;
				OnOptionChanged(activeCheckbox);
			}
			else
			{
				gUI3DCheckbox.Checked = false;
			}
			gUI3DCheckbox.CanUncheck = false;
			gUI3DCheckbox.CheckboxChangeEvent += OnCheckboxChange;
		}
	}

	private void OnCheckboxChange(GUI3DEvent evt)
	{
		GUI3DOnCheckboxChangeEvent gUI3DOnCheckboxChangeEvent = (GUI3DOnCheckboxChangeEvent)evt;
		OnOptionChanged((GUI3DCheckbox)gUI3DOnCheckboxChangeEvent.Target);
		if (activeCheckbox != gUI3DOnCheckboxChangeEvent.Target)
		{
			activeCheckbox.Checked = false;
			activeCheckbox = (GUI3DCheckbox)gUI3DOnCheckboxChangeEvent.Target;
		}
	}

	protected virtual void OnOptionChanged(GUI3DCheckbox current)
	{
		if (this.OptionChangedEvent != null)
		{
			onOptionChangedEvent.Target = this;
			onOptionChangedEvent.ActiveCheckbox = current;
			this.OptionChangedEvent(onOptionChangedEvent);
		}
	}
}
