import 'package:flutter/material.dart';
import 'package:syncfusion_flutter_charts/charts.dart';

class MyColumnChart extends StatelessWidget {
  final List<SalesData> chartData = [
    SalesData('Jan', 35),
    SalesData('Feb', 28),
    SalesData('Mar', 34),
    SalesData('Apr', 32),
    SalesData('May', 40),
    SalesData('Jun', 55),
    SalesData('Jul', 65),
    SalesData('Aug', 62),
    SalesData('Sep', 48),
    SalesData('Oct', 38),
    SalesData('Nov', 30),
    SalesData('Dec', 25),
  ];

  @override
  Widget build(BuildContext context) {
    return SfCartesianChart(
      primaryXAxis: CategoryAxis(),
      series: <ColumnSeries<SalesData, String>>[
        
        ColumnSeries<SalesData, String>(
          spacing: 0.6,
          dataSource: chartData,
          xValueMapper: (SalesData sales, _) => sales.month,
          yValueMapper: (SalesData sales, _) => sales.sales,
        ),
      ],
    );
  }
}

class SalesData {
  final String month;
  final double sales;

  SalesData(this.month, this.sales);
}