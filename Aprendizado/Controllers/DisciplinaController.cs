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
    public class DisciplinaController : Controller
    {
        private DisciplinaModel disciplinaModel = new DisciplinaModel();
        private CursoModel cursoModel = new CursoModel();
        private ProfessorDisciplinaModel professorDisciplinaModel = new ProfessorDisciplinaModel();
        private ProfessorModel professorModel = new ProfessorModel();
        private PessoaModel pessoaModel = new PessoaModel();
        private TurmaModel turmaModel = new TurmaModel();

        public ActionResult Index()
        {
            return View(disciplinaModel.todasDisciplinas());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Disciplina d = new Disciplina();
                ViewBag.Titulo = "Nova Disciplina";

                int idCurso = 1;

                if (id != 0)
                {
                    d = disciplinaModel.obterDisciplina(id);
                    idCurso = d.idCurso;
                    ViewBag.Titulo = "Editar Disciplina";
                }

                ViewBag.idCurso
                    = new SelectList(cursoModel.todosCursos(),
                        "idCurso", "Descricao", idCurso);

                return View(d);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Disciplina d, Curso c)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {

                ViewBag.idCurso
                    = new SelectList(cursoModel.todosCursos(),
                        "idCurso", "Descricao", c);

                if (!validarDisciplina(d))
                {
                    ViewBag.Erro = "Erro na validação da Disciplina";
                    return View(d);
                }

                string erro = null;
                if (d.idDisciplina == 0)
                {
                    erro = disciplinaModel.adicionarDisciplina(d);
                }
                else
                {
                    erro = disciplinaModel.editarDisciplina(d);
                }
                if (erro == null)
                {
                    //return RedirectToAction("Index");
                    return RedirectToAction("ListaProfessorDisciplina", new { idDisciplina = d.idDisciplina });
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(d);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        private bool validarDisciplina(Disciplina disciplina)
        {
            if (disciplina.Descricao == "")
                return false;
            if (disciplina.idCurso == null)
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Disciplina d = disciplinaModel.obterDisciplina(id);
                disciplinaModel.excluirDisciplina(d);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");
        }

        /////////////////////////////// PROFESSORES DA DISCIPLINA ////////////////////////////////////////////////////////

        public ActionResult ListaProfessorDisciplina(int idDisciplina)
        {
            List<Professor_Disciplina> ProfessorDisciplinaDisciplinas =
                professorDisciplinaModel.listarProfessorDisciplinaPorDisciplina(idDisciplina);

            Disciplina d = disciplinaModel.obterDisciplina(idDisciplina);

            ViewBag.idDisciplina = d.idDisciplina;
            ViewBag.DescricaoDisciplina = d.Descricao;
            return View(ProfessorDisciplinaDisciplinas);
        }
        public ActionResult EditProfessorDisciplina(short idDisciplina, int idProfessorDisciplina)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Professor_Disciplina pd = new Professor_Disciplina();
                pd.idDisciplina = idDisciplina;

                int idProfessor = 1;
                int idTurma = 1;
                int idCurso = 1;

                if (idProfessorDisciplina != 0)
                {
                    pd = professorDisciplinaModel.obterProfessorDisciplina(idProfessorDisciplina);
                    idProfessor = pd.idProfessor;
                    idTurma = pd.idTurma;
                    idCurso = pd.Disciplina.idCurso;

                }

                ViewBag.idCurso
                    = new SelectList(cursoModel.todosCursos(),
                        "idCurso", "Descricao", idCurso);

                ViewBag.idTurma
                    = new SelectList(turmaModel.obterTurmasPorCurso(idCurso),
                        "idTurma", "Identificacao", idTurma);

                ViewBag.idProfessor
                    = new SelectList(professorModel.todosProfessores(),
                        "idProfessor", "Pessoa.Nome", idProfessor);

                return View(pd);
            }
            return Redirect("/Shared/Restrito");
        }
        [HttpPost]
        public ActionResult EditProfessorDisciplina(Professor_Disciplina pd, Professor p, Curso c, Turma t)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                ViewBag.idCurso
                    = new SelectList(cursoModel.todosCursos(),
                        "idCurso", "Descricao", c);

                ViewBag.idTurma
                    = new SelectList(turmaModel.obterTurmasPorCurso(c.idCurso),
                        "idTurma", "Identificacao", t);

                ViewBag.idProfessor
                    = new SelectList(professorModel.todosProfessores(),
                        "idProfessor", "Pessoa.Nome", p);

                string erro = null;
                if (pd.idProfessorDisciplina == 0)
                {
                    erro = professorDisciplinaModel.adicionarProfessorDisciplina(pd);
                }
                else
                {
                    erro = professorDisciplinaModel.editarProfessorDisciplina(pd);
                }
                if (erro == null)
                {
                    return RedirectToAction("ListaProfessorDisciplina", new { idDisciplina = pd.idDisciplina });
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(pd);
                }
            }
            return Redirect("/Shared/Restrito");
        }
        public ActionResult DeleteProfessorDisciplina(int idProfessorDisciplina)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Professor_Disciplina pd = professorDisciplinaModel.obterProfessorDisciplina(idProfessorDisciplina);
                professorDisciplinaModel.excluirProfessorDisciplina(pd);
                return RedirectToAction("ListaProfessorDisciplina", new { idDisciplina = pd.idDisciplina });
            }
            return Redirect("/Shared/Restrito");
        }

        public JsonResult ListaTurmas(int idCurso)
        {
            var turmas
                = new SelectList(turmaModel.obterTurmasPorCurso(idCurso), "idTurma", "Identificacao");
            return Json(new { turmas = turmas });
        }

    }

}
