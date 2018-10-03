using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using GenericModels = Prion.Generic.Models;

namespace NotaAzul.Business
{
    public class Menu : Base
    {
        /// <summary> Obtém uma lista contendo objetos de menu e os ajustam para que ocupem seu nível</summary>
        /// <param name="lista"> Lista de menu desordenada </param>
        /// <returns> Lista de menus ajustadas (menu pai preenchido com seus respectivos submenus) </returns>
        public List<GenericModels.Menu> Formatar(List<GenericModels.Menu> lista)
        {
            List<GenericModels.Menu> listanova = new List<GenericModels.Menu>();

            if (lista == null)
            {
                return listanova;
            }

            // preenche os submenus dos itens de menu
            foreach (GenericModels.Menu mp in lista)
            {
                // obtendo os submenus do menu pai (objeto mp)
                var sm = from m in lista
                         where m.IdMenuPai == mp.Id
                         select m;

                // fazendo um cast da lista retornada utilizando LINQ para uma lista de objetos de menu
                mp.Submenus = sm.AsEnumerable().Cast<GenericModels.Menu>().ToList();

                // apenas adiciona à lista nova os menus que não tiverem pai
                if (mp.IdMenuPai == 0)
                {
                    listanova.Add(mp);
                }
            }

            // retorna uma nova lista formatada
            return listanova;
        }

        /// <summary> Obtém uma lista de objetos de menu (com formatação ou a ser formatada [formatar = true]) e retorna uma string HTML </summary>
        /// <param name="lista">Lista a ser transformada em HTML</param>
        /// <param name="formatar">Verifica se a lista passada deve ser formatada ou não</param>
        /// <param name="novaLista">Utilizado para controle de chamada recursiva</param>
        /// <returns> Retorna HTML referente ao menu recebido </returns>
        public string ToHTML(List<GenericModels.Menu> lista, bool formatar = false, bool novaLista = true, String tituloMenuPai = "")
        {
            // caso a lista deva ser formatada, chamar função, caso contrário, utilizar lista passada
            List<GenericModels.Menu> listaFormatada = (formatar) ? Formatar(lista) : lista;

            StringBuilder html = new StringBuilder();

            // caso seja uma nova lista, abrir a tag UL
            if (novaLista)
            {
                html.Append("<ul>");
            }

            String css = "";

            foreach (GenericModels.Menu menu in listaFormatada)
            {
                if (menu.Submenus.Count == 0)
                {
                    css = (menu.ClassCSS.Trim() == "") ? "enable" : menu.ClassCSS;

                    html.Append("<li>");
                    html.Append("<a class='" + css + "' rel='" + menu.Url + "' menu='" + tituloMenuPai + "' title-page='" + menu.TituloPagina + "' subtitle-page='" + menu.SubtituloPagina + "'>" + menu.Titulo + "</a></li>");

                    continue;
                }

                // se chegou aqui é porque existe algum submenu

                html.Append("<li class='subtitle'><a class='action' href='#'>" + menu.Titulo + "</a>");

                /*if (menu.ClassCSS.Trim() != "")
                {
                    html.Append("<img class='" + menu.ClassCSS + "' />");
                }*/

                html.Append("<ul class='submenu' style='display:block;'>");
                html.Append(ToHTML(menu.Submenus, false, false, menu.Titulo));
                html.Append("</ul>");
                html.Append("</li>");
            }

            // caso seja uma nova lista, fechar a tag UL e adicionar uma nova linha
            if (novaLista)
            {
                html.Append("</ul>"); 
                html.Append(Environment.NewLine);
            }

            return html.ToString();
        }
    }
}