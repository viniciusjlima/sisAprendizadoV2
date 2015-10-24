using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class CidadeModel
    {

        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Cidade> todasCidades()
        {
            var lista = from c in db.Cidade
                        select c;
            return lista.ToList();
        }

        public string adicionarCidade(Cidade c)
        {
            string erro = null;
            try
            {
                db.Cidade.AddObject(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Cidade obterCidade(int idCidade)
        {
            var lista = from c in db.Cidade
                        where c.idCidade == idCidade
                        select c;
            return lista.ToList().FirstOrDefault();
        }

        public string editarCidade(Cidade c)
        {
            string erro = null;
            try
            {
                if (c.EntityState == System.Data.EntityState.Detached)
                {
                    db.Cidade.Attach(c);
                }
                db.ObjectStateManager.ChangeObjectState(c, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirCidade(Cidade c)
        {
            string erro = null;
            try
            {
                db.Cidade.DeleteObject(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Cidade> obterCidadesPorEstado(string UF)
        {
            var lista = from c in db.Cidade
                        where c.UF == UF
                        select c;
            return lista.ToList();
        }

    }
}