using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class AtividadeModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Atividade> todasAtividades()
        {
            var lista = from a in db.Atividade
                        where a.idTipo == 1
                        select a;
            return lista.ToList();
        }

        public List<Atividade> todasAvaliacoes()
        {
            var lista = from a in db.Atividade
                        where a.idTipo == 2
                        select a;
            return lista.ToList();
        }

        public List<Atividade> listarAtividadesEAvaliacoes()
        {
            var lista = from a in db.Atividade
                        select a;
            return lista.ToList();
        }

        public List<Atividade> listarAvaliacaoPorAluno(int idAluno)
        {
            var lista = from a in db.Atividade
                        join aa in db.Aluno_Atividade
                        on a.idAtividade equals aa.idAtividade
                        join alu in db.Aluno
                        on aa.idAluno equals alu.idAluno
                        where alu.idAluno == idAluno && a.idTipo == 2
                        select a;
            return lista.ToList();
        }

        public List<Atividade> listarAtividadesPorDisciplina(int idDisciplina)
        {
            var lista = from a in db.Atividade
                        join d in db.Disciplina
                        on a.idDisciplina equals d.idDisciplina
                        where a.idDisciplina == idDisciplina
                        select a;
            return lista.ToList();
        }

        public List<Atividade> listarAtividadesPorTurma(int idTurma)
        {
            var lista = from a in db.Atividade
                        join d in db.Turma
                        on a.idTurma equals d.idTurma
                        where a.idTurma == idTurma
                        && a.idStatus == 1 && a.idTipo == 1
                        select a;
            return lista.ToList();
        }


        public Atividade obterAtividade(int idAtividade)
        {
            var lista = from a in db.Atividade
                        where a.idAtividade == idAtividade
                        select a;
            return lista.ToList().FirstOrDefault();
        }

        public string adicionarAtividade(Atividade a)
        {
            string erro = null;
            try
            {
                db.Atividade.AddObject(a);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarAtividade(Atividade a)
        {
            string erro = null;

            try
            {
                if (a.EntityState == System.Data.EntityState.Detached)
                {
                    db.Atividade.Attach(a);
                }
                db.ObjectStateManager.ChangeObjectState(a,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirAtividade(Atividade a)
        {
            string erro = null;

            try
            {
                db.DeleteObject(a);
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