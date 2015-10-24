using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class AlunoModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Aluno> todosAlunos()
        {
            var lista = from a in db.Aluno
                        select a;
            return lista.ToList();
        }

        public List<Aluno> listarAlunosPorTurma(int idTurma)
        {
            var lista = from a in db.Aluno
                        join t in db.Turma
                        on a.idTurma equals t.idTurma
                        where t.idTurma == idTurma
                        select a;
            return lista.ToList();
        }

        public string adicionarAluno(Aluno a)
        {
            string erro = null;
            try
            {
                db.Aluno.AddObject(a);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Aluno obterAluno(int idAluno)
        {
            var lista = from a in db.Aluno
                        where a.idAluno == idAluno
                        select a;
            return lista.ToList().FirstOrDefault();
        }

        public Aluno obterAluno2(int idPessoa)
        {
            var lista = from a in db.Aluno
                        where a.idPessoa == idPessoa
                        select a;
            return lista.ToList().FirstOrDefault();
        }

        public List<Aluno> listarAlunos(string pesquisa)
        {
            var lista = from a in db.Aluno
                        where a.Pessoa.Nome.Contains(pesquisa)
                        select a;
            return lista.ToList();
        }

        public string editarAluno(Aluno a)
        {
            string erro = null;
            try
            {
                if (a.EntityState == System.Data.EntityState.Detached)
                {
                    db.Aluno.Attach(a);
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

        public string excluirAluno(Aluno a)
        {
            string erro = null;
            try
            {
                db.Aluno.DeleteObject(a);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string validarAluno(Aluno a)
        {
            if (a.Matricula == null || a.Matricula == 0)
            {
                return "A Matricula não pode ser vazia!";
            }
            if (a.idTurma == null || a.idTurma == 0)
            {
                return "Informe uma turma";
            }
            if (a.idPessoa == null || a.idPessoa == 0)
            {
                return "Pessoa nao pode ser nula";
            }
            return null;
        }
    }
}