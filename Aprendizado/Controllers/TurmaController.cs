using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;

namespace Aprendizado.Controllers
{
    public class TurmaController : Controller
    {
        private TurmaModel turmaModel = new TurmaModel();
        private CursoModel cursoModel = new CursoModel();

        public ActionResult Index()
        {
            return View(turmaModel.todasTurmas());
        }

        public ActionResult Edit(int id)
        {
            Turma t = new Turma();
            ViewBag.Titulo = "Nova Turma";

            int idCurso = 1;

            if (id != 0)
            {
                t = turmaModel.obterTurma(id);
                idCurso = t.idCurso;
                ViewBag.Titulo = "Editar Turma";
            }

            ViewBag.idCurso
                = new SelectList(cursoModel.todosCursos(),
                    "idCurso", "Descricao", idCurso);

            return View(t);
        }

        [HttpPost]
        public ActionResult Edit(Turma t, Curso c)
        {
            ViewBag.idCurso
                = new SelectList(cursoModel.todosCursos(),
                    "idCurso", "Descricao", c);
            
            if (!validarTurma(t))
            {
                ViewBag.Erro = "Erro na validação da Turma";
                return View(t);
            }

            string erro = null;
            if (t.idTurma == 0)
            {
                erro = turmaModel.adicionarTurma(t);
            }
            else
            {
                erro = turmaModel.editarTurma(t);
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

        private bool validarTurma(Turma turma)
        {
            if (turma.Identificacao == "")
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            Turma t = turmaModel.obterTurma(id);
            turmaModel.excluirTurma(t);
            return RedirectToAction("Index");
        }

    }
}
