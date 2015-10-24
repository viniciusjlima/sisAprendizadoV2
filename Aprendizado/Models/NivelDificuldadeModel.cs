using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class NivelDificuldadeModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<NivelDificuldade> todosNiveisDificuldade()
        {
            var lista = from nd in db.NivelDificuldade
                        select nd;
            return lista.ToList();
        }

        public NivelDificuldade obterNivelDificuldade(int idNivelDificuldade)
        {
            var lista = from nd in db.NivelDificuldade
                        where nd.idNivelDificuldade == idNivelDificuldade
                        select nd;
            return lista.ToList().FirstOrDefault();
        }

        public string adicionarNivelDificuldade(NivelDificuldade nd)
        {
            string erro = null;
            try
            {
                db.NivelDificuldade.AddObject(nd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarNivelDificuldade(NivelDificuldade nd)
        {
            string erro = null;

            try
            {
                if (nd.EntityState == System.Data.EntityState.Detached)
                {
                    db.NivelDificuldade.Attach(nd);
                }
                db.ObjectStateManager.ChangeObjectState(nd,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirNivelDificuldade(NivelDificuldade nd)
        {
            string erro = null;

            try
            {
                db.DeleteObject(nd);
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