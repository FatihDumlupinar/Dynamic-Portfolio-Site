using DynamicPortfolioSite.Entities.Models.Category;
using System;
using System.Collections.Generic;

namespace DynamicPortfolioSite.Entities.Models.Project
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string SrcUrl { get; set; }
        public string Img { get; set; }

        public int LocalizationId { get; set; }

        public List<CategoryModel> Categories { get; set; } = new();

        public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
