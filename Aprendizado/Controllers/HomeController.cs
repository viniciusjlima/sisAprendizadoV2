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
    public class HomeController : Controller
    {
        private UsuarioModel usuarioModel = new UsuarioModel();
        private PerfilModel perfilModel = new PerfilModel();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario u)
        {
            Usuario banco = usuarioModel.obterUsuarioPorLogin(u.Login);
            if (banco == null || banco == new Usuario())
            {
                ViewBag.Erro = "Usuario Inexistente!";
                return View(u);
            }
            if (u.Senha != banco.Senha)
            {
                ViewBag.Erro = "Senha Incorreta!";
                return View(u);
            }

            Roles.DeleteCookie();
            // Passar perfis do banco para aplicação
            foreach (Perfil p in perfilModel.todosPerfis())
            {
                if (!Roles.RoleExists(p.Descricao))// Testa se a role nao existe
                {
                    Roles.CreateRole(p.Descricao);// adiciona a role
                }
            }

            //Adicionar perfis do usuarui a classe Role
            foreach (Perfil p in perfilModel.listarPerfisPorUsuario(banco.idUsuario))
            {
                // Testa se o usuario nao está na role associada ao banco
                if (!Roles.IsUserInRole(u.Login, p.Descricao))
                {
                    Roles.AddUserToRole(u.Login, p.Descricao); // adiciona o usuario
                }
            }

            //string login = User.Identity.Name;
            //Usuario usuario = usuarioModel.obterUsuarioPorLogin(login);

            //int idUsuarioAutenticado = usuario.idUsuario;
            //ViewBag.IdUsuarioAutenticado = idUsuarioAutenticado;

            FormsAuthentication.SetAuthCookie(u.Login, true);

            return Redirect("/");
        }

        public ActionResult Logoff()
        {
            Usuario u = usuarioModel.obterUsuarioPorLogin(User.Identity.Name);
            // Remover todos os perfis do usuario
            foreach (Perfil p in perfilModel.listarPerfisPorUsuario(u.idUsuario))
            {
                if (Roles.IsUserInRole(u.Login, p.Descricao))
                {
                    Roles.RemoveUserFromRole(u.Login, p.Descricao); // adiciona o usuario
                }
            }
            FormsAuthentication.SignOut();
            return Redirect("/");

        }
    }
}
