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
    [Authorize]
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
            if ((Roles.IsUserInRole(User.Identity.Name, "Administrador")) || (Roles.IsUserInRole(User.Identity.Name, "Professor")))
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
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Tema t, Disciplina d)
        {
            if ((Roles.IsUserInRole(User.Identity.Name, "Administrador")) || (Roles.IsUserInRole(User.Identity.Name, "Professor")))
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
            return Redirect("/Shared/Restrito");
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
            if ((Roles.IsUserInRole(User.Identity.Name, "Administrador")) || (Roles.IsUserInRole(User.Identity.Name, "Professor")))
            {
                Tema t = temaModel.obterTema(id);
                temaModel.excluirTema(t);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }
    }
}
