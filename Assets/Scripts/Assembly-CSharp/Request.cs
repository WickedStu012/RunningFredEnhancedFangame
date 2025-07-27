public class Request
{
	public string name;

	public string description;

	public string price;

	public string currencyCode;

	public string sellerData;

	public Request(string name, string desc, string price, string op)
	{
		this.name = name;
		description = desc;
		this.price = price;
		currencyCode = "USD";
		sellerData = op;
	}
}
