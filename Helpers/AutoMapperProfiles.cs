using System.Linq;
using AutoMapper;
using RealEstate.API.Dtos;
using RealEstate.API.Models;

namespace RealEstate.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailedDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<HouseForCreationDto, House>();
            CreateMap<House, HousesForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                });
            CreateMap<UserPhoto, UserPhotoForDetailedDto>();
            CreateMap<PhotosForCreationDto, Photo>();
            CreateMap<Photo, PhotoForReturnDto>();
        }
    }
}