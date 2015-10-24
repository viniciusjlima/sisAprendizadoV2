using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class PerfilModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public List<Perfil> todosPerfis()
        {
            var lista = from p in db.Perfil
                        select p;
            return lista.ToList();
        }

        public Perfil obterPerfil(int idPerfil)
        {
            var lista = from p in db.Perfil
                        where p.idPerfil == idPerfil
                        select p;
            return lista.ToList().FirstOrDefault();
        }

        public List<Perfil> listarPerfisPorUsuario(int idUsuario)
        {
            var lista = from p in db.Perfil
                        join pu in db.Usuario
                        on p.idPerfil equals pu.idPerfil
                        where pu.idUsuario == idUsuario
                        select p;
            return lista.ToList();
        }

        public string adicionarPerfil(Perfil p)
        {
            string erro = null;
            try
            {
                db.Perfil.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarPerfil(Perfil p)
        {
            string erro = null;

            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.Perfil.Attach(p);
                }
                db.ObjectStateManager.ChangeObjectState(p,
                    System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirPerfil(Perfil p)
        {
            string erro = null;

            try
            {
                db.DeleteObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
    }
}