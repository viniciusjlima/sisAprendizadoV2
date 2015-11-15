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
    public class UsuarioController : Controller
    {
        private UsuarioModel usuarioModel = new UsuarioModel();
        private PerfilModel perfilModel = new PerfilModel();

        public ActionResult Index()
        {
            string login = User.Identity.Name;
            Usuario usuario = usuarioModel.obterUsuarioPorLogin(login);

            int idUsuarioAutenticado = usuario.idUsuario;
            ViewBag.IdUsuarioAutenticado = idUsuarioAutenticado;

            return View(usuarioModel.todosUsuarios());
        }

        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Usuario u = new Usuario();
                ViewBag.Titulo = "Novo Usuario";

                int perfilSelecionado = 1;

                if (id != 0)
                {
                    u = usuarioModel.obterUsuario(id);
                    perfilSelecionado = u.idPerfil;
                    ViewBag.Titulo = "Editar Usuario";
                }

                ViewBag.idPerfil
                    = new SelectList(perfilModel.todosPerfis(),
                        "idPerfil", "Descricao", perfilSelecionado);

                return View(u);
            }
            return Redirect("/Shared/Restrito");
        }

        [HttpPost]
        public ActionResult Edit(Usuario u, Perfil p)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                ViewBag.idPerfil
                    = new SelectList(perfilModel.todosPerfis(),
                        "idPerfil", "Descricao", p);

                if (!validarUsuario(u))
                {
                    ViewBag.Erro = "Erro na validação do Usuario";
                    return View(u);
                }

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
                    return RedirectToAction("../endereco/Edit/0");
                }
                else
                {
                    ViewBag.Erro = erro;
                    return View(u);
                }
            }
            return Redirect("/Shared/Restrito");
        }

        private bool validarUsuario(Usuario usuario)
        {
            if (usuario.Login == "")
                return false;
            if (usuario.Senha == "")
                return false;
            if (usuario.idPerfil == 0)
                return false;

            return true;
        }

        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "Administrador"))
            {
                Usuario u = usuarioModel.obterUsuario(id);
                usuarioModel.excluirUsuario(u);
                return RedirectToAction("Index");
            }
            return Redirect("/Shared/Restrito");

        }
    }

}
