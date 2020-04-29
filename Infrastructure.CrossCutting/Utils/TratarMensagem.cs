using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.CrossCutting.Utils
{
    public static class TratarMensagem
    {
        public static string TratarErro(this string mensagem)
        {
            string mensagemTratada = "";
            string caminho = "";

            if (mensagem.Contains("Could not find file"))
            {
                caminho = mensagem.Split(" ")[4];
                mensagemTratada = String.Format("Arquivo não encontrado no seguinte caminho: {0}", caminho.Replace("\\", "/"));
            }

            return mensagemTratada;
        }
    }
}
