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
    public class RedeSocialService : IRedeSocialService
    {
        private readonly IMapper _mapper;
        private readonly IRedeSocialPersist _redeSocialPersist;
        public RedeSocialService(IMapper mapper, IRedeSocialPersist redeSocialPersist)
        {
            _redeSocialPersist = redeSocialPersist;        
            _mapper = mapper;
        }

        public async Task AddRedeSocial(int id, RedeSocialDto model, bool isEvento) //tratar o erro fora da Persistence, mas dentro da camada Service/App
        {
            try
            {
                var redeSocial = _mapper.Map<RedeSocial>(model);
                if(isEvento){

                redeSocial.EventoId = id; //se não informar o código do evento, cria RedeSocial vazio
                redeSocial.PalestranteId = null;
                }
                else{

                redeSocial.PalestranteId = id;
                redeSocial.EventoId = null;
                }


                _redeSocialPersist.Add<RedeSocial>(redeSocial);
                await _redeSocialPersist.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                var redeSociais = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId); 
                if (redeSociais == null) return null;
                
                foreach(var model in models){

                    if(model.Id == 0){
                        await AddRedeSocial(eventoId, model, true);
                    }
                    else{ //se tem Id do evento, atualiza, se não tiver, adiciona
                        var redeSocial = redeSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);   
                        
                        model.EventoId = eventoId; 

                        _mapper.Map(model, redeSocial); //do model para o destino evento

                        _redeSocialPersist.Update<RedeSocial>(redeSocial);

                        await _redeSocialPersist.SaveChangesAsync();      
                    }
                }

                    var redeSocialRetorno = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId); 

                    return _mapper.Map<RedeSocialDto[]>(redeSocialRetorno); 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> SaveByPalestrante(int palestranteId, RedeSocialDto[] models)
        {
            try
            {
                var redeSociais = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId); 
                if (redeSociais == null) return null;
                
                foreach(var model in models){

                    if(model.Id == 0){
                        await AddRedeSocial(palestranteId, model, false);
                    }
                    else{ //se tem Id do palestrante, atualiza, se não tiver, adiciona
                        var redeSocial = redeSociais.FirstOrDefault(redeSocial => redeSocial.Id == model.Id);   
                        
                        model.PalestranteId = palestranteId; 

                        _mapper.Map(model, redeSocial); //do model para o destino evento

                        _redeSocialPersist.Update<RedeSocial>(redeSocial);

                        await _redeSocialPersist.SaveChangesAsync();      
                    }
                }

                    var redeSocialRetorno = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId); 
                    return _mapper.Map<RedeSocialDto[]>(redeSocialRetorno); 
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        

        public async Task<bool> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId); //pegar o evento Id, sem nenhum palestrante
                if (redeSocial == null) throw new Exception("Rede Social por Evento para delete não encontrado."); //mandar mensagem para Controller, que irá tratar a exceção
                

                _redeSocialPersist.Delete<RedeSocial>(redeSocial); //Especificando que está deletando um evento
                return await _redeSocialPersist.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId); //pegar o evento Id, sem nenhum palestrante
                if (redeSocial == null) throw new Exception("Rede Social por Palestrante para delete não encontrado."); //mandar mensagem para Controller, que irá tratar a exceção
                

                _redeSocialPersist.Delete<RedeSocial>(redeSocial); //Especificando que está deletando um evento
                return await _redeSocialPersist.SaveChangesAsync();
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByEventoIdAsync(int eventoId)
        {
            try
            {
                var redeSociais = await _redeSocialPersist.GetAllByEventoIdAsync(eventoId);
                if (redeSociais == null)  
                {
                    return null;
                }
                
                var resultado = _mapper.Map<RedeSocialDto[]>(redeSociais); 

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto[]> GetAllByPalestranteIdAsync(int palestranteId)
        {
            try
            {
                var redeSociais = await _redeSocialPersist.GetAllByPalestranteIdAsync(palestranteId);
                if (redeSociais == null)  
                {
                    return null;
                }
                
                var resultado = _mapper.Map<RedeSocialDto[]>(redeSociais); 

                return resultado;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<RedeSocialDto> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) 
                {
                    return null;
                }

                var resultado = _mapper.Map<RedeSocialDto>(redeSocial); 

                return resultado;
     
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<RedeSocialDto> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId)
        {
            try
            {
                var redeSocial = await _redeSocialPersist.GetRedeSocialPalestranteByIdsAsync(palestranteId, redeSocialId);
                if (redeSocial == null) 
                {
                    return null;
                }

                var resultado = _mapper.Map<RedeSocialDto>(redeSocial); 

                return resultado;
     
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
