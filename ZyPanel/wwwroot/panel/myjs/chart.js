function drawLineChart(el, chartData) {
    anychart.onDocumentReady(function () {
        let colors = ["#e63946", "#1d3557", "#ffbe0b", "#fca311",
            "#a2d2ff", "#edede9", "#6a040f", "#f72585", "#a98467"];
        if (chartData) {
            let title = el.getAttribute("data-title");
            let yTitle = el.getAttribute("data-y-title");
            let finalChart = [];
            chartData.Series.forEach((item) => {
                finalChart.push([item.DataTitle, ...item.DataValue.split(",")]);
            });

            let dataSet = anychart.data.set(finalChart);
            let chart = anychart.line();
            chart.title(title);
            chart.yAxis().title(yTitle);
            let tooltip = chart.tooltip();
            chart.tooltip().useHtml(true);
            tooltip.titleFormat("{%x}");
            tooltip.format("{%seriesName} : {%value}");

            chartData.SeriesTitle.split(',').forEach((item, index) => {
                let chooseColor = colors[index];
                let itemData = dataSet.mapAs({ x: 0, value: index + 1 });
                let itemSeries = chart.line(itemData);
                itemSeries.name(item);
                itemSeries.stroke(`1 ${chooseColor}`);
            });

            // set the container id for the line chart
            let elId = el.getAttribute("id");
            chart.container(elId);
            chart.legend().enabled(true);
            chart.animation(true);
            chart.draw();
        }
    });
}