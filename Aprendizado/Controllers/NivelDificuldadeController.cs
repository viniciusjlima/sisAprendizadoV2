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
    public class NivelDificuldadeController : Controller
    {
        private NivelDificuldadeModel nivelDificuldadeModel = new NivelDificuldadeModel();

        public ActionResult Index()
        {
            return View(nivelDificuldadeModel.todosNiveisDificuldade());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                NivelDificuldade nd = new NivelDificuldade();
                ViewBag.Titulo = "Novo Nivel de Dificuldade";

                if (id != 0)
                {
                    nd = nivelDificuldadeModel.obterNivelDificuldade(id);
                    ViewBag.Titulo = "Editar Nivel de Dificuldade";
                }

                return View(nd);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(NivelDificuldade t)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                if (!validarNivelDificuldade(t))
                {
                    ViewBag.Erro = "Erro na validação do Nivel de Dificuldade";
                    return View(t);
                }

                string erro = null;
                if (t.idNivelDificuldade == 0)
                {
                    erro = nivelDificuldadeModel.adicionarNivelDificuldade(t);
                }
                else
                {
                    erro = nivelDificuldadeModel.editarNivelDificuldade(t);
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
            return Redirect("/Shared/Restrito");
        }

        private bool validarNivelDificuldade(NivelDificuldade nivelDificuldade)
        {
            if (nivelDificuldade.Descricao == "")
                return false;
            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                NivelDificuldade nd = nivelDificuldadeModel.obterNivelDificuldade(id);
                nivelDificuldadeModel.excluirNivelDificuldade(nd);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }

    }
}
