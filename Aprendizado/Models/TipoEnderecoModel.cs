using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class TipoEnderecoModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<TipoEndereco> todosTiposEnderecos()
        {
            var lista = from t in db.TipoEndereco
                        select t;
            return lista.ToList();
        }

        public List<TipoEndereco> listarTipoEnderecos(string pesquisa)
        {
            var lista = from e in db.TipoEndereco
                        where e.Descricao.Contains(pesquisa)
                        select e;
            return lista.ToList();
        }

        public TipoEndereco obterTipoEndereco(int idTipoEndereco)
        {
            var lista = from t in db.TipoEndereco
                        where t.idTipoEndereco == idTipoEndereco
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public string adicionarTipoEndereco(TipoEndereco t)
        {
            string erro = null;
            try
            {
                db.TipoEndereco.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarTipoEndereco(TipoEndereco t)
        {
            string erro = null;
            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.TipoEndereco.Attach(t);
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

        public string excluirTipoEndereco(TipoEndereco t)
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