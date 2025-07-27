public class GUI3DTabControl : GUI3DObject
{
	public delegate void OnTabControlChangeEvent(GUI3DOnTabControlChangeEvent evt);

	private GUI3DTab[] tabs;

	private GUI3DTab activeTab;

	private GUI3DOnTabControlChangeEvent onTabChangeEvent = new GUI3DOnTabControlChangeEvent();

	public event OnTabControlChangeEvent TabControlChangeEvent;

	protected override void Awake()
	{
		base.Awake();
		activeTab = null;
	}

	private void OnEnable()
	{
		if (tabs == null)
		{
			tabs = GetComponentsInChildren<GUI3DTab>();
		}
		if (tabs != null)
		{
			GUI3DTab[] array = tabs;
			foreach (GUI3DTab gUI3DTab in array)
			{
				gUI3DTab.TabChangeEvent += OnTabChange;
			}
		}
	}

	private void OnDisable()
	{
		activeTab = null;
		if (tabs != null)
		{
			GUI3DTab[] array = tabs;
			foreach (GUI3DTab gUI3DTab in array)
			{
				gUI3DTab.TabChangeEvent -= OnTabChange;
			}
		}
	}

	private void Update()
	{
		if (!(activeTab == null) || tabs == null)
		{
			return;
		}
		GUI3DTab[] array = tabs;
		foreach (GUI3DTab gUI3DTab in array)
		{
			if (gUI3DTab.ActiveStatusAtStart && activeTab == null)
			{
				gUI3DTab.TabActive = true;
				OnTabControlChange(gUI3DTab);
			}
			else
			{
				gUI3DTab.TabActive = false;
			}
		}
	}

	public void SwitchToTab(string tabname)
	{
		if (tabs == null)
		{
			return;
		}
		GUI3DTab[] array = tabs;
		foreach (GUI3DTab gUI3DTab in array)
		{
			if (gUI3DTab.name == tabname)
			{
				gUI3DTab.TabActive = true;
				OnTabControlChange(gUI3DTab);
			}
			else
			{
				gUI3DTab.TabActive = false;
			}
		}
	}

	private void OnTabChange(GUI3DOnTabChangeEvent e)
	{
		GUI3DTab gUI3DTab = (GUI3DTab)e.Target;
		OnTabControlChange(gUI3DTab);
		if (gUI3DTab.TabActive && activeTab != gUI3DTab)
		{
			if (activeTab != null)
			{
				activeTab.TabActive = false;
			}
			activeTab = gUI3DTab;
		}
	}

	protected virtual void OnTabControlChange(GUI3DTab current)
	{
		if (this.TabControlChangeEvent != null)
		{
			onTabChangeEvent.Target = this;
			onTabChangeEvent.ActiveTab = current;
			this.TabControlChangeEvent(onTabChangeEvent);
		}
	}
}
