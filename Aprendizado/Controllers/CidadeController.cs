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
    public class CidadeController : Controller
    {
        private CidadeModel cidadeModel = new CidadeModel();
        private EstadoModel estadoModel = new EstadoModel();

        public ActionResult Index()
        {
            return View(cidadeModel.todasCidades());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Cidade t = new Cidade();
                ViewBag.Titulo = "Nova Cidade";

                string idEstadoSelecionado = "MG";

                if (id != 0)
                {
                    t = cidadeModel.obterCidade(id);
                    idEstadoSelecionado = t.UF;
                    ViewBag.Titulo = "Editar Cidade";
                }

                ViewBag.UF
                    = new SelectList(estadoModel.todosEstados(),
                        "UF", "Descricao", idEstadoSelecionado);


                return View(t);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Cidade t, Estado e)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                ViewBag.UF
                    = new SelectList(estadoModel.todosEstados(),
                        "UF", "Descricao", e);

                if (!validarCidade(t))
                {
                    ViewBag.Erro = "Erro na validação do Cidade";
                    return View(t);
                }

                string erro = null;
                if (t.idCidade == 0)
                {
                    erro = cidadeModel.adicionarCidade(t);
                }
                else
                {
                    erro = cidadeModel.editarCidade(t);
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

        private bool validarCidade(Cidade c)
        {
            if (c.Descricao == "")
                return false;
            if (c.UF == "")
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Cidade t = cidadeModel.obterCidade(id);
                cidadeModel.excluirCidade(t);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");


        }

    }
}
