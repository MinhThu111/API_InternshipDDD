using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Person
    {
        public Person()
        {
            Grades = new HashSet<Grade>();
            Scores = new HashSet<Score>();
            StudentParentContacts = new HashSet<StudentParentContact>();
            TeacherSubjects = new HashSet<TeacherSubject>();
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LastNameSlug { get; set; }
        public string FirstNameSlug { get; set; }
        public DateTime? Birthday { get; set; }
        public int? Gender { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// Dùng để xác định GV hay HS
        /// </summary>
        public int PersonTypeId { get; set; }
        public int? NationalityId { get; set; }
        public int? ReligionId { get; set; }
        public int? FolkId { get; set; }
        public int? AddressId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual PersonType PersonType { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<StudentParentContact> StudentParentContacts { get; set; }
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }
}
