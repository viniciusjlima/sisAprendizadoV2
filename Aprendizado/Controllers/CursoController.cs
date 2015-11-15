using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Controllers
{
    [Authorize]
    public class CursoController : Controller
    {
        private CursoModel cursoModel = new CursoModel();

        public ActionResult Index()
        {
            return View(cursoModel.todosCursos());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
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
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Curso c)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
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
            return Redirect("/Shared/Restrito");
        }

        private bool validarCurso(Curso Curso)
        {
            if (Curso.Descricao == "")
                return false;
            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Curso c = cursoModel.obterCurso(id);
                cursoModel.excluirCurso(c);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }

    }
}
