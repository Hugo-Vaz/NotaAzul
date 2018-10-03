using System;
using System.Collections.Generic;

using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class Situacao : Base
    {
        public List<GenericModels.Situacao> CarregarSituacoesPeloTipo(String tipoSituacao)
        {
            GenericRepository.Situacao repSituacao = new GenericRepository.Situacao(ref this.Conexao);
            return repSituacao.BuscarPeloTipo(tipoSituacao);
        }

        public GenericModels.Situacao CarregarSituacoesPelaSituacao(String tipoSituacao, String situacao)
        {
            GenericRepository.Situacao repSituacao = new GenericRepository.Situacao(ref this.Conexao);
            return repSituacao.BuscarPelaSituacao(tipoSituacao, situacao);
        }
    }
}