using UnityEngine;

public interface IGUI3DPanel
{
	void OnMouseIn(Vector3 position);

	void OnMouseRelease();

	void OnMouseReleaseOutside();

	void OnMouseOut();
}
