using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class GradeStudent
    {
        public int Id { get; set; }
        public int? GradeId { get; set; }
        /// <summary>
        /// id lấy từ person
        /// </summary>
        public int? StudentId { get; set; }
        public int? PositionId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual Grade Grade { get; set; }
        public virtual Position Position { get; set; }
    }
}
