using System;
using System.Collections.Generic;
using GenericHelpers = Prion.Generic.Helpers;
using GenericModels = Prion.Generic.Models;
using GenericRepository = Prion.Generic.Repository;

namespace NotaAzul.Business
{
    public class ConfiguracoesNotas:Base
    {
          /// <summary>
        /// Construtor da Classe
        /// </summary>
        public ConfiguracoesNotas()
        { 
        }
        
        /// <summary>
        /// Salva as configurações do sistema no Banco de Dados
        /// </summary>
        /// <param name="titulos">uma lista de objetos do tipo Prion.Generic.Models.Titulo</param>
        /// <returns>Objeto do tipo Models.Retorno com as propriedades indicando se salvou ou se deu algum erro</returns>
        public GenericHelpers.Retorno Salvar(List<GenericModels.ConfiguracaoSistema>config)
        {
            GenericRepository.ConfiguracaoSistema repConfig = new GenericRepository.ConfiguracaoSistema(ref this.Conexao);

            GenericHelpers.Retorno retorno = repConfig.Salvar(config);
            retorno.Mensagem = "As configurações da forma de avaliação(notas/conceitos e forma de divisão do ano letivo) foram alteradas com sucesso.As alterações terão efeito na próxima vez em que se logar no sistema";
            return retorno;

        }
    }
}