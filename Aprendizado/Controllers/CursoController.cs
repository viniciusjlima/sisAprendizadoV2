using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Controllers
{
    public class CursoController : Controller
    {
        private CursoModel cursoModel = new CursoModel();

        public ActionResult Index()
        {
            return View(cursoModel.todosCursos());
        }

        public ActionResult Edit(int id)
        {
            Curso c = new Curso();
            ViewBag.Titulo = "Novo Curso";

            if (id != 0)
            {
                c = cursoModel.obterCurso(id);
                ViewBag.Titulo = "Editar Curso";
            }

            return View(c);
        }

        [HttpPost]
        public ActionResult Edit(Curso c)
        {
            if (!validarCurso(c))
            {
                ViewBag.Erro = "Erro na validação da Curso";
                return View(c);
            }

            string erro = null;
            if (c.idCurso == 0)
            {
                erro = cursoModel.adicionarCurso(c);
            }
            else
            {
                erro = cursoModel.editarCurso(c);
            }
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(c);
            }
        }

        private bool validarCurso(Curso Curso)
        {
            if (Curso.Descricao == "")
                return false;
            return true;
        }

        public ActionResult Delete(int id)
        {
            Curso c = cursoModel.obterCurso(id);
            cursoModel.excluirCurso(c);
            return RedirectToAction("Index");
        }

    }
}
