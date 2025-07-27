public class BeLordFactory
{
	public static BeLordBase GetInstance(BeLordBackend be)
	{
		switch (be)
		{
		case BeLordBackend.GAMECENTER:
			return BeLordGC.GetInstance();
		case BeLordBackend.GAMECIRCLE:
			return BeLordGameCircle.GetInstance();
		default:
			return null;
		}
	}
}
