using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class ProfessorDisciplinaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Professor_Disciplina> todosProfessorDisciplina()
        {
            var lista = from pd in db.Professor_Disciplina
                        select pd;
            return lista.ToList();
        }

        public string adicionarProfessorDisciplina(Professor_Disciplina pd)
        {
            string erro = null;
            try
            {
                db.Professor_Disciplina.AddObject(pd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarProfessorDisciplina(Professor_Disciplina pd)
        {
            string erro = null;
            try
            {
                if (pd.EntityState == System.Data.EntityState.Detached)
                {
                    db.Professor_Disciplina.Attach(pd);
                }
                db.ObjectStateManager.ChangeObjectState(pd, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Professor_Disciplina obterProfessorDisciplina(int id)
        {
            var lista = from pd in db.Professor_Disciplina
                        where pd.idProfessorDisciplina == id
                        select pd;
            return lista.ToList().FirstOrDefault();
        }

        public string excluirProfessorDisciplina(Professor_Disciplina pa)
        {
            string erro = null;
            try
            {
                db.DeleteObject(pa);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Professor_Disciplina> listarProfessorDisciplinaPorDisciplina(int idDisciplina)
        {
            var lista = from pa in db.Professor_Disciplina
                        where pa.idDisciplina == idDisciplina
                        select pa;

            return lista.ToList();
        }

    }
}