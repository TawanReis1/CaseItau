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
        public InformacaoCampeonatoService(IHostingEnvironment env)
        {
            _env = env;
        }

        public List<Time> ObterInformacoesPorTime(string time)
        {
            var dadosCampeonatos = ObterDadosCampeonatos();
            var timesAgrupadosPorNome = dadosCampeonatos.GroupBy(x => x.Nome)
                .Select(
                    g => new
                    {
                        Nome = g.Key,
                        PontuacaoTotal = g.Sum(s => s.Pontos),
                    }
                ).OrderByDescending(x => x.PontuacaoTotal).ToList();

            var timeComparado = time.ToUpper().RetirarAcentuacao();

            int posicaoTimeComparado = timesAgrupadosPorNome.FindIndex(x => x.Nome == timeComparado) + 1;

            var estatisticasTime = dadosCampeonatos.Where(x => x.Nome == timeComparado)
                .Select(y => { y.Posicao = posicaoTimeComparado; return y; }).ToList();

            return estatisticasTime;
        }

        public List<Time> ObterInformacoesPorEstado(string estado)
        {
            var dadosCampeonatos = ObterDadosCampeonatos();
            var timesAgrupadosPorEstado = dadosCampeonatos.GroupBy(x => x.Estado)
                .Select(
                    g => new
                    {
                        Estado = g.Key,
                        PontuacaoTotal = g.Sum(s => s.Pontos),
                        AnoParticipacao = g.GroupBy(g => g.AnoParticipacao).ToList()
                    }
                ).OrderByDescending(x => x.PontuacaoTotal).ToList();

            var estadoComparado = estado.ToUpper();
            int posicaoEstadoComparado = timesAgrupadosPorEstado.FindIndex(x => x.Estado == estadoComparado) + 1;

            var estatisticasEstado = dadosCampeonatos.Where(x => x.Estado == estadoComparado)
                .Select(y => { y.Posicao = posicaoEstadoComparado; return y; }).ToList();

            return estatisticasEstado;
        }

        public InformacaoComplementar ObterInformacoesComplementares()
        {
            var dadosCampeonatos = ObterDadosCampeonatos();
            var timesAgrupadoPorNome = dadosCampeonatos.GroupBy(x => x.Nome)
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

            var informacaoComplementar = new InformacaoComplementar();

            informacaoComplementar.MelhorMediaGolsFavor = timesAgrupadoPorNome.OrderBy(x => x.MediaGolsFavor).ToList().LastOrDefault().Nome;
            informacaoComplementar.MelhorMediaGolsContra = timesAgrupadoPorNome.OrderBy(x => x.MediaGolsContra).ToList().FirstOrDefault().Nome;
            informacaoComplementar.MaiorNumeroVitorias = timesAgrupadoPorNome.OrderBy(x => x.Vitorias).ToList().LastOrDefault().Nome;
            informacaoComplementar.MenorNumeroVitorias = timesAgrupadoPorNome.OrderBy(x => x.Vitorias).ToList().FirstOrDefault().Nome;
            informacaoComplementar.MelhorMediaVitoriasPorCampeonato = timesAgrupadoPorNome.OrderBy(x => x.MediaVitorias).ToList().LastOrDefault().Nome;
            informacaoComplementar.MenorMediaVitoriasPorCampeonato = timesAgrupadoPorNome.OrderBy(x => x.MediaVitorias).ToList().FirstOrDefault().Nome;

            return informacaoComplementar;
        }

        private List<Time> ObterDadosCampeonatos()
        {
            var linhas = File.ReadAllLines(_env.ContentRootPath + "/DadosCampeonato.txt").ToList();
            var timesFormatados = new List<Time>();

            if (linhas != null && linhas.Count > 0)
            {
                int anoParticipacao = 0;

                foreach (var linha in linhas)
                {
                    var linhaTratada = linha.Formatar();
                    var time = new Time();

                    var linhaTratadaSeparadas = linhaTratada.Split(",").ToList();

                    if (linhaTratadaSeparadas != null && linhaTratadaSeparadas.Count == 1 && linhaTratadaSeparadas[0].Length == 4)
                    {
                        anoParticipacao = Int32.Parse(linhaTratadaSeparadas[0]);
                    }

                    if (linhaTratadaSeparadas != null && linhaTratadaSeparadas.Count == 10 && !linhaTratadaSeparadas[0].Contains("POS"))
                    {
                        linhaTratadaSeparadas[1] = linhaTratadaSeparadas[1].CorrigirOrtografia();

                        time.AnoParticipacao = anoParticipacao;
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
                    }
                }
            }
            else
            {
                throw new ArgumentException("Arquivo não encontrado e/ou inexistente.");
            }

            return timesFormatados;
        }
    }
}