using DynamicPortfolioSite.Repository.Contexts;
using DynamicPortfolioSite.Repository.Repositories.Interfaces;
using DynamicPortfolioSite.Repository.UnitOfWork.Interfaces;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.UnitOfWork.Methods
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Ctor&Fields

        private readonly AppDbContext _appDbContext;

        private readonly IAboutRepository _aboutRepository;

        private readonly IProjectRepository _projectRepository;

        private readonly IProjectAndCategoryRepository _projectAndCategoryRepository;

        private readonly ICategoryRepository _categoryRepository;

        private readonly IContactRepository _contactRepository;

        private readonly IBlogPostRepository _blogPostRepository;

        private readonly IWorkRepository _workRepository;

        private readonly ISkillRepository _skillRepository;

        private readonly IEducationRepository _educationRepository;

        private readonly IAppUserRepository _appUserRepository;

        public UnitOfWork(AppDbContext appDbContext, IAboutRepository aboutRepository, IProjectRepository projectRepository, IProjectAndCategoryRepository projectAndCategoryRepository, ICategoryRepository categoryRepository, IContactRepository contactRepository, IBlogPostRepository blogPostRepository, IWorkRepository workRepository, ISkillRepository skillRepository, IEducationRepository educationRepository, IAppUserRepository appUserRepository)
        {
            _appDbContext = appDbContext;
            _aboutRepository = aboutRepository;
            _projectRepository = projectRepository;
            _projectAndCategoryRepository = projectAndCategoryRepository;
            _categoryRepository = categoryRepository;
            _contactRepository = contactRepository;
            _blogPostRepository = blogPostRepository;
            _workRepository = workRepository;
            _skillRepository = skillRepository;
            _educationRepository = educationRepository;
            _appUserRepository = appUserRepository;
        }

        #endregion

        #region Repositories

        public IAboutRepository AboutRepository => _aboutRepository;

        public IProjectRepository ProjectRepository => _projectRepository;

        public IProjectAndCategoryRepository ProjectAndCategoryRepository => _projectAndCategoryRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public IContactRepository ContactRepository => _contactRepository;

        public IBlogPostRepository BlogPostRepository => _blogPostRepository;

        public IWorkRepository WorkRepository => _workRepository;

        public ISkillRepository SkillRepository => _skillRepository;

        public IEducationRepository EducationRepository => _educationRepository;

        public IAppUserRepository AppUserRepository => _appUserRepository;

        #endregion

        public async Task CommitAsync()
        {
            _ = await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
