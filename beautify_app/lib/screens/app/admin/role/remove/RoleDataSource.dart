import 'package:beautify_app/screens/app/admin/role/models/RoleDto.dart';
import 'package:beautify_app/screens/app/admin/role/models/permissionViewModel.dart';
import 'package:beautify_app/screens/app/dich_vu/service/dichVuService.dart';
import 'package:flutter/material.dart';


class RoleTableSource extends DataTableSource {
  final List<RoleDto> data;

  RoleTableSource({required this.data});

  @override
  DataRow getRow(int index) {
    final RoleDto item = data[index];
    return DataRow(
      selected: true,
      cells: [
      DataCell(Text((index+1).toString())),
      //DataCell(Text(item.id.toString())),
      DataCell(Text(item.name.toString())),
      DataCell(Text(item.displayName)),
      DataCell(Text(item.description.toString())),
      DataCell(Row(children: [
        IconButton(onPressed: (){
          
        }, icon:const Icon(Icons.edit))
      ],))
    ]);
  }

  @override
  bool get isRowCountApproximate => false;

  @override
  int get rowCount => data.length;

  @override
  int get selectedRowCount => 0;
}