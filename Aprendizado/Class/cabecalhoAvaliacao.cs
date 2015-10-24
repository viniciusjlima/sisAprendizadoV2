using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Class
{
    public class cabecalhoAvaliacao
    {
        public short IdDisciplina { get; set; }
        public int IdTurma { get; set; }
        public string Identificacao { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime DataEncerramento { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int StatusidStatus { get; set; }


        public cabecalhoAvaliacao(short idDisciplina, int idTurma, string identificacao, DateTime dataAbertura, 
                                    DateTime dataEncerramento, string titulo, string descricao, int statusidStatus)
        {
            this.IdDisciplina = idDisciplina;
            this.IdTurma = idTurma;
            this.Identificacao = identificacao;
            this.DataAbertura = dataAbertura;
            this.DataEncerramento = dataEncerramento;
            this.Titulo = titulo;
            this.StatusidStatus = statusidStatus;
        }

        public cabecalhoAvaliacao()
        {
            
        }
    }
}