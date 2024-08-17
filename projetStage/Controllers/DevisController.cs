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

        [HttpPost("sendForValidation")]
        public async Task<IActionResult> SendForValidation([FromBody] RequestForValidation model) 
        {
            var demande = await _context.Demandes.Include(d=> d.DemandeArticles).FirstAsync(d => d.Code == model.demandeCode);
            if (demande == null)
            {
                return NotFound();
            }

            var purchaser = await _context.Users.FirstAsync(a => a.Code == model.userCode);
            if(purchaser == null)
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

            //fetch currency
            var currencies = _context.Currencies.ToList();

            // Send email to all validators
            var validators = await _context.Users.Where(u => u.IsValidator).ToListAsync();
            var emailAddresses = validators.Select(v => v.Email).ToList();
            var emailSubject = "New Request Needs Validation";
            var exchangeRates = new Dictionary<string, float> { 
                { "EUR", currencies.First(c=> c.CurrencyCode == "EUR").PriceInEur },
                { "USD", currencies.First(c=> c.CurrencyCode == "USD").PriceInEur },
                { "MAD", currencies.First(c=> c.CurrencyCode == "MAD").PriceInEur },
                { "GBP", currencies.First(c=> c.CurrencyCode == "GBP").PriceInEur }};
            var htmlTable = HTMLTableGenerator.GenerateHtmlTable(demande, devisItems, supplierRequests, exchangeRates);
            var emailBody = $"A new request with code {demande.Code} requires your validation. Please log in to the system to validate the request. <br><br>{htmlTable}";

            foreach (var email in emailAddresses)
            {
                await _emailService.SendEmail(email, emailSubject, emailBody);
            }
            await LogDemandeHistory(model.userCode, demande.Id, $"Request sent to validation by {purchaser.FirstName} {purchaser.LastName}");
            return Ok("Request sent for Validation");
        }

        [HttpPost]
        public async Task<IActionResult> AddDevis([FromBody] CreateDeviseModel data)
        {
            foreach (var model in data.DevisList)
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
                            Delay = item.Delay,
                            Discount = item.Discount
                        };
                        _context.DevisItems.Add(devisItem);
                    }
                    else
                    {
                        existingDevisItem.UnitPrice = item.UnitPrice;
                        existingDevisItem.Devise = item.Devise;
                        existingDevisItem.Delay = item.Delay;
                        existingDevisItem.Discount = item.Discount;
                    }
                    await _context.SaveChangesAsync();
                }
            }
            
            var demande = await _context.Demandes.FirstAsync(d => d.Code == data.DemandeCode);
            var user = await _context.Users.FirstAsync(u=> u.Code == data.UserCode);

            await LogDemandeHistory(data.UserCode, demande.Id, $"Offer added/edited by {user.FirstName} {user.LastName}");
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
    }
}
