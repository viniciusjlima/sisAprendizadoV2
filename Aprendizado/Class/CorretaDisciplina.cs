using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Class
{
    public class CorretaDisciplina
    {
        public string Disciplina { get; set; }
        public int QtdCorretas { get; set; }

        public CorretaDisciplina(string disciplina, int qtdCorretas)
        {
            this.Disciplina = disciplina;
            this.QtdCorretas = qtdCorretas;
        }

        public CorretaDisciplina()
        {
            
        }

    }
}