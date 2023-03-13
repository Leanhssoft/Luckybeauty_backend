import 'package:flutter/material.dart';
import 'package:syncfusion_flutter_charts/charts.dart';

class MyLineChart extends StatelessWidget {
  final List<SalesData> chartData = [
    SalesData('Mon', 35),
    SalesData('Tue', 28),
    SalesData('Wend', 34),
    SalesData('Thur', 32),
    SalesData('Fri', 40),
    SalesData('Jun', 55),
    SalesData('Sat', 65),
    SalesData('Sun', 62)
  ];
  final List<SalesData> chartData2 = [
    SalesData('Mon', 25),
    SalesData('Tue', 28),
    SalesData('Wend', 34),
    SalesData('Thur', 22),
    SalesData('Fri', 30),
    SalesData('Jun', 85),
    SalesData('Sat', 75),
    SalesData('Sun', 52)
  ];
  @override
  Widget build(BuildContext context) {
    return SfCartesianChart(
      primaryXAxis: CategoryAxis(),
      series: <LineSeries<SalesData, String>>[
        LineSeries<SalesData, String>(
          dataSource: chartData,
          xValueMapper: (SalesData sales, _) => sales.week,
          yValueMapper: (SalesData sales, _) => sales.sales,
        ),
        LineSeries<SalesData, String>(
          dataSource: chartData2,
          xValueMapper: (SalesData sales, _) => sales.week,
          yValueMapper: (SalesData sales, _) => sales.sales,
        ),
      ],
    );
  }
}

class SalesData {
  final String week;
  final double sales;

  SalesData(this.week, this.sales);
}
