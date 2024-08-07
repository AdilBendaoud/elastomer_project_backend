using Microsoft.EntityFrameworkCore;
using projetStage.Models;
using System.Drawing;
using System.Text;

namespace projetStage.Helper
{
    public class HTMLTableGenerator
    {
        public static string GenerateHtmlTable(Demande demande, IEnumerable<DevisItem> supplierOffers, List<SupplierRequest> supplierRequests)
        {
            var sb = new StringBuilder();
            var suppliers = supplierRequests.Select(sr => sr.Supplier).Distinct().ToList();
            var selectedSupplier = supplierRequests.SingleOrDefault(sr => sr.isSelectedForValidation);

            sb.Append("<table border='1'><tr>" +
                "<th rowspan=\"2\" style=\"vertical-align: middle; text-align: center;\">Article</th>" +
                "<th rowspan=\"2\" style=\"vertical-align: middle; text-align: center;\">Description</th>" +
                "<th rowspan=\"2\" style=\"vertical-align: middle; text-align: center;\">Quantity</th>");

            foreach (var supplier in suppliers)
            {
                if (selectedSupplier.Supplier.Id == supplier.Id)
                {
                    sb.Append($"<th bgcolor = \"#d1ffd1\" colspan=\"3\" style=\"text-align: center;background-color: #d1ffd1; width: 50%;\">{supplier.Nom}</th>");
                }
                else
                {
                    sb.Append($"<th colspan=\"3\" style=\"text-align: center; width: 50%;\">{supplier.Nom}</th>");
                }
            }

            sb.Append("</tr><tr>");

            foreach (var supplier in suppliers)
            {
                if (selectedSupplier.Supplier.Id == supplier.Id)
                {
                    sb.Append("<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Unit Price</th>" +
                            "<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Total</th>" +
                            "<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Delay</th>");
                }
                else
                {
                    sb.Append("<th style=\"text-align: center;\">Unit Price</th><th style=\"text-align: center;\">Total</th><th style=\"text-align: center;\">Delay</th>");
                }
            }

            sb.Append("</tr>");

            //Exchange rates
            var exchangeRates = new Dictionary<string, decimal> { { "EUR", 1.0m }, { "USD", 0.92m }, { "MAD", 0.092m } };

            //Add rows for each article
            foreach (var article in demande.DemandeArticles)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td style=\"text-align: center;\">{article.Name}</td>");
                    sb.Append($"<td style=\"text-align: center;\">{article.Description}</td>");
                    sb.Append($"<td style=\"text-align: center;\">{article.Qtt.ToString("F3")}</td>");

                    foreach (var supplier in suppliers)
                    {
                        var offer = supplierOffers.FirstOrDefault(o => o.FournisseurId == supplier.Id && o.DemandeArticleId == article.Id);
                        if (offer != null)
                        {
                            var unitPriceStyle = supplier.Id == selectedSupplier.Supplier.Id ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1;text-align: center;'" : "style=\"text-align: center;\"";
                            var totalPriceStyle = supplier.Id == selectedSupplier.Supplier.Id ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1;text-align: center;'" : "style=\"text-align: center;\"";
                            var delayStyle = supplier.Id == selectedSupplier.Supplier.Id ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1;text-align: center;'" : "style=\"text-align: center;\"";

                            sb.Append($"<td {unitPriceStyle}>{offer.UnitPrice.ToString("F3")} {offer.Devise}</td>");
                            sb.Append($"<td {totalPriceStyle}>{(offer.UnitPrice * article.Qtt).ToString("F3")} {offer.Devise}</td>");
                            sb.Append($"<td {delayStyle}>{offer.Delay}</td>");
                        }
                        else
                        {
                            sb.Append("<td style=\"text-align: center;\">-</td><td style=\"text-align: center;\">-</td><td style=\"text-align: center;\">-</td>");
                        }
                    }
                    sb.Append("</tr>");
                }

            //Add total row
            sb.Append("<tr>");
            sb.Append("<td colspan='3' style='font-weight: bold; text-align: center;'>Total</td>");

            foreach (var supplier in suppliers)
            {
                var totalOriginalCurrency = supplierOffers
                    .Where(o => o.FournisseurId == supplier.Id)
                    .Sum(o => o.UnitPrice * demande.DemandeArticles.FirstOrDefault(da => da.Id == o.DemandeArticleId).Qtt);

                var totalInEUR = supplierOffers
                    .Where(o => o.FournisseurId == supplier.Id)
                    .Sum(o => (o.UnitPrice * exchangeRates[o.Devise]) * demande.DemandeArticles.FirstOrDefault(da => da.Id == o.DemandeArticleId).Qtt);

                var totalOriginalStyle = supplier.Id == selectedSupplier.Supplier.Id ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1; font-weight: bold;text-align: center;'" : "style='font-weight: bold;text-align: center;'";
                var totalEURStyle = supplier.Id == selectedSupplier.Supplier.Id ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1; font-weight: bold;text-align: center;'" : "style='font-weight: bold;text-align: center;'";
                if (supplierOffers.FirstOrDefault(o => o.FournisseurId == supplier.Id)?.Devise == "EUR")
                {
                    sb.Append($"<td colspan='3' {totalOriginalStyle}>{totalOriginalCurrency.ToString("F3")} EUR</td>");
                }
                else
                {
                    sb.Append($"<td colspan='2' {totalOriginalStyle}>{totalOriginalCurrency.ToString("F3")} {supplierOffers.FirstOrDefault(o => o.FournisseurId == supplier.Id)?.Devise}</td>");
                    sb.Append($"<td {totalEURStyle}>{totalInEUR.ToString("F3")} EUR</td>");
                }

            }

            sb.Append("</tr>");

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
