using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Application.Dtos;

namespace ProEvents.Application.Contratos
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models);
        Task<bool> DeleteByEvento(int eventoId, int redeSocialId);

        Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);

        Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId); //pegar todas do evento
        Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId);
        
        Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId); //pegar apenas uma do evento
        Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);

    }
}