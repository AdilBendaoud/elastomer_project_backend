using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO.demandes;
using projetStage.Models;
using projetStage.Services;
using System.Text;
using System.Threading.Channels;

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

        [HttpGet("search")]
        public async Task<IActionResult> SearchSuppliers([FromQuery] string query, [FromQuery] string requestCode)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query parameter cannot be empty");
            }

            // Get the DemandeId from the request code
            var demande = await _context.Demandes
                .FirstOrDefaultAsync(d => d.Code == requestCode);

            if (demande == null)
            {
                return NotFound("Demande not found");
            }

            // Get the IDs of suppliers who have already received the request
            var excludedSupplierIds = await _context.SupplierRequests
                .Where(sr => sr.DemandeId == demande.Id)
                .Select(sr => sr.SupplierId)
                .ToListAsync();

            // Filter suppliers by name and exclude the ones already sent the request
            var suppliers = await _context.Fournisseurs
                .Where(s => s.Nom.Contains(query) && !excludedSupplierIds.Contains(s.Id))
                .ToListAsync();

            return Ok(suppliers);
        }

        private async Task LogDemandeHistory(int userCode, int demandeId, string changeDetails)
        {
            var demande = await _context.Demandes.FirstAsync(d => d.Id == demandeId);
            demande.LastModification = DateTime.Now;
            var history = new DemandeHistory
            {
                DemandeId = demandeId,
                DateChanged = DateTime.Now,
                UserCode = userCode,
                Details = changeDetails
            };
            _context.DemandeHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        [HttpPost("send-to-suppliers")]
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

            var purchaser = await _context.Users.FirstOrDefaultAsync(a => a.Code == model.UserCode);
            if (purchaser == null)
            {
                return NotFound("Purchaser not found.");
            }

            var demandeArticles = await _context.DemandeArticles
                .Where(da => da.DemandeId == demande.Id && da.Name != "Delivery Fee")
                .ToListAsync();

            var emailBody = BuildEmailBody(demandeArticles);

            //foreach (var supplier in suppliers)
            //{
            //    var previoudData = await _context.SupplierRequests.FirstOrDefaultAsync(s => s.DemandeId == demande.Id && s.SupplierId == supplier.Id);
            //    if(previoudData == null)
            //    {
            //        var supplierRequest = new SupplierRequest
            //        {
            //            DemandeId = demande.Id,
            //            SupplierId = supplier.Id,
            //            SentAt = DateTime.Now
            //        };
            //        _context.SupplierRequests.Add(supplierRequest);
            //    }

            //    // Enqueue the email sending as a background job
            //    BackgroundJob.Enqueue(() => _emailService.SendEmail(purchaser.Email, supplier.Email, demande.Code, emailBody));
            //}

            await Task.Run(() => SendEmailToSuppliers(demande, purchaser, suppliers, emailBody));

            demande.AcheteurId = purchaser.Id;
            demande.Status = DemandeStatus.WO;

            await LogDemandeHistory(model.UserCode, demande.Id, $"Offer sent to suppliers by {purchaser.FirstName} {purchaser.LastName}");
            await _context.SaveChangesAsync();

            return Ok("Request sent to suppliers successfully.");
        }

        private async Task SendEmailToSuppliers(Demande demande, User purchaser, List<Fournisseur>? suppliers, string? emailBody)
        {
            foreach (var supplier in suppliers)
            {
                var previousData = await _context.SupplierRequests.FirstOrDefaultAsync(s => s.DemandeId == demande.Id && s.SupplierId == supplier.Id);

                if (previousData == null)
                {
                    var supplierRequest = new SupplierRequest
                    {
                        DemandeId = demande.Id,
                        SupplierId = supplier.Id,
                        SentAt = DateTime.Now
                    };
                    _context.SupplierRequests.Add(supplierRequest);
                }

                await _emailService.SendEmail(purchaser.Email, supplier.Email, demande.Code, emailBody);
            }

            await _emailService.SendEmail(purchaser.Email, demande.Code, emailBody);
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
                sb.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", item.Name, item.Description, item.Qtt);
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            return sb.ToString();
        }

        [HttpGet("selected-supplier/{requestCode}")]
        //[Authorize(Roles = "V")]
        public async Task<IActionResult> GetSelectedSupplier(string requestCode)
        {
            var demande = await _context.Demandes
                .Include(d => d.SupplierRequests)
                .ThenInclude(sr => sr.Supplier)
                .FirstOrDefaultAsync(d => d.Code == requestCode);

            if (demande == null)
            {
                return NotFound("Demande not found.");
            }

            var selectedSupplierRequest = await _context.SupplierRequests
                .Include(sr => sr.Supplier)
                .FirstOrDefaultAsync(sr => sr.DemandeId == demande.Id && sr.isSelectedForValidation);

            if (selectedSupplierRequest == null)
            {
                return NotFound("Selected supplier request not found.");
            }

            var offers = await _context.DevisItems
                .Where(di => di.DemandeArticle.DemandeId == demande.Id && di.FournisseurId == selectedSupplierRequest.SupplierId)
                .Select(di => new
                {
                    di.Id,
                    di.DemandeArticleId,
                    di.UnitPrice,
                    di.Devise,
                    di.Delay
                })
                .ToListAsync();

            var result = new
            {
                selectedSupplierRequest.Supplier.Id,
                selectedSupplierRequest.Supplier.Nom,
                Offers = offers
            };

            return Ok(result);
        }

    }
}