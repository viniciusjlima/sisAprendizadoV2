using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class UsuarioModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Usuario> todosUsuarios()
        {
            var lista = from u in db.Usuario
                        select u;
            return lista.ToList();
        }

        public string adicionarUsuario(Usuario u)
        {
            string erro = null;
            try
            {
                db.Usuario.AddObject(u);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarUsuario(Usuario u)
        {
            string erro = null;
            try
            {
                if (u.EntityState == System.Data.EntityState.Detached)
                {
                    db.Usuario.Attach(u);
                }
                db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Usuario obterUsuario(int idUsuario)
        {
            var lista = from u in db.Usuario
                        where u.idUsuario == idUsuario
                        select u;
            return lista.ToList().FirstOrDefault();
        }

        public Usuario obterUsuarioPorLogin(string login)
        {
            var lista = from u in db.Usuario
                        where u.Login == login
                        select u;
            return lista.ToList().FirstOrDefault();
        }

        public Usuario obterUsuarioT(string login)
        {
            var lista = from u in db.Usuario
                        where u.Login == login
                        select u;
            return lista.ToList().FirstOrDefault();
        }

        public List<Usuario> listarUsuarios(string pesquisa, int idPessoa)
        {
            var lista = from u in db.Usuario
                        where u.idPessoa == idPessoa && u.Login.Contains(pesquisa)
                        select u;
            return lista.ToList();
        }

        

        public string excluirUsuario(Usuario u)
        {
            string erro = null;
            try
            {
                db.Usuario.DeleteObject(u);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<Usuario> obterUsuariosPessoas(int idPessoa)
        {
            var lista = from u in db.Usuario
                        where u.idPessoa == idPessoa
                        select u;

            return lista.ToList();
        }
    }
}