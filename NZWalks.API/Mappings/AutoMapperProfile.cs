using AutoMapper;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Region,RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto,Region>().ReverseMap();
            CreateMap<AddWalksRequestDto,Walk>().ReverseMap();
            CreateMap<WalkDto,Walk>().ReverseMap();
            CreateMap<DifficultyDto,Difficulty>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();
        }

    }
}
