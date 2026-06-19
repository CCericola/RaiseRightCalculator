using CsvHelper;
using System.Globalization;
using System.Text;

namespace RaiseRightCalculator.Core;

public static class RebateReportGenerator
{
    private static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("en-US");

    public static string GenerateReport(Stream depositCsv, Stream purchaseCsv, Stream? onlineCsv)
    {
        List<DepositRecord> deposits;
        List<PurchaseRecord> purchases;
        List<OnlineOrderRecord> onlineOrders;

        using (var reader = new StreamReader(depositCsv, Encoding.Unicode, leaveOpen: true))
        using (var csv = new CsvReader(reader, Culture))
            deposits = csv.GetRecords<DepositRecord>().ToList();

        using (var reader = new StreamReader(purchaseCsv, Encoding.Unicode, leaveOpen: true))
        using (var csv = new CsvReader(reader, Culture))
            purchases = csv.GetRecords<PurchaseRecord>().ToList();

        if (onlineCsv is not null)
        {
            using var reader = new StreamReader(onlineCsv, Encoding.Unicode, leaveOpen: true);
            using var csv = new CsvReader(reader, Culture);
            onlineOrders = csv.GetRecords<OnlineOrderRecord>().ToList();
        }
        else
        {
            onlineOrders = [];
        }

        var outputRecords = new List<OutputRecord>();

        foreach (var deposit in deposits.OrderBy(x => x.DepositDate).ThenBy(x => x.DepositID))
        {
            if (!string.IsNullOrEmpty(deposit.OrderID))
            {
                var matches = purchases.Where(x => x.order_id == deposit.OrderID).ToList();
                if (matches.Count == 0)
                    throw new InvalidDataException($"Order ID {deposit.OrderID} not found in purchase file.");

                foreach (var p in matches)
                    outputRecords.Add(new OutputRecord
                    {
                        DepositId = deposit.DepositID.Trim(),
                        DepositDate = deposit.DepositDate,
                        DepositAmount = deposit.DepositAmount,
                        LastName = p.last_name,
                        RebateAmount = p.rebate_dollars
                    });
            }
            else if (!string.IsNullOrEmpty(deposit.EarningsId))
            {
                var matches = onlineOrders.Where(x => x.VoucherNumber == deposit.EarningsId).ToList();
                if (matches.Count == 0)
                    throw new InvalidDataException($"Voucher ID {deposit.EarningsId} not found in online orders file.");

                foreach (var o in matches)
                    outputRecords.Add(new OutputRecord
                    {
                        DepositId = deposit.DepositID.Trim(),
                        DepositDate = deposit.DepositDate,
                        DepositAmount = deposit.DepositAmount,
                        LastName = o.FamilyName,
                        RebateAmount = o.EarningsAmount
                    });
            }
        }

        var writer = new StringWriter();
        writer.WriteLine("Deposit Id, Deposit Date, Deposit Amount, Last Name, Rebate Amount");

        foreach (var family in outputRecords.Select(x => x.LastName).Distinct().OrderBy(x => x))
        {
            var familyTotal = outputRecords.Where(x => x.LastName == family).Sum(x => x.RebateAmount);

            foreach (var row in outputRecords.Where(x => x.LastName == family))
                writer.WriteLine($"{row.DepositId}, {row.DepositDate:yyyy/MM/dd}, {row.DepositAmount:$0.00}, {row.LastName}, {row.RebateAmount:$0.00}");

            writer.WriteLine($"{family} Total, , , , {familyTotal:$0.00}");
            writer.WriteLine();
        }

        return writer.ToString();
    }
}
