using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aprendizado.Entity;

namespace Aprendizado.Models
{
    public class DisciplinaTurmaModel
    {
        private AprendizadoEntities db = new AprendizadoEntities();

        //public List<Disciplina_Turma> todosDisciplinaTurma()
        //{
        //    var lista = from dt in db.Disciplina_Turma
        //                select dt;
        //    return lista.ToList();
        //}

        //public string adicionarDisciplinaTurma(Disciplina_Turma dt)
        //{
        //    string erro = null;
        //    try
        //    {
        //        db.Disciplina_Turma.AddObject(dt);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        erro = ex.Message;
        //    }
        //    return erro;
        //}

        //public string editarDisciplinaTurma(Disciplina_Turma dt)
        //{
        //    string erro = null;
        //    try
        //    {
        //        if (dt.EntityState == System.Data.EntityState.Detached)
        //        {
        //            db.Disciplina_Turma.Attach(dt);
        //        }
        //        db.ObjectStateManager.ChangeObjectState(dt, System.Data.EntityState.Modified);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        erro = ex.Message;
        //    }
        //    return erro;
        //}

        //public Disciplina_Turma obterDisciplinaTurma(int id)
        //{
        //    var lista = from dt in db.Disciplina_Turma
        //                where dt.idDisciplinaTurma == id
        //                select dt;
        //    return lista.ToList().FirstOrDefault();
        //}

        //public string excluirDisciplinaTurma(Disciplina_Turma dt)
        //{
        //    string erro = null;
        //    try
        //    {
        //        db.DeleteObject(dt);
        //        db.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        erro = ex.Message;
        //    }
        //    return erro;
        //}

        //public List<Disciplina_Turma> listarDisciplinaTurmaPorTurma(int idTurma)
        //{
        //    var lista = from dt in db.Disciplina_Turma
        //                where dt.idTurma == idTurma
        //                select dt;
        //    return lista.ToList();
        //}

        //public List<Disciplina_Turma> listarDisciplinaTurmaPorDisciplina(int idDisciplina)
        //{
        //    var lista = from dt in db.Disciplina_Turma
        //                where dt.idDisciplina == idDisciplina
        //                select dt;
        //    return lista.ToList();
        //}
    }
}