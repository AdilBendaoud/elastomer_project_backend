using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.DTO.demandes;
using projetStage.Helper;
using projetStage.Models;
using projetStage.Services;
using System.Threading.Channels;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public DevisController(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("sendForValidation")]
        public async Task<IActionResult> SendForValidation([FromBody] RequestForValidation model) 
        {
            var demande = await _context.Demandes.Include(d=> d.DemandeArticles).FirstAsync(d => d.Code == model.demandeCode);
            if (demande == null)
            {
                return NotFound();
            }

            var supplier = await _context.Fournisseurs.FirstAsync(d=> d.Id == model.supplierId);
            if (demande == null)
            {
                return NotFound();
            }
            var previousSelectedSupplier = await _context.SupplierRequests.FirstOrDefaultAsync(sr => sr.Demande.Code == demande.Code && sr.SupplierId != model.supplierId && sr.isSelectedForValidation);
            if (previousSelectedSupplier != null)
            {
                previousSelectedSupplier.isSelectedForValidation = false;
            }
            var selectedSupplier = await _context.SupplierRequests.FirstAsync(sr => sr.Demande == demande && sr.SupplierId == model.supplierId);
            selectedSupplier.isSelectedForValidation = true;

            demande.Status = DemandeStatus.WV;

            await _context.SaveChangesAsync();

            // fetch suppliers
            var supplierRequests = await _context.SupplierRequests
            .Where(sr => sr.DemandeId == demande.Id)
            .Include(sr => sr.Supplier)
            .ToListAsync();

            var suppliers = supplierRequests.Select(sr => sr.Supplier).Distinct().ToList();

            // Fetch devis items for these suppliers
            var devisItems = await _context.DevisItems
                .Where(di => di.DemandeArticle.DemandeId == demande.Id && suppliers.Select(s => s.Id).Contains(di.FournisseurId))
                .ToListAsync();

            // Send email to all validators
            var validators = await _context.Users.Where(u => u.IsValidator).ToListAsync();
            var emailAddresses = validators.Select(v => v.Email).ToList();
            var emailSubject = "New Request Needs Validation";
            var htmlTable = HTMLTableGenerator.GenerateHtmlTable(demande, devisItems, supplierRequests);
            var emailBody = $"A new request with code {demande.Code} requires your validation. Please log in to the system to validate the request. <br><br>{htmlTable}";

            foreach (var email in emailAddresses)
            {
                _emailService.SendEmail(email, emailSubject, emailBody);
            }
            return Ok("Request sent for Validation");
        }

        [HttpPost]
        public async Task<IActionResult> AddDevis([FromBody] List<AddDevisModel> models)
        {
            foreach (var model in models)
            {
                var supplier = await _context.Fournisseurs.FindAsync(model.SupplierId);
                if (supplier == null)
                {
                    return NotFound($"Supplier with ID {model.SupplierId} not found.");
                }

                foreach (var item in model.Items)
                {
                    var demandeArticle = await _context.DemandeArticles.FindAsync(item.DemandeArticleId);
                    if (demandeArticle == null)
                    {
                        return NotFound($"DemandeArticle with ID {item.DemandeArticleId} not found.");
                    }
                    var existingDevisItem = await _context.DevisItems
                        .SingleOrDefaultAsync(d => d.DemandeArticleId == item.DemandeArticleId && d.FournisseurId == supplier.Id);

                    if (existingDevisItem == null)
                    {
                        var devisItem = new DevisItem
                        {
                            DemandeArticleId = item.DemandeArticleId,
                            FournisseurId = supplier.Id,
                            UnitPrice = item.UnitPrice,
                            Devise = item.Devise,
                            Delay = item.Delay
                        };

                        _context.DevisItems.Add(devisItem);
                    }
                    else
                    {
                        existingDevisItem.UnitPrice = item.UnitPrice;
                        existingDevisItem.Devise = item.Devise;
                        existingDevisItem.Delay = item.Delay;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            return Ok("Devis added successfully.");
        }

        [HttpGet("{demandeCode}")]
        public async Task<IActionResult> GetDevis(string demandeCode)
        {
            var devisList = await _context.DevisItems
                .Where(d => d.DemandeArticle.Demande.Code == demandeCode)
                .Select(d => new
                {
                    SupplierName = d.Fournisseur.Nom,
                    Items = new
                    {
                        d.Id,
                        d.Delay,
                        d.UnitPrice,
                        d.DemandeArticle
                    }
                })
                .ToListAsync();

            return Ok(devisList);
        }
        
        [HttpGet("time")]
        public object GetTime()
        {
            return (new
            {
                DateTime.Now
            });
        }
    }
}
