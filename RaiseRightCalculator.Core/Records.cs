namespace RaiseRightCalculator.Core;

public class OutputRecord
{
    public string DepositId { get; set; } = "";
    public DateTime DepositDate { get; set; }
    public double DepositAmount { get; set; }
    public string LastName { get; set; } = "";
    public double RebateAmount { get; set; }
}

public class PurchaseRecord
{
    public string order_id { get; set; } = "";
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public string custom_family_id { get; set; } = "";
    public string organization_name { get; set; } = "";
    public DateTime order_date { get; set; }
    public string payment_type { get; set; } = "";
    public double net_value { get; set; }
    public double rebate_dollars { get; set; }
    public bool isActive { get; set; }
    public string product_name { get; set; } = "";
    public int quantity { get; set; }
    public string sales_type { get; set; } = "";
    public decimal convenience_fee { get; set; }
    public int DeliveryTypeId { get; set; }
}

public class DepositRecord
{
    public string DepositID { get; set; } = "";
    public DateTime DepositDate { get; set; }
    public double DepositAmount { get; set; }
    public DateTime StartRange { get; set; }
    public DateTime EndRange { get; set; }
    public int? PO { get; set; }
    public string confirmId { get; set; } = "";
    public string OrderID { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public double Earnings { get; set; }
    public string EarningsId { get; set; } = "";
}

public class OnlineOrderRecord
{
    public string VoucherNumber { get; set; } = "";
    public double EarningsAmount { get; set; }
    public string FamilyName { get; set; } = "";
}
