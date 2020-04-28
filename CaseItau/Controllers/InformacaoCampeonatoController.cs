﻿using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Infrastructure.CrossCutting.Application.Models;
using API.Models;
using Microsoft.AspNetCore.Http;
using Domain.DTOs;
using Microsoft.Extensions.Logging;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("informacoes-campeonato")]

    public class InformacaoCampeonatoController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IInformacaoCampeonatoService _informacaoCampeonatoService;
        private readonly ILogger<InformacaoCampeonatoController> _logger;

        public InformacaoCampeonatoController(IInformacaoCampeonatoService informacaoCampeonatoService, IMapper mapper, ILogger<InformacaoCampeonatoController> logger)

        {
            _informacaoCampeonatoService = informacaoCampeonatoService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("time")]
        [ProducesResponseType(typeof(InformacaoTimeResponse), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult ObterInformacoesPorTime([FromQuery]string time)
        {
            try
            {
                _logger.LogInformation("Entrou no método 'ObterInformacoesPorTime'.");
                var informacoesTime = _informacaoCampeonatoService.ObterInformacoesPorTime(time);
                _logger.LogInformation("Finalizou o processamento no serviço.");

                if (informacoesTime != null && informacoesTime.Count == 0)
                {
                    _logger.LogError("O time: {time} não foi encontrado no campeonato desse período (2015 a 2019)", time);
                    return BadRequest(new ErroViewModel(msg: "Time digitado não existe e/ou não participou do campeonato nesse período."));
                }

                var resultado = _mapper.Map<InformacaoTimeResponse>(informacoesTime);
                _logger.LogInformation("Finalizou o processamento do mapeamento.");

                return Ok(new SucessoViewModel(resultado, msg: "Informação do time retornada com sucesso!"));

            } catch (Exception erro)
            {
                _logger.LogCritical("Erro ao executar a controladora: {erro}", erro);
                throw erro;
            }

        }

        [HttpGet("estado")]
        [ProducesResponseType(typeof(InformacaoEstadoResponse), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult ObterInformacoesPorEstado([FromQuery]string estado)
        {
            try
            {
                _logger.LogInformation("Entrou no método 'ObterInformacoesPorEstado'.");
                var informacoesTimeEstado = _informacaoCampeonatoService.ObterInformacoesPorEstado(estado);
                _logger.LogInformation("Finalizou o processamento no serviço.");

                if (informacoesTimeEstado != null && informacoesTimeEstado.Count == 0)
                {
                    _logger.LogError("O estado: {estado} não foi encontrado no campeonato desse período (2015 a 2019)", estado);
                    return BadRequest(new ErroViewModel(msg: "Estado digitado não existe e/ou não participou do campeonato nesse período."));
                }

                var resultado = _mapper.Map<InformacaoEstadoResponse>(informacoesTimeEstado);
                _logger.LogInformation("Finalizou o processamento do mapeamento.");

                return Ok(new SucessoViewModel(resultado, msg: "Informações dos times baseado no estado retornada com sucesso!"));

            } catch (Exception erro)
            {
                _logger.LogCritical("Erro ao executar a controladora: {erro}", erro);
                throw erro;
            }

        }

        [HttpGet("complementar")]
        [ProducesResponseType(typeof(InformacaoComplementar), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public IActionResult ObterInformacoesComplementares()
        {
            try
            {
                _logger.LogInformation("Entrou no método 'ObterInformacoesComplementares'.");
                var informacoesComplementares = _informacaoCampeonatoService.ObterInformacoesComplementares();
                _logger.LogInformation("Finalizou o processamento no serviço.");


                return Ok(new SucessoViewModel(informacoesComplementares, msg: "Informações complementares retornada com sucesso!"));

            } catch (Exception erro)
            {
                _logger.LogCritical("Erro ao executar a controladora: {erro}", erro);
                throw erro;
            }
        }
    }
}