using Domain.Entities;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Infrastructure.CrossCutting.Utils;
using Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class InformacaoCampeonatoService : IInformacaoCampeonatoService
    {
        private IHostingEnvironment _env;
        private readonly ILogger<InformacaoCampeonatoService> _logger;
        public InformacaoCampeonatoService(IHostingEnvironment env, ILogger<InformacaoCampeonatoService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public List<Time> ObterInformacoesPorTime(string time)
        {
            var dadosCampeonato = ObterDadosCampeonato();
            var timesAgrupado = dadosCampeonato.GroupBy(x => x.Nome)
                .Select(
                    g => new
                    {
                        Nome = g.Key,
                        PontuacaoTotal = g.Sum(s => s.Pontos),
                    }
                ).OrderByDescending(x => x.PontuacaoTotal).ToList();

            var timeComparado = time.ToUpper();
            timeComparado = timeComparado.RetirarAcentuacao();

            int posicaoTimeComparado = timesAgrupado.FindIndex(x => x.Nome == timeComparado) + 1;

            var informacoesTime = dadosCampeonato.Where(x => x.Nome == timeComparado)
                .Select(y => { y.Posicao = posicaoTimeComparado; return y; }).ToList();
            _logger.LogInformation("Lista filtrada para trazer apenas informações do time: {time}.", time);

            return informacoesTime;
        }

        public List<Time> ObterInformacoesPorEstado(string estado)
        {
            var dadosCampeonato = ObterDadosCampeonato();
            var timesAgrupado = dadosCampeonato.GroupBy(x => x.Estado)
                .Select(
                    g => new
                    {
                        Nome = g.Key,
                        PontuacaoTotal = g.Sum(s => s.Pontos),
                    }
                ).OrderByDescending(x => x.PontuacaoTotal).ToList();

            var estadoComparado = estado.ToUpper();

            int posicaoEstadoComparado = timesAgrupado.FindIndex(x => x.Nome == estadoComparado) + 1;

            var informacoesTime = dadosCampeonato.Where(x => x.Estado == estadoComparado)
                .Select(y => { y.Posicao = posicaoEstadoComparado; return y; }).ToList();
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
                        Nome = char.ToUpper(g.Key.First()) + g.Key.Substring(1).ToLower(),
                        Vitorias = g.Sum(s => s.Vitorias),
                        MediaVitorias = g.Average(s => s.Vitorias),
                        MediaGolsFavor = g.Average(s => s.GolsFavor),
                        MediaGolsContra = g.Average(s => s.GolsContra),
                    }
                ).ToList();

            _logger.LogInformation("Times agrupados pelo o nome.");

            var informacaoComplementar = new InformacaoComplementar();

            informacaoComplementar.MelhorMediaGolsFavor = timesAgrupado.OrderBy(x => x.MediaGolsFavor).ToList().LastOrDefault().Nome;
            informacaoComplementar.MelhorMediaGolsContra = timesAgrupado.OrderBy(x => x.MediaGolsContra).ToList().FirstOrDefault().Nome;
            informacaoComplementar.MaiorNumeroVitorias = timesAgrupado.OrderBy(x => x.Vitorias).ToList().LastOrDefault().Nome;
            informacaoComplementar.MenorNumeroVitorias = timesAgrupado.OrderBy(x => x.Vitorias).ToList().FirstOrDefault().Nome;
            informacaoComplementar.MelhorMediaVitoriasPorCampeonato = timesAgrupado.OrderBy(x => x.MediaVitorias).ToList().LastOrDefault().Nome;
            informacaoComplementar.MenorMediaVitoriasPorCampeonato = timesAgrupado.OrderBy(x => x.MediaVitorias).ToList().FirstOrDefault().Nome;

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
                    var linhaTratada = linha.Formatar();
                    var time = new Time();

                    var linhaTratadaSeparadas = linhaTratada.Split(",").ToList();

                    if (linhaTratadaSeparadas != null && linhaTratadaSeparadas.Count == 10 && !linhaTratadaSeparadas[0].Contains("POS"))
                    {
                        linhaTratadaSeparadas[1] = linhaTratadaSeparadas[1].CorrigirOrtografia();

                        time.Posicao = Int32.Parse(linhaTratadaSeparadas[0]);
                        time.Nome = linhaTratadaSeparadas[1];
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