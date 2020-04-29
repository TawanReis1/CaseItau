using Infrastructure.CrossCutting.Utils;
using System;

namespace Infrastructure.CrossCutting.Application.Models
{
    [Serializable]
    public class ErroViewModel : _ResultBase
    {
        public ErroViewModel(int status = 400, string msg = "Erro")
        {
            Status = status;

            Mensagem = msg.TratarErro();
        }
    }
}
