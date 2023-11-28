using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
           await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existingWalk= await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk);
           await  dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn=null,string? filterQuery =null,
            string? sortby=null,bool isAscending =true, int pageNumber = 1, int pageSize = 1000)
        {
            var walk= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false)
            {
                 if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walk = walk.Where(x => x.Name.Contains(filterQuery));
                }
            }
            //Sorting
            if(string.IsNullOrWhiteSpace(sortby)==false )
            {
                if (sortby.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walk= isAscending? walk.OrderBy(x=>x.Name):walk.OrderByDescending(x=>x.Name);
                }
                else if(sortby.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walk= isAscending? walk.OrderBy(x => x.LengthInKm) : walk.OrderByDescending(x => x.LengthInKm);
                }   
            }
            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walk.Skip(skipResults).Take(pageSize).ToListAsync();
        
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
          return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
           var existingWalk =await  dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            { 
                return null;
            }
            
            existingWalk.Name= walk.Name;
            existingWalk.Description= walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl= walk.WalkImageUrl;
            existingWalk.DifficultyId= walk.DifficultyId;
            existingWalk.RegionId= walk.RegionId;

            await dbContext.SaveChangesAsync();
            return existingWalk;

        }
    }
}
