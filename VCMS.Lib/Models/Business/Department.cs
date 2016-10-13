namespace VCMS.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        public Department()
        {
            ChildDepartments = new HashSet<Department>();
            Floors = new HashSet<Floor>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string Desciption { get; set; }

        public int? ParentDepartmentId { get; set; }

        public virtual ICollection<Department> ChildDepartments { get; set; }

        public virtual Department ParentDepartment { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }
    }
}
