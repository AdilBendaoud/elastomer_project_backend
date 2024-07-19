using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.DTO.demandes;
using projetStage.Models;

namespace projetStage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DevisController(AppDbContext context)
        {
            _context = context;
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
                        .FirstAsync(d => d.DemandeArticleId == item.DemandeArticleId && d.FournisseurId == supplier.Id);

                    if (existingDevisItem == null)
                    {
                        var devisItem = new DevisItem
                        {
                            DemandeArticleId = item.DemandeArticleId,
                            FournisseurId = supplier.Id,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Devise = item.Devise,
                            Delay = item.Delay
                        };

                        _context.DevisItems.Add(devisItem);
                    }
                    else
                    {
                        existingDevisItem.DemandeArticleId = item.DemandeArticleId;
                        existingDevisItem.FournisseurId = supplier.Id;
                        existingDevisItem.Quantity = item.Quantity;
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
                        d.Quantity,
                        d.UnitPrice,
                        d.DemandeArticle
                    }
                })
                .ToListAsync();

            return Ok(devisList);
        }
    }
}
