$(document).ready(function () {
    $('.tooltipped').tooltip();
    $('select').formSelect();
    $('.timepicker').timepicker({
        i18n: {
            cancel: 'Cancelar',
            clear: 'Limpar',
            done: 'Ok'
        },
        twelveHour: false, // 12 horas, usa AM/PM
        autoclose: false  //Fecha o timepicker automaticamente apos selecionar a hora
    });
    $('.datepicker').datepicker({
        format: 'dd/mm/yyyy',
        autoClose: true,
        i18n: {
            today: 'Hoje',
            clear: 'Limpar',
            cancel: 'Cancelar',
            done: 'Ok',
            nextMonth: 'Próximo mês',
            previousMonth: 'Mês anterior',
            weekdaysAbbrev: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
            weekdaysShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
            weekdays: ['Domingo', 'Segunda-Feira', 'Terça-Feira', 'Quarta-Feira', 'Quinta-Feira', 'Sexta-Feira', 'Sábado'],
            monthsShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
            months: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro']
        }
    });
});