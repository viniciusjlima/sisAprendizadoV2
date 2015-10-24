using System;
using System.Collections.Generic;
using  System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class PerguntaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Pergunta> todasPerguntas()
        {
            var lista = from p in db.Pergunta
                        select p;
            return lista.ToList();

        }

        public Pergunta obterPergunta(int idPergunta)
        {
            var lista = from p in db.Pergunta
                        where p.idPergunta == idPergunta
                        select p;
            return lista.ToList().FirstOrDefault();
        }

        public string adicionarPergunta(Pergunta p)
        {
            string erro = null;
            try
            {
                db.Pergunta.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarPergunta(Pergunta p)
        {
            string erro = null;

            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.Pergunta.Attach(p);
                }
                db.ObjectStateManager.ChangeObjectState(p,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirPergunta(Pergunta p)
        {
            string erro = null;

            try
            {
                db.DeleteObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Pergunta> obterPerguntasPorTema(int idTema)
        {
            var lista = from p in db.Pergunta
                        where p.idTema == idTema
                        select p;
            return lista.ToList();
        }

        public List<Pergunta> listarPerguntasPorAtividade(int idAtividade)
        {
            var lista = from p in db.Pergunta
                        join pa in db.Pergunta_Atividade
                        on p.idPergunta equals pa.idPergunta
                        where pa.idAtividade == idAtividade
                        select p;
            return lista.ToList();
        }

        public List<Pergunta> listarPerguntasPorDisciplina(int idDisciplina)
        {
            var lista = from p in db.Pergunta
                        join t in db.Tema
                        on p.idTema equals t.idTema
                        join d in db.Disciplina
                        on t.idDisciplina equals d.idDisciplina
                        where d.idDisciplina == idDisciplina
                        select p;
            return lista.ToList();
        }

        public List<Pergunta> listarPerguntasParaSorteio(int idTema, int idNivelDificuldade)
        {
            var lista = from p in db.Pergunta
                        join t in db.Tema
                        on p.idTema equals t.idTema
                        join nd in db.NivelDificuldade
                        on p.idNivelDificuldade equals nd.idNivelDificuldade
                        where t.idTema == idTema && nd.idNivelDificuldade == idNivelDificuldade
                        select p;
            return lista.ToList();
        }

    }
}