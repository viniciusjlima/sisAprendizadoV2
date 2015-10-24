﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Controllers
{
    public class TelefoneController : Controller
    {
        private TelefoneModel telefoneModel = new TelefoneModel();
        private TipoTelefoneModel tipoTelefoneModel = new TipoTelefoneModel();
        private PessoaController pessoaController = new PessoaController();

        public ActionResult Index()
        {
            return View(telefoneModel.todosTelefones());
        }

        public ActionResult Edit(int id)
        {
            Telefone t = new Telefone();
            ViewBag.Titulo = "Novo Telefone";

            int idTipoTelefoneSelecionado = 1;

            if (id != 0)
            {
                t = telefoneModel.obterTelefone(id);
                idTipoTelefoneSelecionado = t.idTipoTelefone;
                ViewBag.Titulo = "Editar Telefone";
            }

            ViewBag.idTipoTelefone
                = new SelectList(tipoTelefoneModel.todosTiposTelefones(),
                    "idTipoTelefone", "Descricao", idTipoTelefoneSelecionado);

            return View(t);
        }

        [HttpPost]
        public ActionResult Edit(Telefone t, TipoTelefone te)
        {
            ViewBag.idTipoTelefone
               = new SelectList(tipoTelefoneModel.todosTiposTelefones(),
                   "idTipoTelefone", "Descricao", te);
            
            if (!validarTelefone(t))
            {
                ViewBag.Erro = "Erro na validação do Telefone";
                return View(t);
            }

            string erro = null;
            if (t.idTipoTelefone == 0)
            {
                erro = telefoneModel.adicionarTelefone(t);
            }
            else
            {
                erro = telefoneModel.editarTelefone(t);
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

        private bool validarTelefone(Telefone telefone)
        {
            if (telefone.Numero == 0)
                return false;
            if (telefone.idTipoTelefone == 0)
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            Telefone t = telefoneModel.obterTelefone(id);
            telefoneModel.excluirTelefone(t);
            return RedirectToAction("Index");
        }

    }
}