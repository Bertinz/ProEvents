using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProEvents.Persistence;
using ProEvents.Persistence.Context;
using ProEvents.Application.Contratos;
using ProEvents.Application.Dtos;
using ProEvents.API.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ProEvents.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
           
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;
        public RedesSociaisController(IRedeSocialService redeSocialService, IEventoService eventoService, IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;
            _palestranteService = palestranteService;
        }

        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId) //Nao pode listar redes sociais para quem nao eh dono do evento
        {
            try
            {
                if(!(await AutorEvento(eventoId))) //checa se o evento pertence ao token logado
                    return Unauthorized();

                var redesSociais = await _redeSocialService.GetAllByEventoIdAsync(eventoId); //sempre retornar com os participantes
                if (redesSociais == null) return NoContent(); //erro 404

                return Ok(redesSociais);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rede social por evento. Erro: {ex.Message}");
            }
        } 

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante() //vai listar todas as redes sociais do palestrante apenas com o token de usuario
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId()); //retorna quem eu sou como palestrante

                if(palestrante == null)
                    return Unauthorized();

                var redesSociais = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id); //sempre retornar com os participantes
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar rede social por palestrante. Erro: {ex.Message}");
            }
        } 


        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if(!(await AutorEvento(eventoId))) //so salva se o evento for seu
                    return Unauthorized();

                var redesSociais = await _redeSocialService.SaveByEvento(eventoId, models);
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar rede social por evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante(RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId()); //retorna quem eu sou como palestrante

                if(palestrante == null) 
                    return Unauthorized();

                var redesSociais = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
                if (redesSociais == null) return NoContent();

                return Ok(redesSociais);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar rede social por palestrante. Erro: {ex.Message}");
            }
        }

        [HttpDelete("evento/{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {

                if(!(await AutorEvento(eventoId))) //checa se o evento pertence ao token logado
                    return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId, redeSocialId);
                if (redeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByEvento(eventoId, redeSocialId)
                ? Ok(new { message  = "Rede Social Deletada"})
                : throw new Exception("Ocorreu um problema não especificado ao tentar deletar Rede Social por Evento.");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Rede Social por Evento. Erro: {ex.Message}");
            }
        }

        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {

                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());

                if(palestrante == null) 
                    return Unauthorized();

                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id, redeSocialId); //recuperar redes sociais
                if (redeSocial == null) return NoContent();

                return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocialId)
                ? Ok(new { message  = "Rede Social Deletada"})
                : throw new Exception("Ocorreu um problema não especificado ao tentar deletar Rede Social por Palestrante.");
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Rede Social por Palestrante. Erro: {ex.Message}");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId){
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), eventoId, false); //false = nao listar os palestrantes do evento
            if(evento == null) return false; //se nao listar nenhum evento, quer dizer que aquele eventoId nao eh do usuario logado, retornando falso

            return true;
        }
    }
}
