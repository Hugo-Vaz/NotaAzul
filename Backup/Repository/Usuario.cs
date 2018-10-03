using System.Collections.Generic;
using System.Data.Common;
using Prion.Tools;
using System.Data;
using System;

namespace NotaAzul.Repository
{
    public class Usuario : Prion.Generic.Repository.Usuario
    {
        /// <summary>Construtor da classe</summary>
        /// <param name="conexao">Uma instância de DBFacade</param>
        public Usuario(ref Prion.Data.DBFacade conexao)
            : base(ref conexao)
        {
        }

        /// <summary>Destrutor silencioso</summary>
        ~Usuario()
        {
        }

        /// <summary>
        ///Cria o relacionamento entre Funcionário(Professor) e Usuário
        /// </summary>
        /// <param name="idFuncionario"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public void CriarRelacionamento(Int32 idFuncionario, Int32 idUsuario)
        {
            List<DbParameter> parametros = new List<DbParameter>();
            String sql = "INSERT INTO FuncionarioUsuario(IdFuncionario, IdUsuario) " +
                        "VALUES(@IdFuncionario, @IdUsuario)";

            parametros.Add(this.Conexao.CriarParametro("@IdFuncionario", DbType.Int32, idFuncionario));
            parametros.Add(this.Conexao.CriarParametro("@IdUsuario", DbType.Int32, idUsuario));

            this.Conexao.Insert(sql, parametros);

        }

        /// <summary>
        ///Exclui o relacionamento entre Funcionário(Professor) e Usuário
        /// </summary>
        /// <param name="idFuncionario"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public void ExcluirRelacionamento(Int32[] idsUsuario)
        {

            List<DbParameter> parametros = new List<DbParameter>();
            String strIds = Conversor.ToString(",", idsUsuario);

            String sql = "DELETE FROM FuncionarioUsuario WHERE IdUsuario IN (" + strIds + ") ";
                                 
          
            this.Conexao.Delete(sql, parametros);
        }

    }
}