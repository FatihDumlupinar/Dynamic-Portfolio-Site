using DynamicPortfolioSite.Repository.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Repository.UnitOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IAboutRepository AboutRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectAndCategoryRepository ProjectAndCategoryRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IContactRepository ContactRepository { get; }
        public IBlogPostRepository BlogPostRepository { get; }
        public IWorkRepository WorkRepository { get; }
        public ISkillRepository SkillRepository { get; }
        public IEducationRepository EducationRepository { get; }
        public IAppUserRepository AppUserRepository { get; }

        Task CommitAsync();
    }
}
