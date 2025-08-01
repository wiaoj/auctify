using System.Globalization;
using System.Xml.Serialization;

namespace Auctify.CurrencyExchangeService.WebAPI.Models;
[XmlRoot("Tarih_Date")]
public sealed record TcmbCurrencyRates {
    // <Currency> 
    [XmlElement("Currency")]
    public List<TcmbCurrency> Currencies { get; set; } = [];
}

public record TcmbCurrency {
    // <Currency Kod="USD">
    [XmlAttribute("Kod")]
    public string Code { get; set; } = string.Empty;

    // <Unit>1</Unit> 
    [XmlElement("Unit")]
    public int Unit { get; set; }

    [XmlElement("ForexSelling")]
    public string ForexSellingRateText { get; set; } = string.Empty;

    [XmlIgnore]
    public decimal ForexSelling
        => decimal.TryParse(
            this.ForexSellingRateText,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out decimal rate)
        ? rate
        : 0;
}