using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Models;
using Aprendizado.Entity;
using Aprendizado.Class;

namespace Aprendizado.Controllers
{
    public class AtividadeController : Controller
    {
        private AtividadeModel atividadeModel = new AtividadeModel();
        private DisciplinaModel disciplinaModel = new DisciplinaModel();
        private TemaModel temaModel = new TemaModel();
        private PerguntaModel perguntaModel = new PerguntaModel();
        private PerguntaAtividadeModel perguntaAtividadeModel = new PerguntaAtividadeModel();
        private CursoModel cursoModel = new CursoModel();
        private TurmaModel turmaModel = new TurmaModel();

        private Atividade infoAtividade = new Atividade();
        private AlunoAtividadeModel alunoAtividadeModel = new AlunoAtividadeModel();
        private AlunoModel alunoModel = new AlunoModel();
        private List<Aluno> listaAlunos;

        private List<int> NumerosSorteados;
        private List<Pergunta> listaFinalPerguntas;
        private int contadorPergunta = 0;
        private int nPerguntas = 0;
        DateTime date = DateTime.Now.Date;
        public cabecalhoAvaliacao cabecalho;

        public ActionResult Index()
        {
            var atividades = new List<Atividade>();
             atividades = atividadeModel.listarAtividadesEAvaliacoes();
            DateTime data = DateTime.Now;
            

            for (int i = 0; i < atividades.Count; i++)
            {
                Atividade at = atividadeModel.obterAtividade(atividades[i].idAtividade);
                int result = DateTime.Compare(data, at.DataEncerramento);
                string erro = null;

                if (result > 0)
                {
                    at.idStatus = 2;
                    erro = atividadeModel.editarAtividade(at);
                }
            }


            return View(atividades);
        }

        public ActionResult Atividades()
        {
            return View(atividadeModel.todasAtividades());
        }

        public ActionResult AvaliacoesPorAluno()
        {
            return View(atividadeModel.listarAtividadesEAvaliacoes());
        }

        public ActionResult Edit(int id)
        {
            Atividade a = new Atividade();
            ViewBag.Titulo = "Nova Atividade";

            int idDisciplina = 1;
            int idTurma = 1;

            if (id != 0)
            {
                a = atividadeModel.obterAtividade(id);
                idDisciplina = a.idDisciplina;
                idTurma = a.idTurma;
                ViewBag.Titulo = "Editar Atividade";
            }

            ViewBag.idDisciplina    
                = new SelectList(disciplinaModel.todasDisciplinas(),
                    "idDisciplina", "Descricao", idDisciplina);

            ViewBag.idTurma
                = new SelectList(turmaModel.todasTurmas(),
                    "idTurma", "Identificacao", idTurma);

            return View(a);
        }

        [HttpPost]
        public ActionResult Edit(Atividade a, Disciplina d, Turma t)
        {
            ViewBag.idDisciplina
                = new SelectList(disciplinaModel.todasDisciplinas(),
                    "idDisciplina", "Descricao", d);

            ViewBag.idTurma
                = new SelectList(turmaModel.todasTurmas(),
                    "idTurma", "Identificacao", t);

            a.idStatus = 1; //Definindo Atividade como aberta
            a.idTipo = 1; //TIPO ATIVIDADE

            if (!validarAtividade(a))
            {
                ViewBag.Erro = "Erro na validação da Atividade";
                return View(a);
            }

            string erro = null;
            if (a.idAtividade == 0)
            {
                erro = atividadeModel.adicionarAtividade(a);
            }
            else
            {
                erro = atividadeModel.editarAtividade(a);
            }
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(a);
            }
        }

        private bool validarAtividade(Atividade a)
        {
            if (a.Descricao == "")
                return false;
            if (a.idDisciplina == 0)
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            Atividade a = atividadeModel.obterAtividade(id);
            atividadeModel.excluirAtividade(a);
            return RedirectToAction("Index");
        }

///////////////////// PERGUNTAS DA ATIVIDADE ////////////////////////////////////////////////////////

        public ActionResult ListaPerguntaAtividade(int idAtividade)
        {
            List<Pergunta_Atividade> PerguntaAtividadeAtividades = 
                perguntaAtividadeModel.listarPerguntaAtividadePorAtividade(idAtividade);

            Atividade a = atividadeModel.obterAtividade(idAtividade);

            ViewBag.idAtividade = a.idAtividade;
            ViewBag.IdentificacaoAtividade = a.Identificacao;
            return View(PerguntaAtividadeAtividades);
        }


            

        public ActionResult EditPerguntaAtividade(int idAtividade, int idPerguntaAtividade)
        {
            Pergunta_Atividade pa = new Pergunta_Atividade();
            pa.idAtividade = idAtividade;

            Atividade a = atividadeModel.obterAtividade(idAtividade);

            int idPergunta = 1;

            if (idPerguntaAtividade != 0)
            {
                pa = perguntaAtividadeModel.obterPerguntaAtividade(idPerguntaAtividade);
                idPergunta = pa.idPergunta;
            }


            ViewBag.idPergunta
                = new SelectList(perguntaModel.todasPerguntas(),
                    "idPergunta", "Identificacao", idPergunta);

            return View(pa);
        }
        [HttpPost]
        public ActionResult EditPerguntaAtividade(Pergunta_Atividade pa, Pergunta p)
        {
            ViewBag.idPergunta
                = new SelectList(perguntaModel.todasPerguntas(),
                    "idPergunta", "Identificacao", p);

            string erro = null;
            if (pa.idPerguntaAtividade == 0)
            {
                erro = perguntaAtividadeModel.adicionarPerguntaAtividade(pa);
            }
            else
            {
                erro = perguntaAtividadeModel.editarPerguntaAtividade(pa);
            }
            if (erro == null)
            {
                //if(pa.idPerguntaAtividade !=0)
                //    return RedirectToAction("Index");

                return RedirectToAction("ListaPerguntaAtividade", new { idAtividade = pa.idAtividade });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(pa);
            }
        }
        public ActionResult DeletePerguntaAtividade(int idPerguntaAtividade)
        {
            Pergunta_Atividade pa = perguntaAtividadeModel.obterPerguntaAtividade(idPerguntaAtividade);
            perguntaAtividadeModel.excluirPerguntaAtividade(pa);
            return RedirectToAction("ListaPerguntaAtividade", new { idAtividade = pa.idAtividade });
        }

        public JsonResult ListaCursos(int idCurso)
        {
            var disciplinas
                = new SelectList(disciplinaModel.obterDisciplinaPorCurso(idCurso), "idDisciplina", "Descricao");
            return Json(new { disciplinas = disciplinas });
        }

        public JsonResult ListaTemas(int idDisciplina)
        {
            var temas
                = new SelectList(temaModel.obterTemasPorDisciplina(idDisciplina), "idTema", "Descricao");
            return Json(new { temas = temas });
        }

        public JsonResult ListaTurmas(int curso)
        {
            var turmas
                = new SelectList(turmaModel.obterTurmasPorCurso(curso), "idTurma", "Identificacao");
            return Json(new { turmas = turmas });
        }

        public JsonResult ListaDisciplinas(int curso)
        {
            var disciplinas
                = new SelectList(disciplinaModel.obterDisciplinaPorCurso(curso), "idDisciplina", "Descricao");
            return Json(new { disciplinas = disciplinas });
        }

        public JsonResult ListaPerguntas(int idTema)
        {
            var perguntas
                = new SelectList(perguntaModel.obterPerguntasPorTema(idTema), "idPerguna", "Identificacao");
            return Json(new { perguntas = perguntas });
        }


//////////////// GERAR PROVA AUTOMATICA ///////////////////////////////////////////////////
        private List<Aluno> EditProvaAutomatica(int id)
        {
            Atividade a = new Atividade();
            ViewBag.Titulo = "Nova Atividade";

            short idDisciplina = 1;
            int idTurma = 1;

            

            ViewBag.idDisciplina
                = new SelectList(disciplinaModel.todasDisciplinas(),
                    "idDisciplina", "Descricao", idDisciplina);

            ViewBag.idTurma
                = new SelectList(turmaModel.todasTurmas(),
                    "idTurma", "Identificacao", idTurma);

            listaAlunos = alunoModel.listarAlunosPorTurma(idDisciplina);
            
            return(listaAlunos);
        }


        public ActionResult errosPorTema(int idAluno)
        {
            Aluno a = alunoModel.obterAluno(idAluno);
            List<Tema> temas = alunoAtividadeModel.listarTemasPorAluno(idAluno);
            var listaErradasTema = new List<ErradaTema>();

            for (int i = 0; i < temas.Count; i++)
            {
                int idTema = temas[i].idTema;
                listaErradasTema.Add(new ErradaTema()
                {
                    Tema = temas[i].Descricao,
                    QtdErradas = alunoAtividadeModel.listarPerguntasErradasPorTema2(a.idAluno, idTema)
                });
            }

            listaErradasTema = listaErradasTema.OrderByDescending(c => c.QtdErradas).ToList();

            ViewBag.listaErradasTema = listaErradasTema;

            return View(listaErradasTema);
        }

        private List<ErradaTema> errosPorTemaDisciplina(int idAluno, int idDisciplina)
        {
            Aluno a = alunoModel.obterAluno(idAluno);
            Disciplina d = disciplinaModel.obterDisciplina(idDisciplina);

            List<Tema> temas = alunoAtividadeModel.listarTemasPorAlunoDisciplina(idAluno, idDisciplina);

            var listaErradasTema = new List<ErradaTema>();

            for (int i = 0; i < temas.Count; i++)
            {
                int idTema = temas[i].idTema;
                listaErradasTema.Add(new ErradaTema()
                {
                    Tema = temas[i].Descricao,
                    QtdErradas = alunoAtividadeModel.listarPerguntasErradasPorTema2(a.idAluno, idTema)
                });
            }

            if (listaErradasTema.Count < 2)
            {
                ViewBag.listaErradasTema = temas;
            }

            listaErradasTema = listaErradasTema.OrderByDescending(c => c.QtdErradas).ToList();

            ViewBag.listaErradasTema = listaErradasTema;

            return listaErradasTema;
        }

        private List<Pergunta> prepararSorteio(int idAlunoA, int idDisciplinaA)
        {
            int idAluno = idAlunoA;
            int idDisciplina = idDisciplinaA;

            errosPorTemaDisciplina(idAluno, idDisciplina);
            NumerosSorteados = new List<int>();

            int idTema1 = temaModel.obterTemaPorDescricao(ViewBag.listaErradasTema[0].Tema).idTema;
            int idTema2 = temaModel.obterTemaPorDescricao(ViewBag.listaErradasTema[1].Tema).idTema;
            int idTema3 = temaModel.obterTemaPorDescricao(ViewBag.listaErradasTema[2].Tema).idTema;

            //==================  1º TEMA QUE O ALUNO MAIS ERROU ====================================================

            //----------------------- DIFICEIS --------------------------------------------------
            List<Pergunta> PerguntasDificeisASortearT1 =
                perguntaModel.listarPerguntasParaSorteio(idTema1, 3); // 3 = Nivel de Dificuldade DIFICIL

            contadorPergunta = PerguntasDificeisASortearT1.Count;
            nPerguntas = 3;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasDIficeisT1 = new List<Pergunta>();

            for (int i = 0; i < 3; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasDIficeisT1.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //------ MEDIAS ------------------------------------------------------------------------------------
            List<Pergunta> PerguntasMediasASortearT1 =
                perguntaModel.listarPerguntasParaSorteio(idTema1, 2); // 2 = Nivel de Dificuldade MEDIO

            contadorPergunta = PerguntasMediasASortearT1.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasMediasT1 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasMediasASortearT1[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasMediasT1.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }


            //-------- FACEIS ----------------------------------------------------------------------------------
            List<Pergunta> PerguntasFaceisASortearT1 =
                perguntaModel.listarPerguntasParaSorteio(idTema1, 1); // 1 = Nivel de Dificuldade FACIL

            contadorPergunta = PerguntasFaceisASortearT1.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasFaceisT1 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasFaceisT1.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }


            //==================  2º TEMA QUE O ALUNO MAIS ERROU ====================================================

            //----------------------- DIFICEIS --------------------------------------------------
            List<Pergunta> PerguntasDificeisASortearT2 =
                perguntaModel.listarPerguntasParaSorteio(idTema2, 3); // 3 = Nivel de Dificuldade DIFICIL

            contadorPergunta = PerguntasDificeisASortearT2.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasDificeisT2 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT2[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasDificeisT2.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }


            //----------------------- MEDIAS --------------------------------------------------
            List<Pergunta> PerguntasMediasASortearT2 =
                perguntaModel.listarPerguntasParaSorteio(idTema2, 2); // 2 = Nivel de Dificuldade MEDIO

            contadorPergunta = PerguntasMediasASortearT2.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasMediasT2 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasMediasASortearT2[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasMediasT2.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }


            //----------------------- FACEIS --------------------------------------------------
            List<Pergunta> PerguntasFaceisASortearT2 =
                perguntaModel.listarPerguntasParaSorteio(idTema2, 1); // 1 = Nivel de Dificuldade FACIL

            contadorPergunta = PerguntasFaceisASortearT2.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasFaceisT2 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT2[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasFaceisT2.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }


            //==================  3º TEMA QUE O ALUNO MAIS ERROU ====================================================

            //----------------------- MEDIAS --------------------------------------------------
            List<Pergunta> PerguntasMediasASortearT3 =
                perguntaModel.listarPerguntasParaSorteio(idTema3, 2); // 2 = Nivel de Dificuldade MEDIO

            contadorPergunta = PerguntasMediasASortearT3.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasMediasT3 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasMediasASortearT3[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasMediasT3.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //----------------------- FACEIS --------------------------------------------------
            List<Pergunta> PerguntasFaceisASortearT3 =
                perguntaModel.listarPerguntasParaSorteio(idTema3, 1); // 1 = Nivel de Dificuldade FACIL

            contadorPergunta = PerguntasFaceisASortearT3.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasFaceisT3 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasFaceisASortearT3[NumerosSorteados[i]].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaPerguntasMediasT2.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //------------------------ MONTA LISTA COM PERGUNTAS PARA A ATIVIDADE--------------------------

            NumerosSorteados = new List<int>();
            listaFinalPerguntas = new List<Pergunta>();

            // Adiciona perguntas do tema 1
            //DIFICEIS
            for (int i = 0; i < listaPerguntasDIficeisT1.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasDIficeisT1[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //MEDIAS
            for (int i = 0; i < listaPerguntasMediasT1.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasMediasT1[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //FACEIS
            for (int i = 0; i < listaPerguntasFaceisT1.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasFaceisT1[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            // Adiciona perguntas do Tema 2
            //DIFICEIS
            for (int i = 0; i < listaPerguntasDificeisT2.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasDificeisT2[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //MEDIAS
            for (int i = 0; i < listaPerguntasMediasT2.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasMediasT2[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //FACEIS
            for (int i = 0; i < listaPerguntasFaceisT2.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasFaceisT2[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            // Adiciona perguntas do Tema 3
            //MEDIAS
            for (int i = 0; i < listaPerguntasMediasT3.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasMediasT3[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            //FACEIS
            for (int i = 0; i < listaPerguntasFaceisT3.Count; i++)
            {
                int idPerguntaSorteada = listaPerguntasFaceisT3[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPerguntaSorteada);

                listaFinalPerguntas.Add(new Pergunta()
                {
                    idPergunta = pergunta.idPergunta,
                    idTema = pergunta.idTema,
                    idNivelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }


            return(listaFinalPerguntas);
        }

        private List<int> Sortear(int contadorPergunta, int nPergunta)
        {
            NumerosSorteados.Clear();
            Random random = new Random();
            int nPerguntaSorteada = 0;

            for (int i = 0; i < nPergunta; i++)
            {
                do
                {
                    nPerguntaSorteada = random.Next(0, contadorPergunta);
                } while (NumerosSorteados.Contains(nPerguntaSorteada));

                NumerosSorteados.Add(nPerguntaSorteada);
            }


            return NumerosSorteados;
        }


        public ActionResult EditCabecalho()
        {
            cabecalho = new cabecalhoAvaliacao();

            int idDisciplina=0;
            int idTurma=0;
            int idCurso = 0;

            ViewBag.idCurso
                = new SelectList(cursoModel.todosCursos(),
                    "idCurso", "Descricao", idCurso);

            ViewBag.idTurma
                = new SelectList(turmaModel.obterTurmasPorCurso(idCurso),
                    "idTurma", "Identificacao", idTurma);

            ViewBag.idDisciplina
                = new SelectList(disciplinaModel.obterDisciplinaPorCurso(idCurso),
                    "idDisciplina", "Descricao", idDisciplina);

            return View(cabecalho);
        }

        [HttpPost]
        public ActionResult EditCabecalho(cabecalhoAvaliacao c, Disciplina d, Turma t)
        {
            int idTurma2 = c.IdTurma;
            int idTurma3 = t.idTurma;
            int idDisciplina = c.IdDisciplina;

            if (perguntaModel.listarPerguntasPorDisciplinaPorDificuldade(idDisciplina, 3).Count < 10)
            {
                ViewBag.Insuficientes = "É necessário ao menos 10 perguntas dificeis desta disciplina para gerar uma avaliação.";
                return View("PerguntasInsuficientes");
            }
            else
            {
                if (perguntaModel.listarPerguntasPorDisciplinaPorDificuldade(idDisciplina, 2).Count < 10)
                {
                    ViewBag.Insuficientes = "É necessário ao menos 10 perguntas médias desta disciplina para gerar uma avaliação.";
                    return View("PerguntasInsuficientes");
                }
                else
                {
                    if (perguntaModel.listarPerguntasPorDisciplinaPorDificuldade(idDisciplina, 1).Count < 10)
                    {
                        ViewBag.Insuficientes = "É necessário ao menos 10 perguntas fáceis desta disciplina para gerar uma avaliação.";
                        return View("PerguntasInsuficientes");
                    }
                    else
                    {
                        return RedirectToAction("GerarAvaliacaoAutomatica", c);
                    }
                }
            }

        }

        public ActionResult GerarAvaliacaoAutomatica(cabecalhoAvaliacao c)
        {
            
            int id = 0;
            Atividade atA = new Atividade();
            ViewBag.Titulo = "Nova Avaliacao Automatica";

            int idDisciplina = c.IdDisciplina;
            int idTurma = c.IdTurma;
            

            if (id != 0)
            {
                atA = atividadeModel.obterAtividade(id);
                idDisciplina = atA.idDisciplina;
                idTurma = atA.idTurma;
                ViewBag.Titulo = "Editar Atividade";
            }


            listaAlunos = alunoModel.listarAlunosPorTurma(idTurma);
         
            for (int i = 0; i < listaAlunos.Count; i++)
            {
                
                int idAlunoA = listaAlunos[i].idAluno;
                Aluno a = alunoModel.obterAluno(idAlunoA);
                
                Atividade at = new Atividade();
                Atividade at2 = atividadeModel.obterAtividade(4);

                at.idDisciplina = c.IdDisciplina;
                at.idTurma = c.IdTurma;
                at.Identificacao = c.Identificacao;
                at.DataAbertura = c.DataAbertura;
                at.DataEncerramento = c.DataEncerramento;
                at.Titulo = c.Titulo;
                at.Descricao = c.Descricao;

                at.idStatus = 1; //Definindo Atividade como aberta
                at.idTipo = 2; //Definindo tipo como AVALIAÇÂO

                string erro = null;
                erro = atividadeModel.adicionarAtividade(at);

                ///////////////////// INSERIR PERGUTAS NA ATIVIDADE //////////////////////////

                //Me retorna a lista de perguntas para adicionar na prova
                prepararSorteio(idAlunoA, c.IdDisciplina);
                for (int j = 0; j < listaFinalPerguntas.Count; j++)
                {
                    Pergunta_Atividade pa = new Pergunta_Atividade();

                    pa.idAtividade = at.idAtividade;
                    pa.idPergunta = listaFinalPerguntas[j].idPergunta;

                    erro = perguntaAtividadeModel.adicionarPerguntaAtividade(pa);
                }

                Aluno_Atividade aa = new Aluno_Atividade();
                aa.idAluno = idAlunoA;
                aa.idAtividade = at.idAtividade;
                aa.idStatus = 1;
                erro = alunoAtividadeModel.adicionarAlunoAtividade(aa);
            }

            return RedirectToAction("Index");
        }

    }
}
