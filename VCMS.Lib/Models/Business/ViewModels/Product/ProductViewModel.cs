﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VCMS.Lib.Models
{
    public class ProductViewModel : BaseViewModel
    {
        public ProductViewModel()
        {
            imageFiles = new Dictionary<string, int>();
        }

        public new int Id { get; set; }
        [DataType(DataType.Upload)]

        [Required]
        public string previewImageId { get; set; }
        public string previewImage { get; set; }

        public Dictionary<string, int> imageFiles { get; set; }

        public bool Status { get; set; }

        public int[] Colors { get; set; }

        public int TypologyTypeId { get; set; }

        public string Size { get; set; }

        public string Weight { get; set; }

        public string Price { get; set; }

        public string Warranty { get; set; }

        public string MadeIn { get; set; }

        public string UnitType { get; set; }

        [Required]
        public int[] RoomIds { get; set; }

        public ICollection<Product_Variant> IColors { get; set; }

        public ICollection<File> IImages { get; set; }
    }
}