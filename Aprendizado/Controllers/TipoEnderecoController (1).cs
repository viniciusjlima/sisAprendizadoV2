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
    public class TipoEnderecoController : Controller
    {
        private TipoEnderecoModel teModel = new TipoEnderecoModel();

        public ActionResult Index()
        {
            return View(teModel.todosTiposEnderecos());
        }

        public PartialViewResult List(string q)
        {
            var tiposEndereco = teModel.listarTipoEnderecos(q);
            return PartialView(tiposEndereco);
        }

        public PartialViewResult Edit(int id)
        {
            TipoEndereco te = new TipoEndereco();
            if (id != 0)
            {
                te = teModel.obterTipoEndereco(id);
            }
            return PartialView(te);
        }

        [HttpPost]
        public ActionResult Edit(TipoEndereco te)
        {
            string erro = null;
            if (te.idTipoEndereco == 0)
            {
                erro = teModel.adicionarTipoEndereco(te);
            }
            else
            {
                erro = teModel.editarTipoEndereco(te);
            }
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(te);
            }
        }

        public ActionResult Delete(int id)
        {
            TipoEndereco te = teModel.obterTipoEndereco(id);
            teModel.excluirTipoEndereco(te);
            return RedirectToAction("Index");
        }

    }
}
