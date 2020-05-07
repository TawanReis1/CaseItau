using System;

namespace Infrastructure.CrossCutting.Utils
{
    public static class TratarMensagem
    {
        public static string TratarErro(this string mensagem)
        {
            string mensagemTratada = "";

            if (mensagem.Contains("Could not find file"))
            {
                string caminho = mensagem.Split(" ")[4];
                mensagemTratada = String.Format("Arquivo não encontrado no seguinte caminho: {0}", caminho.Replace("\\", "/"));
            }

            return mensagemTratada == "" ? mensagem : mensagemTratada;
        }
    }
}
