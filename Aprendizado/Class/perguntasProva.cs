using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Class
{
   
    public class perguntasProva
    {
        public int IdPergunta { get; set; }
        public int IdTema { get; set; }
        public int IdNIvelDificuldade { get; set; }
        public string Titulo { get; set; }
        public string Enunciado { get; set; }
        public string Identificacao { get; set; }
        public int Correta { get; set; }

        public perguntasProva(int idPergunta, int idTema, int idNivelDificuldade, string titulo, string enunciado,
                              string identificacao, int correta)
        {
            this.IdPergunta = idPergunta;
            this.IdTema = IdTema;
            this.IdNIvelDificuldade = idNivelDificuldade;
            this.Titulo = titulo;
            this.Enunciado = enunciado;
            this.Identificacao = identificacao;
            this.Correta = correta;
        }

        public perguntasProva()
        {

        }
    }
}