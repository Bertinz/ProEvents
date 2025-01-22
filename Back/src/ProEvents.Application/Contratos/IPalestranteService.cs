using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Application.Dtos;
using ProEvents.Persistence.Models;

namespace ProEvents.Application.Contratos
{
    public interface IPalestranteService
    {
        Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model);
        Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model); //id do usuario ja possui o id do palestrante

        Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false); //em um palestrante, pode incluir ou nao os eventos
        Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false);

    }
}