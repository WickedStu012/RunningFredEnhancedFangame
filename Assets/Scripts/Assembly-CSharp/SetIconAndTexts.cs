using UnityEngine;

public class SetIconAndTexts : MonoBehaviour
{
	public GUI3DObject iconGUI3D;

	public GUI3DButton button;

	private void OnEnable()
	{
		if (button != null)
		{
			button.ClickEvent += OnClick;
		}
		if ((bool)AdManager.Instance && AdManager.Instance.AdMaterial != null && AdManager.Instance.AdMaterial.mainTexture != null)
		{
			iconGUI3D.TextureAtlasSize = new Vector2(AdManager.Instance.AdMaterial.mainTexture.width, AdManager.Instance.AdMaterial.mainTexture.height);
			iconGUI3D.ImageMaterial = AdManager.Instance.AdMaterial;
			iconGUI3D.RefreshUV();
			if (iconGUI3D.GetComponent<Renderer>() == null)
			{
				iconGUI3D.CreateMesh();
			}
			iconGUI3D.GetComponent<Renderer>().material = AdManager.Instance.AdMaterial;
			iconGUI3D.GetComponent<Renderer>().enabled = true;
		}
	}

	private void OnDisable()
	{
		if (button != null)
		{
			button.ClickEvent -= OnClick;
		}
	}

	private void OnClick(GUI3DOnClickEvent evt)
	{
		AdManager.Instance.AdWasTapped();
		AdManager.Instance.RemoveTextureFromMaterial();
		Application.OpenURL(AdManager.Instance.GetCurrentAd().LinkURL);
	}
}
