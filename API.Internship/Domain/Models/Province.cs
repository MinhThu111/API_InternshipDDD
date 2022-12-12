using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Province
    {
        public Province()
        {
            Districts = new HashSet<District>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameSlug { get; set; }
        public int? CountryId { get; set; }
        public string ProvinceCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public int? Status { get; set; }
        public DateTime Timer { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
