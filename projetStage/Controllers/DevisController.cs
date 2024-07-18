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
        public async Task<IActionResult> AddDevis([FromBody] AddDevisModel model)
        {
            var supplier = await _context.Fournisseurs.FindAsync(model.SupplierId);
            if (supplier == null)
            {
                return NotFound("Supplier not found.");
            }

            foreach (var item in model.Items) 
            {
                var existingDevisItem = _context.DevisItems.Find(item.Id);
                
                if (existingDevisItem == null)
                {
                    var devisItem = new DevisItem
                    {
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        Delay = item.Delay,
                        DemandeArticleId = item.DemandeArticleId,
                        Devise = item.Devise,
                        FournisseurId = model.SupplierId,
                    };
                    _context.DevisItems.Add(devisItem);
                }
                else
                {
                    existingDevisItem.UnitPrice = item.UnitPrice;
                    existingDevisItem.Quantity = item.Quantity;
                    existingDevisItem.Devise = item.Devise;
                    existingDevisItem.Delay = item.Delay;
                }
                await _context.SaveChangesAsync();
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
