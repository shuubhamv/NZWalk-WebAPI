using Microsoft.EntityFrameworkCore;
using NZWalk.Api.Data;
using NZWalk.Api.Models.Domain;

namespace NZWalk.Api.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            try
            {

                await dbContext.Regions.AddAsync(region);
                await dbContext.SaveChangesAsync();
                return region;
            }
            catch (Exception ex)
            {

                throw new Exception ("faild to create:",ex);
            }
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            try
            {
                var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.id == id);
                if (existingRegion == null)
                {
                    return null;
                }
                dbContext.Regions.Remove(existingRegion);
                await dbContext.SaveChangesAsync();
                return existingRegion;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<List<Region>> GetAllAsync()
        {
            try
            {

                return await dbContext.Regions.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            try
            {
                return await dbContext.Regions.FirstOrDefaultAsync(x => x.id == id);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            try
            {
                var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.id == id);
                if (existingRegion == null)

                {
                    return null;
                }
                existingRegion.Code = region.Code;
                existingRegion.Name = region.Name;
                existingRegion.RegionImageUrl = region.RegionImageUrl;
                await dbContext.SaveChangesAsync();
                return existingRegion;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
