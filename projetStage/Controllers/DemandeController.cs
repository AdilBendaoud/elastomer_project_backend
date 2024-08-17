using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetStage.Data;
using projetStage.DTO;
using projetStage.DTO.demandes;
using projetStage.Helper;
using projetStage.Models;
using projetStage.Services;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
// Controllers/DemandeController.cs
[Route("api/[controller]")]
[ApiController]
public class DemandeController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public DemandeController(AppDbContext context, IEmailService emailService)
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

    [HttpGet]
    public async Task<IActionResult> GetAllDemandes(
        int userCode,
        int pageNumber = 1,
        int pageSize = 10,
        [FromQuery] string search = null,
        [FromQuery] string status = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null
    )
    {
        User user = GetUser(userCode);
        if (user == null)
        {
            return NotFound("User not found!");
        }

        var demandes = _context.Demandes.Include(d => d.Demandeur).AsQueryable();

        if (user.IsRequester && !user.IsPurchaser && !user.IsAdmin)
        {
            demandes = demandes.Where(d => d.Code.Contains(user.Departement));
        }

        if (user.IsValidator)
        {
            if(user.Departement == "CFO")
            {
                demandes = demandes.Where(d => d.Status == DemandeStatus.WV || d.Status == DemandeStatus.COOValidated);
            }

            if (user.Departement == "COO")
            {
                demandes = demandes.Where(d => d.Status == DemandeStatus.WV || d.Status == DemandeStatus.CFOValidated);
            }
        }

        if (!string.IsNullOrEmpty(search))
        {
            demandes = demandes.Where(d => d.Code.Contains(search) ||
                                           d.Demandeur.FirstName.Contains(search) ||
                                           d.Demandeur.LastName.Contains(search));
        }

        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse(status, out DemandeStatus demandeStatus))
            {
                demandes = demandes.Where(d => d.Status == demandeStatus);
            }
        }

        if (startDate.HasValue)
        {
            demandes = demandes.Where(d => d.OpenedAt >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            demandes = demandes.Where(d => d.OpenedAt <= endDate.Value);
        }

        //if (!string.IsNullOrEmpty(sort))
        //{
        //    var sortParams = sort.Split(',');
        //    var sortProperty = sortParams[0];
        //    var sortOrder = sortParams.Length > 1 && sortParams[1].ToLower() == "desc" ? "desc" : "asc";
        //    demandes = demandes.OrderBy($"{sortProperty} {sortOrder}");
        //}
        //else
        //{
        //    demandes = demandes.OrderBy(d => d.OpenedAt);
        //}

        var totalDemandes = await demandes.CountAsync();
        var paginatedDemandes = await demandes
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new DemandeDto
            {
                Id = d.Id,
                Code = d.Code,
                Status = d.Status,
                OpenedAt = d.OpenedAt,
                LastModification = d.LastModification,
                Demandeur = new DemandeurDto
                {
                    Id = d.Demandeur.Id,
                    Code = d.Demandeur.Code,
                    FirstName = d.Demandeur.FirstName,
                    LastName = d.Demandeur.LastName,
                    Email = d.Demandeur.Email,
                    Departement = d.Demandeur.Departement
                }
            }).OrderByDescending(d=> d.LastModification)
            .ToListAsync();

        var response = new PaginatedResponse<DemandeDto>
        {
            TotalCount = totalDemandes,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            Items = paginatedDemandes
        };

        return Ok(response);
    }

    [HttpGet("{demandeCode}/articles")]
    public async Task<IActionResult> GetArticles(string demandeCode)
    {
        var demande = await _context.Demandes
            .Include(d => d.DemandeArticles)
            .FirstOrDefaultAsync(d => d.Code == demandeCode);

        if (demande == null)
        {
            return NotFound("Demande not found.");
        }

        var articles = demande.DemandeArticles
            .Select(da => new
            {
                da.Id,
                Name = da.Name,
                Description = da.Description,
                Quantity = da.Qtt,
                FamilleDeProduit = da.FamilleDeProduit,
                Destination = da.Destination,
                PurchaseOrder = da.BonCommande
            })
            .ToList();

        if (articles == null || !articles.Any())
        {
            return NotFound("No articles found.");
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
         
        // Add updated articles
        foreach (var article in model.Articles)
        {
            // Check if the article already exists
            var existingArticle = await _context.DemandeArticles.FindAsync(article.Id);

            // If article doesn't exist, create and save it
            if (existingArticle == null)
            {

                // Create DemandeArticle with the new article
                var demandeArticle = new DemandeArticle
                {
                    DemandeId = demande.Id,
                    Name = article.Name,
                    Description = article.Description,
                    Destination = article.Destination,
                    FamilleDeProduit = article.FamilleDeProduit,
                    CreatedAt = DateTime.Now,
                    Qtt = article.Quantity
                };
                await _context.DemandeArticles.AddAsync(demandeArticle);
            }
            else
            {
                // Create DemandeArticle with the existing article

                existingArticle.Name = article.Name;
                existingArticle.Description = article.Description;
                existingArticle.Destination = article.Destination;
                existingArticle.Qtt = article.Quantity;

                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
        }

        await LogDemandeHistory(model.userCode, demande.Id, $"{user.FirstName} {user.LastName} Updated articles for request {demande.Code}");

        return Ok("Articles updated successfully.");
    }

    [HttpPut("{demandeCode}/add-purchase-order/{userCode}")]
    public async Task<IActionResult> AddPurchaseOrder(int userCode, string demandeCode, [FromBody] List<AddPurchaseOrderModel> model)
    {
        var demande = await _context.Demandes.FirstOrDefaultAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }

        var user = GetUser(userCode);

        // Get all articles related to the demande
        var allArticles = await _context.DemandeArticles
            .Where(da => da.DemandeId == demande.Id && da.Name != "Delivery Fee")
            .OrderBy(da => da.Id) // Ensure order
            .ToListAsync();

        // Iterate through the articles and apply the PO logic
        string lastAppliedPO = null;

        foreach (var article in allArticles)
        {
            // Find if there's a specific PO provided in the input model
            var inputArticle = model.FirstOrDefault(a => a.Id == article.Id);

            if (inputArticle != null && !string.IsNullOrEmpty(inputArticle.PurchaseOrder))
            {
                // If the input has a PO, apply it
                article.BonCommande = inputArticle.PurchaseOrder;
                lastAppliedPO = inputArticle.PurchaseOrder; // Update the last applied PO
            }
            else if (string.IsNullOrEmpty(article.BonCommande))
            {
                // If the article doesn't have a PO and no specific PO was provided, apply the last applied PO
                article.BonCommande = lastAppliedPO;
            }
        }

        await LogDemandeHistory(user.Code, demande.Id, $"PO added by user {user.FirstName} {user.LastName}");
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("products-suggestions")]
    public async Task<IActionResult> GetSuggestions([FromQuery] string query, [FromQuery] string type)
    {
        var articleSuggestions = new List<Article>();
        var demandeArticleSuggestions = new List<Article>();

        switch (type)
        {
            case "article":
                articleSuggestions = await _context.Articles
                    .Where(a => a.Nom.Contains(query))
                    .ToListAsync();

                demandeArticleSuggestions = await _context.DemandeArticles
                    .Where(da => da.Name.Contains(query))
                    .Select(da => new Article
                    {
                        Id = da.Id,
                        Nom = da.Name,
                        Description = da.Description,
                        FamilleDeProduit = da.FamilleDeProduit,
                        Destination = da.Destination
                    })
                    .ToListAsync();

                var uniqueArticles = articleSuggestions
                    .Concat(demandeArticleSuggestions)
                    .GroupBy(a => new { a.Nom, a.Description })
                    .Select(g => g.First())
                    .ToList();

                return Ok(uniqueArticles);

            case "description":
                articleSuggestions = await _context.Articles
                    .Where(a => a.Description.Contains(query))
                    .ToListAsync();

                demandeArticleSuggestions = await _context.DemandeArticles
                    .Where(da => da.Description.Contains(query))
                    .Select(da => new Article
                    {
                        Id = da.Id,
                        Nom = da.Name,
                        Description = da.Description,
                        FamilleDeProduit = da.FamilleDeProduit,
                        Destination = da.Destination
                    })
                    .ToListAsync();

                var uniqueDescriptions = articleSuggestions
                    .Concat(demandeArticleSuggestions)
                    .GroupBy(a => a.Description)
                    .Select(g => g.First())
                    .ToList();

                return Ok(uniqueDescriptions);

            case "familleDeProduit":
                articleSuggestions = await _context.Articles
                    .Where(a => a.FamilleDeProduit.Contains(query))
                    .ToListAsync();

                demandeArticleSuggestions = await _context.DemandeArticles
                    .Where(da => da.FamilleDeProduit.Contains(query))
                    .Select(da => new Article
                    {
                        Id = da.Id,
                        Nom = da.Name,
                        Description = da.Description,
                        FamilleDeProduit = da.FamilleDeProduit,
                        Destination = da.Destination
                    })
                    .ToListAsync();

                var uniqueFamilles = articleSuggestions
                    .Concat(demandeArticleSuggestions)
                    .GroupBy(a => a.FamilleDeProduit)
                    .Select(g => g.First())
                    .ToList();

                return Ok(uniqueFamilles);

            case "destination":
                articleSuggestions = await _context.Articles
                    .Where(a => a.Destination.Contains(query))
                    .ToListAsync();

                demandeArticleSuggestions = await _context.DemandeArticles
                    .Where(da => da.Destination.Contains(query))
                    .Select(da => new Article
                    {
                        Id = da.Id,
                        Nom = da.Name,
                        Description = da.Description,
                        FamilleDeProduit = da.FamilleDeProduit,
                        Destination = da.Destination
                    })
                    .ToListAsync();

                var uniqueDestinations = articleSuggestions
                    .Concat(demandeArticleSuggestions)
                    .GroupBy(a => a.Destination)
                    .Select(g => g.First())
                    .ToList();

                return Ok(uniqueDestinations);

            default:
                return BadRequest("Invalid type parameter.");
        }
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
                              .Where(d => d.Code.Contains(departmentCode) && d.OpenedAt.Value.Year % 100 == currentYear)
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
            OpenedAt = DateTime.Now,
            IsValidateurCFOValidated = false,
            IsValidateurCOOValidated = false,
            IsValidateurCFORejected = false,
            IsValidateurCOORejected = false
        };
        _context.Demandes.Add(demande);
        await _context.SaveChangesAsync();

        // Add each article to the request
        foreach (var article in model.Articles)
        {
            var demandeArticle = new DemandeArticle
            {
                DemandeId = demande.Id,
                CreatedAt = DateTime.Now,
                Qtt = article.Quantity,
                Destination = article.Destination,
                FamilleDeProduit = article.FamilleDeProduit,
                Name = article.Name,
                BonCommande = null,
                Description = article.Description
            };
            _context.DemandeArticles.Add(demandeArticle);
        }

        // Add the delivery price as a separate row
        var deliveryArticle = new DemandeArticle
        {
            DemandeId = demande.Id,
            CreatedAt = DateTime.Now,
            Qtt = 1,
            Name = "Delivery Fee",
            Description = "Delivery fee for the request",
            FamilleDeProduit = "Service",
            Destination = "N/A",
            BonCommande = null
        };
        _context.DemandeArticles.Add(deliveryArticle);

        await _context.SaveChangesAsync();

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

        var suppliersRequestSentTo = await _context.SupplierRequests
                    .Where(sr=> sr.Demande.Code == demandeCode)
                    .Select(s => new
                    {
                        s.Supplier.Id,
                        s.Supplier.Nom,
                        s.isSelectedForValidation,
                        Offer = _context.DevisItems.Where(o => o.DemandeArticle.Demande.Code == demandeCode && o.FournisseurId == s.SupplierId)
                        .ToArray()
                    })
            .ToListAsync();

        return Ok(suppliersRequestSentTo);
    }

    [HttpGet("{demandeCode}")]
    public async Task<IActionResult> GetDetails(string demandeCode) 
    {
        var demande = await _context.Demandes.FirstAsync(d=> d.Code == demandeCode);
        if(demande == null)
            { return NotFound(); }
        return Ok(demande);
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

    [HttpPut("{requestCode}/validate/{userCode}")]
    public async Task<IActionResult> ValidateDemande(string requestCode, int userCode)
    {
        var demande = await _context.Demandes.Include(d=> d.DemandeArticles).FirstAsync(d => d.Code == requestCode);
        if (demande == null)
        {
            return NotFound();
        }

        var validator = await _context.Users.FirstAsync(u => u.Code == userCode);

        if (validator.Departement == "CFO")
        {
            demande.IsValidateurCFOValidated = true;
            demande.IsValidateurCFORejected = false;
            demande.ValidatedOrRejectedByCFOAt = DateTime.Now;
            demande.ValidateurCFO = validator;
            demande.Status = DemandeStatus.CFOValidated;
        }
        else if (validator.Departement == "COO")
        {
            demande.IsValidateurCOOValidated = true;
            demande.IsValidateurCOORejected = false;
            demande.ValidatedOrRejectedByCOOAt = DateTime.Now;
            demande.ValidateurCOO = validator;
            demande.Status = DemandeStatus.COOValidated;
        }
        else
        {
            return Forbid();
        }

        if (demande.IsValidateurCFOValidated == true && demande.IsValidateurCOOValidated == true)
        {
            demande.Status = DemandeStatus.Validated;
        }

        _context.Entry(demande).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        await LogDemandeHistory(validator.Code, demande.Id, $"Validated by user {validator.FirstName} {validator.LastName}");

        var supplierRequests = _context.SupplierRequests
            .Where(sr => sr.DemandeId == demande.Id)
            .Include(sr => sr.Supplier)
            .ToList();
        var suppliers = supplierRequests.Select(sr => sr.Supplier).Distinct().ToList();

        // Fetch devis items for these suppliers specific to the demande
        var devisItems = await _context.DevisItems
            .Where(di => di.DemandeArticle.DemandeId == demande.Id && supplierRequests.Select(sr => sr.SupplierId).Contains(di.FournisseurId))
            .ToListAsync();

        // Retrieve all purchasers' emails
        var purchaserEmails = await _context.Users
            .Where(u => u.IsPurchaser && u.IsActive)
            .Select(u => u.Email)
            .ToListAsync();

        // Generate HTML table
        var currencies = _context.Currencies.ToList();
        var exchangeRates = new Dictionary<string, float> {
                { "EUR", currencies.First(c=> c.CurrencyCode == "EUR").PriceInEur },
                { "USD", currencies.First(c=> c.CurrencyCode == "USD").PriceInEur },
                { "MAD", currencies.First(c=> c.CurrencyCode == "MAD").PriceInEur },
                { "GBP", currencies.First(c=> c.CurrencyCode == "GBP").PriceInEur }};
        var htmlTable = HTMLTableGenerator.GenerateHtmlTable(demande, devisItems, supplierRequests, exchangeRates);
        var subject = "New Request Has Been Validated";
        var body = $"Request with code {requestCode} has been validated by {validator.FirstName} {validator.LastName}.<br><br>{htmlTable}";

        await _emailService.SendEmailAsync(subject, body, purchaserEmails, validator.Email);

        return Ok();
    }

    [HttpPut("{requestCode}/reject/{userCode}")]
    public async Task<IActionResult> RejectDemande(string requestCode, int userCode)
    {
        var demande = await _context.Demandes.FirstAsync(d => d.Code == requestCode);
        if (demande == null)
        {
            return NotFound();
        }

        var validator = await _context.Users.FirstAsync(u => u.Code == userCode);

        if (validator.Departement == "CFO" || validator.Departement == "COO")
        {
            demande.Status = DemandeStatus.Rejected;
            demande.IsValidateurCFORejected = true;
            demande.IsValidateurCFOValidated = false;
            demande.IsValidateurCOOValidated = false;
            demande.ValidatedOrRejectedByCFOAt = DateTime.Now;
            demande.ValidateurCFO = validator;
        }
        else if (validator.Departement == "COO")
        {
            demande.Status = DemandeStatus.Rejected;
            demande.IsValidateurCOORejected = true;
            demande.IsValidateurCFOValidated = false;
            demande.IsValidateurCOOValidated = false;
            demande.ValidatedOrRejectedByCFOAt = DateTime.Now;
            demande.ValidateurCOO = validator;
        }
        else
        {
            return Forbid();
        }

        _context.Entry(demande).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        await LogDemandeHistory(validator.Code, demande.Id, $"Rejected by user {validator.FirstName} {validator.LastName}");

        return Ok();
    }

    [HttpPut("{demandeCode}/cancel")]
    public async Task<IActionResult> CancelDemande(string demandeCode, [FromBody] CancelDemandeModel model)
    {
        var demande = await _context.Demandes.Include(d=> d.Demandeur).FirstAsync(d => d.Code == demandeCode);
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

        if (user.IsPurchaser && (demande.Status == DemandeStatus.WO || demande.Demandeur.IsPurchaser))
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

    [HttpPut("{demandeCode}/done/{userCode}")]
    public async Task<IActionResult> MarkAsDone(string demandeCode, int userCode)
    {
        var demande = await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }

        var user = GetUser(userCode);

        if (user == null)
        {
            return NotFound();
        }

        if (user.IsPurchaser)
        {
            demande.Status = DemandeStatus.Done;
            await _context.SaveChangesAsync();
            await LogDemandeHistory(userCode, demande.Id, $"Request Mark as done by {user.FirstName} {user.LastName}");
        }
        return Ok();
    }

    [HttpPost("{demandeCode}/comment")]
    public async Task<IActionResult> AddComment(string demandeCode, [FromBody] AddCommentModel model)
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

        if (user.IsValidator)
        {
            if(user.Departement == "COO")
            {
                demande.CommentCOO = model.Comment;
                await _context.SaveChangesAsync();
                await LogDemandeHistory(user.Code, demande.Id, $"{user.FirstName} {user.LastName} added a comment");
                return Ok();
            }
            if(user.Departement == "CFO")
            {
                demande.CommentCFO = model.Comment;
                await _context.SaveChangesAsync();
                await LogDemandeHistory(user.Code, demande.Id, $"{user.FirstName} {user.LastName} added a comment");
                return Ok();
            }
        }

        return BadRequest();
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

    [HttpPut("{demandeCode}/set-wv/{userCode}")]
    public async Task<IActionResult> SetWaitingValidation(string demandeCode, int userCode)
    {
        var demande = await _context.Demandes.FirstAsync(d => d.Code == demandeCode);
        if (demande == null)
        {
            return NotFound();
        }
        demande.Status = DemandeStatus.WO;
        demande.ValidateurCFOId = null;
        demande.ValidateurCOOId = null;
        demande.CommentCFO = null;
        demande.CommentCOO = null;
        demande.IsValidateurCOOValidated = false;
        demande.IsValidateurCFOValidated = false;
        demande.IsValidateurCOORejected = false;
        demande.IsValidateurCFORejected = false;
        demande.ValidatedOrRejectedByCFOAt = null;
        demande.ValidatedOrRejectedByCOOAt = null;

        var user = GetUser(userCode);

        if (user == null)
        {
            return NotFound();
        }

        await LogDemandeHistory(user.Code, demande.Id, $"{user.FirstName} {user.LastName} reopened the request for validation");
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("cheapest-offers")]
    public async Task<IActionResult> GetCheapestOffers([FromQuery] string name, [FromQuery] string description)
    {
        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(description))
        {
            return BadRequest("Name or description must be provided.");
        }

        // Fetch the matching articles
        var matchingArticles = await _context.DemandeArticles
            .Where(da => (string.IsNullOrEmpty(name) || da.Name.Contains(name)) &&
                         (string.IsNullOrEmpty(description) || da.Description.Contains(description)))
            .Select(da => da.Id)
            .ToListAsync();

        if (!matchingArticles.Any())
        {
            return NotFound("No matching articles found.");
        }

        // Fetch the cheapest offers for the matching articles
        var cheapestOffers = await _context.DevisItems
            .Where(di => matchingArticles.Contains(di.DemandeArticleId))
            .OrderBy(di => di.UnitPrice)
            .Take(10)
            .Select(di => new
            {
                SupplierName = di.Fournisseur.Nom,
                UnitPrice = di.UnitPrice,
                Devise = di.Devise
            })
            .ToListAsync();

        return Ok(cheapestOffers);
    }

    private User GetUser(int code)
    {
        var user = _context.Users.SingleOrDefault(u => u.Code == code);
        return user;
    }
}