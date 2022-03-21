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

function obterBeneficiarios() {
    let beneficiarios = [];

    $('#tableBeneficiarios tbody tr').each((ind, obj) => {
        var obj = $(obj);

        beneficiarios.push(new Beneficiario(obj.find('#CPF').val(), obj.find('#Nome').val()));
    });

    return beneficiarios;
}

function adicionarBeneficiario(benef = Beneficiario) {
    let tdBotoes = $('<td class="row"/>').html(
        '<button type="button" class="btn btn-sm btn-primary">Alterar</button>' +
        '<button type="button" class="btn btn-sm btn-primary">Excluir</button>');
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
        let errors = [];
        let beneficiarios = obterBeneficiarios();

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
            if (beneficiarios[i].equals(this))
            {
                errors.push({ campo: 'CPF', error: 'Não devem existir dois beneficiários com o mesmo CPF.' })
            }
        }

        if (errors.length > 0) {
            return errors;
        }

        let response = await $.get('../CpfValido?cpf=' + encodeURIComponent(this.cpf));

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