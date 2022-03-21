async function incluirBeneficiario() {
    let modal = $('#modalBeneficiarios');
    let benef = new Beneficiario(modal.find('#CPF').val(), modal.find('#Nome').val());
    let resultado = await benef.validar();

    if (resultado.length > 0) {
        for (var i = 0; i < resultado.length; i++) {
            adicionarError('#modalBeneficiarios #' + resultado[i].campo, resultado[i].error)
        }
        return;
    }
}

function adicionarError(exp, error) {
    console.log(exp);
    console.log(error);
}

class Beneficiario {
    constructor(cpf, nome) {
        this.cpf = cpf;
        this.nome = nome;
    }

    async validar() {
        let errors = [];

        if (this.cpf.length < 1) {
            errors.push({ campo: "CPF", error: "O Campo é obrigatório!" })
        }

        if (this.nome.length < 1) {
            errors.push({ campo: "Nome", error: "O Campo é obrigatório!" })
        }

        if (this.cpf.length < 14) {
            errors.push({ campo: 'CPF', error: 'O CPF deve seguir o formato: 000.000.000-00' })
        }

        if (errors.length > 0) {
            return errors;
        }

        let response = await $.get('../CpfValido?cpf=' + encodeURIComponent(this.cpf));

        if (response.valido == false) {
            errors.push({ campo: 'CPF', error: 'O CPF informado não é válido!' })
        }

        return errors;
    }
}