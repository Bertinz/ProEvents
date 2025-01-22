using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;
using AutoMapper;
using ProEvents.Persistence.Contratos;
using ProEvents.Application.Contratos;
using ProEvents.Application.Dtos;
using ProEvents.Persistence.Models;


namespace ProEvents.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IMapper _mapper;
        private readonly IPalestrantePersist _palestrantePersist;
        public PalestranteService(IMapper mapper, IPalestrantePersist palestrantePersist)
        {
            _palestrantePersist = palestrantePersist;           
            _mapper = mapper;
        }

        public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model) //tratar o erro fora da Persistence, mas dentro da camada Service/App
        {
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantePersist.Add<Palestrante>(palestrante); //o elemento adicionado é alterado, o Id é alterado quando é realizado um Save
                
                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDto>(palestranteRetorno); 
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrante(int userId, PalestranteUpdateDto model)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false); 
                if (palestrante == null) return null;
                
                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante); //do model para o destino Palestrante

                _palestrantePersist.Update<Palestrante>(palestrante);
                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false); 

                    return _mapper.Map<PalestranteDto>(palestranteRetorno); 
                }
                return null;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(pageParams, includeEventos);
                if (palestrantes == null) return null;
                
                
                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes); // [] no Dto se der problema

                //erro do pagelist n ter construtor vazio para em seguida criar um mapeamento na mao, preenchendo o conteudo
                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalPages = palestrantes.TotalPages;
                resultado.PageSize = palestrantes.PageSize;
                resultado.TotalCount = palestrantes.TotalCount;

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, includeEventos);
                if (palestrante == null) 
                {
                    return null;
                }

                var resultado = _mapper.Map<PalestranteDto>(palestrante); 

                return resultado;

                //Substituído pelo código acima


                //var eventoRetorno = new EventoDto(){
                //        Id = evento.Id,                       
                //        Local = evento.Local,
                //        DataEvento = evento.DataEvento.ToString(),
                //        Tema = evento.Tema,
                //        QtdPessoas = evento.QtdPessoas,
                //        ImagemURL = evento.ImagemURL,
                //        Telefone = evento.Telefone,
                //        Email = evento.Email};
     
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
