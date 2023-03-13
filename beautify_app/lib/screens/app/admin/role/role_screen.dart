import 'package:flutter/material.dart';

class RoleScreen extends StatefulWidget {
  const RoleScreen({super.key});

  @override
  State<RoleScreen> createState() => _RoleScreenState();
}

class _RoleScreenState extends State<RoleScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Row(
                  children: [
                    InkWell(
                      onTap: () {
                        Navigator.pop(context);
                      },
                      child: Container(
                        padding: const EdgeInsets.all(10),
                        decoration: BoxDecoration(
                            color: Colors.grey.withOpacity(0.40),
                            borderRadius: BorderRadius.circular(30)),
                        child: const Icon(
                          Icons.arrow_back_ios_new,
                          size: 22,
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.all(16),
                child: Align(
                  alignment: Alignment.topRight,
                  child: ElevatedButton(
                    onPressed: () {
                      // showDialog(
                      //     context: context,
                      //     builder: (BuildContext context) {
                      //       return const CreateTenantModal(
                      //         headerModel: "Thêm mới Tenant",
                      //       );
                      //     });
                    },
                    child: const Text("Thêm mới"),
                  ),
                ),
              ),
              SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                child: Row(
                  children: <Widget>[
                    DataTable(
                      columnSpacing: 8.0,
                      horizontalMargin: 16.0,
                      columns: const [
                        DataColumn(label: Text('Column 1')),
                        DataColumn(label: Text('Column 2')),
                        DataColumn(label: Text('Column 3')),
                        DataColumn(label: Text('Column 4')),
                        DataColumn(label: Text('Column 5')),
                        DataColumn(label: Text('Column 6')),
                      ],
                      rows: const [
                        DataRow(cells: [
                          DataCell(Text('Row 1, Column 1')),
                          DataCell(Text('Row 1, Column 2')),
                          DataCell(Text('Row 1, Column 3')),
                          DataCell(Text('Row 1, Column 1')),
                          DataCell(Text('Row 1, Column 2')),
                          DataCell(Text('Row 1, Column 3')),
                        ]),
                        DataRow(cells: [
                          DataCell(Text('Row 2, Column 1')),
                          DataCell(Text('Row 2, Column 2')),
                          DataCell(Text('Row 2, Column 3')),
                          DataCell(Text('Row 2, Column 1')),
                          DataCell(Text('Row 2, Column 2')),
                          DataCell(Text('Row 2, Column 3')),
                        ]),
                        DataRow(cells: [
                          DataCell(Text('Row 3, Column 1')),
                          DataCell(Text('Row 3, Column 2')),
                          DataCell(Text('Row 3, Column 3')),
                          DataCell(Text('Row 3, Column 1')),
                          DataCell(Text('Row 3, Column 2')),
                          DataCell(Text('Row 3, Column 3')),
                        ]),
                      ],
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
