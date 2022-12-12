using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class SchoolInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int? AddressId { get; set; }
        public string Sologan { get; set; }
        public DateTime? EstablishDate { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }
    }
}
