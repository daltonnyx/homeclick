using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VCMS.Lib.Models.Business
{
    public class PostViewModel
    {
        public int postId { get; set; }

        [Required]
        [MaxLength(512)]
        public string title { get; set; }

        [Required]
        public int[] categoryIds { get; set; }

        [Required]
        public string previewImageId { get; set; }

        public string previewImage { get; set; }

        [Required]
        public string excerpt { get; set; }

        [AllowHtml]
        public string htmlContent { get; set; }

        public bool status { get; set; }
    }
}
