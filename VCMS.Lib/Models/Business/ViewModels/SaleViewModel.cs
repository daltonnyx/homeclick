namespace VCMS.Lib.Models.Business
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SaleViewModel : BaseViewModel
    {
        public int saleId { get; set; }

        public int? percent { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}
