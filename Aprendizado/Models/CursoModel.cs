using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class CursoModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Curso> todosCursos()
        {
            var lista = from c in db.Curso
                        select c;
            return lista.ToList();
        }

        public Curso obterCurso(int idCurso)
        {
            var lista = from c in db.Curso
                        where c.idCurso == idCurso
                        select c;
            return lista.ToList().FirstOrDefault();
        }


        public string adicionarCurso(Curso c)
        {
            string erro = null;
            try
            {
                db.Curso.AddObject(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarCurso(Curso c)
        {
            string erro = null;

            try
            {
                if (c.EntityState == System.Data.EntityState.Detached)
                {
                    db.Curso.Attach(c);
                }
                db.ObjectStateManager.ChangeObjectState(c,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirCurso(Curso c)
        {
            string erro = null;

            try
            {
                db.DeleteObject(c);
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