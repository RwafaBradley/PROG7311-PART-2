using GLMS.Web.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace GLMS.Web.Tests;

public class CurrencyConversionTests
{
    private static IConfiguration BuildConfig(decimal rate = 18.5m)
    {
        var values = new Dictionary<string, string?>
        {
            ["ExternalApis:DefaultUsdToZarRate"] = rate.ToString()
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    [Fact]
    public void ConvertUsdToZar_ShouldMultiplyCorrectly()
    {
        var strategy = new FixedRateCurrencyStrategy(BuildConfig(18.5m));

        var result = strategy.ConvertUsdToZar(10m);

        Assert.Equal(185.00m, result);
    }

    [Fact]
    public void ConvertUsdToZar_ShouldRoundToTwoDecimals()
    {
        var strategy = new FixedRateCurrencyStrategy(BuildConfig(18.3333m));

        var result = strategy.ConvertUsdToZar(2m);

        Assert.Equal(36.67m, result);
    }
}
