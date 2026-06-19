using RaiseRightCalculator.Core;

const string SkipFile = "skip";

if (args.Length != 4)
{
    Console.Error.WriteLine("Usage: RaiseRightCalculator <deposit-file> <purchase-file> <online-file> <output-file>");
    Console.Error.WriteLine("       Pass \"skip\" for <online-file> to omit online orders.");
    Console.Error.WriteLine("Example: RaiseRightCalculator 202412-OrgAutoOrderRebate.csv 202412-FamilyOrderHistoryByProduct.csv 202412-ShopOnlineEarningsByFamily.csv 202412-Raiseright.csv");
    return 1;
}

var depositFile = args[0];
var purchaseFile = args[1];
var onlineFile = args[2];
var outputFile = args[3];

try
{
    using var depositStream = File.OpenRead(depositFile);
    using var purchaseStream = File.OpenRead(purchaseFile);
    Stream onlineStream = onlineFile == SkipFile ? null : File.OpenRead(onlineFile);

    var csv = RebateReportGenerator.GenerateReport(depositStream, purchaseStream, onlineStream);
    onlineStream?.Dispose();

    File.WriteAllText(outputFile, csv);
    Console.WriteLine($"Report written to {outputFile}");
    return 0;
}
catch (FileNotFoundException ex)
{
    Console.Error.WriteLine($"File not found: {ex.FileName}");
    return 1;
}
catch (InvalidDataException ex)
{
    Console.Error.WriteLine($"Data error: {ex.Message}");
    return 1;
}
