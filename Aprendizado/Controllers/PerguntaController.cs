using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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



        public ActionResult Index()
        {
            return View(perguntaModel.todasPerguntas());
        }

        public ActionResult Edit(int id)
        {
            Pergunta p = new Pergunta();
            ViewBag.Titulo = "Nova Pergunta";

            int idTemaSeleciondo = 1;
            int idNivelDificuldadeSelecionado = 1;
            int correta = 0;

            if (id != 0)
            {
                p = perguntaModel.obterPergunta(id);
                idTemaSeleciondo = p.idTema;
                idNivelDificuldadeSelecionado = p.idNivelDificuldade;
                correta = p.Correta;
                ViewBag.Titulo = "Editar Pergunta";
            }

            ViewBag.idTema
                = new SelectList(temaModel.todosTemas(),
                    "idTema", "Descricao", idTemaSeleciondo);

            ViewBag.idNivelDificuldade
                = new SelectList(nivelDificuldadeModel.todosNiveisDificuldade(),
                    "idNivelDificuldade", "Descricao", idNivelDificuldadeSelecionado);

            ViewBag.Correta
                = new SelectList(alternativaModel.obterAlternativasPorPergunta(id),
                    "idAlternativa", "Descricao", correta);

            return View(p);
        }

        [HttpPost]
        public ActionResult Edit(Pergunta p, Tema t, NivelDificuldade nd, Alternativa al)
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
                return RedirectToAction("EditAlternativa", new { idAlternativa = 0, p.idPergunta });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(p);
            }
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
            Pergunta p = perguntaModel.obterPergunta(id);
            perguntaModel.excluirPergunta(p);
            return RedirectToAction("Index");
        }


//////////////////////////////// ALTERNATIVAS DA PERGUNTA ////////////////////////////////////////////////////////

        public ActionResult ListaAlternativas(int idPergunta)
        {
            ViewBag.idPergunta = idPergunta;
            Pergunta p = perguntaModel.obterPergunta(idPergunta);
            ViewBag.IdentificacaoPergunta = p.Identificacao;
            ViewBag.TituloPergunta = p.Titulo;
            return View(alternativaModel.obterAlternativasPorPergunta(idPergunta));
        }

        public ActionResult EditAlternativa(int idAlternativa, int idPergunta)
        {
            Alternativa a = new Alternativa();
            a.idPergunta = idPergunta;
            if (idAlternativa != 0)
            {
                a = alternativaModel.obterAlternativa(idAlternativa);
            }

            return View(a);
        }

        [HttpPost]
        public ActionResult Editalternativa(Alternativa a, Pergunta p)
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

        public ActionResult DeleteAlternativa(int idAlternativa)
        {
            Alternativa a = alternativaModel.obterAlternativa(idAlternativa);
            alternativaModel.excluirAlternativa(a);
            return RedirectToAction("ListaAlternativas", new { idPergunta = a.idPergunta });
        }

    }
}
