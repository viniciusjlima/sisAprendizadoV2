using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Class
{
    public class ErradaDisciplina
    {
        public string Disciplina { get; set; }
        public int QtdErradas { get; set; }

        public ErradaDisciplina(string disciplina, int qtdErradas)
        {
            this.Disciplina = disciplina;
            this.QtdErradas = qtdErradas;
        }

        public ErradaDisciplina()
        {
            
        }

    }
}