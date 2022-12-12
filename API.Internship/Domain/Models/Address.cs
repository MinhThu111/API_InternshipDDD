using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AddressNumber { get; set; }
        public string AddressText { get; set; }
        public int? CountryId { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }
    }
}
