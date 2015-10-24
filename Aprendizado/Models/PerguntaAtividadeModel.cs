using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class PerguntaAtividadeModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Pergunta_Atividade> todosPerguntaAtividade()
        {
            var lista = from pa in db.Pergunta_Atividade
                        select pa;
            return lista.ToList();
        }

        public string adicionarPerguntaAtividade(Pergunta_Atividade pa)
        {
            string erro = null;
            try
            {
                db.Pergunta_Atividade.AddObject(pa);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarPerguntaAtividade(Pergunta_Atividade pa)
        {
            string erro = null;
            try
            {
                if (pa.EntityState == System.Data.EntityState.Detached)
                {
                    db.Pergunta_Atividade.Attach(pa);
                }
                db.ObjectStateManager.ChangeObjectState(pa, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Pergunta_Atividade obterPerguntaAtividade(int id)
        {
            var lista = from pa in db.Pergunta_Atividade
                        where pa.idPerguntaAtividade == id
                        select pa;
            return lista.ToList().FirstOrDefault();
        }

        public string excluirPerguntaAtividade(Pergunta_Atividade pa)
        {
            string erro = null;
            try
            {
                db.DeleteObject(pa);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Pergunta_Atividade> listarPerguntaAtividadePorAtividade(int idAtividade)
        {
            var lista = from pa in db.Pergunta_Atividade
                        where pa.idAtividade == idAtividade
                        select pa;

            return lista.ToList();
        }
    }
}