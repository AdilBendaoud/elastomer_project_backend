using Microsoft.AspNetCore.Mvc;

namespace projetStage.DTO.demandes
{
    public class RequestForValidation
    {
        public int userCode { get; set; }
        public string demandeCode { get; set; }
        public int supplierId { get; set; } 
    }
}
