using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Models;
using Aprendizado.Entity;

namespace Aprendizado.Controllers
{
    public class TemaController : Controller
    {
        private TemaModel temaModel = new TemaModel();
        private DisciplinaModel disciplinaModel = new DisciplinaModel();

        public ActionResult Index()
        {
            return View(temaModel.todosTemas());
        }

        public ActionResult Edit(int id)
        {
            Tema t = new Tema();
            ViewBag.Titulo = "Novo Tema";

            int idDisciplinaSelecionda = 1;

            if (id != 0)
            {
                t = temaModel.obterTema(id);
                idDisciplinaSelecionda = t.idDisciplina;
                ViewBag.Titulo = "Editar Tema";
            }

            ViewBag.idDisciplina
                = new SelectList(disciplinaModel.todasDisciplinas(),
                    "idDisciplina", "Descricao", idDisciplinaSelecionda);

            return View(t);
        }

        [HttpPost]
        public ActionResult Edit(Tema t, Disciplina d)
        {
            ViewBag.idDisciplina
                = new SelectList(disciplinaModel.todasDisciplinas(),
                    "idDisciplina", "Descricao", d);

            if (!validarTema(t))
            {
                ViewBag.Erro = "Erro na validação do Tema";
                return View(t);
            }

            string erro = null;
            if (t.idTema == 0)
            {
                erro = temaModel.adicionarTema(t);
            }
            else
            {
                erro = temaModel.editarTema(t);
            }
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(t);
            }
        }

        private bool validarTema(Tema tema)
        {
            if (tema.Descricao == "")
                return false;
            if (tema.idDisciplina == 0)
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            Tema t = temaModel.obterTema(id);
            temaModel.excluirTema(t);
            return RedirectToAction("Index");
        }
    }
}
