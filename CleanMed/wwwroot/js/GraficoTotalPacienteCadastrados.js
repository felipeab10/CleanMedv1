$(document).ready(function () {

    $.ajax({
        url: "/Dashboard/GraficoTotalPacienteCadastrados",
        method: "POST",
        success: function (data) {
            $("canvas#GraficoTotalPacienteCadastrados").remove();
            $("div.GraficoTotalPacienteCadastrados").append('<canvas id="GraficoTotalPacienteCadastrados"></canvas>');
            //alert(data);
            var ctx = document.getElementById('GraficoTotalPacienteCadastrados').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ['Total de Pacientes Cadastrados'],
                    datasets: [{
                        label: 'Total',
                        data: [data],
                        backgroundColor: [
                            '#009688',
                       
                        ],
                        borderColor: [
                            '#048E81',
                       
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }

    })





});

