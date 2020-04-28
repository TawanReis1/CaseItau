using System.Collections.Generic;
using System.Linq;
using API.Models;
using AutoMapper;
using Domain.Entities;

namespace API.Mapping
{
    public class InformacaoEstadoProfile : Profile
    {
        public InformacaoEstadoProfile()
        {
            CreateMap<List<Time>, InformacaoTimeResponse>()
                .AfterMap((src, dest) =>
                {
                    if (src != null && src.Count > 0)
                    {
                        dest.Nome = char.ToUpper(src.FirstOrDefault().Nome.First()) + src.FirstOrDefault().Nome.Substring(1).ToLower();
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
