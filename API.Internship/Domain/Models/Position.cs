using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PositionCode { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual GradeStudent IdNavigation { get; set; }
    }
}
