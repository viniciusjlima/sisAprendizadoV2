using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class TipoModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Tipo> todosTipos()
        {
            var lista = from t in db.Tipo
                        select t;
            return lista.ToList();
        }

        public List<Tipo> listarTipos(string pesquisa)
        {
            var lista = from e in db.Tipo
                        where e.Descricao.Contains(pesquisa)
                        select e;
            return lista.ToList();
        }

        public Tipo obterTipo(int idTipo)
        {
            var lista = from t in db.Tipo
                        where t.idTipo == idTipo
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public string adicionarTipo(Tipo t)
        {
            string erro = null;
            try
            {
                db.Tipo.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarTipo(Tipo t)
        {
            string erro = null;
            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.Tipo.Attach(t);
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

        public string excluirTipo(Tipo t)
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