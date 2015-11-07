using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Class
{
    public class AtividadeDisciplina
    {
        public string Descricao { get; set; }
        public int QtdAtividades { get; set; }
        public int QtdAvaliacoes { get; set; }

        public AtividadeDisciplina(string descricao, int qtdAvaliacoes, int qtdAtividades)
        {
            this.Descricao = descricao;
            this.QtdAtividades = qtdAtividades;
            this.QtdAvaliacoes = qtdAvaliacoes;
        }

        public AtividadeDisciplina()
        {
            
        }

    }
}