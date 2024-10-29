using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEvents.Domain;
using AutoMapper;
using ProEvents.Persistence.Contratos;
using ProEvents.Application.Contratos;
using ProEvents.Application.Dtos;


namespace ProEvents.Application
{
    public class LoteService : ILoteService
    {
        private readonly IMapper _mapper;
        private readonly IGeralPersist _geralPersist;
        private readonly ILotePersist _lotePersist;
        public LoteService(IGeralPersist geralPersist, IMapper mapper, ILotePersist lotePersist)
        {
            _lotePersist = lotePersist;
            _geralPersist = geralPersist;            
            _mapper = mapper;
        }

        public async Task AddLote(int eventoId, LoteDto model) //tratar o erro fora da Persistence, mas dentro da camada Service/App
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId; //se não informar o código do evento, cria lote vazio

                _geralPersist.Add<Lote>(lote);
                await _geralPersist.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes= await _lotePersist.GetLotesByEventoIdAsync(eventoId); 
                if (lotes== null) return null;
                
                foreach(var model in models){

                    if(model.Id == 0){
                        await AddLote(eventoId, model);
                    }
                    else{ //se tem Id do evento, atualiza, se não tiver, adiciona
                        var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);   
                        
                        model.EventoId = eventoId; 

                        _mapper.Map(model, lote); //do model para o destino evento

                        _geralPersist.Update<Lote>(lote);

                        await _geralPersist.SaveChangesAsync();      
                    }
                }

                    var loteRetorno = await _lotePersist.GetLotesByEventoIdAsync(eventoId); 

                    return _mapper.Map<LoteDto[]>(loteRetorno); 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId); //pegar o evento Id, sem nenhum palestrante
                if (lote == null) throw new Exception("Lote para delete não encontrado."); //mandar mensagem para Controller, que irá tratar a exceção
                

                _geralPersist.Delete<Lote>(lote); //Especificando que está deletando um evento
                return await _geralPersist.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if (lotes == null)  
                {
                    return null;
                }
                
                var resultado = _mapper.Map<LoteDto[]>(lotes); 

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                if (lote == null) 
                {
                    return null;
                }

                var resultado = _mapper.Map<LoteDto>(lote); 

                return resultado;
     
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
