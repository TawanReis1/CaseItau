using System.Globalization;
using System.Text;

namespace Infrastructure.CrossCutting.Utils
{
    public class TratarTexto
    {
        public static string Formatar(string linha)
        {
            var linhaFormatada = linha.Replace("\t\t", "\t").Replace("\t", ",").ToUpper();
            linhaFormatada = linhaFormatada.Replace(",,", ",");

            //StringBuilder sbReturn = new StringBuilder();
            //var arrayText = linhaFormatada.Normalize(NormalizationForm.FormD).ToCharArray();
            //foreach (char letter in arrayText)
            //{
            //    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
            //        sbReturn.Append(letter);
            //}

            //return sbReturn.ToString();

            return RetirarAcentuacao(linhaFormatada);
        }

        public static string RetirarAcentuacao(string linhaFormatada)
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
    }
}
