using System.ComponentModel.DataAnnotations;

namespace VCMS.Lib.Models
{
    public class ParentChildViewModel
    {
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int ChildId { get; set; }
    }
}
