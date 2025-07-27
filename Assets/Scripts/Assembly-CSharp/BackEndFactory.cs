public class BackEndFactory
{
	public static iBackEnd Create(BackEndType bet)
	{
		switch (bet)
		{
		case BackEndType.GOOGLE_APP_ENGINE:
			return new GAEBackend();
		case BackEndType.ICLOUD:
			return new ICloudBackEnd();
		case BackEndType.WHISPERSYNC:
			return new WhispersyncBackEnd();
		case BackEndType.NONE:
			return null;
		default:
			return null;
		}
	}
}
