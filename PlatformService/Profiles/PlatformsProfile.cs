using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // Source --> Target
            // Database --> Model --> DTO
            
            // 1. Now since our Platform Class and PlatformReadDto Class names are same, AutoMapper will automatically map them
            // We will see other examples where this is not the case - so we will instruct AutoMapper there
            CreateMap<Platform, PlatformReadDto>();

            // 2. We will have use cases where Dto is source and Platform is target
            // We need to explicitly add that, it is not implicit
            CreateMap<PlatformCreateDto, Platform>();

            // 3. We push a ReadDto - once it is available - into the Message Bus
            CreateMap<PlatformReadDto, PlatformPublishedDto>();

            // 4. gRPC
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
            
        }
    }
}