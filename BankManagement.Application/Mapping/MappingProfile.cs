using AutoMapper;
using BankManagement.Core.Entities;
using BankManagement.Application.DTOs;

namespace BankManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity → DTO
            CreateMap<Account, AccountDto>();

            // DTO → Entity (for Create)
            CreateMap<CreateAccountDto, Account>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src =>
                    $"ACC{DateTime.Now:yyyyMMdd}{new Random().Next(1000, 9999)}"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance)) // ✅ THIS IS CRITICAL!
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType));

            // DTO → Entity (for Update - partial)
            CreateMap<UpdateAccountDto, Account>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AccountNumber, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.AccountType, opt => opt.Ignore());
        }
    }
}