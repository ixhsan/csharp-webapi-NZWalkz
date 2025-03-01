using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalkz.API.Data;
using NZWalkz.API.Models.Domain;
using NZWalkz.API.Models.DTO;
using NZWalkz.API.Repositories;

namespace NZWalkz.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController(IRegionRepository regionRepository, IMapper mapper) : Controller
    {

        /// <summary>
        /// GET ALL REGIONS
        /// GET: http:localhost:port/api/Regions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            //Get Data from DB - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Model to DTOs using mapper -> List<RegionDto> as the end type
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

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
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            //var regionDomain = DbContext.Regions.Find(id);
            // Get Region Domain Model From DB
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain is null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO using AutoMapper
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto);
        }

        /// <summary>
        /// POST CREATE REGION
        /// POST: http:localhost:port/api/Regions
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            // Map or Convert DTO to Domain Model using AutoMapper
            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to CreateAsync Region
            regionDomain = await regionRepository.CreateAsync(regionDomain);

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // Map or Convert DTO to Domain Model using AutoMapper
            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);

            // Pass to region repository
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);

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
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel is null)
            {
                return NotFound();
            }

            // Map or Convert DTO to Domain Model using AutoMapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
