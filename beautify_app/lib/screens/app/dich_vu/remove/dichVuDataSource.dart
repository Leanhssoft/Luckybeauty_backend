import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';
import 'package:flutter/material.dart';

class DichVuTableSource extends DataTableSource {
  final List<dynamic> data;

  DichVuTableSource({required this.data});

  @override
  DataRow getRow(int index) {
    final DichVuViewModel item = data[index];
    return DataRow(cells: [
      DataCell(Text(item.id)),
      DataCell(Text(item.maHangHoa.toString())),
      DataCell(Text(item.tenHangHoa)),
      DataCell(Text(item.loaiHangHoa)),
      DataCell(Text(item.giaBan)),
      DataCell(Text(item.trangThaiText)),
    ]);
  }

  @override
  bool get isRowCountApproximate => false;

  @override
  int get rowCount => data.length;

  @override
  int get selectedRowCount => 0;
}
