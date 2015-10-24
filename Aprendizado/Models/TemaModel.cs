using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class TemaModel
    {
         private AprendizadoEntities db = new AprendizadoEntities();

         public List<Tema> todosTemas()
         {
             var lista = from t in db.Tema
                         select t;
             return lista.ToList();
         }

        public Tema obterTema(int idTema)
        {
            var lista = from t in db.Tema
                        where t.idTema == idTema
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public Tema obterTemaPorDescricao(string tema)
        {
            var lista = from t in db.Tema
                        where t.Descricao == tema
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public List<Tema> obterTemasPorDisciplina(int idDisciplina)
        {
            var lista = from d in db.Tema
                        where d.idDisciplina == idDisciplina
                        select d;
            return lista.ToList();
        }

        public string adicionarTema(Tema t)
        {
            string erro = null;
            try
            {
                db.Tema.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarTema(Tema t)
        {
            string erro = null;

            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.Tema.Attach(t);
                }
                db.ObjectStateManager.ChangeObjectState(t,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirTema(Tema t)
        {
            string erro = null;

            try
            {
                db.DeleteObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Tema> listarTemaPorCurso(int idAluno)
        {
            var lista = from a in db.Aluno
                        join t in db.Turma on a.idTurma equals t.idTurma
                        join d in db.Disciplina on t.idCurso equals d.idCurso
                        join te in db.Tema on d.idDisciplina equals te.idDisciplina
                        where a.idAluno == idAluno
                        select te;
            return lista.ToList();
        }

    }
}