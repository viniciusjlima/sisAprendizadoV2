using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class TurmaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Turma> todasTurmas()
        {
            var lista = from t in db.Turma
                        select t;
            return lista.ToList();
        }

        public Turma obterTurma(int idTurma)
        {
            var lista = from t in db.Turma
                        where t.idTurma == idTurma
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public List<Turma> obterTurmasPorCurso(int idCurso)
        {
            var lista = from t in db.Turma
                        where t.idCurso == idCurso
                        select t;
            return lista.ToList();
        }

        public string adicionarTurma(Turma t)
        {
            string erro = null;
            try
            {
                db.Turma.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarTurma(Turma t)
        {
            string erro = null;

            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.Turma.Attach(t);
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

        public string excluirTurma(Turma t)
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
    }
}