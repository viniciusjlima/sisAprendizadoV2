using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class TelefoneModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Telefone> todosTelefones()
        {
            var lista = from t in db.Telefone
                        select t;
            return lista.ToList();
        }

        public string adicionarTelefone(Telefone t)
        {
            string erro = null;
            try
            {
                db.Telefone.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Telefone obterTelefone(int idTelefone)
        {
            var lista = from t in db.Telefone
                        where t.idTelefone == idTelefone
                        select t;
            return lista.ToList().FirstOrDefault();
        }

        public string editarTelefone(Telefone t)
        {
            string erro = null;
            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.Telefone.Attach(t);
                }
                db.ObjectStateManager.ChangeObjectState(t, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirTelefone(Telefone t)
        {
            string erro = null;
            try
            {
                db.Telefone.DeleteObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Telefone> obterTelefonesPessoas(int idPessoa)
        {
            var lista = from t in db.Telefone
                        where t.idPessoa == idPessoa
                        select t;

            return lista.ToList();
        }
    }
}