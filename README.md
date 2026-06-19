# RaiseRight Rebate Calculator

Combines three monthly CSV reports from RaiseRight into a single treasurer report showing rebate earnings per family.

## Web app (no install required)

Open **https://ccericola.github.io/RaiseRightCalculator/** in any browser, upload the three CSV files, and download the report. All processing happens in-browser — nothing is uploaded to a server.

### Reports to download from RaiseRight

Log in to the [Coordinator Dashboard](https://www.raiseright.com/shop/account/coordinator-dashboard), then export these three reports as CSV:

| # | Report | URL |
|---|--------|-----|
| 1 | Online Payment and Shop Online Deposit Slip | `/Report/Selection?rpt=OrgAutoOrderRebate` |
| 2 | Order History by Family and Product (90 days) | `/Report/Selection?rpt=FamilyOrderDetail` |
| 3 | Non-Gift Card Earnings by Family | `/Report/Selection?rpt=ShopOnlineEarningsByFamily` |

---

## Console app (for developers)

```
dotnet run -- <deposit-file> <purchase-file> <online-file> <output-file>
```

Pass `skip` for `<online-file>` if there are no online orders that month.

See [`RaiseRightCalculator/read.me`](RaiseRightCalculator/read.me) for detailed instructions and examples.

## Solution structure

| Project | Purpose |
|---------|---------|
| `RaiseRightCalculator.Core` | Shared parsing and report generation logic |
| `RaiseRightCalculator` | CLI front-end |
| `RaiseRightCalculator.Web` | Blazor WebAssembly front-end (GitHub Pages) |
