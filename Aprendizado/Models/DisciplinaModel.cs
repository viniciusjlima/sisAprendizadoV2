using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class DisciplinaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Disciplina> todasDisciplinas()
        {
            var lista = from d in db.Disciplina
                        select d;
            return lista.ToList();
        }

        public Disciplina obterDisciplina(int idDisciplina)
        {
            var lista = from d in db.Disciplina
                        where d.idDisciplina == idDisciplina
                        select d;
            return lista.ToList().FirstOrDefault();
        }

        public List<Disciplina> obterDisciplinaPorCurso(int idCurso)
        {
            var lista = from d in db.Disciplina
                        where d.idCurso == idCurso
                        select d;
            return lista.ToList();
        }

        public List<Disciplina> obterDisciplinaPorTurma(int idTurma)
        {
            var lista = from d in db.Disciplina
                        join c in db.Curso
                        on d.idCurso equals c.idCurso
                        join t in db.Turma
                        on c.idCurso equals t.idTurma
                        where t.idTurma == idTurma
                        select d;
            return lista.ToList();
        }

        public string adicionarDisciplina(Disciplina d)
        {
            string erro = null;
            try
            {
                db.Disciplina.AddObject(d);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarDisciplina(Disciplina d)
        {
            string erro = null;

            try
            {
                if (d.EntityState == System.Data.EntityState.Detached)
                {
                    db.Disciplina.Attach(d);
                }
                db.ObjectStateManager.ChangeObjectState(d,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirDisciplina(Disciplina d)
        {
            string erro = null;

            try
            {
                db.DeleteObject(d);
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