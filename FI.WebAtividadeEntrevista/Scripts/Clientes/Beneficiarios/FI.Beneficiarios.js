async function incluirBeneficiario() {
    let modal = $('#modalBeneficiarios');
    let benef = new Beneficiario(modal.find('#CPF').val(), modal.find('#Nome').val());
    let resultado = await benef.validar();

    if (resultado.length > 0) {
        for (var i = 0; i < resultado.length; i++) {
            adicionarError('#modalBeneficiarios .header #' + resultado[i].campo, resultado[i].error)
        }
        return;
    }

    adicionarBeneficiario(benef);
}

async function salvarAlteracaoBeneficiario(event) {
    let tr = $(event.target).closest('tr');
    let cpf = tr.find('#CPF');
    let nome = tr.find('#Nome');
    let button = tr.find('#btnAlterar');
    let benef = new Beneficiario(cpf.val(), nome.val());
    let resultado = await benef.validar(true);

    if (resultado.length > 0) {
        for (var i = 0; i < resultado.length; i++) {
            adicionarError('#modalBeneficiarios tr[edicao=' + tr.attr('edicao') + '] #' + resultado[i].campo, resultado[i].error)
        }
        return;
    }

    tr.removeAttr('edicao');
    tr.find('input')
        .attr('disabled', 'disabled');

    button.html('Alterar');
    button.removeClass('btn-success');
    button.addClass('btn-primary');
    button.attr('onclick', 'alterarBeneficario(event)');
}

function obterBeneficiarios(ultimosValores = Boolean) {
    let beneficiarios = [];

    $('#tableBeneficiarios tbody tr').each((ind, obj) => {
        obj = $(obj);
        let cpf = obj.find('#CPF')
        let nome = obj.find('#Nome');
        let benef = new Beneficiario(cpf.val(), nome.val());

        if (obj
            .find('input')
            .attr('disabled') == undefined &&
            ultimosValores)
        {
            benef = new Beneficiario(cpf.data('last-value'), nome.data('last-value'))
        }

        beneficiarios.push(benef);
    });

    return beneficiarios;
}

function removerBeneficario(event) {
    $(event.target)
        .closest('tr')
        .remove();
}

function alterarBeneficario(event) {
    let tr = $(event.target).closest('tr');
    let cpf = tr.find('#CPF');
    let nome = tr.find('#Nome');
    let button = tr.find('#btnAlterar');

    tr.find('input')
        .removeAttr('disabled');

    tr.attr('edicao', $('#modalBeneficiarios tr[edicao]').length);

    cpf.data('last-value', cpf.val());
    nome.data('last-value', nome.val());

    button.html('Salvar');
    button.removeClass('btn-primary');
    button.addClass('btn-success');
    button.attr('onclick', 'salvarAlteracaoBeneficiario(event)');
}

function adicionarBeneficiario(benef = Beneficiario) {
    let tdBotoes = $('<td class="row"/>').html(
        '<button id="btnAlterar" type="button" class="btn btn-sm btn-primary" onclick="alterarBeneficario(event)">Alterar</button>' +
        '<button id="btnExcluir" type="button" class="btn btn-sm btn-primary" onclick="removerBeneficario(event)">Excluir</button>');
    let tBody = $('#tableBeneficiarios tbody');
    let tr = $('<tr/>');

    tr.append(criarColunaCpf(benef));
    tr.append(criarColunaNome(benef));
    tr.append(tdBotoes);

    tBody.append(tr);
}

function criarColunaCpf(benef = Beneficiario) {
    let td = defaultTd();
    let input = defaultInput();

    input.attr('id', 'CPF');
    input.attr('type', 'cpf');
    input.attr('maxlength', '14');
    input.attr('placeholder', 'Ex.: 000.000.000-00');
    input.on('keyup', cpf);
    input.val(benef.cpf);

    td.find('.col-md-12 .form-group')
        .append(input);
    return td;
}

function criarColunaNome(benef = Beneficiario) {
    let td = defaultTd();
    let input = defaultInput();

    input.attr('id', 'Nome');
    input.attr('type', 'text');
    input.attr('maxlength', '50');
    input.attr('placeholder', 'Ex.: João');
    input.val(benef.nome);

    td.find('.col-md-12 .form-group')
        .append(input);
    return td;
}

var defaultTd = () =>
    $('<td/>')
        .append($('<div class="col-md-12"/>')
            .append($('<div class="form-group"/>')));

const defaultInput = () => $('<input class="form-control" disabled/>');

class Beneficiario {
    constructor(cpf, nome) {
        this.cpf = cpf;
        this.nome = nome;
    }

    async validar() {
        await this.validar(true);
    }

    async validar(ultimosValores = Boolean) {
        let errors = [];
        let beneficiarios = obterBeneficiarios(ultimosValores);

        if (this.cpf.length < 1) {
            errors.push({ campo: "CPF", error: "O Campo é obrigatório!" })
        }

        if (this.nome.length < 1) {
            errors.push({ campo: "Nome", error: "O Campo é obrigatório!" })
        }

        if (this.cpf.length < 14) {
            errors.push({ campo: 'CPF', error: 'O CPF deve seguir o formato: 000.000.000-00' })
        }

        for (var i = 0; i < beneficiarios.length && errors.length < 1; i++) {
            if (beneficiarios[i].equals(this)) {
                errors.push({ campo: 'CPF', error: 'Não devem existir dois beneficiários com o mesmo CPF.' })
            }
        }

        if (errors.length > 0) {
            return errors;
        }

        let response = await $.get('../../Cliente/CpfValido?cpf=' + encodeURIComponent(this.cpf));

        if (response.valido == false) {
            errors.push({ campo: 'CPF', error: response.error })
        }

        return errors;
    }

    equals(benef = Beneficiario) {
        let thisCpf = this.cpf.replace(/[^\d]/g, "");

        let benefCpf = benef.cpf.replace(/[^\d]/g, "");

        return thisCpf == benefCpf;
    }
}