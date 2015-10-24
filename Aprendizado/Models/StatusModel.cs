using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class StatusModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Status> todosStatuss()
        {
            var lista = from s in db.Status
                        select s;
            return lista.ToList();
        }

        public Status obterStatus(int idStatus)
        {
            var lista = from s in db.Status
                        where s.idStatus == idStatus
                        select s;
            return lista.ToList().FirstOrDefault();
        }


        public string adicionarStatus(Status s)
        {
            string erro = null;
            try
            {
                db.Status.AddObject(s);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarStatus(Status s)
        {
            string erro = null;

            try
            {
                if (s.EntityState == System.Data.EntityState.Detached)
                {
                    db.Status.Attach(s);
                }
                db.ObjectStateManager.ChangeObjectState(s,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirStatus(Status s)
        {
            string erro = null;

            try
            {
                db.DeleteObject(s);
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