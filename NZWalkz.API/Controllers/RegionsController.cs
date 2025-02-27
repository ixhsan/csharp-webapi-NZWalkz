using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalkz.API.Data;
using NZWalkz.API.Models.Domain;
using NZWalkz.API.Models.DTO;

namespace NZWalkz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(NZWalkzDbContext DbContext) : Controller
    {

        /// <summary>
        /// GET ALL REGIONS
        /// GET: http:localhost:port/api/Regions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllRegions()
        {
            // in-memory data example
            //var regionsDomain = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland Region",
            //        Code = "AKL",
            //        RegionImageUrl = "https://a.travel-assets.com/findyours-php/viewfinder/images/res70/179000/179003-North-Island.jpg"
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington Region",
            //        Code = "WLG",
            //        RegionImageUrl = "https://a.travel-assets.com/findyours-php/viewfinder/images/res70/179000/179180-Wellington.jpg"
            //    }
            //};

            //Get Data from DB - Domain Models
            var regionsDomain = DbContext.Regions.ToList();

            // Map Domain Model to DTOs
            var regionsDto = new List<RegionDto>();

            foreach(var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            // Return DTOs
            return Ok(regionsDto);
        }

        /// <summary>
        /// GET A SINGLE REGION
        /// GET: http:localhost:port/api/Regions/:id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {

            //var regionDomain = DbContext.Regions.Find(id);
            // Get Region Domain Model From DB
            var regionDomain = DbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain is null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }

        /// <summary>
        /// CREATE REGION
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomain = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain Model to Create Region
            DbContext.Regions.Add(regionDomain);
            DbContext.SaveChanges();

            // Map Domain Model back to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetRegionById), new {id = regionDto.Id}, regionDto);
        }
    }
}
