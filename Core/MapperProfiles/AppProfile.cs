using AutoMapper;
using Core.Models;
using Data.Entities;

namespace Core.MapperProfiles;

public class AppProfile : Profile
{
    public AppProfile()
    {
        //course
        CreateMap<CourseModelCreate, Course>();
        CreateMap<CourseModelEdit, Course>();
        CreateMap<Course, CourseModelEdit>();
        CreateMap<Course, CourseModel>();
        CreateMap<Course, CourseModelDetailed>();

        //lecture
        CreateMap<Lecture, LectureModel>();

        //user
        CreateMap<RegisterModel, User>()
            .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(x => x.PasswordHash, opt => opt.Ignore());
    }
}
