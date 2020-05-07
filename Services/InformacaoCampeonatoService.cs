using Domain.Entities;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Infrastructure.CrossCutting.Utils;
using Domain.DTOs;

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
            var timesAgrupadosPorAno = dadosCampeonatos.GroupBy(x => x.AnoParticipacao).ToList();

            List<AnalisePorCampeonato> analisePorCampeonato = new List<AnalisePorCampeonato>();
            List<AnalisePorCampeonato> melhoresPorCampeonato = new List<AnalisePorCampeonato>();
            List<AnalisePorCampeonato> pioresPorCampeonato = new List<AnalisePorCampeonato>();

            foreach (var ano in timesAgrupadosPorAno)
            {
                analisePorCampeonato = new List<AnalisePorCampeonato>();

                foreach (var time in ano)
                {
                    string nomeTime = time.Nome.PrimeiraLetraMaiuscula();
                    double percentualVitorias = ((double)time.Vitorias / (double)time.Jogos) * 100;

                    analisePorCampeonato.Add(new AnalisePorCampeonato { AnoParticipacao = ano.Key, Nome = nomeTime, PercentualVitorias = Math.Round(percentualVitorias, 2) });
                }

                analisePorCampeonato.OrderByDescending(x => x.PercentualVitorias);

                melhoresPorCampeonato.Add(analisePorCampeonato.FirstOrDefault());
                pioresPorCampeonato.Add(analisePorCampeonato.LastOrDefault());
            }

            var timesAgrupadoPorNome = dadosCampeonatos.GroupBy(x => x.Nome)
                .Select(
                    g => new
                    {
                        Nome = g.Key.PrimeiraLetraMaiuscula(),
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
            informacaoComplementar.MelhorMediaVitoriasPorCampeonato = melhoresPorCampeonato;
            informacaoComplementar.MenorMediaVitoriasPorCampeonato = pioresPorCampeonato;

            return informacaoComplementar;
        }

        private List<Time> ObterDadosCampeonatos()
        {
            var linhas = File.ReadAllLines(_env.ContentRootPath + "/DadosCampeonatos.txt").ToList();
            var timesFormatados = new List<Time>();

            if (linhas != null && linhas.Count > 0)
            {
                int anoParticipacao = 0;

                foreach (var linha in linhas)
                {
                    var linhaFormatada = linha.Formatar();
                    var time = new Time();

                    var linhaTratada = linhaFormatada.Split(",").ToList();

                    if (linhaTratada != null && linhaTratada.Count == 1 && linhaTratada[0].Length == 4)
                    {
                        anoParticipacao = Int32.Parse(linhaTratada[0]);
                    }

                    if (linhaTratada != null && linhaTratada.Count == 10 && !linhaTratada[0].Contains("POS"))
                    {
                        linhaTratada[1] = linhaTratada[1].CorrigirOrtografia();

                        time.AnoParticipacao = anoParticipacao;
                        time.Posicao = Int32.Parse(linhaTratada[0]);
                        time.Nome = linhaTratada[1];
                        time.Estado = linhaTratada[2];
                        time.Pontos = Int32.Parse(linhaTratada[3]);
                        time.Jogos = Int32.Parse(linhaTratada[4]);
                        time.Vitorias = Int32.Parse(linhaTratada[5]);
                        time.Empates = Int32.Parse(linhaTratada[6]);
                        time.Derrotas = Int32.Parse(linhaTratada[7]);
                        time.GolsFavor = Int32.Parse(linhaTratada[8]);
                        time.GolsContra = Int32.Parse(linhaTratada[9]);

                        timesFormatados.Add(time);
                    }
                }
            }

            return timesFormatados;
        }
    }
}