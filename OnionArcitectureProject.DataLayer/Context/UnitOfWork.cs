using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using OnionArcitectureProject.Common;

namespace OnionArcitectureProject.DataLayer.Context
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHostingEnvironment _environment;

        public UniqData UniqData { get; }

        public UnitOfWork(ApplicationDbContext context,
            IHostEnvironment hostEnvironment,
            IHostingEnvironment environment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _environment = environment;
        }

        public Task<int> SaveChangesAsync()
            => _context.SaveChangesAsync();

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
            => _context.Set<TEntity>();

        public void Dispose()
            => _context.Dispose();

        public async void UploadImage(IFormFile file  , string fileName)
        {
            long totalBytes = file.Length;
            
            byte[] buffer = new byte[16 * 1024];
            using (FileStream output = System.IO.File.Create(GetPathAndFileName(fileName)))
            {
                using (Stream input = file.OpenReadStream())
                {
                    int readBytes;
                    while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, readBytes);
                        totalBytes += readBytes;
                    }
                }
            }
        }

        private string EnsureFileName(string fileName)
        {
            return Guid.NewGuid().ToString()+ Path.GetExtension(fileName);
        }

        private string GetPathAndFileName(string fileName)
        {
            string path = _hostEnvironment.ContentRootPath + "wwwroot\\User\\Images\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + fileName;
        }
    }
}
