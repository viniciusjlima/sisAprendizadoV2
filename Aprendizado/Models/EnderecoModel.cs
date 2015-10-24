using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class EnderecoModel
    {

        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Endereco> todosEnderecos()
        {
            var lista = from e in db.Endereco
                        select e;
            return lista.ToList();
        }

        public string adicionarEndereco(Endereco e)
        {
            string erro = null;
            try
            {
                db.Endereco.AddObject(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Endereco obterEndereco(int idEndereco)
        {
            var lista = from e in db.Endereco
                        where e.idEndereco == idEndereco
                        select e;
            return lista.ToList().FirstOrDefault();
        }

        public string editarEndereco(Endereco e)
        {
            string erro = null;
            try
            {
                if (e.EntityState == System.Data.EntityState.Detached)
                {
                    db.Endereco.Attach(e);
                }
                db.ObjectStateManager.ChangeObjectState(e, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirEndereco(Endereco e)
        {
            string erro = null;
            try
            {
                db.Endereco.DeleteObject(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Endereco> obterEnderecosPessoas(int idPessoa)
        {
            var lista = from e in db.Endereco
                        where e.idPessoa == idPessoa
                        select e;

            return lista.ToList();
        }

    }
}