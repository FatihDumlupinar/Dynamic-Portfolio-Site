using System.Collections.Generic;

namespace DynamicPortfolioSite.Entities.Models.About
{
    public class AboutEditModel
    {
        public int LocalizationId { get; set; }
        public string Text { get; set; }

        public List<AboutWorkListModel> Works { get; set; }
        public List<AboutSkillListModel> Skills { get; set; }
        public List<AboutEducationListModel> Educations { get; set; }
    }
}
