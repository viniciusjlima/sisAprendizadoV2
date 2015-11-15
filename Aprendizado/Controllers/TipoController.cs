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
    public class TipoController : Controller
    {
        private TipoModel tipoModel = new TipoModel();

        public ActionResult Index()
        {
            return View(tipoModel.todosTipos());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Tipo tt = new Tipo();
                ViewBag.Titulo = "Novo Tipo";

                if (id != 0)
                {
                    tt = tipoModel.obterTipo(id);
                    ViewBag.Titulo = "Editar Tipo";
                }

                return View(tt);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Tipo tt)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {

                if (!validarTipo(tt))
                {
                    ViewBag.Erro = "Erro na validação do Tipo";
                    return View(tt);
                }

                string erro = null;
                if (tt.idTipo == 0)
                {
                    erro = tipoModel.adicionarTipo(tt);
                }
                else
                {
                    erro = tipoModel.editarTipo(tt);
                }
                if (erro == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(tt);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        private bool validarTipo(Tipo tipo)
        {
            if (tipo.Descricao == "")
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {

                Tipo t = tipoModel.obterTipo(id);
                tipoModel.excluirTipo(t);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }

    }
}
