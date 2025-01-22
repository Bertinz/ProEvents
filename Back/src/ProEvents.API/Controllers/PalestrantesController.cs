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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ProEvents.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEvents.Persistence.Models;

namespace ProEvents.API.Controllers
{
    [Authorize] //sempre que tiver um metodo tentando acessar um extension method, precisa autorizar a requisicao (autorizou todas)
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
           
        private readonly IPalestranteService _palestranteService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public PalestrantesController(IPalestranteService palestranteService, IWebHostEnvironment hostEnvironment, IAccountService accountService)
        {
            _palestranteService = palestranteService;
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams) //IActionResult permite retornar os status code do http
        {
            try
            {
                var palestrantes = await _palestranteService.GetAllPalestrantesAsync(pageParams, true); //sempre retornar com os participantes //Pega id a partir do token (User.Get), retornando os palestrantes apenas de quem ta passando o token
                if (palestrantes == null) return NoContent(); //erro 404

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes() //ActionResult<Evento> consegue trabalhar com detalhes de evento
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrante == null) return NoContent(); //NotFound("Mensagem") = erro 404

                return Ok(palestrante);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDto model)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false); //ta dando post, mas antes precisa saber se o usuario ja possui um palestrante
                if (palestrante == null)
                    palestrante = await _palestranteService.AddPalestrantes(User.GetUserId(), model); //se nao tiver palestrante, cria um

                return Ok(palestrante);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDto model) 
        {
            try
            {
                var palestrantes = await _palestranteService.UpdatePalestrante(User.GetUserId(), model); //pega o palestrante sendo passado como parametro e atualiza baseado no token, nao havendo necessidade de passar o id do usuario
                if (palestrantes == null) return NoContent();

                return Ok(palestrantes);
            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

    }
}
