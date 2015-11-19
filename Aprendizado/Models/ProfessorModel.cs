using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class ProfessorModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Professor> todosProfessores()
        {
            var lista = from p in db.Professor
                        select p;
            return lista.ToList();
        }

        public Professor obterProfessor(int idProfessor)
        {
            var lista = from p in db.Professor
                        where p.idProfessor == idProfessor
                        select p;
            return lista.ToList().FirstOrDefault();
        }

        public Professor obterProfessorPorPessoa(int idPessoa)
        {
            var lista = from p in db.Professor
                        join pe in db.Pessoa on p.idPessoa equals pe.idPessoa
                        where p.idPessoa == idPessoa
                        select p;
            return lista.ToList().FirstOrDefault();
        }

        public Pessoa listarPessoaPorProfessor(int idProfessor)
        {
            var lista = from p in db.Pessoa
                        join pu in db.Professor
                        on p.idPessoa equals pu.idPessoa
                        where pu.idProfessor == idProfessor
                        select p;
            return lista.ToList().FirstOrDefault();
        }



        public string adicionarProfessor(Professor p)
        {
            string erro = null;
            try
            {
                db.Professor.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarProfessor(Professor p)
        {
            string erro = null;

            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.Professor.Attach(p);
                }
                db.ObjectStateManager.ChangeObjectState(p,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirProfessor(Professor p)
        {
            string erro = null;

            try
            {
                db.DeleteObject(p);
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