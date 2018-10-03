/**
 * @descrição: Gera o objeto de combobox para todas as tabelas do sistema. Gera os controles de manipulação (botões de CARREGAR, NOVO, ALTERAR, EXCLUIR)
 * @data: 06/05/2013 - 17:30
 **/


/**************************************************************************************************
**** Alinea.ComboBox
*************************************************************************************************/
if (NotaAzul.Alinea == null) { NotaAzul.Alinea = {}; }

NotaAzul.Alinea.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {

            var cfgUser = arguments[0] || null;

            var cfg = {
                autoLoad: false, // não vai carregar a lista de alíneas
                el: "comboboxAlinea",
                itens: Prion.Helpers.ListaAlineas(),
                clear: true,
                valueDefault: true,
                //value: "", // COMO É VAZIO, NÃO PRECISA INFORMAR
                //text: "", // COMO É VAZIO, NÃO PRECISA INFORMAR
                textEmpty: "Selecione uma Alínea",
                insert: {
                    show: false
                },
                update: {
                    show: false
                },
                remove: {
                    show: false
                }
            }

            cfg = Prion.Apply(cfg, cfgUser);
            if (cfg.id != null) {
                cfg.el = cfg.id;
            }

            Prion.ComboBox.Carregar(cfg);
        }
    }
} ();
/**************************************************************************************************
**** /Alinea.ComboBox
*************************************************************************************************/


/**************************************************************************************************
**** NotaAzul.Bairro
**************************************************************************************************/
if (NotaAzul.Bairro == null) { NotaAzul.Bairro = {}; }

NotaAzul.Bairro.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            // obtém o idCidade. Se não foi informado, usa o id default (comboboxCidade)
            var idOrigem = ((cfgUser == null) || (cfgUser.idOrigem == null)) ? "comboboxCidade" : cfgUser.idOrigem;

            var cfg = {
                id: "comboboxBairro",
                modulo: "BAIRRO",
                controller: "Bairro",
                combobox: {
                    textEmpty: "Selecione um Bairro"
                },
                load: {
                    fn: function () {
                        var idDestino = this.id;
                        NotaAzul.Bairro.ComboBox.Load(idOrigem, idDestino);
                    }
                },
                insert: {
                    title: "Novo Bairro",
                    tooltip: "Novo Bairro",
                    action: function () { NotaAzul.Bairro.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Bairro",
                    tooltip: "Alterar Bairro",
                    action: function () { NotaAzul.Bairro.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Bairro",
                    tooltip: "Excluir Bairro"
                },
                window: {
                    object: "Bairro",
                    title: { text: "Bairro" },
                    height: 208,
                    width: 540,
                    btnSave: { obj: "NotaAzul.Bairro.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Bairro.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        },

        Load: function (idOrigem, idDestino) {
            if ((idOrigem == null) || (idDestino == null)) { return; }

            var idValue = idOrigem.getValue().value;
            if ((idValue == null) || (idValue == "")) { return; }

           // console.log("load bairro");

            Prion.ComboBox.Carregar({
                url: rootDir + "Bairro/GetLista",
                el: idDestino,
                filter: "Paginar=false&IdCidade=" + idValue,
                clear: true,
                valueDefault: true
            });
        }
    }
} ();
/**************************************************************************************************
 **** NotaAzul.Bairro
 *************************************************************************************************/

/**************************************************************************************************
**** NotaAzul.BandeiraCartao.ComboBox
*************************************************************************************************/
if (NotaAzul.BandeiraCartao == null) { NotaAzul.BandeiraCartao = {}; }

NotaAzul.BandeiraCartao.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            // obtém a lista de cartão bandeira
            var bandeiraCartao = NotaAzul.GetConfig("cartao_bandeira:lista");
            var arr = bandeiraCartao.Valor.split(",");
            var arr2 = [];

            if ((arr != null) && (arr.length > 0)) {
                for (var i = 0; i < arr.length; i++) {
                    arr2.push({
                        value: arr[i].trim(),
                        text: arr[i].trim()
                    });
                }
            }

            var cfgUser = arguments[0] || null;
            var cfg = {
                el: "comboboxBandeiraCartao",
                itens: arr2,
                clear: true,
                sort: true,
                valueDefault: true,
                value: "",
                text: "",
                textEmpty: "Selecione uma bandeira de Cartão"
            }

            cfg = Prion.Apply(cfg, cfgUser);
            if (cfg.id != null) {
                cfg.el = cfg.id;
            }
            Prion.ComboBox.Carregar(cfg);
        }

    };
} ();
/**************************************************************************************************
**** NotaAzul.BandeiraCartao.ComboBox
**************************************************************************************************/


/**************************************************************************************************
 **** NotaAzul.Cargo
 *************************************************************************************************/
if (NotaAzul.Cargo == null) { NotaAzul.Cargo = {}; }

NotaAzul.Cargo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxCargo",
                modulo: "CARGO",
                controller: "Cargo",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Cargo"
                },
                insert: {
                    title: "Novo Cargo",
                    tooltip: "Novo Cargo",
                    action: function () { NotaAzul.Cargo.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Cargo",
                    tooltip: "Alterar Cargo",
                    action: function () { NotaAzul.Cargo.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Cargo",
                    tooltip: "Excluir Cargo"
                },
                window: {
                    object: "Cargo",
                    title: { text: "Cargo" },
                    height: 208,
                    width: 540,
                    btnSave: { obj: "NotaAzul.Cargo.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Cargo.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** NotaAzul.Cargo
 *************************************************************************************************/



/**************************************************************************************************
 **** Cidade.ComboBox
 *************************************************************************************************/
if (NotaAzul.Cidade == null) { NotaAzul.Cidade = {}; }

NotaAzul.Cidade.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            // obtém a cidade default
            var cidadeDefault = NotaAzul.GetConfig("cidade:id_cidade_default");

            var cfgUser = arguments[0] || null;

            // obtém o idEstado. Se não foi informado, usa o id default (comboboxEstado)
            var idOrigem = ((cfgUser == null) || (cfgUser.idOrigem == null)) ? "comboboxEstado" : cfgUser.idOrigem;

            var cfg = {
                id: "comboboxCidade",
                modulo: "CIDADE",
                combobox: {
                    selectedDefault: {
                        value: (cidadeDefault == null) ? null : cidadeDefault.Valor
                    },
                    textEmpty: "Selecione uma Cidade"
                },
                load: {
                    fn: function () {
                        var idDestino = this.id;
                        var fnAfterLoad = null;

                        if ((cfgUser != null) && (cfgUser.observer != null)) {
                            // o que será feito antes de carregar os registros de cidade
                            if (cfgUser.observer.beforeLoad != null) {
                                cfgUser.observer.beforeLoad.call(this);
                            }

                            // o que será feito após carregar os registros de cidade
                            if (cfgUser.observer.afterLoad != null) {
                                fnAfterLoad = cfgUser.observer.afterLoad;
                            }
                        }

                        NotaAzul.Cidade.ComboBox.Load(idOrigem, idDestino, fnAfterLoad);
                    }
                },
                controller: "Cidade",
                insert: {
                    title: "Nova Cidade",
                    tooltip: "Nova Cidade",
                    action: function () { NotaAzul.Cidade.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Cidade",
                    tooltip: "Alterar Cidade",
                    action: function () { NotaAzul.Cidade.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Cidade",
                    tooltip: "Excluir Cidade"
                },
                window: {
                    object: "Cidade",
                    title: { text: "Cidade" },
                    height: 208,
                    width: 540,
                    btnSave: { obj: "NotaAzul.Cidade.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Cidade.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        },

        Load: function (idOrigem, idDestino, fnAfterLoad) {
            if ((idOrigem == null) || (idDestino == null)) { return; }

            var idValue = idOrigem.getValue().value;
            if ((idValue == null) || (idValue == "")) { return; }

            console.log("load cidade");

            var cidadeDefault = NotaAzul.GetConfig("cidade:id_cidade_default");

            Prion.ComboBox.Carregar({
                url: rootDir + "Cidade/GetLista",
                el: idDestino,
                filter: "Paginar=false&IdEstado=" + idValue,
                clear: true,
                valueDefault: true,
                selectedDefault: {
                    value: (cidadeDefault == null) ? null : cidadeDefault.Valor
                },
                action: {
                    complete: fnAfterLoad
                }
            });
        }
    }
} ();
/**************************************************************************************************
 **** /Cidade.ComboBox
 *************************************************************************************************/


/**************************************************************************************************
**** NotaAzul.Cargo
*************************************************************************************************/
if (NotaAzul.Cargo == null) { NotaAzul.Cargo = {}; }

NotaAzul.Cargo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxCargo",
                modulo: "CARGO",
                controller: "Cargo",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Cargo"
                },
                insert: {
                    title: "Novo Cargo",
                    tooltip: "Novo Cargo",
                    action: function () { NotaAzul.Cargo.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Cargo",
                    tooltip: "Alterar Cargo",
                    action: function () { NotaAzul.Cargo.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Cargo",
                    tooltip: "Excluir Cargo"
                },
                window: {
                    object: "Cargo",
                    title: { text: "Cargo" },
                    height: 208,
                    width: 540,
                    btnSave: { obj: "NotaAzul.Cargo.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Cargo.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
**** NotaAzul.Cargo
*************************************************************************************************/


/**************************************************************************************************
 **** ChequeEstado.ComboBox
 *************************************************************************************************/
if (NotaAzul.ChequeEstado == null) { NotaAzul.ChequeEstado = {}; }

NotaAzul.ChequeEstado.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxChequeEstado",
                modulo: "CHEQUE_ESTADO",
                controller: "ChequeEstado",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Estado do Cheque"
                },
                insert: {
                    title: "Novo Estado",
                    tooltip: "Novo Estado",
                    action: function () { NotaAzul.ChequeEstado.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Estado do Cheque",
                    tooltip: "Alterar Estado do Cheque",
                    action: function () { NotaAzul.ChequeEstado.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Estado do Cheque",
                    tooltip: "Excluir Estado do Cheque"
                },
                window: {
                    width: 580,
                    height: 300,
                    object: "ChequeEstado",
                    title: { text: "Estado de um Cheque" },
                    btnSave: { obj: "NotaAzul.ChequeEstado.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.ChequeEstado.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /ChequeEstado.ComboBox
 *************************************************************************************************/

/**************************************************************************************************
**** Curso.ComboBox
*************************************************************************************************/
if (NotaAzul.Curso == null) { NotaAzul.Curso = {}; }

NotaAzul.Curso.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxCurso",
                modulo: "CURSO",
                controller: "Curso",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Curso"
                },
                insert: {
                    title: "Novo Curso",
                    tooltip: "Novo Curso",
                    action: function () { NotaAzul.Curso.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Curso",
                    tooltip: "Alterar Curso",
                    action: function () { NotaAzul.Curso.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Curso",
                    tooltip: "Excluir Curso"
                },
                window: {
                    width: 580,
                    height: 300,
                    object: "Curso",
                    title: { text: "Curso" },
                    btnSave: { obj: "NotaAzul.Curso.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Curso.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
**** /Curso.ComboBox
*************************************************************************************************/


/**************************************************************************************************
 **** CursoAnoLetivo.ComboBox
 *************************************************************************************************/
if (NotaAzul.CursoAnoLetivo == null) { NotaAzul.CursoAnoLetivo = {}; }

NotaAzul.CursoAnoLetivo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                autoLoad: false, // indica que o carregamento NÃO será realizado automaticamente, apesar de montar toda a estrutura do combobox
                id: "comboboxCursoAnoLetivo",
                modulo: "CURSO_ANO_LETIVO",
                controller: "CursoAnoLetivo",
                filter: "Entidades=Situacao,Curso",
                combobox: {
                    textEmpty: "Selecione um Curso"
                },
                insert: {
                    title: "Novo Curso",
                    tooltip: "Novo Curso",
                    action: function () { NotaAzul.CursoAnoLetivo.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Curso",
                    tooltip: "Alterar Curso",
                    action: function () { NotaAzul.CursoAnoLetivo.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Curso",
                    tooltip: "Excluir Curso"
                },
                window: {
                    object: "CursoAnoLetivo",
                    title: { text: "Curso de um ano letivo" },
                    height: 380,
                    width: 600,
                    btnSave: { obj: "NotaAzul.CursoAnoLetivo.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.CursoAnoLetivo.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /CursoAnoLetivo.ComboBox
 *************************************************************************************************/

/**************************************************************************************************
**** NotaAzul.DivisaoAnoLetivo.ComboBox
*************************************************************************************************/
if (NotaAzul.DivisaoAnoLetivo == null) { NotaAzul.DivisaoAnoLetivo = {}; }

NotaAzul.DivisaoAnoLetivo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {

            var divisaoAnoLetivo = [
                { "value": "bimestral", "text": "Bimestral(4 períodos de 2 meses)" },
                { "value": "trimestral","text":"Trimestral(3 períodos de 3 meses)" },
                { "value": "semestral", "text": "Semestral(2 períodos de 6 meses)" }
            ];            

            var cfgUser = arguments[0] || null;
            var cfg = {
                el: "comboboxDivisaoAnoLetivo",
                itens: divisaoAnoLetivo,
                clear: true,
                sort: true,
                valueDefault: true,
                value: "",
                text: "",
                textEmpty: "Selecione a forma como o ano letivo será dividido"
            }

            cfg = Prion.Apply(cfg, cfgUser);
            if (cfg.id != null) {
                cfg.el = cfg.id;
            }
            Prion.ComboBox.Carregar(cfg);
        }

    };
} ();
/**************************************************************************************************
**** NotaAzul.DivisaoAnoLetivo.ComboBox
**************************************************************************************************/

/**************************************************************************************************
 **** Estado.ComboBox
 *************************************************************************************************/
if (NotaAzul.Estado == null) { NotaAzul.Estado = {}; }

NotaAzul.Estado.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            // obtém o estado default
            var estadoDefault = NotaAzul.GetConfig("estado:id_uf_default");

            var cfgUser = arguments[0] || null;

            var cfg = {
                el: "comboboxEstado",
                itens: Prion.Helpers.ListaEstados(),
                clear: true,
                valueDefault: true,
                //value: "", // COMO É VAZIO, NÃO PRECISA INFORMAR
                //text: "", // COMO É VAZIO, NÃO PRECISA INFORMAR
                textEmpty: "Selecione um Estado",
                selectedDefault: {
                    value: (estadoDefault == null) ? null : estadoDefault.Valor
                }
            }

            cfg = Prion.Apply(cfg, cfgUser);
            if (cfg.id != null) {
                cfg.el = cfg.id;
            }

            // DEFINE O EVENTO change PARA O COMBOBOX
            //Prion.Event.remove(cfg.el, "change", fnLoadCidade);
            //Prion.Event.add(cfg.el, "change", fnLoadCidade);
            // FIM DO EVENTO change


            /*cfg.action = {
                complete: function () {
                    var idOrigem = cfg.el, idDestino = idCidade;
                    NotaAzul.Cidade.Load.Load(cfg.el, idDestino);
                }
            }*/


            Prion.ComboBox.Carregar(cfg);
        }
    }
} ();
/**************************************************************************************************
 **** /Estado.ComboBox
 *************************************************************************************************/

 /**************************************************************************************************
**** NotaAzul.FormaNota.ComboBox
*************************************************************************************************/
if (NotaAzul.FormaNota == null) { NotaAzul.FormaNota = {}; }

NotaAzul.FormaNota.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {

            var formaNota = [
                { "value": "base_cem", "text": "0 - 100" },
                { "value": "base_dez", "text": "0 - 10" },
                { "value": "base_conceitual", "text": "F - A" }
            ];
            

            var cfgUser = arguments[0] || null;
            var cfg = {
                el: "comboboxDivisaoAnoLetivo",
                itens: formaNota,
                clear: true,
                sort: true,
                valueDefault: true,
                value: "",
                text: "",
                textEmpty: "Selecione a forma como o ano letivo será dividido"
            }

            cfg = Prion.Apply(cfg, cfgUser);
            if (cfg.id != null) {
                cfg.el = cfg.id;
            }
            Prion.ComboBox.Carregar(cfg);
        }

    };
} ();
/**************************************************************************************************
**** NotaAzul.FormaNota.ComboBox
**************************************************************************************************/

/**************************************************************************************************
**** NotaAzul.Funcionario.ComboBox
*************************************************************************************************/
if (NotaAzul.Funcionario == null) { NotaAzul.Funcionario = {}; }

NotaAzul.Funcionario.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxFuncionario",
                modulo: "FUNCIONARIO",
                controller: "Funcionario",
                filter: "Entidades=Situacao,Cargo",
                combobox: {
                    textEmpty: "Selecione um Funcionário"
                },
                insert: {
                    title: "Novo Funcionário",
                    tooltip: "Novo Funcionário",
                    action: function () { NotaAzul.Funcionario.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Funcionário",
                    tooltip: "Alterar Funcionário",
                    action: function () { NotaAzul.Curso.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Funcionário",
                    tooltip: "Excluir Funcionário"
                },
                window: {
                    width: 580,
                    height: 300,
                    object: "Funcionario",
                    title: { text: "Funcionário" },
                    btnSave: { obj: "NotaAzul.Funcionario.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Funcionario.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        },

    }
} ();
/**************************************************************************************************
**** /NotaAzul.Funcionario.ComboBox
*************************************************************************************************/

/**************************************************************************************************
 **** GrauParentesco.ComboBox
 *************************************************************************************************/
if (NotaAzul.GrauParentesco == null) { NotaAzul.GrauParentesco = {}; }

NotaAzul.GrauParentesco.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            // obtém a lista de grau de parentesco
            var grauParentesco = NotaAzul.GetConfig("grau_parentesco:lista");
            var arr = grauParentesco.Valor.split(",");
            var arr2 = [];

            if ((arr != null) && (arr.length > 0)) 
            {
                for (var i = 0; i < arr.length; i++) 
                {
                    arr2.push({
                        value: arr[i].trim(),
                        text: arr[i].trim()
                    });
                }
            }

            var cfgUser = arguments[0] || null;
            var cfg = {
                el: "comboboxGrauParentesco",
                itens: arr2,
                clear: true,
                sort: true,
                valueDefault: true,
                value: "",
                text: "",
                textEmpty: "Selecione um Grau de Parentesco"
            }

            cfg = Prion.Apply(cfg, cfgUser);
            if (cfg.id != null) {
                cfg.el = cfg.id;
            }
            Prion.ComboBox.Carregar(cfg);
        }

    };
} ();
/**************************************************************************************************
**** /GrauParentesco.ComboBox
**************************************************************************************************/

/**************************************************************************************************
 **** GrupoDesconto.ComboBox
 *************************************************************************************************/
if (NotaAzul.GrupoDesconto == null) { NotaAzul.GrupoDesconto = {}; }

NotaAzul.GrupoDesconto.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxGrupoDesconto",
                modulo: "GRUPO_DESCONTO",
                controller: "GrupoDesconto",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Grupo de Desconto"
                },
                insert: {
                    title: "Novo Grupo de Desconto",
                    tooltip: "Novo Grupo de Desconto",
                    action: function () { NotaAzul.GrupoDesconto.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Grupo de Desconto",
                    tooltip: "Alterar Grupo de Desconto",
                    action: function () { NotaAzul.GrupoDesconto.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Grupo de Desconto",
                    tooltip: "Excluir Grupo de Desconto"
                },
                window: {
                    object: "GrupoDesconto",
                    title: { text: "Grupo de Desconto" },
                    height: 305,
                    width: 670,
                    btnSave: { obj: "NotaAzul.GrupoDesconto.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.GrupoDesconto.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /GrupoDesconto.ComboBox
 *************************************************************************************************/


/**************************************************************************************************
 **** Segmento.ComboBox
 *************************************************************************************************/
if (NotaAzul.Segmento == null) { NotaAzul.Segmento = {}; }

NotaAzul.Segmento.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxSegmento",
                modulo: "SEGMENTO",
                controller: "Segmento",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Segmento"
                },
                insert: {
                    title: "Novo Segmento",
                    tooltip: "Novo Segmento",
                    action: function () { NotaAzul.Segmento.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Segmento",
                    tooltip: "Alterar Segmento",
                    action: function () { NotaAzul.Segmento.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Segmento",
                    tooltip: "Excluir Segmento"
                },
                window: {
                    height: 206,
                    width: 540,
                    object: "Segmento",
                    title: { text: "Segmento" },
                    btnSave: { obj: "NotaAzul.Segmento.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Segmento.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /Segmento.ComboBox
 *************************************************************************************************/


/**************************************************************************************************
**** TipoMedia.ComboBox
*************************************************************************************************/
if (NotaAzul.TipoMedia == null ) { NotaAzul.TipoMedia = {};}

 NotaAzul.TipoMedia.ComboBox = function(){
     return {
            Gerar: function (/**config*/) {

                var tipoMedia = [
                    { "value": "aritmetica_simples", "text": "Média Aritmética Simples" },
                    { "value": "aritmetica_ponderada", "text": "Média Aritmética Ponderada" },
                    { "value": "geometrica", "text": "Média Geométrica" },
                    { "value": "harmonica", "text": "Média Harmônica" }
                ];
            

                var cfgUser = arguments[0] || null;
                var cfg = {
                    el: "comboboxTipoMedia",
                    itens: tipoMedia,
                    clear: true,
                    sort: true,
                    valueDefault: true,
                    value: "",
                    text: "",
                    textEmpty: "Selecione a forma como a média dos alunos será calculada"
                }

                cfg = Prion.Apply(cfg, cfgUser);
                if (cfg.id != null) {
                    cfg.el = cfg.id;
                }
                Prion.ComboBox.Carregar(cfg);
            }

        };
 }();

/**************************************************************************************************
**** /TipoMedia.ComboBox
*************************************************************************************************/

/**************************************************************************************************
**** TituloTipo.ComboBox
*************************************************************************************************/
if (NotaAzul.TituloTipo == null) { NotaAzul.TituloTipo = {}; }

NotaAzul.TituloTipo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxTituloTipo",
                modulo: "TIPO_TITULO",
                controller: "TituloTipo",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Tipo"
                },
                insert: {
                    title: "Novo Tipo",
                    tooltip: "Novo Tipo",
                    action: function () { NotaAzul.TituloTipo.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Tipo",
                    tooltip: "Alterar Tipo",
                    action: function () { NotaAzul.TituloTipo.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Tipo",
                    tooltip: "Excluir Tipo"
                },
                window: {
                    object: "TituloTipo",
                    width: 510,
                    height: 256,
                    title: { text: "Tipos de Título" },
                    btnSave: { obj: "NotaAzul.TituloTipo.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.TituloTipo.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
**** /TituloTipo.ComboBox
*************************************************************************************************/


/**************************************************************************************************
 **** Turma.ComboBox
 *************************************************************************************************/
if (NotaAzul.Turma == null) { NotaAzul.Turma = {}; }

NotaAzul.Turma.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxTurma",
                modulo: "TURMA",
                controller: "Turma",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione uma Turma"
                },
                insert: {
                    title: "Nova Turma",
                    tooltip: "Nova Turma",
                    action: function () { NotaAzul.Turma.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Turma",
                    tooltip: "Alterar Turma",
                    action: function () { NotaAzul.Turma.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Turma",
                    tooltip: "Excluir Turma"
                },
                window: {
                    object: "Turma",
                    title: { text: "Turma" },
                    btnSave: { obj: "NotaAzul.Turma.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Turma.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /Turma.ComboBox
 *************************************************************************************************/



/**************************************************************************************************
 **** TurmaAnoLetivo.ComboBox
 *************************************************************************************************/
if (NotaAzul.TurmaAnoLetivo == null) { NotaAzul.TurmaAnoLetivo = {}; }

NotaAzul.TurmaAnoLetivo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxTurmaAnoLetivo",
                modulo: "TURMA_ANO_LETIVO",
                controller: "CursoAnoLetivo",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione uma Turma"
                },
                insert: {
                    title: "Nova Turma",
                    tooltip: "Nova Turma",
                    action: function () { NotaAzul.TurmaAnoLetivo.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Turma",
                    tooltip: "Alterar Turma",
                    action: function () { NotaAzul.TurmaAnoLetivo.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Turma",
                    tooltip: "Excluir Turma"
                },
                window: {
                    object: "TurmaAnoLetivo",
                    title: { text: "Turma de um ano letivo" },
                    btnSave: { obj: "NotaAzul.TurmaAnoLetivo.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.TurmaAnoLetivo.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /TurmaAnoLetivo.ComboBox
 *************************************************************************************************/



/**************************************************************************************************
 **** Turno.ComboBox
 *************************************************************************************************/
if (NotaAzul.Turno == null) { NotaAzul.Turno = {}; }

NotaAzul.Turno.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxTurno",
                modulo: "TURNO",
                controller: "Turno",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Turno"
                },
                insert: {
                    title: "Novo Turno",
                    tooltip: "Novo Turno",
                    action: function () { NotaAzul.Turno.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Turno",
                    tooltip: "Alterar Turno",
                    action: function () { NotaAzul.Turno.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Turno",
                    tooltip: "Excluir Turno"
                },
                window: {
                    width: 450,
                    height: 206,
                    object: "Turno",
                    title: { text: "Turno" },
                    btnSave: { obj: "NotaAzul.Turno.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.Turno.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /Turno.ComboBox
 *************************************************************************************************/



/**************************************************************************************************
 **** UsuarioGrupo.ComboBox
 *************************************************************************************************/
if (NotaAzul.UsuarioGrupo == null) { NotaAzul.UsuarioGrupo = {}; }

NotaAzul.UsuarioGrupo.ComboBox = function () {
    return {
        Gerar: function (/**config*/) {
            var cfgUser = arguments[0] || null;

            var cfg = {
                id: "comboboxUsuarioGrupo",
                modulo: "USUARIO_GRUPO",
                controller: "UsuarioGrupo",
                filter: "Entidades=Situacao",
                combobox: {
                    textEmpty: "Selecione um Grupo de Usuário"
                },
                insert: {
                    title: "Novo Grupo de Usuário",
                    tooltip: "Novo Grupo de Usuário",
                    action: function () { NotaAzul.UsuarioGrupo.Dados.Novo(); }
                },
                update: {
                    title: "Alterar Grupo de Usuário",
                    tooltip: "Alterar Grupo de Usuário",
                    action: function () { NotaAzul.UsuarioGrupo.Dados.Novo(); }
                },
                remove: {
                    title: "Excluir Grupo de Usuário",
                    tooltip: "Excluir Grupo de Usuário"
                },
                window: {
                    height: 260,
                    width: 490,
                    object: "UsuarioGrupo",
                    title: { text: "Grupo de Usuário" },
                    btnSave: { obj: "NotaAzul.UsuarioGrupo.Dados", fn: "Salvar" },
                    btnClear: { obj: "NotaAzul.UsuarioGrupo.Dados", fn: "Novo" }
                }
            };

            Prion.ButtonsForComboBox(cfg, cfgUser);
        }
    }
} ();
/**************************************************************************************************
 **** /UsuarioGrupo.ComboBox
 *************************************************************************************************/