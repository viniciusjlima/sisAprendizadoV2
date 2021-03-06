﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;
using Aprendizado.ViewModels;

namespace Aprendizado.Controllers
{
    [Authorize]
    public class AlunoAtividadeController : Controller
    {
        private AlunoModel alunoModel = new AlunoModel();
        private AtividadeModel atividadeModel = new AtividadeModel();
        private UsuarioModel usuarioModel = new UsuarioModel();
        private TurmaModel turmaModel = new TurmaModel();
        private AlunoAtividadeModel alunoAtividadeModel = new AlunoAtividadeModel();
        private PerguntaAtividadeModel perguntaAtividadeModel = new PerguntaAtividadeModel();
        private PerguntaModel perguntaModel = new PerguntaModel();
        private DisciplinaModel disciplinaModel = new DisciplinaModel();

        private QuestaoRespostaModel questaoRespostaModel = new QuestaoRespostaModel();
        private AlternativaModel alternativaModel = new AlternativaModel();



        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                // Recebe o id do usuario do Aluno Logado
                string loginAluno = User.Identity.Name;
                Usuario usuario = usuarioModel.obterUsuarioPorLogin(loginAluno);
                int idUsuarioAluno = usuario.idUsuario;
                int idPessoaAluno = usuario.idPessoa;
                ViewBag.IdUsuarioAluno = idUsuarioAluno;
                ViewBag.IdPessoaAluno = idUsuarioAluno;

                Aluno a = alunoModel.obterAluno2(idPessoaAluno);
                //ViewBag.NomeAluno = a.Pessoa.Nome;

                ViewBag.IdAluno = a.idAluno;

                return View(atividadeModel.listarAtividadesPorTurma(a.idTurma));
            }

            return View("/Shared/Restrito");
        }

        public ActionResult Avaliacoes()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                // Recebe o id do usuario do Aluno Logado
                string loginAluno = User.Identity.Name;
                Usuario usuario = usuarioModel.obterUsuarioPorLogin(loginAluno);
                int idUsuarioAluno = usuario.idUsuario;
                int idPessoaAluno = usuario.idPessoa;
                ViewBag.IdUsuarioAluno = idUsuarioAluno;
                ViewBag.IdPessoaAluno = idUsuarioAluno;

                Aluno a = alunoModel.obterAluno2(idPessoaAluno);
                //ViewBag.NomeAluno = a.Pessoa.Nome;

                ViewBag.IdAluno = a.idAluno;

                return View(atividadeModel.listarAvaliacaoPorAluno(a.idAluno));
            }

            return View("/Shared/Restrito");
        }

        public ActionResult VerificaAlunoAtividade(int idAluno, int idAtividade)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                Aluno_Atividade aa = new Aluno_Atividade();
                aa = alunoAtividadeModel.verficaAlunoAtividade(idAluno, idAtividade);

                if (aa == null)
                {
                    int idAlunoAtividade = 0;
                    return RedirectToAction("AcessarAtividade", new { idAluno, idAtividade, idAlunoAtividade });
                }
                else
                {
                    if (((aa.idStatus == 1) || (aa.idStatus == 0)) && ((aa.idStatus != 3)&&(aa.idStatus != 2)))
                    {
                        if (aa.idAlunoAtividade != 0)
                        {
                            int idAlunoAtividade = aa.idAlunoAtividade;

                            if(aa.Atividade.idTipo == 1)
                                return RedirectToAction("AcessarAtividade", new { idAluno, idAtividade, idAlunoAtividade });

                            return RedirectToAction("AcessarAvaliacao", new { idAluno, idAtividade, idAlunoAtividade });
                        }
                        else
                        {
                            int idAlunoAtividade = 0;

                            if (aa.Atividade.idTipo == 1)
                                return RedirectToAction("AcessarAtividade", new { idAluno, idAtividade, idAlunoAtividade });

                            return RedirectToAction("AcessarAvaliacao", new { idAluno, idAtividade, idAlunoAtividade });
                        }
                    }
                    else
                    {
                        aa.idStatus = 3;
                        return RedirectToAction("Respostas", new { idAlunoAtividade = aa.idAlunoAtividade });
                    }
                }
            }
            return View("/Shared/Restrito");
        }

        public ActionResult AcessarAvaliacao(int idAluno, int idAtividade, int idAlunoAtividade)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                Aluno_Atividade aa = new Aluno_Atividade();
                aa.idAluno = idAluno;
                aa.idAtividade = idAtividade;
                aa.idAlunoAtividade = idAlunoAtividade;

                if (idAlunoAtividade != 0)
                {
                    aa = alunoAtividadeModel.obterAlunoAtividade(idAlunoAtividade);
                    idAtividade = aa.idAtividade;
                    idAluno = aa.idAluno;
                    if ((aa.Atividade.idTipo == 2) && (aa.idStatus == 1))
                    {
                        return View(aa);
                    }
                    aa.idStatus = 3;

                }


                return View(aa);
            }
            return Redirect("/Shared/Restrito");
        }


        [HttpPost]
        public ActionResult AcessarAvaliacao(Aluno_Atividade aa)
        {
            Aluno_Atividade at = alunoAtividadeModel.obterAlunoAtividade(aa.idAlunoAtividade);

            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {

                string erro = null;

                if ((at.idStatus == 1)||(at.idStatus == null))
                {
                    aa.idStatus = 1; // Acessando Atividade pela primeira vez: idStatus = Aberto
                    erro = alunoAtividadeModel.adicionarAlunoAtividade(at);

                    return RedirectToAction("RealizarAtividade", new { idAlunoAtividade = at.idAlunoAtividade });
                }
                else
                {
                    at.idStatus = 3; // Tentando acessar atividade novamente: idStatus = Realizado
                    erro = alunoAtividadeModel.editarAlunoAtividade(at);

                    return RedirectToAction("Respostas", new { idAlunoAtividade = at.idAlunoAtividade });
                }

            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult AcessarAtividade(int idAluno, int idAtividade, int idAlunoAtividade)
        {
            Aluno_Atividade aa = new Aluno_Atividade();
            aa.idAluno = idAluno;
            aa.idAtividade = idAtividade;

            if (idAlunoAtividade != 0)
            {
                aa = alunoAtividadeModel.obterAlunoAtividade(idAlunoAtividade);
                idAtividade = aa.idAtividade;
                idAluno = aa.idAluno;
                aa.idStatus = 3;
            }


            return View(aa);
        }


        [HttpPost]
        public ActionResult AcessarAtividade(Aluno_Atividade aa)
        {
            string erro = null;
            if (aa.idAlunoAtividade == 0)
            {
                aa.idStatus = 1; // Acessando Atividade pela primeira vez: idStatus = Aberto
                erro = alunoAtividadeModel.adicionarAlunoAtividade(aa);
            }
            else
            {
                aa.idStatus = 2; // Tentando acessar atividade novamente: idStatus = Realizado
                erro = alunoAtividadeModel.editarAlunoAtividade(aa);
            }
            if (erro == null)
            {
                return RedirectToAction("RealizarAtividade", new { idAlunoAtividade = aa.idAlunoAtividade });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(aa);
            }
        }


        private bool validarAlunoAtividade(Aluno_Atividade aa)
        {
            if (aa.idAluno == 0)
                return false;
            if (aa.idAtividade == 0)
                return false;

            return true;
        }

        [Authorize]
        public ActionResult Detalhar(int idAluno, int idAtividade)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {

            }
            return Redirect("/Shared/Login");
        }

        public ActionResult RealizarAtividade(int idAlunoAtividade)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                Aluno_Atividade aa = alunoAtividadeModel.obterAlunoAtividade(idAlunoAtividade);


                if (aa.idStatus == 1)
                {
                    List<Pergunta_Atividade> PerguntaAtividadeAtividades =
                         perguntaAtividadeModel.listarPerguntaAtividadePorAtividade(aa.idAtividade);

                    ViewBag.idAtividade = aa.idAtividade;
                    ViewBag.NomeAluno = aa.Aluno.Pessoa.Nome;
                    ViewBag.Atividade = aa.Atividade.Identificacao;
                    ViewBag.idAlunoAtividade = aa.idAlunoAtividade;


                    return View(PerguntaAtividadeAtividades);
                }
                else
                {
                    return RedirectToAction("Respostas", new { idAlunoAtividade = aa.idAlunoAtividade });
                }
            }
            return Redirect("/Shared/Restrito");

        }

        public ActionResult EditResposta(int idAlunoAtividade, int idPergunta)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                Questao_Resposta qr = new Questao_Resposta();

                Questao_Resposta q = questaoRespostaModel.verficaRespostaAluno(idAlunoAtividade, idPergunta);

                int idQuestaoResposta = 1;

                if (q == null)
                {
                    idQuestaoResposta = 0;
                }
                else
                {

                    idQuestaoResposta = questaoRespostaModel.verficaRespostaAluno(idAlunoAtividade, idPergunta).idQuestaoResposta;
                }

                qr.idAlunoAtividade = idAlunoAtividade;
                qr.idPergunta = idPergunta;

                Pergunta p = perguntaModel.obterPergunta(idPergunta);
                ViewBag.Enunciado = p.Enunciado;

                int idAlternativa = 1;

                if (idQuestaoResposta != 0)
                {
                    qr = questaoRespostaModel.obterQuestaoResposta(idQuestaoResposta);
                    idAlternativa = qr.idAlternativa;
                }

                ViewBag.idAlternativa
                    = new SelectList(alternativaModel.listarAlternativasPorPergunta(idPergunta),
                        "idAlternativa", "Descricao", idAlternativa);

                ViewBag.Pergunta = p.Enunciado;

                return View(qr);
            }
            return Redirect("/Shared/Restrito");
        }
        [HttpPost]
        public ActionResult EditResposta(Questao_Resposta qr, Alternativa a)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                ViewBag.idAlternativa
                    = new SelectList(alternativaModel.listarAlternativasPorPergunta(a.idPergunta),
                        "idAlternativa", "Descricao", a.idAlternativa);

                string erro = null;
                if (qr.idQuestaoResposta == 0)
                {
                    erro = questaoRespostaModel.adicionarQuestaoResposta(qr);
                }
                else
                {
                    erro = questaoRespostaModel.editarQuestaoResposta(qr);
                }
                if (erro == null)
                {
                    return RedirectToAction("RealizarAtividade", new { idAlunoAtividade = qr.idAlunoAtividade });
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(qr);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult Finalizar(int idAlunoAtividade)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                List<Questao_Resposta> Respostas =
                    questaoRespostaModel.listarQuestoesRespostaPorAlunoAtividade(idAlunoAtividade);

                Aluno_Atividade aa = alunoAtividadeModel.obterAlunoAtividade(idAlunoAtividade);
                ViewBag.IdentificacaoAtividade = aa.Atividade.Identificacao;
                ViewBag.TiTuloAtividade = aa.Atividade.Titulo;

                string erro = null;
                aa.idStatus = 3;
                erro = alunoAtividadeModel.editarAlunoAtividade(aa);

                return RedirectToAction("Avaliacoes");
            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult Respostas(int idAlunoAtividade)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                List<Questao_Resposta> Respostas =
                    questaoRespostaModel.listarQuestoesRespostaPorAlunoAtividade(idAlunoAtividade);

                Aluno_Atividade aa = alunoAtividadeModel.obterAlunoAtividade(idAlunoAtividade);
                ViewBag.IdentificacaoAtividade = aa.Atividade.Identificacao;
                ViewBag.TiTuloAtividade = aa.Atividade.Titulo;

                int qtdRespostas = questaoRespostaModel.listarQuestoesRespostaPorAlunoAtividade(idAlunoAtividade).Count;
                int certas = questaoRespostaModel.listarQuestoesRespostaCorretasPorAlunoAtividade(idAlunoAtividade);
                int porcentagem = (certas * 100) / 10;

                if (aa.Atividade.idTipo == 1)
                {
                    ViewBag.Tipo = "Atividade";
                }
                else
                {
                    ViewBag.Tipo = "Avaliação";
                }

                ViewBag.QtdRespostas = qtdRespostas;

                if (ViewBag.QtdRespostas < 10)
                {
                    ViewBag.MsgRespostas = "Apenas " + ViewBag.QtdRespostas;
                }
                else
                {
                    ViewBag.MsgRespostas = "Todas";
                }

                ViewBag.Porcentagem = porcentagem;

                return View(Respostas);
            }
            return Redirect("/Shared/Restrito");
        }

        public JsonResult ListaTurmas(int curso)
        {
            var turmas
                = new SelectList(turmaModel.obterTurmasPorCurso(curso), "idTurma", "Identificacao");
            return Json(new { turmas = turmas });
        }

        public JsonResult ListaDisciplinas(int curso)
        {
            var disciplinas
                = new SelectList((disciplinaModel.obterDisciplinaPorCurso(curso)), "idTurma", "Identificacao");
            return Json(new { disciplinas = disciplinas });
        }

    }
}
