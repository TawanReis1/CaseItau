namespace API.Models
{
    public class InformacaoTimeResponse
    {
        public int Posicao { get; set; }

        public string Nome { get; set; }

        public int PontuacaoTotal { get; set; }

        public int TotalJogos { get; set; }

        public int TotalVitorias { get; set; }

        public int TotalEmpates { get; set; }

        public int TotalDerrotas { get; set; }

        public int TotalGolsFavor { get; set; }

        public int TotalGolsContra { get; set; }

        public int SaldoGols { get; set; }

        public int QuantidadeCampeonatosDisputados { get; set; }
    }
}
