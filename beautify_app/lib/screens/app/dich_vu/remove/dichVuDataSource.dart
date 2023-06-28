import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';
import 'package:flutter/material.dart';
import 'package:syncfusion_flutter_datagrid/datagrid.dart';

class DichVuTableSource extends DataTableSource {
  final List<dynamic> data;

  DichVuTableSource({required this.data});

  @override
  DataRow getRow(int index) {
    final DichVuViewModel item = data[index];
    return DataRow(cells: [
      DataCell(Text(item.id)),
      // DataCell(Text(item.maHangHoa.toString())),
      // DataCell(Text(item.tenHangHoa)),
      // DataCell(Text(item.loaiHangHoa)),
      // DataCell(Text(item.giaBan)),
      // DataCell(Text(item.trangThaiText)),
    ]);
  }

  @override
  bool get isRowCountApproximate => false;

  @override
  int get rowCount => data.length;

  @override
  int get selectedRowCount => 0;
}
// class DichVuDataSource extends DataGridSource {
//   List<DataGridRow> dataGridRows = [];

//   DichVuDataSource({required List<DichVuViewModel> products}) {
//     dataGridRows = products
//         .map<DataGridRow>((dataGridRow) => DataGridRow(
//               cells: [
//                 DataGridCell<String>(
//                     columnName: 'maHangHoa', value: dataGridRow.maHangHoa),
//                 DataGridCell<String>(
//                     columnName: 'tenHangHoa', value: dataGridRow.tenHangHoa),
//                 DataGridCell<double>(
//                     columnName: 'giaBan', value: dataGridRow.giaBan),
//               ],
//             ))
//         .toList();
//   }
//   @override
//   List<DataGridRow> get rows => dataGridRows;

//   @override
//   DataGridRowAdapter? buildRow(DataGridRow row) {
//     return DataGridRowAdapter(
//         cells: row.getCells().map<Widget>((dataGridCell) {
//       return Container(
//           alignment: (dataGridCell.columnName == "id" ||
//                   dataGridCell.columnName == 'salary')
//               ? Alignment.centerRight
//               : Alignment.centerLeft,
//           padding: EdgeInsets.symmetric(horizontal: 16.0),
//           child: Text(
//             dataGridCell.value.toString(),
//             overflow: TextOverflow.ellipsis,
//           ));
//     }).toList());
//   }
// }
