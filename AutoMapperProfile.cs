using AutoMapper;
using track_expense.api.ApiResponseModels;
using track_expense.api.ViewModels.TableVM;

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


            //Mappings for table : locales
            CreateMap<LocalesVM, LocaleRegionList>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.code, opt => opt.MapFrom(src => src.code));

            CreateMap<LocalesVM, LocaleCurrencyList>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.currency, opt => opt.MapFrom(src => src.currency))
                .ForMember(dest => dest.currencycode, opt => opt.MapFrom(src => src.currencycode));
        }
    }
}
