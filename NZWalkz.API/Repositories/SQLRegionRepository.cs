using Microsoft.EntityFrameworkCore;
using NZWalkz.API.Data;
using NZWalkz.API.Models.Domain;
using NZWalkz.API.Models.DTO;

namespace NZWalkz.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        public SQLRegionRepository(NZWalkzDbContext nZWalkzDbContext) => DbContext = nZWalkzDbContext;

        public NZWalkzDbContext DbContext { get; }

        public async Task<List<Region>> GetAllAsync()
        {
            return await DbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await DbContext.Regions.AddAsync(region);
            await DbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion is null)
            {
                return null;
            }

            if (!String.IsNullOrEmpty(region.Name))
            {
                existingRegion.Name = region.Name;
            }

            if (!String.IsNullOrEmpty(region.Code))
            {
                existingRegion.Code = region.Code;
            }

            if (!String.IsNullOrEmpty(region.RegionImageUrl))
            {
                existingRegion.RegionImageUrl = region.RegionImageUrl;
            }

            await DbContext.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion is null)
            {
                return null;
            }

            DbContext.Regions.Remove(existingRegion);
            await DbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
