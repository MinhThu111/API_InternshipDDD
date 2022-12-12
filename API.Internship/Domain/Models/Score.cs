using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Score
    {
        public int Id { get; set; }
        public int? Score1 { get; set; }
        public int? ScoreTypeId { get; set; }
        public int? SubjectId { get; set; }
        public int StudentId { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual ScoreType ScoreType { get; set; }
        public virtual Person Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
