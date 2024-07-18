using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.DTO.demandes;
using projetStage.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

// Controllers/DemandeController.cs
[Route("api/[controller]")]
[ApiController]
public class DemandeController : ControllerBase
{
    private readonly AppDbContext _context;

    public DemandeController(AppDbContext context)
    {
        _context = context;
    }

    private async Task LogDemandeHistory(int code, int demandeId, string changeDetails)
    {
        var user = GetUser(code);
        var history = new DemandeHistory
        {
            DemandeId = demandeId,
            DateChanged = DateTime.UtcNow,
            UserCode = user.Code,
            Details = changeDetails
        };
        _context.DemandeHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDemandes(int userCode, int pageNumber = 1, int pageSize = 10)
    {
        User user = GetUser(userCode);
        if (user == null)
        {
            return NotFound("User not Found !");
        }

        var demandes = _context.Demandes.Include(d => d.Demandeur);

        if (user.IsRequester)
        {
            demandes.Where(d => d.Demandeur.Code == userCode);
        }

        var totalDemandes = await demandes.CountAsync();
        var paginatedDemandes = await demandes
            .OrderBy(d => d.OpenedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new PaginatedResponse<Demande>
        {
            TotalCount = totalDemandes,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            Items = paginatedDemandes
        };

        return Ok(response);
    }

    [HttpGet("{demandeCode}/articles")]
    public async Task<IActionResult> GetArticle(string demandeCode)
    {
        var demande =  await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }

        var articles = await _context.DemandeArticles
            .Select(da => new CreateDemandeArticleModel
            {
                Id = da.Id,
                Name = da.ArticleId != null ? da.Article.Nom : da.Name,
                Description = da.ArticleId != null ? da.Article.Description : da.Description,
                Quantity = da.Qtt,
                FamilleDeProduit = da.ArticleId != null ? da.Article.FamilleDeProduit : da.FamilleDeProduit,
                Destination = da.ArticleId != null ? da.Article.Destination : da.Destination
            })
            .ToListAsync();

        if (articles == null)
        {
            return NotFound();
        }
        return Ok(articles);
    }

    [HttpPut("{demandeCode}/update-articles")]
    public async Task<IActionResult> UpdateDemandeArticles(string demandeCode, [FromBody] UpdateArticlesModel model)
    {
        var user = GetUser(model.userCode);

        var demande = await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }

        // Ensure the provided demande ID matches the ID in the route
        if (model.DemandeCode != demandeCode)
        {
            return BadRequest("Demande ID mismatch.");
        }

        // Clear existing articles for the demande
        var existingDemandeArticles = _context.DemandeArticles.Where(da => da.Demande.Code == demandeCode);
        _context.DemandeArticles.RemoveRange(existingDemandeArticles);
        await _context.SaveChangesAsync();

        // Add updated articles
        foreach (var article in model.Articles)
        {
            // Check if the article already exists
            var existingArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Nom == article.Name && a.Description == article.Description);

            // If article doesn't exist, create and save it
            if (existingArticle == null)
            {

                // Create DemandeArticle with the new article
                var demandeArticle = new DemandeArticle
                {
                    DemandeId = demande.Id,
                    Destination = article.Destination,
                    FamilleDeProduit = article.FamilleDeProduit,
                    CreatedAt = DateTime.UtcNow,
                    Qtt = article.Quantity,
                    Status = "Updated"
                };
                _context.DemandeArticles.Add(demandeArticle);
            }
            else
            {
                // Create DemandeArticle with the existing article
                var demandeArticle = new DemandeArticle
                {
                    DemandeId = demande.Id,
                    ArticleId = existingArticle.Id,
                    CreatedAt = DateTime.UtcNow,
                    Qtt = article.Quantity,
                    Status = "Updated"
                };
                _context.DemandeArticles.Add(demandeArticle);
            }

            await _context.SaveChangesAsync();
        }

        await LogDemandeHistory(model.userCode, demande.Id, $"{user.FirstName} {user.LastName} Updated articles for request {demande.Code}");

        return Ok("Articles updated successfully.");
    }


    [HttpGet("products-suggestions")]
    public async Task<IActionResult> GetSuggestions([FromQuery] string query, [FromQuery] string type)
    {
        ICollection<Article> suggestions = new List<Article>();

        switch (type)
        {
            case "article":
                suggestions = await _context.Articles.Where(a => a.Nom.Contains(query)).Distinct().ToListAsync();
                break;
            case "description":
                suggestions = await _context.Articles.Where(a => a.Description.Contains(query)).Distinct().ToListAsync();
                break;
            case "familleDeProduit":
                suggestions = await _context.Articles.Where(a => a.FamilleDeProduit.Contains(query)).Distinct().ToListAsync();
                break;
            case "destination":
                suggestions = await _context.Articles.Where(a => a.Destination.Contains(query)).Distinct().ToListAsync();
                break;
            default:
                break;
        }

        return Ok(suggestions);
    }

    [HttpGet("generate-code/{demandeurCode}")]
    public async Task<IActionResult> GenerateDemandeCode(int demandeurCode)
    {
        var demandeur = await _context.Users.FirstAsync(u => u.Code == demandeurCode);
        if (demandeur == null)
        {
            return NotFound();
        }

        string departmentCode = demandeur.Departement;
        int currentYear = DateTime.UtcNow.Year % 100; // Get the last two digits of the year
        string yearCode = currentYear.ToString("D2");

        int count = await _context.Demandes
                              .Where(d => d.Demandeur.Departement == departmentCode && d.OpenedAt.Value.Year % 100 == currentYear)
                              .CountAsync();
        string newCode = $"DA{departmentCode}{yearCode}{(count + 1):D4}";
        return Ok(new { Code = newCode });

    }

    [HttpPost]
    public async Task<IActionResult> CreateDemande([FromBody] CreateDemandeModel model)
    {
        var demandeur = _context.Users.SingleOrDefault(u => u.Id == model.DemandeurId && u.IsRequester);
        if (demandeur == null)
        {
            return NotFound();
        }

        var demande = new Demande
        {
            Code = model.Code,
            DemandeurId = model.DemandeurId,
            Status = DemandeStatus.Created,
            OpenedAt = DateTime.UtcNow,
            IsValidateurCFOValidated = false,
            IsValidateurCOOValidated = false,
            IsValidateurCFORejected = false,
            IsValidateurCOORejected = false
        };
        _context.Demandes.Add(demande);
        await _context.SaveChangesAsync();

        foreach (var article in model.Articles)
        {
            // Check if the article already exists
            var existingArticle = await _context.Articles.FirstOrDefaultAsync(
                a => a.Nom == article.Name &&
                a.Description == article.Description && 
                a.Destination == article.Destination && 
                a.FamilleDeProduit == article.FamilleDeProduit);

            // If article doesn't exist, create and save it
            if (existingArticle == null)
            {
                // Create DemandeArticle with the new article
                var demandeArticle = new DemandeArticle
                {
                    DemandeId = demande.Id,
                    CreatedAt = DateTime.UtcNow,
                    Qtt = article.Quantity,
                    Destination = article.Destination,
                    FamilleDeProduit = article.FamilleDeProduit,
                    Name = article.Name,
                    Description = article.Description,
                    Status = "Created",
                };
                _context.DemandeArticles.Add(demandeArticle);
            }
            else
            {
                // Create DemandeArticle with the existing article
                var demandeArticle = new DemandeArticle
                {
                    DemandeId = demande.Id,
                    ArticleId = existingArticle.Id,
                    CreatedAt = DateTime.UtcNow,
                    Qtt = article.Quantity,
                    Status = "Created"
                };
                _context.DemandeArticles.Add(demandeArticle);
            }
            await _context.SaveChangesAsync();
        }

        await LogDemandeHistory(model.DemandeurCode, demande.Id, $"Initial creation of request by {demandeur.FirstName} {demandeur.LastName}");
        return Ok("Request created");
    }

    [HttpGet("{demandeCode}/suppliers")]
    public async Task<IActionResult> GetOffers(string demandeCode)
    {
        var demande = await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }
        
        // Get the list of suppliers associated with the demande
        var suppliers = await _context.Fournisseurs
            .Select(s => new
            {
                s.Id,
                s.Nom,
                Offer = _context.DevisItems
                    .Where(o => o.DemandeArticle.Demande.Code == demandeCode && o.FournisseurId == s.Id)
                    .ToArray()
            })
            .ToListAsync();

        return Ok(suppliers);
    }

    [HttpGet("{demandeCode}/suppliers/{supplierId}")]
    public async Task<IActionResult> GetOffers(string demandeCode, int supplierId)
    {
        var demande = await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }

        var supplier = await _context.Fournisseurs.FirstAsync(f=> f.Id == supplierId);
        if (supplier == null)
        {
            return NotFound();
        }

        // Get the list of suppliers associated with the demande
        var supplierWithOffers = new
        {
            supplier.Id,
            supplier.Nom,
            Offer = _context.DevisItems
                    .Where(o => o.DemandeArticle.Demande.Code == demandeCode && o.FournisseurId == supplier.Id)
                    .FirstOrDefault()
        };

        return Ok(supplierWithOffers);
    }


    //[HttpPut("{id}/validate")]
    //public async Task<IActionResult> ValidateDemande(int id, [FromBody] int validateurId)
    //{
    //    var demande = await _context.Demandes.FindAsync(id);
    //    if (demande == null)
    //    {
    //        return NotFound();
    //    }

    //    var validator = await _context.Users.FindAsync(validateurId);

    //    if (validator.Departement == "CFO")
    //    {
    //        demande.IsValidateurCFOValidated = true;
    //        demande.ValidatedOrRejectedByCFOAt = DateTime.UtcNow;
    //        demande.ValidateurCFO = validator;
    //    }
    //    else if (validator.Departement == "COO")
    //    {
    //        demande.IsValidateurCOOValidated = true;
    //        demande.ValidatedOrRejectedByCFOAt = DateTime.UtcNow;
    //        demande.ValidateurCOO = validator;
    //    }
    //    else
    //    {
    //        return Forbid();
    //    }

    //    if (demande.IsValidateurCFOValidated == true && demande.IsValidateurCOOValidated == true)
    //    {
    //        demande.Status = DemandeStatus.Validated;
    //    }

    //    _context.Entry(demande).State = EntityState.Modified;
    //    await _context.SaveChangesAsync();

    //    await LogDemandeHistory(validator.Code, demande.Id, $"Validated by user {validator.FirstName} {validator.LastName}");

    //    return NoContent();
    //}

    //[HttpPut("{id}/reject")]
    //public async Task<IActionResult> RejectDemande(int id, [FromBody] int validateurId)
    //{
    //    var demande = await _context.Demandes.FindAsync(id);
    //    if (demande == null)
    //    {
    //        return NotFound();
    //    }

    //    var validator = await _context.Validateurs.FindAsync(validateurId);

    //    if (validator.Departement == "CFO")
    //    {
    //        demande.IsValidateurCFORejected = true;
    //        demande.ValidatedOrRejectedByCFOAt = DateTime.UtcNow;
    //        demande.ValidateurCFO = validator;
    //    }
    //    else if (validator.Departement == "COO")
    //    {
    //        demande.IsValidateurCOORejected = true;
    //        demande.ValidatedOrRejectedByCFOAt = DateTime.UtcNow;
    //        demande.ValidateurCOO = validator;
    //    }
    //    else
    //    {
    //        return Forbid();
    //    }

    //    _context.Entry(demande).State = EntityState.Modified;
    //    await _context.SaveChangesAsync();

    //    await LogDemandeHistory(validator.Code ,demande.Id, $"Rejected by user {validator.FirstName} {validator.LastName}");

    //    return NoContent();
    //}

    [HttpPut("{demandeCode}/cancel")]
    public async Task<IActionResult> CancelDemande(string demandeCode, [FromBody] CancelDemandeModel model)
    {
        var demande = await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }

        var user = GetUser(model.UserCode);

        if (user == null)
        {
            return NotFound();
        }

        if (user.IsRequester && !user.IsPurchaser)
        {
            if (demande.Status == DemandeStatus.Created)
            {
                demande.Status = DemandeStatus.Cancel;
                await _context.SaveChangesAsync();
                await LogDemandeHistory(user.Code, demande.Id, $"Cancelled by user {user.FirstName} {user.LastName}");
                return Ok("Request Cancelled");
            }
            else
            {
                return BadRequest("Request can't be cancelled");
            }
        }

        if (user.IsPurchaser)
        {
            if (demande.Status == DemandeStatus.WO)
            {
                demande.Status = DemandeStatus.Cancel;
                await _context.SaveChangesAsync();
                await LogDemandeHistory(user.Code, demande.Id, $"Cancelled by user {user.FirstName} {user.LastName}");
                return Ok("Request Cancelled");
            }
            else
            {
                return BadRequest("Request can't be cancelled");
            }
        }
        return NoContent();
    }

    [HttpGet("{demandeCode}/history")]
    public async Task<IActionResult> GetHistory(string demandeCode)
    {
        var demandeHisoty = await _context.DemandeHistories.Where(dh => dh.Demande.Code == demandeCode).ToListAsync();

        if (demandeHisoty == null)
        {
            return NotFound();
        }
        return Ok(demandeHisoty);
    }

    private User GetUser(int code)
    {
        var user = _context.Users.SingleOrDefault(u => u.Code == code);
        return user;
    }
}