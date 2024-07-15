using AutoMapper;

namespace ReceiptProcessor.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReceiptDTO, Receipt>()
                .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateOnly.Parse(src.PurchaseDate)))
                .ForMember(dest => dest.PurchaseTime, opt => opt.MapFrom(src => TimeOnly.Parse(src.PurchaseTime)))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => double.Parse(src.Total)));

            CreateMap<ItemDTO, Item>();
        }
    }
}
