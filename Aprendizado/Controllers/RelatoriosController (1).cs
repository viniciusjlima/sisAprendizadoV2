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
    [Authorize]
    public class RelatoriosController : Controller
    {
        private AlunoModel alunoModel = new AlunoModel();
        private AtividadeModel atividadeModel = new AtividadeModel();
        private UsuarioModel usuarioModel = new UsuarioModel();
        private TurmaModel turmaModel = new TurmaModel();
        private AlunoAtividadeModel alunoAtividadeModel = new AlunoAtividadeModel();
        private PerguntaAtividadeModel perguntaAtividadeModel = new PerguntaAtividadeModel();
        private PerguntaModel perguntaModel = new PerguntaModel();
        private TemaModel temaModel = new TemaModel();
        private NivelDificuldadeModel ndModel = new NivelDificuldadeModel();

        private List<int> NumerosSorteados;
        private int contadorPergunta = 0;
        private int nPerguntas = 0;

        private AprendizadoEntities db = new AprendizadoEntities();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Aluno"))
            {
                // Recebe o id do usuario do Aluno Logado
                string loginAluno = User.Identity.Name;
                Usuario usuario = usuarioModel.obterUsuarioPorLogin(loginAluno);
                int idUsuarioAluno = usuario.idUsuario;
                int idPessoaAluno = usuario.idPessoa;
                ViewBag.IdUsuarioAluno = idUsuarioAluno;
                ViewBag.IdPessoaAluno = idUsuarioAluno;

                Aluno a = alunoModel.obterAluno2(idPessoaAluno);
                //ViewBag.NomeAluno = a.Pessoa.Nome;

                ViewBag.idAluno = a.idAluno;
                ViewBag.Nome = a.Pessoa.Nome;

                int qtdErros = alunoAtividadeModel.listarPerguntasErradas(a.idAluno).Count;
                int qtdAcertos = alunoAtividadeModel.listarPerguntasCertas(a.idAluno).Count;

                ViewBag.qtdErros = qtdErros;
                ViewBag.qtdAcertos = qtdAcertos;

                errosPorTema(a.idAluno);

                
                return View();
            }

            return Redirect("/Shared/Error");
        }

        public ActionResult PerguntasQueOAlunoErrou(int idAluno)
        {
            int qtdErros = alunoAtividadeModel.listarPerguntasErradas(idAluno).Count;
            ViewBag.qtdErros = qtdErros;

            return View(alunoAtividadeModel.listarPerguntasErradas(idAluno));
        }

        public ActionResult PerguntasQueOAlunoAcertou(int idAluno)
        {
            int qtdAcertos = alunoAtividadeModel.listarPerguntasCertas(idAluno).Count;
            ViewBag.qtdAcertos = qtdAcertos;

            return View(alunoAtividadeModel.listarPerguntasCertas(idAluno));
        }


        public ActionResult Temas(int idAluno)
        {
            int qtdErros = alunoAtividadeModel.listarPerguntasErradasPorTema(idAluno, 3).Count;
            ViewBag.qtdErros = qtdErros;

            return View(temaModel.listarTemaPorCurso(idAluno));
        }

        public ActionResult PerguntasQueOAlunoErrouPorTema(int idAluno, int idTema)
        {
            int qtdErros = alunoAtividadeModel.listarPerguntasErradasPorTema(idAluno, idTema).Count;
            ViewBag.qtdErros = qtdErros;

            return View(alunoAtividadeModel.listarPerguntasErradasPorTema(idAluno, idTema));
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
            ViewBag.idAluno = idAluno;
            return View(listaErradasTema);
        }

        public JsonResult ListaErradasTema(int idAluno)
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

            return Json(new { listaErradasTema = listaErradasTema });
        }

        public ActionResult prepararSorteio()
        {
            int idAluno = 1;
            errosPorTema(idAluno);
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
                perguntaModel.listarPerguntasParaSorteio(idTema1, 3); // 3 = Nivel de Dificuldade DIFICIL

            contadorPergunta = PerguntasDificeisASortearT2.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasDificeisT2 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
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
                perguntaModel.listarPerguntasParaSorteio(idTema1, 2); // 2 = Nivel de Dificuldade MEDIO

            contadorPergunta = PerguntasMediasASortearT2.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasMediasT2 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
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
                perguntaModel.listarPerguntasParaSorteio(idTema1, 1); // 1 = Nivel de Dificuldade FACIL

            contadorPergunta = PerguntasFaceisASortearT2.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasFaceisT2 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
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
                perguntaModel.listarPerguntasParaSorteio(idTema1, 2); // 2 = Nivel de Dificuldade MEDIO

            contadorPergunta = PerguntasMediasASortearT3.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasMediasT3 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
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
                perguntaModel.listarPerguntasParaSorteio(idTema1, 1); // 1 = Nivel de Dificuldade FACIL

            contadorPergunta = PerguntasFaceisASortearT3.Count;
            nPerguntas = 1;
            Sortear(contadorPergunta, nPerguntas);

            var listaPerguntasFaceisT3 = new List<Pergunta>();

            for (int i = 0; i < 1; i++)
            {
                int idPerguntaSorteada = PerguntasDificeisASortearT1[NumerosSorteados[i]].idPergunta;
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

            var listaFinalPerguntas = new List<Pergunta>();

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

         // Adiciona perguntas do Tema 2
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

            var testeView = new List<perguntasProva>();

            for (int i = 0; i < listaFinalPerguntas.Count; i++)
            {
                int idPergunta = listaFinalPerguntas[i].idPergunta;
                Pergunta pergunta = perguntaModel.obterPergunta(idPergunta);

                testeView.Add(new perguntasProva()
                {
                    IdPergunta = pergunta.idPergunta,
                    IdTema = pergunta.idTema,
                    IdNIvelDificuldade = pergunta.idNivelDificuldade,
                    Titulo = pergunta.Titulo,
                    Enunciado = pergunta.Enunciado,
                    Identificacao = pergunta.Identificacao,
                    Correta = pergunta.Correta
                });
            }

            return View(testeView);
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

    }
}
