using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEvents.Domain;
using ProEvents.Application.Dtos;

namespace ProEvents.Application.Helpers
{
    public class ProEventsProfile : Profile
    {
        public ProEventsProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap(); //reverse map realiza o oposto tbm
        }
    }
}