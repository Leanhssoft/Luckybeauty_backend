import 'package:beautify_app/screens/app/admin/user/models/userDto.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class UserTableSource extends DataTableSource {
  final List<UserDto> data;

  UserTableSource({required this.data});

  @override
  DataRow getRow(int index) {
    final UserDto item = data[index];
    return DataRow(selected: true, cells: [
      DataCell(Container(
          alignment: Alignment.center, child: Text((index + 1).toString()))),
      DataCell(Text(item.userName.toString())),
      DataCell(Text(item.fullName)),
      DataCell(Text(item.roleNames.toString())),
      DataCell(Text(item.emailAddress)),
      DataCell(Text(
        item.isActive == true ? "Hoạt động" : "Đã khóa",
        style: GoogleFonts.roboto(
            fontSize: 14, color: Colors.blue.withOpacity(.7)),
      )),
      DataCell(Container(
        alignment: Alignment.center,
        child: Row(
          children: [
            IconButton(onPressed: () {}, icon: const Icon(Icons.edit)),
            IconButton(onPressed: () {}, icon: const Icon(Icons.delete))
          ],
        ),
      ))
    ]);
  }

  @override
  bool get isRowCountApproximate => false;

  @override
  int get rowCount => data.length;

  @override
  int get selectedRowCount => 0;
}
