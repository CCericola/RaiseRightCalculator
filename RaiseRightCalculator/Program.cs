using CsvHelper;

internal class Program
{
    private static void Main(string[] args)
    {
        var depositFile = args[0];
        var purchaseFile = args[1];

        List<DepositRecord> sourceDepositRecords;
        List<PurchaseRecord> sourcePurchaseRecords;

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

        Console.WriteLine("Deposit Id, Deposit Date, Deposit AMount, Last Name, Rebate Amount");

        foreach (var currentDeposit in sourceDepositRecords.OrderBy(x => x.DepositDate).ThenBy(x => x.DepositID))
        {
            var currentPurchaseRecords = sourcePurchaseRecords.Where(x => x.order_id == currentDeposit.OrderID);

            if (currentPurchaseRecords.Count() == 0) {
                // Abort, something is very wrong.
                Console.WriteLine($"Order ID {currentDeposit.OrderID} not found, aborting");
                break;
            }

            foreach (var currentPurchaseRecord in currentPurchaseRecords) {
                Console.WriteLine($"{currentDeposit.DepositID.Trim()}, {currentDeposit.DepositDate:yyyy/MM/dd}, {currentDeposit.DepositAmount:$0.00}, {currentPurchaseRecord.last_name}, {currentDeposit.RebateAmount:$0.00}");
            }
        }
    }
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
    public double net_cost { get; set; }
    public double rebate_dollars { get; set; }
    public bool isActive { get; set;}
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