using Newtonsoft.Json;

using (StreamReader r = new StreamReader("input.json"))
{
	var random = new Random();
	string json = r.ReadToEnd();
	MarketValueResponseData data = JsonConvert.DeserializeObject<MarketValueResponseData>(json);
	foreach(var itemData in data.Data)
    {
		if (itemData.NetInvested.HasValue && itemData.NetInvested.Value != 0)
        {
			itemData.NetInvested = itemData.NetInvested * (random.NextDouble() * 0.1d) + itemData.NetInvested * 0.9d;
		}

		if (itemData.TotalAssets.HasValue && itemData.TotalAssets.Value != 0)
		{
			itemData.TotalAssets = itemData.TotalAssets * (random.NextDouble() * 0.1d) + itemData.TotalAssets * 0.9d;
		}
	}
	var newJson = JsonConvert.SerializeObject(data);
	File.WriteAllText("output.json", newJson);
}

public class MarketValueResponseData
{
	public string[] Accounts;

	public string[] EmptyAccounts;

	public MarketValueDatesData Dates;

	public MarketValueItemData[] Data;
}

public class MarketValueDatesData
{
	public DateTimeOffset RunDate { get; internal set; }

	public DateTimeOffset AsOfDate { get; internal set; }
}

public class MarketValueItemData
{
	public string AccountNumber;

	public DateTimeOffset Date;

	public double? TotalAssets;

	public double? TotalLiabilities;

	public double? NetInvested;

	public bool IsMonthEnd;

	public bool IsYearEnd;

	public string Lob;
}