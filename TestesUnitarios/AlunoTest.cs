using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aprendizado;
using Aprendizado.Entity;
using Aprendizado.Models;
using Aprendizado.Controllers;



namespace TestesUnitarios
{
    [TestClass]
    public class AlunoTest
    {
        public Aluno aluno1, aluno2;

        public Professor professor1, professor2;


        [TestInitialize]
        public void InicializarTest()
        {
            aluno1 = new Aluno()
            {
                idPessoa = 1,
                idAluno = 1,
                Matricula = 12029050
            };

            professor1 = new Professor()
            {
                idProfessor = 1,
                idPessoa = 2,
                Especializacao = "redes"
            };

        }

        [TestMethod]
        public void Garantir_Que_2_Alunos_Sao_Iguais_Quando_Tem_Mesmo_IdAluno()
        {
            aluno2 = new Aluno()
            {
                idPessoa = 2,
                idAluno = 0,
                Matricula = 111111
            };
            Assert.AreEqual(aluno1.idAluno, aluno2.idAluno);
        }


        [TestMethod]
        public void Garantir_Que_2_Alunos_Sao_Iguais_Quando_Tem_Mesma_Matricula()
        {
            aluno2 = new Aluno()
            {
                idPessoa = 2,
                idAluno = 2,
                Matricula = 12029050
            };
            Assert.AreEqual(aluno1.Matricula, aluno2.Matricula);
        }

        [TestMethod]
        public void Garantir_Que_2_Alunos_Sao_Iguais_Quando_Tem_Mesmo_idPessoa()
        {
            aluno2 = new Aluno()
            {
                idPessoa = 1,
                idAluno = 2,
                Matricula = 111111
            };

            Assert.AreEqual(aluno1.idPessoa, aluno2.idPessoa);
        }


        // ============================ PROFESSOR =======================

        [TestMethod]
        public void Garantir_Que_2_professors_Sao_Iguais_Quando_Tem_Mesmo_IdProfessor()
        {
            professor2 = new Professor()
            {
                idProfessor = 1,
                idPessoa = 3,
                Especializacao = "banco"
            };
            Assert.AreEqual(professor1.idProfessor, professor2.idProfessor);
        }

        [TestMethod]
        public void Garantir_Que_2_professors_Sao_Iguais_Quando_Tem_Mesmo_idProfessor()
        {
            professor2 = new Professor()
            {
                idProfessor = 1,
                idPessoa = 3,
                Especializacao = "banco"
            };
            Assert.AreEqual(professor1.idProfessor, professor2.idProfessor);
        }

        [TestMethod]
        public void test_obterprofessor()
        {
            int idprofessor = 1;
            professor2 = new Professor()
            {
                idProfessor = 1,
                idPessoa = 3,
                Especializacao = "banco"
            };
            Assert.AreEqual(professor2.idProfessor, idprofessor);
        }
    }
}
