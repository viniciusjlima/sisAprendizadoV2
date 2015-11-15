using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Aprendizado.Models;
using Aprendizado.Entity;

namespace Aprendizado.Controllers
{
    public class PerguntaController : Controller
    {
        private PerguntaModel perguntaModel = new PerguntaModel();
        private TemaModel temaModel = new TemaModel();
        private Disciplina disciplina = new Disciplina();
        private DisciplinaModel disciplinaModel = new DisciplinaModel();
        private AlternativaModel alternativaModel = new AlternativaModel();
        private NivelDificuldadeModel nivelDificuldadeModel = new NivelDificuldadeModel();
        private CursoModel cursoModel = new CursoModel();

        public ActionResult Index()
        {
            return View(perguntaModel.todasPerguntas());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {

                Pergunta p = new Pergunta();
                ViewBag.Titulo = "Nova Pergunta";

                int idCurso = 1;
                int idDisciplina = 1;

                int idTema = 1;
                int idNivelDificuldadeSelecionado = 1;
                int correta = 1;

                if (id != 0)
                {
                    p = perguntaModel.obterPergunta(id);
                    idCurso = p.Tema.Disciplina.idCurso;
                    idDisciplina = p.Tema.idDisciplina;
                    idTema = p.idTema;
                    idNivelDificuldadeSelecionado = p.idNivelDificuldade;
                    correta = p.Correta;
                    ViewBag.Titulo = "Editar Pergunta";
                }


                ViewBag.idCurso
                    = new SelectList(cursoModel.todosCursos(),
                        "idCurso", "Descricao", idCurso);

                ViewBag.idDisciplina
                    = new SelectList(disciplinaModel.obterDisciplinaPorCurso(idCurso),
                        "idDisciplina", "Descricao", idDisciplina);

                ViewBag.idTema
                    = new SelectList(temaModel.obterTemasPorDisciplina(idCurso),
                        "idTema", "Descricao", idTema);

                ViewBag.idNivelDificuldade
                    = new SelectList(nivelDificuldadeModel.todosNiveisDificuldade(),
                        "idNivelDificuldade", "Descricao", idNivelDificuldadeSelecionado);

                ViewBag.Correta
                    = new SelectList(alternativaModel.obterAlternativasPorPergunta(id),
                        "idAlternativa", "Descricao", correta);

                return View(p);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Pergunta p, Tema t, NivelDificuldade nd, Alternativa al)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                ViewBag.idTema
                    = new SelectList(temaModel.todosTemas(),
                        "idTema", "Descricao", t);

                ViewBag.idNivelDificuldade
                    = new SelectList(nivelDificuldadeModel.todosNiveisDificuldade(),
                        "idNivelDificuldade", "Descricao", nd);

                ViewBag.Correta
                    = new SelectList(alternativaModel.obterAlternativasPorPergunta(p.idPergunta),
                        "idAlternativa", "Descricao", al);

                if (!validarPergunta(p))
                {
                    ViewBag.Erro = "Erro na validação da Pergunta";
                    return View(p);
                }

                string erro = null;
                if (p.idPergunta == 0)
                {
                    erro = perguntaModel.adicionarPergunta(p);
                }
                else
                {
                    erro = perguntaModel.editarPergunta(p);
                }
                if (erro == null)
                {
                    if (p.Correta == 0)
                    {
                        return RedirectToAction("EditAlternativa", new { idAlternativa = 0, p.idPergunta });
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(p);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        private bool validarPergunta(Pergunta pergunta)
        {

            if (pergunta.Identificacao == "")
                return false;
            if (pergunta.Titulo == "")
                return false;
            if (pergunta.Enunciado == "")
                return false;
            if (pergunta.Correta == null)
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                Pergunta p = perguntaModel.obterPergunta(id);
                perguntaModel.excluirPergunta(p);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }


        public JsonResult ListaDisciplinas(int curso)
        {
            var disciplinas
                = new SelectList(disciplinaModel.obterDisciplinaPorCurso(curso), "idDisciplina", "Descricao");
            return Json(new { disciplinas = disciplinas });
        }

        public JsonResult ListaTemas(int disciplina)
        {
            var temas
                = new SelectList(temaModel.obterTemasPorDisciplina(disciplina), "idTema", "Descricao");
            return Json(new { temas = temas });
        }


        //////////////////////////////// ALTERNATIVAS DA PERGUNTA ////////////////////////////////////////////////////////

        public ActionResult ListaAlternativas(int idPergunta)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                ViewBag.idPergunta = idPergunta;
                Pergunta p = perguntaModel.obterPergunta(idPergunta);
                ViewBag.IdentificacaoPergunta = p.Identificacao;
                ViewBag.TituloPergunta = p.Titulo;
                return View(alternativaModel.obterAlternativasPorPergunta(idPergunta));
            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult EditAlternativa(int idAlternativa, int idPergunta)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                Alternativa a = new Alternativa();
                a.idPergunta = idPergunta;
                if (idAlternativa != 0)
                {
                    a = alternativaModel.obterAlternativa(idAlternativa);
                }

                return View(a);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Editalternativa(Alternativa a, Pergunta p)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                string erro = null;
                if (a.idAlternativa == 0)
                {
                    erro = alternativaModel.adicionarAlternativa(a);
                }
                else
                {
                    erro = alternativaModel.editarAlternativa(a);
                }
                if (erro == null)
                {
                    if (p.idPergunta == 0)
                    {
                        erro = "p.idPEssoa vazio";
                    }
                    return RedirectToAction("ListaAlternativas", new { idPergunta = a.idPergunta });
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(a);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult DeleteAlternativa(int idAlternativa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                Alternativa a = alternativaModel.obterAlternativa(idAlternativa);
                alternativaModel.excluirAlternativa(a);
                return RedirectToAction("ListaAlternativas", new { idPergunta = a.idPergunta });
            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult EscolheCorreta(int idPergunta, int idAlternativa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Professor"))
            {
                Pergunta p = perguntaModel.obterPergunta(idPergunta);
                Alternativa a = alternativaModel.obterAlternativa(idAlternativa);

                p.Correta = a.idAlternativa;

                if (!validarPergunta(p))
                {
                    ViewBag.Erro = "Erro na validação da Pergunta";
                    return View(p);
                }

                string erro = null;
                if (p.idPergunta == 0)
                {
                    erro = perguntaModel.adicionarPergunta(p);
                }
                else
                {
                    erro = perguntaModel.editarPergunta(p);
                }
                if (erro == null)
                {
                    return RedirectToAction("ListaAlternativas", new { idPergunta = a.idPergunta });
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(p);
                }
            }
            return Redirect("/Shared/Restrito");
        }

    }
}
