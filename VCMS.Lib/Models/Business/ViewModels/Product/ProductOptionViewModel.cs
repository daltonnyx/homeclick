namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOptionViewModel
    {
        public int id { get; set; }

        [StringLength(128)]
        public string name { get; set; }

        public string description { get; set; }

        [StringLength(128)]
        public string previewImageId { get; set; }

        public string previewImage { get; set; }

        public int? productId { get; set; }

        public bool status { get; set; }

        public string productName { get; set; }

        public Dictionary<string, int[]> variants { get; set; }

        public string[] imageFiles { get; set; }

        public Dictionary<string, string> details { get; set; }

    }
}
