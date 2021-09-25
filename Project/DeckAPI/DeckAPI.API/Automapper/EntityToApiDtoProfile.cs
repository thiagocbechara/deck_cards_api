using AutoMapper;
using DeckAPI.API.Dtos;
using DeckAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DeckAPI.API.Automapper
{
    public class EntityToApiDtoProfile : Profile
    {
        public EntityToApiDtoProfile()
        {
            CreateMap<Card, DrawDto>();
            CreateMap<Deck, DeckDto>()
                .ForMember(x => x.Cards, opt => opt.MapFrom(x => x.Cards.Select(c => c.Value)));
        }
    }
}
