using System;

public class OnSale
{
	public DateTime FromDate;

	public DateTime ToDate;

	public float Discount;

	public int ItemId;

	public int RealDiscount;

	public OnSale()
	{
	}

	public OnSale(DateTime fromDate, DateTime to, float discount, int itemId)
	{
		FromDate = fromDate;
		ToDate = to;
		Discount = (100f - discount) / 100f;
		ItemId = itemId;
		RealDiscount = (int)discount;
	}

	public OnSale(DateTime fromDate, DateTime to, float discount)
	{
		FromDate = fromDate;
		ToDate = to;
		Discount = (100f - discount) / 100f;
		ItemId = -1;
		RealDiscount = (int)discount;
	}

	public OnSale(DateTime fromDate, float discount)
	{
		FromDate = fromDate;
		ToDate = fromDate.AddDays(1.0);
		Discount = (100f - discount) / 100f;
		ItemId = -1;
		RealDiscount = (int)discount;
	}

	public OnSale(DateTime fromDate, float discount, int itemId)
	{
		FromDate = fromDate;
		ToDate = fromDate.AddDays(1.0);
		Discount = (100f - discount) / 100f;
		ItemId = itemId;
		RealDiscount = (int)discount;
	}
}
