using AutoMapper;
using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Models;

namespace FreeCourse.Services.Catalog.Mapping
{
    public class GeneralMapping:Profile //Profile automapperdan geliyor.
    {
        public GeneralMapping()
        {
            CreateMap<Course, CourseDto>().ReverseMap(); //Course'dan bir course dto nesnesi oluşutrabiliriz. tam terisin de oluşturabiliriz anlamında reversemap kullandık.
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();

            CreateMap<Course,CourseCreateDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();
        }

    }
}
