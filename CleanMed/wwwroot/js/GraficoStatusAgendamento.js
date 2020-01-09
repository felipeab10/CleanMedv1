function buscarChartStatusAgendamento(dataAgenda) {
   // alert(dataAgenda);
    var data = $('#DataAgenda').val();
   
    if (data != "") {
        data = $('#DataAgenda').val();
    
    } else {
        data = dataAgenda
      
    }
    $.ajax({
        url: "/Dashboard/GraficoStatusAgendamento",
        method: "POST",
        data: { dataAgenda: data   },
        success: function (dados) {
            $("canvas#GraficoStatusAgendamento").remove();
            $("div.GraficoStatusAgendamento").append('<canvas id="GraficoStatusAgendamento" style="width:300px; height:300px;margin-left:-40px; "></canvas>');

            var ctx = document.getElementById('GraficoStatusAgendamento').getContext('2d');

            var grafico = new Chart(ctx, {
                type: 'doughnut',
                data: {
                    labels: ['Livres','Agendados', 'Confirmados', 'Atendidos', 'Cancelados', 'Excluidos'],
                    datasets: [
                        {
                            label: "Total",
                            backgroundColor: ["#4caf50", "#2196f3", "#ff9800", "#212121", "#e53935","#dd2c00"],
                            data: [dados.livre, dados.agendados, dados.confirmados, dados.atendidos, dados.cancelados, dados.excluidos]
                        }
                    ]
                },
                options: {
                    responsive: false,
                    title: {
                        display: true,
                        text: "Total por tipo: " + data
                    }
                }
            });
        }
    })
}