using System;

namespace Infrastructure.CrossCutting.Application.Models
{
    public class _ResultBase
    {
        /// <summary>
        /// Status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Mensagem.
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Data e hora corrente.
        /// </summary>
        private DateTime _Data;

        /// <summary>
        /// Data e hora em formato amigável.
        /// </summary>
        public string DataFormatada => _Data.ToString("dd/MM/yyyy HH:mm:ss");


        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public _ResultBase()
        {
            _Data = DateTime.Now;
        }
    }
}
