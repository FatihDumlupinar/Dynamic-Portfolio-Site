using System;
using System.ComponentModel.DataAnnotations;

namespace DynamicPortfolioSite.Entities.Entities.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate{ get; set; }

        public int? UpdatedByUserId { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int LocalizationId { get; set; }

    }
}
