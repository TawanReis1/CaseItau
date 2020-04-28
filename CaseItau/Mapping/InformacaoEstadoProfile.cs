using System.Collections.Generic;
using System.Linq;
using API.Models;
using AutoMapper;
using Domain.Entities;

namespace API.Mapping
{
    public class InformacaoTimeProfile : Profile
    {
        public InformacaoTimeProfile()
        {
            CreateMap<List<Time>, InformacaoEstadoResponse>()
                .AfterMap((src, dest) =>
                {
                    if (src != null && src.Count > 0)
                    {
                        dest.Estado = src.FirstOrDefault().Estado;
                        dest.Posicao = src.FirstOrDefault().Posicao;

                        foreach (var time in src)
                        {
                            dest.PontuacaoTotal += time.Pontos;
                            dest.TotalJogos += time.Jogos;
                            dest.TotalVitorias += time.Vitorias;
                            dest.TotalEmpates += time.Empates;
                            dest.TotalDerrotas += time.Derrotas;
                            dest.TotalGolsFavor += time.GolsFavor;
                            dest.TotalGolsContra += time.GolsContra;
                            dest.SaldoGols = dest.TotalGolsFavor - dest.TotalGolsContra;
                            dest.QuantidadeCampeonatosDisputados = src.Count;
                        }
                    }
                });
        }
    }
}
