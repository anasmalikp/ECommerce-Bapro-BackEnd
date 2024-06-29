using AutoMapper;
using BaproBackend.Data.DTO;
using BaproBackend.Data.Models;

namespace BaproBackend.Data
{
    public class MapperClass:Profile
    {
        public MapperClass()
        {
            CreateMap<products, ProductsDTO>().ReverseMap();
            CreateMap<category, CategoriesDTO>().ReverseMap();
            CreateMap<users, UsersDTO>().ReverseMap();
        }
    }
}
