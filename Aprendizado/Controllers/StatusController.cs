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
    public class StatusController : Controller
    {
        private StatusModel statusModel = new StatusModel();

        public ActionResult Index()
        {
            return View(statusModel.todosStatuss());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Status s = new Status();
                ViewBag.Titulo = "Novo Status";

                if (id != 0)
                {
                    s = statusModel.obterStatus(id);
                    ViewBag.Titulo = "Editar Status";
                }

                return View(s);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Status s)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                if (!validarStatus(s))
                {
                    ViewBag.Erro = "Erro na validação do Status";
                    return View(s);
                }

                string erro = null;
                if (s.idStatus == 0)
                {
                    erro = statusModel.adicionarStatus(s);
                }
                else
                {
                    erro = statusModel.editarStatus(s);
                }
                if (erro == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(s);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        private bool validarStatus(Status status)
        {
            if (status.Descricao == "")
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Status t = statusModel.obterStatus(id);
                statusModel.excluirStatus(t);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }

    }
}
