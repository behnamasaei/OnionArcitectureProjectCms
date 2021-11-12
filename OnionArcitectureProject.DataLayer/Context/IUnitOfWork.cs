using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace OnionArcitectureProject.DataLayer.Context
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// More information
        /// https://dev.to/moe23/step-by-step-repository-pattern-and-unit-of-work-with-asp-net-core-5-3l92
        /// </summary>


        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();


        void UploadImage(IFormFile file , string fileName);
    }
}
