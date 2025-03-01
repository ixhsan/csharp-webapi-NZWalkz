using AutoMapper;
using NZWalkz.API.Models.Domain;
using NZWalkz.API.Models.DTO;
namespace NZWalkz.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegionDto,Region>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        }


    }
}
