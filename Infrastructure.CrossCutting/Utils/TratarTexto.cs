using System.Globalization;
using System.Linq;
using System.Text;

namespace Infrastructure.CrossCutting.Utils
{
    public static class TratarTexto
    {
        public static string Formatar(this string linha)
        {
            var linhaFormatada = linha.Replace("\t\t", "\t").Replace("\t", ",").ToUpper();
            linhaFormatada = linhaFormatada.Replace(",,", ",");

            return RetirarAcentuacao(linhaFormatada);
        }

        public static string RetirarAcentuacao(this string linhaFormatada)
        {
            StringBuilder sb = new StringBuilder();
            var listaTexto = linhaFormatada.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letra in listaTexto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letra) != UnicodeCategory.NonSpacingMark)
                    sb.Append(letra);
            }

            return sb.ToString();
        }

        public static string CorrigirOrtografia(this string palavra)
        {
            switch (palavra)
            {
                case "ATHLETICO PR":
                    palavra = "ATLETICO PR";
                    break;

                case "VASCO DA GAMA":
                    palavra = "VASCO";
                    break;

                case "CORITITA":
                    palavra = "CORITIBA";
                    break;
                default:
                    break;
            }

            return palavra;
        }

        public static string PrimeiraLetraMaiuscula(this string palavra)
        {
            return char.ToUpper(palavra.First()) + palavra.Substring(1).ToLower();
        }
    }
}
