using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Grade
    {
        public Grade()
        {
            GradeStudents = new HashSet<GradeStudent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string GradeCode { get; set; }
        /// <summary>
        /// Id lấy từ person. Xác định GVCN
        /// </summary>
        public int? TeacherId { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual Person Teacher { get; set; }
        public virtual ICollection<GradeStudent> GradeStudents { get; set; }
    }
}
