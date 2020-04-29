using API.Controllers;
using Domain.Interfaces.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Domain.Entities;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace TDD
{
    public class CaseItauTests
    {
        private readonly Mock<ILogger<InformacaoCampeonatoController>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IInformacaoCampeonatoService> _informacaoCampeonatoService;
        private InformacaoCampeonatoController _informacaoCaompeonatoController;

        public CaseItauTests()
        {
            _logger = new Mock<ILogger<InformacaoCampeonatoController>>();
            _mapper = new Mock<IMapper>();
            _informacaoCampeonatoService = new Mock<IInformacaoCampeonatoService>();
            _informacaoCaompeonatoController = new InformacaoCampeonatoController(_informacaoCampeonatoService.Object, _mapper.Object, _logger.Object);
        }


        #region Obter Informaçoes Por Time
        [Fact]
        public void Obter_Informacoes_Por_Time_Teste_Valido()
        {
            string time = "São Paulo";

            List<Time> dados = new List<Time>();
            dados.Add(new Time { Posicao = 2, Nome = "São Paulo", Estado = "SP", Pontos = 62, Jogos = 38, Vitorias = 18, Empates = 8, Derrotas = 12, GolsFavor = 53, GolsContra = 47 });
            dados.Add(new Time { Posicao = 8, Nome = "São Paulo", Estado = "SP", Pontos = 48, Jogos = 38, Vitorias = 14, Empates = 12, Derrotas = 14, GolsFavor = 60, GolsContra = 49 });
            dados.Add(new Time { Posicao = 3, Nome = "São Paulo", Estado = "SP", Pontos = 58, Jogos = 38, Vitorias = 15, Empates = 11, Derrotas = 12, GolsFavor = 50, GolsContra = 41 });
            dados.Add(new Time { Posicao = 6, Nome = "São Paulo", Estado = "SP", Pontos = 50, Jogos = 38, Vitorias = 14, Empates = 12, Derrotas = 12, GolsFavor = 52, GolsContra = 40 });
            dados.Add(new Time { Posicao = 7, Nome = "São Paulo", Estado = "SP", Pontos = 43, Jogos = 38, Vitorias = 15, Empates = 13, Derrotas = 11, GolsFavor = 59, GolsContra = 39 });

            _informacaoCampeonatoService.Setup(x => x.ObterInformacoesPorTime(time)).Returns(dados);

            var resultado = _informacaoCaompeonatoController.ObterInformacoesPorTime(time);
            var okResultado = resultado as OkObjectResult;

            Assert.NotNull(okResultado);
            Assert.NotNull(okResultado.Value);
        }

        [Fact]
        public void Obter_Informacoes_Por_Time_Teste_Invalido()
        {
            string time = "São Paulo";
            _informacaoCampeonatoService.Setup(x => x.ObterInformacoesPorTime(time)).Throws<Exception>();

            var resultado = _informacaoCaompeonatoController.ObterInformacoesPorTime(time);
            var resultadoErro = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(500, resultadoErro.StatusCode);
        }
        #endregion

        #region Obter Informações Por Estado
        [Fact]
        public void Obter_Informacoes_Por_Estado_Teste_Valido()
        {
            string estado = "RJ";

            List<Time> dados = new List<Time>();
            dados.Add(new Time { Posicao = 2, Nome = "Fluminense", Estado = "RJ", Pontos = 62, Jogos = 38, Vitorias = 18, Empates = 8, Derrotas = 12, GolsFavor = 53, GolsContra = 47 });
            dados.Add(new Time { Posicao = 8, Nome = "Fluminense", Estado = "RJ", Pontos = 48, Jogos = 38, Vitorias = 14, Empates = 12, Derrotas = 14, GolsFavor = 60, GolsContra = 49 });
            dados.Add(new Time { Posicao = 3, Nome = "Fluminense", Estado = "RJ", Pontos = 58, Jogos = 38, Vitorias = 15, Empates = 11, Derrotas = 12, GolsFavor = 50, GolsContra = 41 });
            dados.Add(new Time { Posicao = 6, Nome = "Fluminense", Estado = "RJ", Pontos = 50, Jogos = 38, Vitorias = 14, Empates = 12, Derrotas = 12, GolsFavor = 52, GolsContra = 40 });
            dados.Add(new Time { Posicao = 7, Nome = "Fluminense", Estado = "RJ", Pontos = 43, Jogos = 38, Vitorias = 15, Empates = 13, Derrotas = 11, GolsFavor = 59, GolsContra = 39 });

            _informacaoCampeonatoService.Setup(x => x.ObterInformacoesPorEstado(estado)).Returns(dados);

            var resultado = _informacaoCaompeonatoController.ObterInformacoesPorEstado(estado);
            var okResultado = resultado as OkObjectResult;

            Assert.NotNull(okResultado);
            Assert.NotNull(okResultado.Value);
        }

        [Fact]
        public void Obter_Informacoes_Por_Estado_Teste_Invalido()
        {
            string estado = "RJ";
            _informacaoCampeonatoService.Setup(x => x.ObterInformacoesPorEstado(estado)).Throws<Exception>();

            var resultado = _informacaoCaompeonatoController.ObterInformacoesPorEstado(estado);
            var resultadoErro = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(500, resultadoErro.StatusCode);
        }

        #endregion

        #region Obter Informações Complementares
        [Fact]
        public void Obter_Informacoes_Complementares_Teste_Valido()
        {
            InformacaoComplementar dados = new InformacaoComplementar()
            {
                MaiorNumeroVitorias = "Palmeiras",
                MelhorMediaGolsContra = "São Paulo",
                MelhorMediaGolsFavor = "Corinthians",
                MelhorMediaVitoriasPorCampeonato = "Santos",
                MenorMediaVitoriasPorCampeonato = "Parana",
                MenorNumeroVitorias = "Joinville"
            };

            _informacaoCampeonatoService.Setup(x => x.ObterInformacoesComplementares()).Returns(dados);

            var resultado = _informacaoCaompeonatoController.ObterInformacoesComplementares();
            var okResultado = resultado as OkObjectResult;

            Assert.NotNull(okResultado);
            Assert.NotNull(okResultado.Value);
        }

        [Fact]
        public void Obter_Informacoes_Complementares_Teste_Invalido()
        {
            _informacaoCampeonatoService.Setup(x => x.ObterInformacoesComplementares()).Throws<Exception>();

            var resultado = _informacaoCaompeonatoController.ObterInformacoesComplementares();

            var erroResultado = Assert.IsType<StatusCodeResult>(resultado);
            Assert.Equal(500, erroResultado.StatusCode);
        }

        #endregion
    }
}