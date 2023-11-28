using NZWalks.API.Model.Domain;
using System.Net;

namespace NZWalks.API.Repositories
{
    public interface IImageRepository
    {
        Task <Image>Upload(Image image);
    }
}
