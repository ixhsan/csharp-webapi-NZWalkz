using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalkz.API.Data;
using NZWalkz.API.Models.Domain;

namespace NZWalkz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(NZWalkzDbContext DbContext) : Controller
    {

        /// <summary>
        /// Get All Regions
        /// </summary>
        /// <returns></returns>
        /// GET: http:localhost:port/api/regions
        [HttpGet]
        public IActionResult GetAllRegions()
        {
            // dummy
            //var regions = new List<Region>
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

            var regions = DbContext.Regions.ToList();

            return Ok(regions);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {
            var region = DbContext.Regions.Find(id);

            if (region is null)
            {
                return NotFound();
            }

            return Ok(region);
        }
    }
}
