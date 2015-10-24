using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Controllers
{
    public class TipoTelefoneController : Controller
    {
        private TipoTelefoneModel tipoTelefoneModel = new TipoTelefoneModel();

        public ActionResult Index()
        {
            return View(tipoTelefoneModel.todosTiposTelefones());
        }

        public ActionResult Edit(int id)
        {
            TipoTelefone tt = new TipoTelefone();
            ViewBag.Titulo = "Novo TipoTelefone";

            if (id != 0)
            {
                tt = tipoTelefoneModel.obterTipoTelefone(id);
                ViewBag.Titulo = "Editar TipoTelefone";
            }

            return View(tt);
        }

        [HttpPost]
        public ActionResult Edit(TipoTelefone tt)
        {

            if (!validarTipoTelefone(tt))
            {
                ViewBag.Erro = "Erro na validação do TipoTelefone";
                return View(tt);
            }

            string erro = null;
            if (tt.idTipoTelefone == 0)
            {
                erro = tipoTelefoneModel.adicionarTipoTelefone(tt);
            }
            else
            {
                erro = tipoTelefoneModel.editarTipoTelefone(tt);
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

        private bool validarTipoTelefone(TipoTelefone tipoTelefone)
        {
            if (tipoTelefone.Descricao == "")
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            TipoTelefone t = tipoTelefoneModel.obterTipoTelefone(id);
            tipoTelefoneModel.excluirTipoTelefone(t);
            return RedirectToAction("Index");
        }

    }
}
