# HandyUtilities, .Net C# Xsv data processing library 
This is CSV, TSV and any character separated value data processing library.

## Installation

NuGet gallery: https://www.nuget.org/packages/HandyUtilities.Xsv/

    PM> Install-Package HandyUtilities.Xsv

## Reading XSV data

### Can read a XSV Data with the XsvReader.

```cs
public class XmlReader
{
    //**************************************************************************
    // Synchronous methods (Supported in .Net3.5, 4.0, 4.5, 4.5.1)
    //**************************************************************************
    //Reads a line of characters from the internal text reader.
    public string ReadLine();

    //Reads a row of XSV data and returns the data as sequence of string.
    public IEnumerable<string> ReadXsvLine(ICollection<string> delimiters);

    //Reads all rows of XSV data from the current position to end of data.
    public IEnumerable<string[]> ReadXsvToEnd(ICollection<string> delimiters);
    
    //**************************************************************************
    // Aynchronous methods (Supported in .Net 4.0, 4.5, 4.5.1)
    //**************************************************************************
    //Reads a line of characters from the internal text reader.
    public Task<string> ReadLineAsync();

    //Reads a row of XSV data and returns the data as array of string.
    public Task<string[]> ReadXsvLineAsync(ICollection<string> delimiters);

    //Reads all rows of XSV data from the current position to end of data.
    public Task<IList<<string[]>> ReadXsvToEndAsync(ICollection<string> delimiters);
    
    //**************************************************************************
    // Create IObservable<T>, (Using Rx) (Supported in .Net 4.0, 4.5, 4.5.1)
    //**************************************************************************
    public IObservable<string[]> ReadXsvObservable(IObserver<string[]> observer,
        CancellationToken ct, ICollection<string> delimiters);
}
```

### Reads from XsvReader to a XsvData object. And accesses by indexer.

```cs
[TestMethod]
public void ReadCsvTest_indexer()
{
    string data = "No,ClassOfShip,Name,TaxIncludedPrice,Price,Maker" + Environment.NewLine
        + "110,BattleShip,比叡　Hiei,2625,\"2,500\",Ｈ" + Environment.NewLine
        + "119,AerialBattleship,伊勢　Ise,3360,\"3,200\",Ｈ" + Environment.NewLine
        + "216,AircraftCarrier,瑞鳳　Zuihou,2100,\"2,000\",Ｈ" + Environment.NewLine
        + "320,LightCruiser ,名取　Natori,1365,\"1,300\",Ｔ" + Environment.NewLine
        + "334,HeavyCruiser ,那智　Nachi,1890,\"1,800\",Ｈ" + Environment.NewLine
        + "429,Destroyer,桜　Sakura,630,600,Ｔ";

    var expected = new[]{ 
        new { No = 110, ClassOfShip = "BattleShip", Name = "比叡　Hiei", TaxIncludedPrice = 2625, Price = 2500, Maker = Maker.Ｈ },
        new { No = 119, ClassOfShip = "AerialBattleship", Name = "伊勢　Ise", TaxIncludedPrice = 3360, Price = 3200, Maker = Maker.Ｈ },
        new { No = 216, ClassOfShip = "AircraftCarrier", Name = "瑞鳳　Zuihou", TaxIncludedPrice = 2100, Price = 2000, Maker = Maker.Ｈ},
        new { No = 320, ClassOfShip = "LightCruiser", Name = "名取　Natori", TaxIncludedPrice = 1365, Price = 1300, Maker = Maker.Ｔ },
        new { No = 334, ClassOfShip = "HeavyCruiser", Name = "那智　那智", TaxIncludedPrice = 1890, Price = 1800, Maker = Maker.Ｈ },
        new { No = 429, ClassOfShip = "Destroyer", Name = "桜　Sakura", TaxIncludedPrice = 630, Price = 600, Maker = Maker.Ｔ }
    };

    var target = new XsvData(new[] { "," });
    using (var reader = new XsvReader(new System.IO.StringReader(data)))
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

### XsvField structure has many type conversion methods.

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
        priceFormat.CurrencySymbol = "￥";
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
        this["TaxIncludedPrice"] = new XsvData(this.TaxIncludedPrice,"0￥");
        this["Price"] = new XsvData(this.Price,"0,000 yen");
        this["Maker] = new XsvData(this.Maker);
    }
}

enum Maker
{
    UNKNOWN,
    Ａ,
    Ｔ,
    Ｈ,
    Ｓ
}

[TestMethod]
public void ReadCsvTest_inhelitedXsvDataRow()
{
	string data = "No,ClassOfShip,Name,TaxIncludedPrice,Price,Maker" + Environment.NewLine
        + "110,BattleShip,比叡　Hiei,2625￥,\"2,500 yen\",Ｈ" + Environment.NewLine
        + "119,AerialBattleship,伊勢　Ise,3360￥,\"3,200 yen\",Ｈ" + Environment.NewLine
        + "216,AircraftCarrier,瑞鳳　Zuihou,2100￥,\"2,000 yen\",Ｈ" + Environment.NewLine
        + "320,LightCruiser ,名取　Natori,1365￥,\"1,300 yen\",Ｔ" + Environment.NewLine
        + "334,HeavyCruiser ,那智　Nachi,1890￥,\"1,800 yen\",Ｈ" + Environment.NewLine
        + "xxx,,,xxx,xxx,xxx";

    var expected = new[]{ 
        new { No = 110, ClassOfShip = "BattleShip", Name = "比叡　Hiei", TaxIncludedPrice = 2625, Price = 2500, Maker = Maker.Ｈ },
        new { No = 119, ClassOfShip = "AerialBattleship", Name = "伊勢　Ise", TaxIncludedPrice = 3360, Price = 3200, Maker = Maker.Ｈ },
        new { No = 216, ClassOfShip = "AircraftCarrier", Name = "瑞鳳　Zuihou", TaxIncludedPrice = 2100, Price = 2000, Maker = Maker.Ｈ},
        new { No = 320, ClassOfShip = "LightCruiser", Name = "名取　Natori", TaxIncludedPrice = 1365, Price = 1300, Maker = Maker.Ｔ },
        new { No = 334, ClassOfShip = "HeavyCruiser", Name = "那智　那智", TaxIncludedPrice = 1890, Price = 1800, Maker = Maker.Ｈ },
        new { No = 0, ClassOfShip = "Unknown", Name = "Unnamed", TaxIncludedPrice = -1, Price = -1, Maker= Maker.UNKNOWN }
    };
    
    var target = new XsvData<ShipModelDataRow>(new[] { "," });
    using (var reader = new XsvReader(new System.IO.StringReader(data)))
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

### Using TypedXsvData\<T\> class

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
        + "103,BattleShip,山城　Yamashiro,1785,\"1,700\",Ａ" + Environment.NewLine
        + "215,AircraftCarrier,信濃　Shinano,2940,\"2,800\",Ｔ" + Environment.NewLine
        + "442,Destroyer,陽炎　Kagero,1050,\"1,000\",Ａ" + Environment.NewLine;

    var expected = new[]{ 
        new { No = 103, ClassOfShip = "BattleShip", Name = "山城　Yamashiro", TaxIncludedPrice = 1785, Price = 1700, Maker= Maker.Ａ },
        new { No = 215, ClassOfShip = "AircraftCarrier", Name = "信濃　Shinano", TaxIncludedPrice = 2940, Price = 2800, Maker= Maker.Ｔ },
        new { No = 442, ClassOfShip = "Destroyer", Name = "陽炎　Kagero", TaxIncludedPrice = 1050, Price = 1000, Maker= Maker.Ａ }
    };

    var target = new TypedXsvData<ShipModel>(new[] { "," }, isAutoBinding: true);
    using (var reader = new XsvReader(new System.IO.StringReader(data)))
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
        + "103,BattleShip,山城　Yamashiro,1.785K￥,\"1,700￥\",Ａ" + Environment.NewLine
        + "215,AircraftCarrier,信濃　Shinano,2.940K￥,\"2,800￥\",Ｔ" + Environment.NewLine
        + "442,Destroyer,陽炎　Kagero,1.050K￥,\"1,000￥\",Ａ" + Environment.NewLine;

    var expected = new[]{ 
        new { No = 103, ClassOfShip = "BattleShip", Name = "山城　Yamashiro", TaxIncludedPrice_KY = 1.785, Price = 1700, Maker= Maker.Ａ },
        new { No = 215, ClassOfShip = "AircraftCarrier", Name = "信濃　Shinano", TaxIncludedPrice_KY = 2.940, Price = 2800, Maker= Maker.Ｔ },
        new { No = 442, ClassOfShip = "Destroyer", Name = "陽炎　Kagero", TaxIncludedPrice_KY = 1.050, Price = 1000, Maker= Maker.Ａ }
    };

    var target = new TypedXsvData<ShipModelC>(new[] { "," }, isAutoBinding: true);
    target.Attached += (o, e) =>
    {
        var numFormat = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        numFormat.CurrencySymbol = "￥";
        e.Fields.Price = e.Row["Price"].AsInt32(NumberStyles.Currency, numFormat);

        numFormat.CurrencySymbol = "K￥";
        e.Fields.TaxIncludedPrice_KY = e.Row["TaxIncludedPrice"].AsDouble(NumberStyles.Currency, numFormat);

    };
    target.Updated += (o, e) =>
    {
        e.Row["TaxIncludedPrice"] = new XsvField(e.Fields.TaxIncludedPrice_KY, "0.000K￥");
        e.Row["Price"] = new XsvField(e.Fields.Price, "0,000￥");
    };

    using (var reader = new XsvReader(new System.IO.StringReader(data)))
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
