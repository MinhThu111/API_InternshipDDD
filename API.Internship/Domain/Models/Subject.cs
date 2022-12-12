using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Scores = new HashSet<Score>();
            TeacherSubjects = new HashSet<TeacherSubject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string SubjectCode { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}
