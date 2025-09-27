let pieChart;

export function renderChart(tooltipLines) {
    const tooltipLinesJson = JSON.parse(tooltipLines);
    const data = {
        labels: ["North", "North East", "East", "South East", "South", "South West", "West", "North West"],
        datasets: [{
            data: [1, 1, 1, 1, 1, 1, 1, 1],
            backgroundColor: [
                'rgba(255, 99, 132, 0.6)',
                'rgba(54, 162, 235, 0.6)',
                'rgba(255, 206, 86, 0.6)',
                'rgba(75, 192, 192, 0.6)',
                'rgba(153, 102, 255, 0.6)',
                'rgba(255, 159, 64, 0.6)',
                'rgba(100, 100, 255, 0.6)',
                'rgba(200, 200, 100, 0.6)'
            ]
        }]
    };

    Chart.Tooltip.positioners.leftside = function (elements, eventPosition) {
        const chart = this.chart;
        return {
            x: chart.chartArea.left,
            y: chart.chartArea.bottom
        };
    };

    const options = {
        rotation: -7 * Math.PI,
        plugins: {
            tooltip: {
                position: 'leftside',
                callbacks: {
                    label: function (context) {
                        return tooltipLinesJson[context.dataIndex];
                    }
                }
            },
            legend: {
                position: 'right',
                onClick: null
            }
        }
    };

    const ctx = document.getElementById('pieChart').getContext('2d');
    pieChart = new Chart(ctx, {
        type: 'pie',
        data: data,
        options: options
    });
}

export function updateChart(tooltipLines) {
    const tooltipLinesJson = JSON.parse(tooltipLines);

    pieChart.options.plugins.tooltip.callbacks.label = function (context) {
        return tooltipLinesJson[context.dataIndex];
    };

    pieChart.update(); 
}