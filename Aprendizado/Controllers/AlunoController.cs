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
    [Authorize]
    public class AlunoController : Controller
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
        private AlunoAtividadeModel alunoAtividadeModel = new AlunoAtividadeModel();

        public ActionResult Index()
        {

            return View(alunoModel.todosAlunos());
        }

        [Authorize]
        public ActionResult EditPessoa(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Pessoa p;

                if (id == 0)
                {
                    ViewBag.Titulo = "Novo Aluno";
                    p = new Pessoa();
                }
                else
                {
                    ViewBag.Titulo = "Editar Aluno";
                    p = pessoaModel.obterPessoa(id);
                }
                return View(p);
            }
            return Redirect("/Shared/Login");
        }

        [HttpPost]
        public ActionResult EditPessoa(Pessoa p)
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

        public PartialViewResult List(string q)
        {
            var tiposPessoa = pessoaModel.listarPessoas(q);
            return PartialView(tiposPessoa);
        }

        //////////////////////////// ALUNO ////////////////////////////////////////////////////////////////

        [Authorize]
        public ActionResult EditAluno(int idAluno, int idPessoa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Aluno a = new Aluno();
                a.idPessoa = idPessoa;
                if (idAluno != 0)
                {
                    a = alunoModel.obterAluno(idAluno);
                    ViewBag.Nome = a.Pessoa.Nome;
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
            return Redirect("/Shared/Login");
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
                return RedirectToAction("EditUsuario", new { idUsuario = 0, p.idPessoa });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(a);
            }
        }

        /////////////////////// USUARIO ///////////////////////////////////////////////////////////

        public ActionResult ListaUsuarios(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(usuarioModel.obterUsuariosPessoas(idPessoa));
        }

        [Authorize]
        public ActionResult EditUsuario(int idUsuario, int idPessoa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
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
            return Redirect("/Shared/Login");
        }

        [HttpPost]
        public ActionResult EditUsuario(Usuario u, Perfil p, Pessoa pa)
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

        ///////////////  ENDERECO  ////////////////////////////////////////////////////////////

        public ActionResult ListaEnderecos(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(enderecoModel.obterEnderecosPessoas(idPessoa));
        }

        [Authorize]
        public ActionResult EditEndereco(int idEndereco, int idPessoa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
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
            return Redirect("/Shared/Login");
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

        //////////////////////// TELEFONONE  ///////////////////////////////////////////////////////////////

        public ActionResult ListaTelefones(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(telefoneModel.obterTelefonesPessoas(idPessoa));
        }

        [Authorize]
        public ActionResult EditTelefone(int idTelefone, int idPessoa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
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
            return Redirect("/Shared/Login");
        }

        [HttpPost]
        public ActionResult EditTelefone(Telefone t, TipoTelefone tt, Pessoa p)
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

        [Authorize]
        public ActionResult EditTelefoneProfessor(int idTelefone, int idPessoa)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
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
            return Redirect("/Shared/Login");
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
                return RedirectToAction("IndexAluno");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(t);
            }
        }

        [Authorize]
        public ActionResult DeleteTelefone(int idTelefone)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                Telefone t = telefoneModel.obterTelefone(idTelefone);
                telefoneModel.excluirTelefone(t);
                return RedirectToAction("ListaTelefones", new { idPessoa = t.idPessoa });
            }
            return Redirect("/Shared/Login");
        }

    }

}
