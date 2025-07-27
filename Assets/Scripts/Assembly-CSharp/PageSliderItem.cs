public class PageSliderItem : GUI3DObject
{
	public ItemInfo Item;

	public virtual void Create(ItemInfo item)
	{
		Item = item;
		Tag = item.Tag;
	}
}
