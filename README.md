# HandyUtilities, .Net C# Xsv data processing library 
This is CSV, TSV and any character separated value data processing library.

## Reading XSV data
Can read from TextReader. And accesses by indexer.

```cs
[TestMethod]
public void ReadCsvTest_indexer()
{
    string data = "No,ClassOfShip,Name,TaxIncludedPrice,Price,Maker" + Environment.NewLine
        + "110,BattleShip,î‰âbÅ@Hiei,2625,\"2,500\",Çg" + Environment.NewLine
        + "119,AerialBattleship,à…ê®Å@Ise,3360,\"3,200\",Çg" + Environment.NewLine
        + "216,AircraftCarrier,êêñPÅ@Zuihou,2100,\"2,000\",Çg" + Environment.NewLine
        + "320,LightCruiser ,ñºéÊÅ@Natori,1365,\"1,300\",Çs" + Environment.NewLine
        + "334,HeavyCruiser ,ìﬂíqÅ@Nachi,1890,\"1,800\",Çg" + Environment.NewLine
        + "429,Destroyer,ç˜Å@Sakura,630,600,Çs";

    var expected = new[]{ 
        new { No = 110, ClassOfShip = "BattleShip", Name = "î‰âbÅ@Hiei", TaxIncludedPrice = 2625, Price = 2500, Maker = Maker.Çg },
        new { No = 119, ClassOfShip = "AerialBattleship", Name = "à…ê®Å@Ise", TaxIncludedPrice = 3360, Price = 3200, Maker = Maker.Çg },
        new { No = 216, ClassOfShip = "AircraftCarrier", Name = "êêñPÅ@Zuihou", TaxIncludedPrice = 2100, Price = 2000, Maker = Maker.Çg},
        new { No = 320, ClassOfShip = "LightCruiser", Name = "ñºéÊÅ@Natori", TaxIncludedPrice = 1365, Price = 1300, Maker = Maker.Çs },
        new { No = 334, ClassOfShip = "HeavyCruiser", Name = "ìﬂíqÅ@ìﬂíq", TaxIncludedPrice = 1890, Price = 1800, Maker = Maker.Çg },
        new { No = 429, ClassOfShip = "Destroyer", Name = "ç˜Å@Sakura", TaxIncludedPrice = 630, Price = 600, Maker = Maker.Çs }
    };

    var target = new XsvData(new[] { "," });
    using (var reader = new System.IO.StringReader(data))
    {
        target.Read(reader, headerExists: true);
    }

	//Enumerates row data, and accesses by indexer.
    foreach (XsvDataRow x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
    {
        Assert.AreEqual(x.exp.No, x.row["No"].AsInt32());
        Assert.AreEqual(x.exp.ClassOfShip, x.row["ClassOfShip"].AsString());
        Assert.AreEqual(x.exp.Name, x.row["Name"].AsString());
        Assert.AreEqual(x.exp.TaxIncludedPrice, x.row["TaxIncludedPrice"].AsInt32());
        Assert.AreEqual(x.exp.Price, x.row["Price"].AsInt32());
        Assert.AreEqual(x.exp.Maker, x.row["Maker"].AsEnum<Maker>());
    }
}
```

XsvField structure has many type conversion methods.

```cs
public class XsvDataRow : IDictionary<string, XsvField>
{
	public XsvField this[string key] {get; set;}
	
	//Set values to specific-type fields from source data of XsvField.
	protected virtual void AttachFields();
	
	//Update source data of XsvField.
	protected virtual void UpdateFields();
	
	//...
}

public struct XsvField
{
	public int AsInt32();
    public int AsInt32(int defaultValue);
    public int? AsNullableInt32();
    public int AsInt32(IFormatProvider formatProvider);
    public int AsInt32(NumberStyles numberStyles);
    public int AsInt32(NumberStyles numberStyles, IFormatProvider formatProvider);
    public int? AsNullableInt32(NumberStyles numberStyles);
    public int? AsNullableInt32(NumberStyles numberStyles, IFormatProvider formatProvider);
    public int AsInt32(NumberStyles numberStyles, int defaultValue);
    public int AsInt32(NumberStyles numberStyles, IFormatProvider formatProvider, int defaultValue);

    //AsInt64(), AsDouble(), AsDecimal(), AsDateTime(), AsEnum() ...and more.
}
```

## Mapping of fields

### Inherit XsvDataRow class 
Inherit XsvDataRow class and override methods, AttachFields() and UpdateFields().

```cs
class ShipModelDataRow : XsvDataRow
{
    public int No { set; get; }
    public string ClassOfShip { set; get; }
    public string Name { set; get; }
    public int TaxIncludedPrice { set; get; }
    public int Price { set; get; }
    public Maker Maker{ set; get; }

    public ShipModelDataRow()
        : base()
    { }

	//Set values to specific-type fields from source data of XsvField.
    protected override void AttachFields()
    {
        this.No = this["No"].AsInt32(defaultValue:0 );
        this.ClassOfShip = this["ClassOfShip"].AsString( defaultValue:"Unknown" );
        this.Name = this["Name"].AsString( defaultValue:"Unnamed" );
        
		var priceFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        priceFormat.CurrencySymbol = "Åè";
		this.TaxIncludedPrice = this["TaxIncludedPrice"].AsInt32( NumberStyles.Currency, priceFormat, defaultValue:-1 );
		
		numFormat.CurrencySymbol = "yen";
		this.Price = this["Price"].AsInt32( NumberStyles.Currency, priceFormat, defaultValue:-1 );
        
		this.Maker= this["Maker].AsEnum<Maker>( defaultValue:Maker.UNKNOWN );
    }

	//Update source data of XsvField.
    protected override void UpdateFields()
    {
		this["No"] = new XsvData(this.No);
        this["ClassOfShip"] = new XsvData(this.ClassOfShip);
        this["Name"] = new XsvData(this.Name);
        this["TaxIncludedPrice"] = new XsvData(this.TaxIncludedPrice,"0Åè");
        this["Price"] = new XsvData(this.Price,"0,000 yen");
        this["Maker] = new XsvData(this.Maker);
    }
}

enum Maker
{
    UNKNOWN,
    Ç`,
    Çs,
    Çg,
    Çr
}

[TestMethod]
public void ReadCsvTest_inhelitedXsvDataRow()
{
	string data = "No,ClassOfShip,Name,TaxIncludedPrice,Price,Maker" + Environment.NewLine
        + "110,BattleShip,î‰âbÅ@Hiei,2625Åè,\"2,500 yen\",Çg" + Environment.NewLine
        + "119,AerialBattleship,à…ê®Å@Ise,3360Åè,\"3,200 yen\",Çg" + Environment.NewLine
        + "216,AircraftCarrier,êêñPÅ@Zuihou,2100Åè,\"2,000 yen\",Çg" + Environment.NewLine
        + "320,LightCruiser ,ñºéÊÅ@Natori,1365Åè,\"1,300 yen\",Çs" + Environment.NewLine
        + "334,HeavyCruiser ,ìﬂíqÅ@Nachi,1890Åè,\"1,800 yen\",Çg" + Environment.NewLine
        + "xxx,,,xxx,xxx,xxx";

    var expected = new[]{ 
        new { No = 110, ClassOfShip = "BattleShip", Name = "î‰âbÅ@Hiei", TaxIncludedPrice = 2625, Price = 2500, Maker = Maker.Çg },
        new { No = 119, ClassOfShip = "AerialBattleship", Name = "à…ê®Å@Ise", TaxIncludedPrice = 3360, Price = 3200, Maker = Maker.Çg },
        new { No = 216, ClassOfShip = "AircraftCarrier", Name = "êêñPÅ@Zuihou", TaxIncludedPrice = 2100, Price = 2000, Maker = Maker.Çg},
        new { No = 320, ClassOfShip = "LightCruiser", Name = "ñºéÊÅ@Natori", TaxIncludedPrice = 1365, Price = 1300, Maker = Maker.Çs },
        new { No = 334, ClassOfShip = "HeavyCruiser", Name = "ìﬂíqÅ@ìﬂíq", TaxIncludedPrice = 1890, Price = 1800, Maker = Maker.Çg },
        new { No = 0, ClassOfShip = "Unknown", Name = "Unnamed", TaxIncludedPrice = -1, Price = -1, Maker= Maker.UNKNOWN }
    };
    
    var target = new XsvData<ShipModelDataRow>(new[] { "," });
    using (var reader = new System.IO.StringReader(data))
    {
        target.Read(reader, true);
    }

    foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
    {
        Assert.AreEqual(x.exp.No, x.row.No);
        Assert.AreEqual(x.exp.ClassOfShip, x.row.ClassOfShip);
        Assert.AreEqual(x.exp.Name, x.row.Name);
        Assert.AreEqual(x.exp.TaxIncludedPrice, x.row.TaxIncludedPrice);
        Assert.AreEqual(x.exp.Price, x.row.Price);
        Assert.AreEqual(x.exp.Maker x.row.Maker;
    }
}
```

### Using TypedXsvData<T> class

#### Auto-binding
Supports auto-binding. Bind to public properties of any class.
Sets the value of "default(T)", if fails convert a field.

```cs
class ShipModel
{
    public int No { set; get; }
    public string ClassOfSip { set; get; }
    public string Name { set; get; }
    public int TaxIncludedPrice { set; get; }
    public int Price { set; get; }
    public Maker Maker { set; get; }
}

[TestMethod]
public void ReadXsvTest_autoBinding()
{
    var data =
          "No,ClassOfShip,Name,TaxIncludedPrice,Price,Maker" + Environment.NewLine
        + "103,BattleShip,éRèÈÅ@Yamashiro,1785,\"1,700\",Ç`" + Environment.NewLine
        + "215,AircraftCarrier,êMîZÅ@Shinano,2940,\"2,800\",Çs" + Environment.NewLine
        + "442,Destroyer,ózâäÅ@Kagero,1050,\"1,000\",Ç`" + Environment.NewLine;

    var expected = new[]{ 
        new { No = 103, ClassOfShip = "BattleShip", Name = "éRèÈÅ@Yamashiro", TaxIncludedPrice = 1785, Price = 1700, Maker= Maker.Ç` },
        new { No = 215, ClassOfShip = "AircraftCarrier", Name = "êMîZÅ@Shinano", TaxIncludedPrice = 2940, Price = 2800, Maker= Maker.Çs },
        new { No = 442, ClassOfShip = "Destroyer", Name = "ózâäÅ@Kagero", TaxIncludedPrice = 1050, Price = 1000, Maker= Maker.Ç` }
    };

    var target = new TypedXsvData<ShipModel>(new[] { "," }, isAutoBinding: true);
    using (var reader = new System.IO.StringReader(data))
    {
        target.Read(reader, true);
    }

    foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
    {
        Assert.AreEqual(x.exp.No, x.row.Fields.No);
        Assert.AreEqual(x.exp.ClassOfShip, x.row.Fields.ClassOfShip);
        Assert.AreEqual(x.exp.Name, x.row.Fields.Name);
        Assert.AreEqual(x.exp.TaxIncludedPrice, x.row.Fields.TaxIncludedPrice);
        Assert.AreEqual(x.exp.Price, x.row.Fields.Price);
        Assert.AreEqual(x.exp.Maker x.row.Fields.Maker);
    }
}
```

#### Customize of a field-mapping using EventHandler

```cs
class ShipModelB:ShipModel
{
    public double TaxIncludedPrice_KY { set; get; }
}

[TestMethod]
public void ReadXsvTest_eventHandler()
{
    var data =
          "No,ClassOfShip,Name,TaxIncludedPrice,Price,Maker" + Environment.NewLine
        + "103,BattleShip,éRèÈÅ@Yamashiro,1.785KÅè,\"1,700Åè\",Ç`" + Environment.NewLine
        + "215,AircraftCarrier,êMîZÅ@Shinano,2.940KÅè,\"2,800Åè\",Çs" + Environment.NewLine
        + "442,Destroyer,ózâäÅ@Kagero,1.050KÅè,\"1,000Åè\",Ç`" + Environment.NewLine;

    var expected = new[]{ 
        new { No = 103, ClassOfShip = "BattleShip", Name = "éRèÈÅ@Yamashiro", TaxIncludedPrice_KY = 1.785, Price = 1700, Maker= Maker.Ç` },
        new { No = 215, ClassOfShip = "AircraftCarrier", Name = "êMîZÅ@Shinano", TaxIncludedPrice_KY = 2.940, Price = 2800, Maker= Maker.Çs },
        new { No = 442, ClassOfShip = "Destroyer", Name = "ózâäÅ@Kagero", TaxIncludedPrice_KY = 1.050, Price = 1000, Maker= Maker.Ç` }
    };

    var target = new TypedXsvData<ShipModelC>(new[] { "," }, isAutoBinding: true);
    target.Attached += (o, e) =>
    {
        var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        numFormat.CurrencySymbol = "Åè";
        e.Fields.Price = e.Row["Price"].AsInt32(NumberStyles.Currency, numFormat);

        numFormat.CurrencySymbol = "KÅè";
        e.Fields.TaxIncludedPrice_KY = e.Row["TaxIncludedPrice"].AsDouble(NumberStyles.Currency, numFormat);

    };
    target.Updated += (o, e) =>
    {
        e.Row["TaxIncludedPrice"] = new XsvField(e.Fields.TaxIncludedPrice_KY, "0.000KÅè");
        e.Row["Price"] = new XsvField(e.Fields.Price, "0,000Åè");
    };

    using (var reader = new System.IO.StringReader(data))
    {
        target.Read(reader, true);
    }

    foreach (var x in target.Rows.Zip(expected, (a, b) => new { row = a, exp = b }))
    {
        Assert.AreEqual(x.exp.No, x.row.Fields.No);
        Assert.AreEqual(x.exp.ClassOfShip, x.row.Fields.ClassOfShip);
        Assert.AreEqual(x.exp.Name, x.row.Fields.Name);
        Assert.AreEqual(x.exp.TaxIncludedPrice_KY, x.row.Fields.TaxIncludedPrice_KY);
        Assert.AreEqual(x.exp.Price, x.row.Fields.Price);
        Assert.AreEqual(x.exp.Maker x.row.Fields.Maker);
    }

    var sb = new StringBuilder();
    using (var writer = new StringWriter(sb))
    {
        target.Write(writer, headerExists: true, updateFields: true, delimiter: ",");
        Assert.AreEqual(data, sb.ToString());
    }
}
```