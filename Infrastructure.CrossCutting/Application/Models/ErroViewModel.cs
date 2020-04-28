using System;

namespace Infrastructure.CrossCutting.Application.Models
{
    [Serializable]
    public class ErroViewModel : _ResultBase
    {
        public ErroViewModel(int status = 404, string msg = "Erro")
        {
            Status = status;

            Mensagem = msg;
        }
    }
}
