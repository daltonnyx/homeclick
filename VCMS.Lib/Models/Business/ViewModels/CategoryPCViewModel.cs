using System.ComponentModel.DataAnnotations;

namespace VCMS.Lib.Models.Business
{
    public class ParentChildViewModel
    {
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int ChildId { get; set; }
    }
}
