public class AvatarItem : PageSliderItem
{
	public GUI3DText Name;

	public GUI3DObject Icon;

	public override void Create(ItemInfo item)
	{
		base.Create(item);
		if (Name != null)
		{
			Name.SetDynamicText(Item.Name);
		}
		if (Item.Picture != string.Empty)
		{
			if (Icon.GetComponent<UnityEngine.Renderer>() == null)
			{
				Icon.TextureName = Item.Picture;
				Icon.CreateOwnMesh = true;
				Icon.CreateMesh();
			}
			else
			{
				Icon.RefreshMaterial(Item.Picture);
			}
		}
	}
}
