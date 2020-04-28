using System.Globalization;
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
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = linhaFormatada.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            return sbReturn.ToString();
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
    }
}
