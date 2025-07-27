using UnityEngine;

public class GUI3DSlider : GUI3DProgressBar, IGUI3DInteractiveObject
{
	public delegate void OnClickEvent(GUI3DOnClickEvent evt);

	public delegate void OnPressEvent(GUI3DOnPressEvent evt);

	public delegate void OnReleaseEvent(GUI3DOnReleaseEvent evt);

	public delegate void OnRollOverEvent(GUI3DOnRollOverEvent evt);

	public delegate void OnRollOutEvent(GUI3DOnRollOutEvent evt);

	public delegate void OnDragEvent(GUI3DOnDragEvent evt);

	public delegate void OnSliderValueChangedEvent(GUI3DOnSliderValueChangedEvent evt);

	public bool CheckEvents = true;

	private GUI3DOnClickEvent onClickEvent = new GUI3DOnClickEvent();

	private GUI3DOnPressEvent onPressEvent = new GUI3DOnPressEvent();

	private GUI3DOnReleaseEvent onReleaseEvent = new GUI3DOnReleaseEvent();

	private GUI3DOnRollOverEvent onRollOverEvent = new GUI3DOnRollOverEvent();

	private GUI3DOnRollOutEvent onRollOutEvent = new GUI3DOnRollOutEvent();

	private GUI3DOnDragEvent onDragEvent = new GUI3DOnDragEvent();

	private GUI3DOnSliderValueChangedEvent onSliderValueChangedEvent = new GUI3DOnSliderValueChangedEvent();

	private bool isRolledOver;

	public event OnClickEvent ClickEvent;

	public event OnPressEvent PressEvent;

	public event OnReleaseEvent ReleaseEvent;

	public event OnRollOverEvent RollOverEvent;

	public event OnRollOutEvent RollOutEvent;

	public event OnDragEvent DragEvent;

	public event OnDragEvent CancelDragEvent;

	public event OnSliderValueChangedEvent SliderValueChangedEvent;

	public bool IsRolledOver()
	{
		return isRolledOver;
	}

	public bool CheckEventsEnabled()
	{
		return CheckEvents;
	}

	public bool IsDraggeable()
	{
		return false;
	}

	public virtual void OnMouseOver()
	{
		isRolledOver = true;
		if (this.RollOverEvent != null)
		{
			onRollOverEvent.Target = this;
			this.RollOverEvent(onRollOverEvent);
		}
	}

	public virtual void OnMouseOut()
	{
		isRolledOver = false;
		if (this.RollOutEvent != null)
		{
			onRollOutEvent.Target = this;
			this.RollOutEvent(onRollOutEvent);
		}
	}

	public virtual void OnClick(Vector3 position)
	{
		if (this.ClickEvent != null)
		{
			onClickEvent.Target = this;
			onClickEvent.Position = position;
			this.ClickEvent(onClickEvent);
		}
	}

	public virtual void OnRelease()
	{
		if (this.ReleaseEvent != null)
		{
			onReleaseEvent.Target = this;
			this.ReleaseEvent(onReleaseEvent);
		}
	}

	public virtual void OnDrag(Vector3 relativePosition)
	{
		if (this.DragEvent != null)
		{
			onDragEvent.Target = this;
			onDragEvent.RelativePosition = relativePosition;
			onDragEvent.Cancelled = false;
			this.DragEvent(onDragEvent);
		}
	}

	public virtual void OnCancelDrag()
	{
		if (this.CancelDragEvent != null)
		{
			onDragEvent.Target = this;
			onDragEvent.Cancelled = true;
			this.CancelDragEvent(onDragEvent);
		}
	}

	public virtual void OnPress(Vector3 position)
	{
		float num = Size().x / (float)SegmentsCount / 2f;
		float num2 = position.x - Position().x + Size().x / 2f;
		num2 = (float)Mathf.RoundToInt(num2 / num) * num;
		base.Progress = Mathf.Max(0, Mathf.Min(Mathf.FloorToInt(num2 * 100f / Size().x), 100));
		if (this.PressEvent != null)
		{
			onPressEvent.Target = this;
			onPressEvent.Position = position;
			this.PressEvent(onPressEvent);
		}
		if (this.SliderValueChangedEvent != null)
		{
			onSliderValueChangedEvent.Target = this;
			onSliderValueChangedEvent.Value = base.Progress;
			this.SliderValueChangedEvent(onSliderValueChangedEvent);
		}
	}
}
