using NZWalks.API.Model.Domain;
using System.Runtime.InteropServices;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>>  GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);//nullable
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);//nullable
        Task <Region?>DeleteAsync(Guid id);
    }
}
