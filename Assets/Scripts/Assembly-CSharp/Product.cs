public class Product
{
	public string iss;

	public string aud;

	public string typ;

	public int exp;

	public int iat;

	public Request request;

	public Product(string name, string desc, string price, string op)
	{
		iss = "03443234414327739318";
		aud = "Google";
		typ = "google/payments/inapp/item/v1";
		int pHPTime = DateUtil.GetPHPTime();
		exp = pHPTime + 3600;
		iat = pHPTime;
		request = new Request(name, desc, price, op);
	}
}
