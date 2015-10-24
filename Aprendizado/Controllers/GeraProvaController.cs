using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;
using Aprendizado.Class;



namespace Aprendizado.Controllers
{
    public class GeraProvaController : Controller
    {
        private AtividadeModel atividadeModel = new AtividadeModel();
        private DisciplinaModel disciplinaModel = new DisciplinaModel();
        private TemaModel temaModel = new TemaModel();
        private PerguntaModel perguntaModel = new PerguntaModel();
        private PerguntaAtividadeModel perguntaAtividadeModel = new PerguntaAtividadeModel();
        private CursoModel cursoModel = new CursoModel();
        private TurmaModel turmaModel = new TurmaModel();
        private AlunoModel alunoModel = new AlunoModel();

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult EscolherAlunosTurma(int id)
        {
            ViewBag.Titulo = "Nova Avaliação";

            int idDisciplina = 1;
            int idTurma = 1;

            ViewBag.idDisciplina
                = new SelectList(disciplinaModel.todasDisciplinas(),
                    "idDisciplina", "Descricao", idDisciplina);

            ViewBag.idTurma
                = new SelectList(turmaModel.todasTurmas(),
                    "idTurma", "Identificacao", idTurma);

            List<Aluno> listaAlunos =
                alunoModel.listarAlunosPorTurma(idDisciplina);
            

            return View(listaAlunos);
        }



    }
}
