using UnityEngine;

public interface IGUI3DInteractiveObject
{
	bool CheckEventsEnabled();

	bool IsDraggeable();

	void OnMouseOver();

	void OnMouseOut();

	void OnClick(Vector3 position);

	void OnDrag(Vector3 relativePosition);

	void OnCancelDrag();

	void OnPress(Vector3 position);

	void OnRelease();

	Vector3 RealPosition();

	Vector3 Size();

	void CleanTextures();

	void ShowTextures();

	bool IsRolledOver();
}
