using Lombiq.EInvoiceValidator.Extensions;
using Lombiq.EInvoiceValidator.Services;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Lombiq.OSOCE.NuGet.Tests;

public class EInvoiceValidatorTests
{
    public static TheoryData<string> InvoiceFilePaths
    {
        get
        {
            var theoryData = new TheoryData<string>();
            var filePath = Path.Combine("SampleInvoices");
            foreach (var file in Directory.GetFiles(filePath, "*.xml", SearchOption.AllDirectories))
            {
                theoryData.Add(file);
            }

            return theoryData;
        }
    }

    [Theory]
    [MemberData(nameof(InvoiceFilePaths))]
    public async Task TestInvoiceValidation(string filePath)
    {
        var services = new ServiceCollection();
        services.AddEInvoiceValidationServices();
        var serviceProvider = services.BuildServiceProvider();

        var invoiceValidationService = serviceProvider.GetRequiredService<IInvoiceValidationService>();

        using var streamReaderInner = new StreamReader(filePath);
        var result = await invoiceValidationService.ValidateInvoiceAsync(
            streamReaderInner.BaseStream,
            cancellationToken: TestContext.Current.CancellationToken);

        if (filePath.Contains("failing"))
        {
            result.Successful.ShouldBeFalse($"{filePath} should fail validation but did not.");
        }
        else
        {
            result.Successful.ShouldBeTrue($"{filePath} should pass validation but did not.");
        }
    }
}
