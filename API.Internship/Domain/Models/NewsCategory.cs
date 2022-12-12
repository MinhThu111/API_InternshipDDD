using System;
using System.Collections.Generic;

namespace API.Internship.Domain.Models
{
    public partial class NewsCategory
    {
        public NewsCategory()
        {
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 0:Giới thiệu;1:Tin tức;2:Thông báo
        /// </summary>
        public int? Type { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime Timer { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
