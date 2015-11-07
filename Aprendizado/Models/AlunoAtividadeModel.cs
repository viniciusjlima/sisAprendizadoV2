using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class AlunoAtividadeModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Aluno_Atividade> todosAlunoAtividade()
        {
            var lista = from aa in db.Aluno_Atividade
                        select aa;
            return lista.ToList();
        }

        public string adicionarAlunoAtividade(Aluno_Atividade aa)
        {
            string erro = null;
            try
            {
                db.Aluno_Atividade.AddObject(aa);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarAlunoAtividade(Aluno_Atividade aa)
        {
            string erro = null;
            try
            {
                if (aa.EntityState == System.Data.EntityState.Detached)
                {
                    db.Aluno_Atividade.Attach(aa);
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

        public Aluno_Atividade obterAlunoAtividade(int id)
        {
            var lista = from aa in db.Aluno_Atividade
                        where aa.idAlunoAtividade == id
                        select aa;
            return lista.ToList().FirstOrDefault();
        }

        public Aluno_Atividade verficaAlunoAtividade(int idAluno, int idAtividade)
        {
            var lista = from aa in db.Aluno_Atividade
                        where aa.idAluno == idAluno
                        && aa.idAtividade == idAtividade
                        select aa;
            return lista.ToList().FirstOrDefault();
        }

        public string excluirAlunoAtividade(Aluno_Atividade aa)
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

        public List<Aluno_Atividade> listarAlunoAtividadePorAluno(int idAluno)
        {
            var lista = from aa in db.Aluno_Atividade
                        where aa.idAluno == idAluno
                        select aa;

            return lista.ToList();
        }

        public List<Pergunta_Atividade> listarPerguntasPorAtividade(int idAtividade)
        {
            var lista = from pa in db.Pergunta_Atividade
                        where pa.idAtividade == idAtividade
                        select pa;

            return lista.ToList();
        }

        public List<Pergunta> listarPerguntasErradas(int idAluno)
        {
            var lista = from p in db.Pessoa
                            join alu in db.Aluno on p.idPessoa equals alu.idAluno
                            join aa in db.Aluno_Atividade on alu.idAluno equals aa.idAluno
                            join qr in db.Questao_Resposta on aa.idAlunoAtividade equals qr.idAlunoAtividade
                            join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                            join per in db.Pergunta on qr.idPergunta equals per.idPergunta
                            join t in db.Tema on per.idTema equals t.idTema
                            join nd in db.NivelDificuldade on per.idNivelDificuldade equals nd.idNivelDificuldade
                        where qr.idAlternativa != per.Correta && alu.idAluno == idAluno
                        select per;
            return lista.ToList();
        }

        public List<Pergunta> listarPerguntasCertas(int idAluno)
        {
            var lista = from p in db.Pessoa
                        join alu in db.Aluno on p.idPessoa equals alu.idAluno
                        join aa in db.Aluno_Atividade on alu.idAluno equals aa.idAluno
                        join qr in db.Questao_Resposta on aa.idAlunoAtividade equals qr.idAlunoAtividade
                        join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                        join per in db.Pergunta on qr.idPergunta equals per.idPergunta
                        join t in db.Tema on per.idTema equals t.idTema
                        join nd in db.NivelDificuldade on per.idNivelDificuldade equals nd.idNivelDificuldade
                        where qr.idAlternativa == per.Correta && alu.idAluno == idAluno
                        select per;
            return lista.ToList();
        }

        public List<Tema> listarTemasPorAluno(int idAluno)
        {
            var lista = from t in db.Tema
                        join d in db.Disciplina on t.idDisciplina equals d.idDisciplina
                        join c in db.Curso on d.idCurso equals c.idCurso
                        join tu in db.Turma on c.idCurso equals tu.idCurso
                        join al in db.Aluno on tu.idTurma equals al.idTurma
                        where al.idAluno == idAluno
                        select t;
            return lista.ToList();
        }

        public List<Tema> listarTemasPorAlunoDisciplina(int idAluno, int idDisciplina)
        {
            var lista = from t in db.Tema
                        join d in db.Disciplina on t.idDisciplina equals d.idDisciplina
                        join c in db.Curso on d.idCurso equals c.idCurso
                        join tu in db.Turma on c.idCurso equals tu.idCurso
                        join al in db.Aluno on tu.idTurma equals al.idTurma
                        where al.idAluno == idAluno && d.idDisciplina == idDisciplina
                        select t;
            return lista.ToList();
        }

        public List<Pergunta> listarPerguntasErradasPorTema(int idAluno, int idTema)
        {
            var lista = from p in db.Pessoa
                        join alu in db.Aluno on p.idPessoa equals alu.idAluno
                        join aa in db.Aluno_Atividade on alu.idAluno equals aa.idAluno
                        join qr in db.Questao_Resposta on aa.idAlunoAtividade equals qr.idAlunoAtividade
                        join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                        join per in db.Pergunta on qr.idPergunta equals per.idPergunta
                        join t in db.Tema on per.idTema equals t.idTema
                        join nd in db.NivelDificuldade on per.idNivelDificuldade equals nd.idNivelDificuldade
                        where qr.idAlternativa != per.Correta && alu.idAluno == idAluno && t.idTema == idTema
                        select per;
            return lista.ToList();
        }

        public int listarPerguntasErradasPorTema2(int idAluno, int idTema)
        {
            var lista = from p in db.Pessoa
                        join alu in db.Aluno on p.idPessoa equals alu.idAluno
                        join aa in db.Aluno_Atividade on alu.idAluno equals aa.idAluno
                        join qr in db.Questao_Resposta on aa.idAlunoAtividade equals qr.idAlunoAtividade
                        join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                        join per in db.Pergunta on qr.idPergunta equals per.idPergunta
                        join t in db.Tema on per.idTema equals t.idTema
                        join nd in db.NivelDificuldade on per.idNivelDificuldade equals nd.idNivelDificuldade
                        where qr.idAlternativa != per.Correta && alu.idAluno == idAluno && t.idTema == idTema
                        select per;
            return lista.Count();
        }

        public int listarPerguntasErradasPorDisciplina(int idAluno, int idDisciplina)
        {
            var lista = from p in db.Pessoa
                        join alu in db.Aluno on p.idPessoa equals alu.idAluno
                        join aa in db.Aluno_Atividade on alu.idAluno equals aa.idAluno
                        join qr in db.Questao_Resposta on aa.idAlunoAtividade equals qr.idAlunoAtividade
                        join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                        join per in db.Pergunta on qr.idPergunta equals per.idPergunta
                        join t in db.Tema on per.idTema equals t.idTema
                        join nd in db.NivelDificuldade on per.idNivelDificuldade equals nd.idNivelDificuldade
                        join d in db.Disciplina on t.idDisciplina equals d.idDisciplina
                        where qr.idAlternativa != per.Correta && alu.idAluno == idAluno && d.idDisciplina == idDisciplina
                        select per;
            return lista.Count();
        }

        public int listarPerguntasCorretasPorDisciplina(int idAluno, int idDisciplina)
        {
            var lista = from p in db.Pessoa
                        join alu in db.Aluno on p.idPessoa equals alu.idAluno
                        join aa in db.Aluno_Atividade on alu.idAluno equals aa.idAluno
                        join qr in db.Questao_Resposta on aa.idAlunoAtividade equals qr.idAlunoAtividade
                        join al in db.Alternativa on qr.idAlternativa equals al.idAlternativa
                        join per in db.Pergunta on qr.idPergunta equals per.idPergunta
                        join t in db.Tema on per.idTema equals t.idTema
                        join nd in db.NivelDificuldade on per.idNivelDificuldade equals nd.idNivelDificuldade
                        join d in db.Disciplina on t.idDisciplina equals d.idDisciplina
                        where qr.idAlternativa == per.Correta && alu.idAluno == idAluno && d.idDisciplina == idDisciplina
                        select per;
            return lista.Count();
        }

        public int listarAtividadesFeitasPorAluno(int idAluno, int idDisciplina)
        {
            var lista = from aa in db.Aluno_Atividade
                        join a in db.Atividade on aa.idAtividade equals a.idAtividade
                        join d in db.Disciplina on a.idDisciplina equals d.idDisciplina
                        where aa.idAluno == idAluno && aa.idStatus == 3 && a.idTipo == 1 && d.idDisciplina == idDisciplina
                        select aa;

            return lista.Count();
        }

        public int listarAvaliacoesFeitasPorAluno(int idAluno, int idDisciplina)
        {
            var lista = from aa in db.Aluno_Atividade
                        join a in db.Atividade on aa.idAtividade equals a.idAtividade
                        join d in db.Disciplina on a.idDisciplina equals d.idDisciplina
                        where aa.idAluno == idAluno && aa.idStatus == 3 && a.idTipo == 2 && d.idDisciplina == idDisciplina
                        select aa;

            return lista.Count();
        }

    }
}