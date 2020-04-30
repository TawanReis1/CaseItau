using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs
{
    public class AnalisePorCampeonato
    {
        public int AnoParticipacao { get; set; }

        public string Nome { get; set; }

        public double PercentualVitorias { get; set; }
    }
}
