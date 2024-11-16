using AutoMapper;
using VibeTeste.Application.DTO;
using VibeTeste.Domain.Entities;

namespace VibeTeste.Application.Mapper
{
    public class EntityToDTO : Profile
    {
        public EntityToDTO()
        {
            CreateMap<PlacemarkEntity, PlacemarkDTO>().ReverseMap();
        }
    }
}
