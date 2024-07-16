using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO.demandes;
using projetStage.Models;
using projetStage.Services;
using System.Text;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public SuppliersController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers([FromQuery] string demandeCode)
        {
            if (string.IsNullOrEmpty(demandeCode))
            {
                return BadRequest("Demande code is required.");
            }

            // Get the Demande ID based on the demandeCode
            var demande = await _context.Demandes.FirstOrDefaultAsync(d => d.Code == demandeCode);
            if (demande == null)
            {
                return NotFound("Demande not found.");
            }

            // Get the list of suppliers and check if the request has been sent to each supplier
            var suppliers = await _context.Fournisseurs
                .Select(s => new
                {
                    s.Id,
                    s.Nom,
                    IsRequestSent = _context.SupplierRequests.Any(sr => sr.DemandeId == demande.Id && sr.SupplierId == s.Id)
                })
                .ToListAsync();

            return Ok(suppliers);
        }

        private async Task LogDemandeHistory(int userCode, int demandeId, string changeDetails)
        {
            var history = new DemandeHistory
            {
                DemandeId = demandeId,
                DateChanged = DateTime.UtcNow,
                UserCode = userCode,
                Details = changeDetails
            };
            _context.DemandeHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        [HttpPost("send-to-suppliers")]
        [Authorize(Roles = "P")]
        public async Task<IActionResult> SendToSuppliers([FromBody] SendRequestToSuppliersModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RequestCode) || model.SupplierIds == null || !model.SupplierIds.Any())
            {
                return BadRequest("Invalid request payload.");
            }

            var demande = await _context.Demandes.FirstOrDefaultAsync(d => d.Code == model.RequestCode);
            if (demande == null)
            {
                return NotFound("Demande not found.");
            }

            var suppliers = await _context.Fournisseurs.Where(s => model.SupplierIds.Contains(s.Id)).ToListAsync();
            if (suppliers.Count != model.SupplierIds.Count)
            {
                return BadRequest("Some suppliers not found.");
            }

            var purchaser = await _context.Users.FirstAsync(a => a.Code == model.UserCode);
            if (purchaser == null)
            {
                return NotFound("Purchaser not found.");
            }

            var demandeArticles = await _context.DemandeArticles
                .Include(da => da.Article)
                .Where(da => da.DemandeId == demande.Id)
                .ToListAsync();

            var emailBody = BuildEmailBody(demandeArticles);

            foreach (var supplier in suppliers)
            {
                var supplierRequest = new SupplierRequest
                {
                    DemandeId = demande.Id,
                    SupplierId = supplier.Id,
                    SentAt = DateTime.UtcNow
                };
                _context.SupplierRequests.Add(supplierRequest);
                _emailService.SendEmail(purchaser.Email, supplier.Email, demande.Code, emailBody);
            }

            _emailService.SendEmail(purchaser.Email, demande.Code, emailBody);

            demande.AcheteurId = purchaser.Id;
            demande.Status = DemandeStatus.WO;

            await LogDemandeHistory(model.UserCode, demande.Id, $"Offer sent to suppliers by {purchaser.FirstName} {purchaser.LastName}");
            await _context.SaveChangesAsync();

            return Ok("Request sent to suppliers successfully.");
        }
        private string BuildEmailBody(List<DemandeArticle> demandeArticles)
        {
            var sb = new StringBuilder();
            sb.Append("<html><body>");
            sb.Append("<h3>Request Details</h3>");
            sb.Append("<table border='1'>");
            sb.Append("<tr><th>Article</th><th>Description</th><th>Quantity</th></tr>");

            foreach (var item in demandeArticles)
            {
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", item.Article.Nom ,item.Article.Description, item.Qtt);
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            return sb.ToString();
        }
    }
}
