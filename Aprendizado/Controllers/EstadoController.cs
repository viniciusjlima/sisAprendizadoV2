using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;
using Aprendizado.ViewModels;

namespace Aprendizado.Controllers
{
    public class EstadoController : Controller
    {
        private EstadoModel estadoModel = new EstadoModel();

        public ActionResult Index()
        {
            return View(estadoModel.todosEstados());
        }

        public PartialViewResult List(string q)
        {
            var todosEstados = estadoModel.listarEstados(q);
            return PartialView(todosEstados);
        }

        public PartialViewResult Edit(string UF)
        {
            Estado e = new Estado();
            ViewBag.Titulo = "Novo Estado";

            if (UF != "")
            {
                e = estadoModel.obterEstado(UF);
                ViewBag.Titulo = "Editar Estado";
            }

            return PartialView(e);
        }

        [HttpPost]
        public ActionResult Edit(Estado e)
        {
            //string erro = null;
            //if (e.UF == "")
            //    erro = estadoModel.adicionarEstado(e);
            //else
            //    erro = estadoModel.editarEstado(e);
            //if (erro == null)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    ViewBag.Erro = erro;
            //    return View(e);
            //}

            if (!validarEstado(e))
            {
                ViewBag.Erro = "Erro na validação do Estado";
                return View(e);
            }

            string erro = null;
            if (e.UF == "")
            {
                erro = estadoModel.adicionarEstado(e);
            }
            else
            {
                erro = estadoModel.editarEstado(e);
            }
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(e);
            }
        }

        private bool validarEstado(Estado estado)
        {
            if (estado.UF == "")
            {
                return false;
            }
            if (estado.Descricao == "")
            {
                return false;
            }

            return true;
        }

        public ActionResult Delete(string uf)
        {
            Estado e = estadoModel.obterEstado(uf);
            estadoModel.excluirEstado(e);
            return RedirectToAction("Index");
        }

    }
}
