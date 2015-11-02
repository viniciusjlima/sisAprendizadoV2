using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class AlternativaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Alternativa> todasAlternativas()
        {
            var lista = from a in db.Alternativa
                        select a;
            return lista.ToList();
        }

        public string adicionarAlternativa(Alternativa a)
        {
            string erro = null;
            try
            {
                db.Alternativa.AddObject(a);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Alternativa obterAlternativa(int idAlternativa)
        {
            var lista = from a in db.Alternativa
                        where a.idAlternativa == idAlternativa
                        select a;
            return lista.ToList().FirstOrDefault();
        }

        public List<Alternativa> obterAlternativaPorPergunta(int idPergunta)
        {
            var lista = from a in db.Alternativa
                        where a.idPergunta == idPergunta
                        select a;
            return lista.ToList();
        }

        public List<Alternativa> listarAlternativasPorPergunta(int idPergunta)
        {
            var lista = from a in db.Alternativa
                        where a.idPergunta == idPergunta
                        select a;

            return lista.ToList();
        }

        public string editarAlternativa(Alternativa a)
        {
            string erro = null;
            try
            {
                if (a.EntityState == System.Data.EntityState.Detached)
                {
                    db.Alternativa.Attach(a);
                }
                db.ObjectStateManager.ChangeObjectState(a, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirAlternativa(Alternativa a)
        {
            string erro = null;
            try
            {
                db.Alternativa.DeleteObject(a);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Alternativa> obterAlternativasPorPergunta(int idPergunta)
        {
            var lista = from a in db.Alternativa
                        where a.idPergunta == idPergunta
                        select a;
            return lista.ToList();
        }

    }
}