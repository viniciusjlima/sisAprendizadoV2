using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Controllers
{
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

        [HttpPost]
        public ActionResult Edit(Cidade t, Estado e)
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
            Cidade t = cidadeModel.obterCidade(id);
            cidadeModel.excluirCidade(t);
            return RedirectToAction("Index");
        }

    }
}
