using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;
using Aprendizado.ViewModels;

namespace Aprendizado.Controllers
{
    public class PessoaController : Controller
    {
        private PessoaModel pessoaModel = new PessoaModel();
        private EnderecoModel enderecoModel = new EnderecoModel();
        private TipoEnderecoModel tipoEnderecoModel = new TipoEnderecoModel();
        private EstadoModel estadoModel = new EstadoModel();
        private CidadeModel cidadeModel = new CidadeModel();
        private TelefoneModel telefoneModel = new TelefoneModel();
        private TipoTelefoneModel tipoTelefoneModel = new TipoTelefoneModel();
        private UsuarioModel usuarioModel = new UsuarioModel();
        private PerfilModel perfilModel = new PerfilModel();
        private AlunoModel alunoModel = new AlunoModel();
        private TurmaModel turmaModel = new TurmaModel();
        private ProfessorModel professorModel = new ProfessorModel();

        public ActionResult Index()
        {
            return View(pessoaModel.todasPessoas());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Pessoa p)
        {
            pessoaModel.adicionarPessoa(p);
            return RedirectToAction("Index");
        }

        public ActionResult EditPessoa(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Admimistrador"))
            {
                Pessoa p;

                if (id == 0)
                {
                    ViewBag.Titulo = "Nova Pessoa";
                    p = new Pessoa();
                }
                else
                {
                    ViewBag.Titulo = "Editar Aluno";
                    p = pessoaModel.obterPessoa(id);
                }
                return View(p);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult EditPessoa(Pessoa p)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Admimistrador"))
            {
                string erro = pessoaModel.validarPessoa(p);
                if (erro == null)
                {
                    if (p.idPessoa == 0)
                    {
                        erro = pessoaModel.adicionarPessoa(p);
                    }
                    else
                    {
                        erro = pessoaModel.editarPessoa(p);
                    }
                }

                if (erro == null)
                {
                    if (p.idPessoa == 0)
                    {
                        erro = "p.idPEssoa vazio";
                    }
                    return RedirectToAction("EditAluno", new { idAluno = 0, p.idPessoa });
                }
                else
                {
                    ViewBag.Error = erro;
                    return View(p);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        public ActionResult DeletePessoa(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Admimistrador"))
            {

                Pessoa p = pessoaModel.obterPessoa(id);
                pessoaModel.excluirPessoa(p);
                return RedirectToAction("IndexAluno");
            }
            return Redirect("/Shared/Restrito");

        }

        public PartialViewResult List(string q)
        {
            var tiposPessoa = pessoaModel.listarPessoas(q);
            return PartialView(tiposPessoa);
        }

        //////////// ENDEREÇO ////////////////////////////////////////////////////////////////////////////////

        public ActionResult ListaEnderecos(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(enderecoModel.obterEnderecosPessoas(idPessoa));
        }

        public ActionResult EditEndereco(int idEndereco, int idPessoa)
        {
            Endereco e = new Endereco();
            e.idPessoa = idPessoa;
            if (idEndereco != 0)
            {
                e = enderecoModel.obterEndereco(idEndereco);
            }

            string estadoSelecionado = "MG";
            int cidadeSelecionada = 1; // 1 = Patos de Minas
            int tipoEnderecoSelecionado = 1;

            if (idEndereco != 0)
            {
                estadoSelecionado = e.Cidade.UF;
                cidadeSelecionada = e.idCidade;
                tipoEnderecoSelecionado = e.idTipoEndereco;
            }

            ViewBag.UF
                = new SelectList(estadoModel.todosEstados(), "UF", "Descricao",
                    estadoSelecionado);
            ViewBag.IdCidade
                = new SelectList(cidadeModel.obterCidadesPorEstado(estadoSelecionado),
                    "idCidade", "Descricao", cidadeSelecionada);
            ViewBag.idTipoEndereco
                = new SelectList(tipoEnderecoModel.todosTiposEnderecos(),
                    "idTipoEndereco", "Descricao", tipoEnderecoSelecionado);


            return View(e);
        }

        [HttpPost]
        public ActionResult EditEndereco(Endereco e, Estado estado, Cidade cidade, TipoEndereco tipo, Pessoa p)
        {
            ViewBag.UF
                = new SelectList(estadoModel.todosEstados(), "UF", "Descricao",
                    estado);
            ViewBag.idCidade
                = new SelectList(cidadeModel.obterCidadesPorEstado(estado.UF),
                    "idCidade", "Descricao", cidade);
            ViewBag.idTipoEndereco
                = new SelectList(tipoEnderecoModel.todosTiposEnderecos(),
                    "idTipoEndereco", "Descricao", tipo);

            string erro = null;
            if (e.idEndereco == 0)
            {
                erro = enderecoModel.adicionarEndereco(e);
            }
            else
            {
                erro = enderecoModel.editarEndereco(e);
            }
            if (erro == null)
            {
                if (p.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }

                return RedirectToAction("EditTelefone", new { idTelefone = 0, p.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(e);
            }
        }

        public ActionResult DeleteEndereco(int idEndereco)
        {
            Endereco e = enderecoModel.obterEndereco(idEndereco);
            enderecoModel.excluirEndereco(e);
            return RedirectToAction("ListaEnderecos", new { idPessoa = e.idPessoa });
        }

        public JsonResult ListaCidades(string estado)
        {
            var cidades
                = new SelectList(cidadeModel.obterCidadesPorEstado(estado), "idCidade", "Descricao");
            return Json(new { cidades = cidades });
        }

        //////////////////////////////// TELEFONE ///////////////////////////////////////////////////////////

        public ActionResult ListaTelefones(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(telefoneModel.obterTelefonesPessoas(idPessoa));
        }

        public ActionResult EditTelefoneAluno(int idTelefone, int idPessoa)
        {
            Telefone t = new Telefone();
            t.idPessoa = idPessoa;
            if (idTelefone != 0)
            {
                t = telefoneModel.obterTelefone(idTelefone);
            }

            int tipoTelefone = 1;

            if (idTelefone != 0)
            {
                tipoTelefone = t.idTipoTelefone;
            }

            ViewBag.idTipoTelefone
                = new SelectList(tipoTelefoneModel.todosTiposTelefones(),
                    "idTipoTelefone", "Descricao", tipoTelefone);

            return View(t);
        }

        [HttpPost]
        public ActionResult EditTelefoneAluno(Telefone t, TipoTelefone tt, Pessoa p)
        {
            ViewBag.idTipoTelefone
                = new SelectList(tipoTelefoneModel.todosTiposTelefones(),
                    "idTipoTelefone", "Descricao", tt);

            string erro = null;
            if (t.idTelefone == 0)
            {
                erro = telefoneModel.adicionarTelefone(t);
            }
            else
            {
                erro = telefoneModel.editarTelefone(t);
            }
            if (erro == null)
            {
                if (p.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("IndexAluno");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(t);
            }
        }

        public ActionResult EditTelefoneProfessor(int idTelefone, int idPessoa)
        {
            Telefone t = new Telefone();
            t.idPessoa = idPessoa;
            if (idTelefone != 0)
            {
                t = telefoneModel.obterTelefone(idTelefone);
            }

            int tipoTelefone = 1;

            if (idTelefone != 0)
            {
                tipoTelefone = t.idTipoTelefone;
            }

            ViewBag.idTipoTelefone
                = new SelectList(tipoTelefoneModel.todosTiposTelefones(),
                    "idTipoTelefone", "Descricao", tipoTelefone);

            return View(t);
        }

        [HttpPost]
        public ActionResult EditTelefoneProfessor(Telefone t, TipoTelefone tt, Pessoa p)
        {
            ViewBag.idTipoTelefone
                = new SelectList(tipoTelefoneModel.todosTiposTelefones(),
                    "idTipoTelefone", "Descricao", tt);

            string erro = null;
            if (t.idTelefone == 0)
            {
                erro = telefoneModel.adicionarTelefone(t);
            }
            else
            {
                erro = telefoneModel.editarTelefone(t);
            }
            if (erro == null)
            {
                if (p.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("IndexProfessor");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(t);
            }
        }


        public ActionResult DeleteTelefone(int idTelefone)
        {
            Telefone t = telefoneModel.obterTelefone(idTelefone);
            telefoneModel.excluirTelefone(t);
            return RedirectToAction("ListaTelefones", new { idPessoa = t.idPessoa });
        }

        //////////////// USUARIOS //////////////////////////////////////////////////////////////////////

        public ActionResult ListaUsuarios(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(usuarioModel.obterUsuariosPessoas(idPessoa));
        }

        public ActionResult EditUsuarioAluno(int idUsuario, int idPessoa)
        {
            Usuario u = new Usuario();
            u.idPessoa = idPessoa;
            u.idUsuario = 1;
            if (idUsuario != 0)
            {
                u = usuarioModel.obterUsuario(idUsuario);
            }

            int perfilSelecionado = 1;

            if (idUsuario != 0)
            {
                perfilSelecionado = u.idPerfil;
            }

            return View(u);
        }

        [HttpPost]
        public ActionResult EditUsuarioAluno(Usuario u, Perfil p, Pessoa pa)
        {
            u.idPerfil = 1;

            string erro = null;
            if (u.idUsuario == 0)
            {
                erro = usuarioModel.adicionarUsuario(u);
            }
            else
            {
                erro = usuarioModel.editarUsuario(u);
            }
            if (erro == null)
            {
                if (pa.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("EditEndereco", new { idEndereco = 0, pa.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(u);
            }
        }

        // USUARIO PROFESSOR
        public ActionResult EditUsuarioProfessor(int idUsuario, int idPessoa)
        {
            Usuario u = new Usuario();
            u.idPessoa = idPessoa;
            u.idPerfil = 2;

            if (idUsuario != 0)
            {
                u = usuarioModel.obterUsuario(idUsuario);
            }

            int perfilSelecionado = 2;

            if (idUsuario != 0)
            {
                perfilSelecionado = u.idPerfil;
            }

            return View(u);
        }

        [HttpPost]
        public ActionResult EditUsuarioProfessor(Usuario u, Perfil p, Pessoa pa)
        {
            u.idPerfil = 2;

            string erro = null;
            if (u.idUsuario == 0)
            {
                erro = usuarioModel.adicionarUsuario(u);
            }
            else
            {
                erro = usuarioModel.editarUsuario(u);
            }
            if (erro == null)
            {
                if (pa.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("EditEndereco", new { idEndereco = 0, pa.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(u);
            }
        }

        // USUARIO ADMINISTRADOR
        public ActionResult EditUsuarioAdministrador(int idUsuario, int idPessoa)
        {
            Usuario u = new Usuario();
            u.idPessoa = idPessoa;
            u.idPerfil = 3;

            if (idUsuario != 0)
            {
                u = usuarioModel.obterUsuario(idUsuario);
            }

            int perfilSelecionado = 3;

            if (idUsuario != 0)
            {
                perfilSelecionado = u.idPerfil;
            }

            return View(u);
        }

        [HttpPost]
        public ActionResult EditUsuarioAdministrador(Usuario u, Perfil p, Pessoa pa)
        {

            string erro = null;
            if (u.idUsuario == 0)
            {
                erro = usuarioModel.adicionarUsuario(u);
            }
            else
            {
                erro = usuarioModel.editarUsuario(u);
            }
            if (erro == null)
            {
                if (pa.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("EditEndereco", new { idEndereco = 0, pa.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(u);
            }
        }
        // FIM USUARIO ADMINISTRADOR

        public ActionResult DeleteUsuario(int idUsuario)
        {
            Usuario u = usuarioModel.obterUsuario(idUsuario);
            usuarioModel.excluirUsuario(u);
            return RedirectToAction("ListaUsuarios", new { idPessoa = u.idPessoa });
        }

        ///////////////// ALUNO  //////////////////////////////////////////////////////////////////////////////////

        public ActionResult IndexAluno()
        {
            //return View(pessoaModel.todasPessoas());
            AlunoViewModel vma = new AlunoViewModel();
            vma.tiposAluno = alunoModel.todosAlunos();
            return View(vma);
        }

        public ActionResult EditAluno(int idAluno, int idPessoa)
        {
            Aluno a = new Aluno();
            a.idPessoa = idPessoa;
            if (idAluno != 0)
            {
                a = alunoModel.obterAluno(idAluno);
            }

            int turmaSelecionada = 1;
            int pessoaSelecionada = idPessoa;

            if (idAluno != 0)
            {
                turmaSelecionada = a.Turma.idTurma;
                pessoaSelecionada = a.idPessoa;
            }

            ViewBag.idTurma
                = new SelectList(turmaModel.todasTurmas(), "idTurma", "Identificacao",
                    turmaSelecionada);

            return View(a);
        }

        [HttpPost]
        public ActionResult EditAluno(Aluno a, Pessoa p, Turma t)
        {
            ViewBag.idTurma
                = new SelectList(turmaModel.todasTurmas(), "idTurma", "Identificacao",
                    t);

            string erro = null;
            if (a.idAluno == 0)
            {
                erro = alunoModel.adicionarAluno(a);
            }
            else
            {
                erro = alunoModel.editarAluno(a);
            }
            if (erro == null)
            {
                if (p.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("EditUsuarioAluno", new { idUsuario = 0, p.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(a);
            }
        }

        public ActionResult DeleteAluno(int idAluno)
        {
            Aluno a = alunoModel.obterAluno(idAluno);
            alunoModel.excluirAluno(a);
            return RedirectToAction("IndexAluno", new { idPessoa = a.idPessoa });
        }

        //////////////////////////// PROFESSOR  //////////////////////////////////////////////////////////////////////////////////

        public ActionResult IndexProfessor()
        {
            //return View(pessoaModel.todasPessoas());
            ProfessorViewModel vmp = new ProfessorViewModel();
            vmp.tiposProfessor = professorModel.todosProfessores();
            return View(vmp);
        }

        public ActionResult EditProfessor(int idProfessor, int idPessoa)
        {
            Professor p = new Professor();
            p.idPessoa = idPessoa;
            if (idProfessor != 0)
            {
                p = professorModel.obterProfessor(idProfessor);
            }

            int pessoaSelecionada = idPessoa;

            if (idProfessor != 0)
            {
                pessoaSelecionada = p.idPessoa;
            }

            return View(p);
        }

        [HttpPost]
        public ActionResult EditProfessor(Professor professor, Pessoa pessoa)
        {
            string erro = null;
            if (professor.idProfessor == 0)
            {
                erro = professorModel.adicionarProfessor(professor);
            }
            else
            {
                erro = professorModel.editarProfessor(professor);
            }
            if (erro == null)
            {
                if (pessoa.idPessoa == 0)
                {
                    erro = "p.idPEssoa vazio";
                }
                return RedirectToAction("EditUsuarioProfessor", new { idUsuario = 0, pessoa.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(professor);
            }
        }

        public ActionResult DeleteProfessor(int idProfessor)
        {
            Professor p = professorModel.obterProfessor(idProfessor);
            professorModel.excluirProfessor(p);
            return RedirectToAction("IndexProfessor", new { idPessoa = p.idPessoa });
        }

    }

}
