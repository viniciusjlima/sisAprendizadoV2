using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Class
{
    public class ErradaTema
    {
        public string Tema { get; set; }
        public int QtdErradas { get; set; }

        public ErradaTema(string tema, int qtdErradas)
        {
            this.Tema = tema;
            this.QtdErradas = qtdErradas;
        }

        public ErradaTema()
        {
            
        }

    }
}