using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class StudentParentContact
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ParentContactId { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual ParentContact ParentContact { get; set; }
        public virtual Person Student { get; set; }
    }
}
