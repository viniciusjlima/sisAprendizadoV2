using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Aprendizado.Models
{
    public class PessoaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public List<Pessoa> todasPessoas()
        {
            var lista = from p in db.Pessoa
                        select p;
            return lista.ToList();
        }

        //public List<Pessoa> todosAdministradores()
        //{
        //    var lista = from u in db.Usuario
        //                join p in db.Pessoa
        //                on u.idPerfil = 3 && p.idPessoa = u.id
        //                select p;
        //    return lista.ToList();
        //}

        public string adicionarPessoa(Pessoa p)
        {
            string erro = null;
            try
            {
                db.Pessoa.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Pessoa obterPessoa(int idPessoa)
        {
            var lista = from p in db.Pessoa
                        where p.idPessoa == idPessoa
                        select p;
            return lista.ToList().FirstOrDefault();
        }

        public List<Pessoa> listarPessoas(string pesquisa)
        {
            var lista = from p in db.Pessoa
                        where p.Nome.Contains(pesquisa)
                        select p;
            return lista.ToList();
        }

        public string editarPessoa(Pessoa p)
        {
            string erro = null;
            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.Pessoa.Attach(p);
                }
                db.ObjectStateManager.ChangeObjectState(p, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string excluirPessoa(Pessoa p)
        {
            string erro = null;
            try
            {
                db.Pessoa.DeleteObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string validarPessoa(Pessoa p)
        {
            if (p.Nome == null || p.Nome == "")
            {
                return "O nome não pode ser vazio!";
            }
            if (p.CPF == null || p.CPF.Length > 11)
            {
                return "CPF inválido";
            }
            if (p.Nascimento == null || p.Nascimento > DateTime.Now.Date)
            {
                return "Data de nascimento inválida";
            }
           
            return null;
        }

        


    }
}