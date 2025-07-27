using UnityEngine;

public interface IGUI3DObject
{
	GUI3DPanel GetPanel();

	void SetPanel(GUI3DPanel panel);

	Vector3 RealPosition();

	Vector3 Size();

	Vector3 GetObjectSize();

	void CleanTextures();

	void ShowTextures();
}
