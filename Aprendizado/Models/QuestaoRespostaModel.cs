using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class QuestaoRespostaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Questao_Resposta> todasQuestoesResposta()
        {
            var lista = from q in db.Questao_Resposta
                        select q;
            return lista.ToList();
        }

        public List<Questao_Resposta> listarQuestoesRespostaPorAlunoAtividade(int idAlunoAtividade)
        {
            var lista = from qr in db.Questao_Resposta
                        where qr.idAlunoAtividade == idAlunoAtividade
                        select qr;

            return lista.ToList();
        }

        public int  listarQuestoesRespostaCorretasPorAlunoAtividade(int idAlunoAtividade)
        {
            var lista = from qr in db.Questao_Resposta
                        join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                        join p in db.Pergunta on qr.idPergunta equals p.idPergunta
                        where qr.idAlunoAtividade == idAlunoAtividade && al.idAlternativa == p.Correta
                        select qr;

            return lista.ToList().Count;
        }

        public Questao_Resposta verficaRespostaAluno(int idAlunoAtividade, int idPergunta)
        {
            var lista = from qr in db.Questao_Resposta
                        where qr.idAlunoAtividade == idAlunoAtividade
                        && qr.idPergunta == idPergunta
                        select qr;
            return lista.ToList().FirstOrDefault();
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

        public Questao_Resposta obterQuestaoResposta2(int idAlunoAtividade, int idPergunta)
        {
            var lista = from aa in db.Questao_Resposta
                        where aa.idAlunoAtividade == idAlunoAtividade
                        && aa.idPergunta == idPergunta
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