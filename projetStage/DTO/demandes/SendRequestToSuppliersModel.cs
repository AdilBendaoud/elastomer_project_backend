﻿namespace projetStage.DTO.demandes
{
    public class SendRequestToSuppliersModel
    {
        public int UserCode { get; set; }
        public string RequestCode { get; set; }
        public List<string> SupplierNames { get; set; }
    }

}
