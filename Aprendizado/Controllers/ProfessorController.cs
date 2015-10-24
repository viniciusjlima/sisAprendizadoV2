using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aprendizado.Entity;
using Aprendizado.Models;
using Aprendizado.ViewModels;

namespace Aprendizado.Controllers
{
    public class ProfessorController : Controller
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
        private ProfessorModel professorModel = new ProfessorModel();

        public ActionResult Index()
        {
            return View(professorModel.todosProfessores());
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
            Pessoa p;
            if (id == 0)
            {
                p = new Pessoa();
            }
            else
            {
                p = pessoaModel.obterPessoa(id);
            }
            return View(p);
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
                return RedirectToAction("EditProfessor", new { idProfessor = 0, p.idPessoa });
            }
            else
            {
                ViewBag.Error = erro;
                return View(p);
            }
        }
        
        public ActionResult DeletePessoa(int id)
        {
            Pessoa p = pessoaModel.obterPessoa(id);
            pessoaModel.excluirPessoa(p);
            return RedirectToAction("IndexProfessor");



        }

        public PartialViewResult List(string q)
        {
            var tiposPessoa = pessoaModel.listarPessoas(q);
            return PartialView(tiposPessoa);
        }

////////////////////////////// PROFESSOR /////////////////////////////////////////////////////////////////////////////////

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
                return RedirectToAction("EditUsuario", new { idUsuario = 0, pessoa.idPessoa });
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

        /////////////////////////////////// USUARIOS //////////////////////////////////////////////////////////////////////

        public ActionResult ListaUsuarios(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(usuarioModel.obterUsuariosPessoas(idPessoa));
        }

        public ActionResult EditUsuario(int idUsuario, int idPessoa)
        {
            Usuario u = new Usuario();
            u.idPessoa = idPessoa;
            u.idUsuario = 2;
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
        public ActionResult EditUsuario(Usuario u, Perfil p, Pessoa pa)
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

/////////////////////////////// ENDEREÇO ////////////////////////////////////////////////////////////////////////////////

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

/////////////////////////////////////////////////// TELEFONE ///////////////////////////////////////////////////////////

        public ActionResult ListaTelefones(int idPessoa)
        {
            ViewBag.idPessoa = idPessoa;
            Pessoa p = pessoaModel.obterPessoa(idPessoa);
            ViewBag.NomePessoa = p.Nome;
            return View(telefoneModel.obterTelefonesPessoas(idPessoa));
        }

        public ActionResult EditTelefone(int idTelefone, int idPessoa)
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
        public ActionResult EditTelefone(Telefone t, TipoTelefone tt, Pessoa p, Perfil perfil)
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

    }

}
