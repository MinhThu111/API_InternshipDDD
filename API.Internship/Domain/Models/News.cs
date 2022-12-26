using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class News
    {
        public int Id { get; set; }
        public int? NewsCategoryId { get; set; }
        public string Title { get; set; }
        public string TitleSlug { get; set; }
        /// <summary>
        /// id lấy từ person. Xác định GVCN
        /// </summary>
        public string Description { get; set; }
        public string Detail { get; set; }
        public string AvatarUrl { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual NewsCategory NewsCategory { get; set; }
    }
}
