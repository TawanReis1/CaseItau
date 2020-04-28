using Domain.DTOs;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces.Services
{
    public interface IInformacaoCampeonatoService
    {
        List<Time> ObterInformacoesPorTime(string time);

        List<Time> ObterInformacoesPorEstado(string estado);

        public InformacaoComplementar ObterInformacoesComplementares();
    }
}
