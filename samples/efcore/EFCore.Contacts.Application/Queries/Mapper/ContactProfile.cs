using AutoMapper;
using EFCore.Contacts.Application.Queries.Models;
using EFCore.Contacts.Domain;

namespace EFCore.Contacts.Application.Queries.Mapper;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<Contact, ContactDto>();
    }
}
