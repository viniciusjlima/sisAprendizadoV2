using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class QuestaoResposta
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Questao_Resposta> todasQuestoesResposta()
        {
            var lista = from q in db.Questao_Resposta
                        select q;
            return lista.ToList();
        }

        public string adicionarQuestaoResposta(Questao_Resposta aa)
        {
            string erro = null;
            try
            {
                db.Questao_Resposta.AddObject(aa);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarQuestaoResposta(Questao_Resposta aa)
        {
            string erro = null;
            try
            {
                if (aa.EntityState == System.Data.EntityState.Detached)
                {
                    db.Questao_Resposta.Attach(aa);
                }
                db.ObjectStateManager.ChangeObjectState(aa, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Questao_Resposta obterQuestaoResposta(int id)
        {
            var lista = from aa in db.Questao_Resposta
                        where aa.idQuestaoResposta == id
                        select aa;
            return lista.ToList().FirstOrDefault();
        }

        public string excluirQuestaoResposta(Questao_Resposta aa)
        {
            string erro = null;
            try
            {
                db.DeleteObject(aa);
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