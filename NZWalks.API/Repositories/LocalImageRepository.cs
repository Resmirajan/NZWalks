using Microsoft.AspNetCore.Http;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor,NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            HttpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath,
                "Images",$"{image.FileName}{image.FileExtension}");


            //Upload image to local path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

        //http://localhost:1234/images/image.jpg

            var urlFilePath = $"{HttpContextAccessor.HttpContext.Request.Scheme}:// {HttpContextAccessor.HttpContext.Request.Host}{HttpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;


            //Add Image to the image table
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;

        }
    }
}
