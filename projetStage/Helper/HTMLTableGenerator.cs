using Microsoft.EntityFrameworkCore;
using projetStage.Models;
using System.Drawing;
using System.Text;

namespace projetStage.Helper
{
    public class HTMLTableGenerator
    {
        public static string GenerateHtmlTable(
            Demande demande,
            IEnumerable<DevisItem> supplierOffers,
            List<SupplierRequest> supplierRequests,
            Dictionary<string, float> exchangeRates)
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
                    sb.Append($"<th bgcolor = \"#d1ffd1\" colspan=\"4\" style=\"text-align: center;background-color: #d1ffd1; width: 50%;\">{supplier.Nom}</th>");
                }
                else
                {
                    sb.Append($"<th colspan=\"4\" style=\"text-align: center; width: 50%;\">{supplier.Nom}</th>");
                }
            }

            sb.Append("</tr><tr>");

            foreach (var supplier in suppliers)
            {
                if (selectedSupplier.Supplier.Id == supplier.Id)
                {
                    sb.Append("<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Unit Price</th>" +
                            "<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Discount</th>" +
                            "<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Total</th>" +
                            "<th bgcolor = \"#d1ffd1\" style=\"text-align: center;background-color: #d1ffd1;\">Delay</th>");
                }
                else
                {
                    sb.Append("<th style=\"text-align: center;\">Unit Price</th>" +
                        "<th style=\"text-align: center;\">Discount</th>" +
                        "<th style=\"text-align: center;\">Total</th>" +
                        "<th style=\"text-align: center;\">Delay</th>");
                }
            }

            sb.Append("</tr>");

            // Add rows for each article
            foreach (var article in demande.DemandeArticles)
            {
                // Check if the article is the "Delivery Fee" and if no supplier has an offer for it
                if (article.Name == "Delivery Fee")
                {
                    bool hasDeliveryOffer = suppliers.Any(supplier => supplierOffers.Any(o => o.FournisseurId == supplier.Id && o.DemandeArticleId == article.Id));
                    if (!hasDeliveryOffer)
                    {
                        continue; // Skip this article if no supplier offers delivery
                    }
                }

                sb.Append("<tr>");
                sb.Append($"<td style=\"text-align: center;\">{article.Name}</td>");
                sb.Append($"<td style=\"text-align: center;\">{article.Description}</td>");
                sb.Append($"<td style=\"text-align: center;\">{article.Qtt.ToString()}</td>");

                foreach (var supplier in suppliers)
                {
                    var style = supplier.Id == selectedSupplier.Supplier.Id ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1;text-align: center;'" : "style=\"text-align: center;\"";
                    var offer = supplierOffers.FirstOrDefault(o => o.FournisseurId == supplier.Id && o.DemandeArticleId == article.Id);
                    if (offer != null)
                    {
                        
                        float convertedUnitPrice = (float)offer.UnitPrice * exchangeRates[offer.Devise];
                        float? unitPriceAfterDiscount = offer.Discount != 0 ? (convertedUnitPrice - (convertedUnitPrice * offer.Discount / 100)) : convertedUnitPrice;

                        sb.Append($"<td {style}> € {convertedUnitPrice.ToString("F2")}</td>");
                        sb.Append($"<td {style}>{offer.Discount}%</td>");
                        sb.Append($"<td {style}> € {(unitPriceAfterDiscount * article.Qtt)?.ToString("F2")}</td>");
                        sb.Append($"<td {style}>{offer.Delay}</td>");
                    }
                    else
                    {
                        sb.Append(
                            $"<td {style}>-</td>" +
                            $"<td {style}>-</td>" +
                            $"<td {style}>-</td>" +
                            $"<td {style}>-</td>"
                        );
                    }
                }
                sb.Append("</tr>");
            }

            // Add total row
            sb.Append("<tr>");
            sb.Append("<td colspan='3' style='font-weight: bold; text-align: center;'>Total</td>");

            foreach (var supplier in suppliers)
            {
                var totalInEUR = supplierOffers
                    .Where(o => o.FournisseurId == supplier.Id)
                    .Sum(o =>
                    {
                        var articleQuantity = demande.DemandeArticles.FirstOrDefault(da => da.Id == o.DemandeArticleId)?.Qtt ?? 0;
                        var unitPriceWithDiscount = o.Discount != 0
                            ? ((float)o.UnitPrice - ((float)o.UnitPrice * o.Discount / 100))
                            : (float)o.UnitPrice;
                        return unitPriceWithDiscount * exchangeRates[o.Devise] * articleQuantity;
                    });

                var totalEURStyle = supplier.Id == selectedSupplier.Supplier.Id
                    ? "bgcolor = \"#d1ffd1\" style='background-color: #d1ffd1; font-weight: bold;text-align: center;'"
                    : "style='font-weight: bold;text-align: center;'";

                sb.Append($"<td colspan='4' {totalEURStyle}> € {((decimal)totalInEUR).ToString("F2")}</td>");
            }

            sb.Append("</tr>");

            sb.Append("</table>");

            return sb.ToString();
        }

    }
}
