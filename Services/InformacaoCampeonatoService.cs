﻿using Domain.Entities;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Infrastructure.CrossCutting.Utils;
using Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class InformacaoCampeonatoService : IInformacaoCampeonatoService
    {
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;
        private readonly ILogger<InformacaoCampeonatoService> _logger;
        public InformacaoCampeonatoService(IMapper mapper, IHostingEnvironment env, ILogger<InformacaoCampeonatoService> logger)
        {
            _mapper = mapper;
            _env = env;
            _logger = logger;
        }

        public List<Time> ObterInformacoesPorTime(string time)
        {
            var dadosCampeonato = ObterDadosCampeonato();
            var timeComparado = time.ToUpper();
            timeComparado = TratarTexto.RetirarAcentuacao(timeComparado);

            var informacoesTime = dadosCampeonato.Where(x => x.Nome == timeComparado).ToList();
            _logger.LogInformation("Lista filtrada para trazer apenas informações do time: {time}.", time);

            return informacoesTime;
        }

        public List<Time> ObterInformacoesPorEstado(string estado)
        {
            var dadosCampeonato = ObterDadosCampeonato();
            var estadoComparado = estado.ToUpper();

            var informacoesTime = dadosCampeonato.Where(x => x.Estado == estadoComparado).ToList();
            _logger.LogInformation("Lista filtrada para trazer apenas informações do estado: {estado}.", estado);

            return informacoesTime;
        }

        public InformacaoComplementar ObterInformacoesComplementares()
        {
            var dadosCampeonato = ObterDadosCampeonato();
            var timesAgrupado = dadosCampeonato.GroupBy(x => x.Nome)
                .Select(
                    g => new
                    {
                        Key = char.ToUpper(g.Key.First()) + g.Key.Substring(1).ToLower(),
                        Vitorias = g.Sum(s => s.Vitorias),
                        MediaVitorias = g.Average(s => s.Vitorias),
                        MediaGolsFavor = g.Average(s => s.GolsFavor),
                        MediaGolsContra = g.Average(s => s.GolsContra),
                    }
                ).ToList();

            _logger.LogInformation("Times agrupados pelo o nome.");

            var informacaoComplementar = new InformacaoComplementar();

            informacaoComplementar.MelhorMediaGolsFavor = timesAgrupado.OrderBy(x => x.MediaGolsFavor).ToList().LastOrDefault().Key;
            informacaoComplementar.MelhorMediaGolsContra = timesAgrupado.OrderBy(x => x.MediaGolsContra).ToList().FirstOrDefault().Key;
            informacaoComplementar.MaiorNumeroVitorias = timesAgrupado.OrderBy(x => x.Vitorias).ToList().LastOrDefault().Key;
            informacaoComplementar.MenorNumeroVitorias = timesAgrupado.OrderBy(x => x.Vitorias).ToList().FirstOrDefault().Key;
            informacaoComplementar.MelhorMediaVitoriasPorCampeonato = timesAgrupado.OrderBy(x => x.MediaVitorias).ToList().LastOrDefault().Key;
            informacaoComplementar.MenorMediaVitoriasPorCampeonato = timesAgrupado.OrderBy(x => x.MediaVitorias).ToList().FirstOrDefault().Key;

            _logger.LogInformation("Finalizado lógica de comparação.");

            return informacaoComplementar;
        }

        private List<Time> ObterDadosCampeonato()
        {
            var linhas = File.ReadAllLines(_env.ContentRootPath + "/DadosCampeonato.txt").ToList();
            _logger.LogInformation("Coletou todos os dados do arquivo.");

            var timesFormatados = new List<Time>();

            if (linhas != null && linhas.Count > 0)
            {
                foreach (var linha in linhas)
                {
                    var linhaTratada = TratarTexto.Formatar(linha);
                    var time = new Time();

                    var linhaTratadaSeparadas = linhaTratada.Split(",").ToList();

                    if (linhaTratadaSeparadas != null && linhaTratadaSeparadas.Count == 10 && !linhaTratadaSeparadas[0].Contains("POS"))
                    {
                        time.Posicao = Int32.Parse(linhaTratadaSeparadas[0]);
                        time.Nome = linhaTratadaSeparadas[1] == "ATHLETICO PR" ? "ATLETICO PR" : linhaTratadaSeparadas[1];
                        time.Estado = linhaTratadaSeparadas[2];
                        time.Pontos = Int32.Parse(linhaTratadaSeparadas[3]);
                        time.Jogos = Int32.Parse(linhaTratadaSeparadas[4]);
                        time.Vitorias = Int32.Parse(linhaTratadaSeparadas[5]);
                        time.Empates = Int32.Parse(linhaTratadaSeparadas[6]);
                        time.Derrotas = Int32.Parse(linhaTratadaSeparadas[7]);
                        time.GolsFavor = Int32.Parse(linhaTratadaSeparadas[8]);
                        time.GolsContra = Int32.Parse(linhaTratadaSeparadas[9]);

                        timesFormatados.Add(time);
                        _logger.LogInformation("Adicionou o time: {time}, à lista.", time.Nome);
                    }
                }
            }

            return timesFormatados;
        }
    }
}