using System;

namespace NotaAzul.Helpers
{
    /// <summary>
    /// Casse responsável por mapear os campos das tabelas do projeto NotaAzul
    /// </summary>
    static class MapeamentoTabela
    {
        public static string Aluno(String aliasTabela = "Aluno", String aliasCampos = "Aluno")
        {
            aliasTabela = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasTabela);
            aliasCampos = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasCampos);

            return String.Format("" +
                "{0}Id as '{1}Id', " +
                "{0}IdCorporacao as '{1}IdCorporacao', " +
                "{0}IdSituacao as '{1}IdSituacao', " +
                "{0}IdImagem as '{1}IdImagem', " +
                "{0}Nome as '{1}Nome', " +
                "{0}Sexo as '{1}Sexo', " +
                "{0}GrupoSanguineo as '{1}GrupoSanguineo', " +
                "{0}CPF as '{1}CPF', " +
                "{0}RG as '{1}RG', " +
                "{0}DiaPagamento as '{1}DiaPagamento', " +
                "{0}DataCadastro as '{1}DataCadastro'",
                aliasTabela,
                aliasCampos);
        }

        public static string Curso(String aliasTabela = "Curso", String aliasCampos = "Curso")
        {
            aliasTabela = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasTabela);
            aliasCampos = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasCampos);

            return String.Format("" +
                "{0}Id as '{1}Id', " +
                "{0}IdCorporacao as '{1}IdCorporacao', " +
                "{0}IdSegmento as '{1}IdSegmento', " +
                "{0}IdSituacao as '{1}IdSituacao', " +
                "{0}Nome as '{1}Nome', " +
                "{0}DataCadastro as '{1}DataCadastro'",
                aliasTabela,
                aliasCampos);
        }

        public static string Segmento(String aliasTabela = "Segmento", String aliasCampos = "Segmento")
        {
            aliasTabela = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasTabela);
            aliasCampos = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasCampos);

            return String.Format("" +
                "{0}Id as '{1}Id', " +
                "{0}IdCorporacao as '{1}IdCorporacao', " +
                "{0}IdSituacao as '{1}IdSituacao', " +
                "{0}Nome as '{1}Nome', " +
                "{0}DataCadastro as '{1}DataCadastro'",
                aliasTabela,
                aliasCampos);
        }

        public static string Turma(String aliasTabela = "Turma", String aliasCampos = "Turma")
        {
            aliasTabela = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasTabela);
            aliasCampos = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasCampos);

            return String.Format("" +
                "{0}Id as '{1}Id', " +
                "{0}IdCorporacao as '{1}IdCorporacao', " +
                "{0}IdSituacao as '{1}IdSituacao', " +
                "{0}Nome as '{1}Nome', " +
                "{0}DataCadastro as '{1}DataCadastro'",
                aliasTabela,
                aliasCampos);
        }

        public static string Turno(String aliasTabela = "Turno", String aliasCampos = "Turno")
        {
            aliasTabela = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasTabela);
            aliasCampos = Prion.Generic.Helpers.MapeamentoTabela.AcertarAliasTabela(aliasCampos);

            return String.Format("" +
                "{0}Id as '{1}Id', " +
                "{0}IdCorporacao as '{1}IdCorporacao', " +
                "{0}IdSituacao as '{1}IdSituacao', " +
                "{0}Nome as '{1}Nome', " +
                "{0}DataCadastro as '{1}DataCadastro'",
                aliasTabela,
                aliasCampos);
        }
    }
}