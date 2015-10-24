using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.ViewModels
{
    public class RealizarAtividadeViewModel
    {
        public List<Pergunta> perguntas { get; set; }
        public List<Alternativa> alternativas { get; set; }
    }
}