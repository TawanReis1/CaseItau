using System.Collections.Generic;

namespace Domain.DTOs
{
    public class InformacaoComplementar
    {
        public string MelhorMediaGolsFavor { get; set; }

        public string MelhorMediaGolsContra { get; set; }

        public string MaiorNumeroVitorias { get; set; }

        public string MenorNumeroVitorias { get; set; }

        public ICollection<AnalisePorCampeonato> MelhorMediaVitoriasPorCampeonato { get; set; }

        public ICollection<AnalisePorCampeonato> MenorMediaVitoriasPorCampeonato { get; set; }
    }
}
