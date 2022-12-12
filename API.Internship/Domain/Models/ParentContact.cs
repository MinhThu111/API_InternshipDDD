using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class ParentContact
    {
        public ParentContact()
        {
            StudentParentContacts = new HashSet<StudentParentContact>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public int AddressId { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual ICollection<StudentParentContact> StudentParentContacts { get; set; }
    }
}
