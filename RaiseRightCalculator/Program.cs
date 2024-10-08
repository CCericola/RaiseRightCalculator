using CsvHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        var depositFile = args[0];
        var purchaseFile = args[1];
        var outputFile = args[2];

        List<DepositRecord> sourceDepositRecords;
        List<PurchaseRecord> sourcePurchaseRecords;
        List<OutputRecord> outputRecords = new List<OutputRecord>();

        // Unsure why, be we need to force unicode for this to be read properly
        using (var depositFileReader = new StreamReader(depositFile, System.Text.Encoding.Unicode)) {
            using (var csvReader = new CsvReader(depositFileReader, System.Globalization.CultureInfo.CurrentCulture)) {
                sourceDepositRecords = csvReader.GetRecords<DepositRecord>().ToList();
            }
        }

        using (var purchaseFileReader = new StreamReader(purchaseFile, System.Text.Encoding.Unicode)) {
            using (var csvReader = new CsvReader(purchaseFileReader, System.Globalization.CultureInfo.CurrentCulture)) {
                sourcePurchaseRecords = csvReader.GetRecords<PurchaseRecord>().ToList();
            }
        }

        foreach (var currentDeposit in sourceDepositRecords.OrderBy(x => x.DepositDate).ThenBy(x => x.DepositID)) {
            var currentPurchaseRecords = sourcePurchaseRecords.Where(x => x.order_id == currentDeposit.OrderID);

            if (currentPurchaseRecords.Count() == 0) {
                Console.WriteLine($"Order ID {currentDeposit.OrderID} not found, aborting");
                break;
            }

            foreach (var currentPurchaseRecord in currentPurchaseRecords) {
                outputRecords.Add(new OutputRecord() {
                    DepositId = currentDeposit.DepositID.Trim(),
                    DepositDate = currentDeposit.DepositDate,
                    DepositAmount = currentDeposit.DepositAmount,
                    LastName = currentPurchaseRecord.last_name,
                    RebateAmount = currentDeposit.RebateAmount
                });
            }
        }

        using (var outputFileWriter = new StreamWriter(outputFile)) {
            outputFileWriter.WriteLine("Deposit Id, Deposit Date, Deposit AMount, Last Name, Rebate Amount");

            foreach (var currentFamily in outputRecords.Select(x => x.LastName).Distinct().OrderBy(x => x)) {
                var familyTotal = outputRecords.Where(x => x.LastName == currentFamily).Select(x => x.RebateAmount).Sum();
                
                foreach (var currentFamilyDeposit in outputRecords.Where(x => x.LastName == currentFamily)) {
                    outputFileWriter.WriteLine($"{currentFamilyDeposit.DepositId.Trim()}, {currentFamilyDeposit.DepositDate:yyyy/MM/dd}, {currentFamilyDeposit.DepositAmount:$0.00}, {currentFamilyDeposit.LastName}, {currentFamilyDeposit.RebateAmount:$0.00}");
                }

                outputFileWriter.WriteLine($"{currentFamily} Total, , , , {familyTotal:$0.00}");
                outputFileWriter.WriteLine();
            }
        }
    }
}

internal class OutputRecord {
    public string DepositId { get; set; }
    public DateTime DepositDate { get; set; }
    public double DepositAmount { get; set; }
    public string LastName { get; set; }
    public double RebateAmount { get; set; }
}

internal class PurchaseRecord {
    public string order_id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
    public string custom_family_id { get; set; }
    public string organization_name { get; set; }
    public DateTime  order_date { get; set; }
    public string payment_type { get; set; }
    public double net_value { get; set; }
    // public double net_cost { get; set; }
    public double rebate_dollars { get; set; }
    public bool isActive { get; set; }

    public string product_name { get; set;}
    
    public int quantity { get; set; }
    
    public string sales_type { get; set; }
    
    public decimal convenience_fee { get; set; }
    public int DeliveryTypeId { get; set; }

}

internal class DepositRecord {
    public string DepositID { get; set; }
    public DateTime DepositDate { get; set; }
    public double DepositAmount { get; set; }
    public DateTime StartRange { get; set; }
    public DateTime EndRange { get; set; }
    public int poid { get; set; }
    public string confirmId { get; set; }
    public string OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public double RebateAmount { get; set; }

}