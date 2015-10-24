using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{

    public class TipoTelefoneModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<TipoTelefone> todosTiposTelefones()
        {
            var lista = from t in db.TipoTelefone
                        select t;
            return lista.ToList();
        }

        public TipoTelefone obterTipoTelefone(int idTipoTelefone)
        {
            var lista = from t in db.TipoTelefone
                        where t.idTipoTelefone == idTipoTelefone
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public string adicionarTipoTelefone(TipoTelefone t)
        {
            string erro = null;
            try
            {
                db.TipoTelefone.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarTipoTelefone(TipoTelefone t)
        {
            string erro = null;

            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.TipoTelefone.Attach(t);
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

        public string excluirTipoTelefone(TipoTelefone t)
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