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
        public IActionResult Index()
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

            foreach (var region in regionsDomain)
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
        /// GET SINGLE REGION
        /// GET: http:localhost:port/api/Regions/:id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
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
        /// POST CREATE REGION
        /// POST: http:localhost:port/api/Regions
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
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

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        /// <summary>
        /// PUT UPDATE REGION
        /// PUT: http:localhost:port/api/Regions/:id
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if Region Exist
            var regionDomain = DbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain is null)
            {
                return NotFound();
            }


            if (!String.IsNullOrEmpty(updateRegionRequestDto.Name))
            {
                regionDomain.Name = updateRegionRequestDto.Name;
            }

            if (!String.IsNullOrEmpty(updateRegionRequestDto.Code))
            {
                regionDomain.Code = updateRegionRequestDto.Code;
            }

            if (!String.IsNullOrEmpty(updateRegionRequestDto.RegionImageUrl))
            {
                regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            }

            // Use Domain Model to Update Region

            DbContext.SaveChanges();

            // Map Domain Model back to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            // Return DTO
            return Ok(regionDto);
        }

        /// <summary>
        /// DELETE DELETE REGION
        /// DELETE: http:localhost:port/api/Regions/:id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute]Guid id)
        {
            var regionDomainModel = DbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomainModel is null)
            {
                return NotFound();
            }

            // Delete Region
            DbContext.Regions.Remove(regionDomainModel);
            DbContext.SaveChanges();

            // Return Deleted region back
            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
