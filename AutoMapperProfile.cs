using AutoMapper;
using track_expense.api.ApiResponseModels;
using track_expense.api.ViewModels;

namespace track_expense.api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModelVM, UserLoginResponse>()
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.avatar, opt => opt.MapFrom(src => src.avatar))
                .ForMember(dest => dest.firstname, opt => opt.MapFrom(src => src.firstname))
                .ForMember(dest => dest.lastname, opt => opt.MapFrom(src => src.lastname))
                .ForMember(dest => dest.dateverified, opt => opt.MapFrom(src => src.dateverified));
        }
    }
}
