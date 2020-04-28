using System;

namespace Infrastructure.CrossCutting.Application.Models
{
    [Serializable]
    public class SucessoViewModel : _ResultBase
    {
        public object Dados { get; set; }

        public SucessoViewModel(object obj, int status = 200, string msg = "Ok")
        {
            Status = status;

            Mensagem = msg;

            Dados = obj;
        }
    }
}
