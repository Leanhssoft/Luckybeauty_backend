import 'package:flutter/material.dart';

class UserScreen extends StatefulWidget {
  const UserScreen({super.key});

  @override
  State<UserScreen> createState() => _UserScreenState();
}

class _UserScreenState extends State<UserScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
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
              child: Column(
                children: <Widget>[
                  DataTable(
                    columnSpacing: 8.0,
                    horizontalMargin: 16.0,
                    columns: const [
                      DataColumn(
                          label: Center(
                              child:
                                  Text('STT', textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                              child:
                                  Text('Ảnh', textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                              child: Text('Mã NV',
                                  textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                              child: Text('Họ tên',
                                  textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                              child: Text('Giới tính',
                                  textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                              child: Text('Ngày sinh',
                                  textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                        child: Text('Số điện thoại',
                            textAlign: TextAlign.center),
                      )),
                      DataColumn(
                          label: Center(
                              child: Text('Email',
                                  textAlign: TextAlign.center))),
                      DataColumn(
                          label: Center(
                        child: Text('Ngày làm việc',
                            textAlign: TextAlign.center),
                      )),
                      DataColumn(
                          label: Center(
                        child:
                            Text('Trạng thái', textAlign: TextAlign.center),
                      )),
                      DataColumn(
                          label: Center(
                              child: Text('Hành động',
                                  textAlign: TextAlign.center))),
                    ],
                    rows: [
                      DataRow(cells: [
                        const DataCell(
                          SizedBox(width: 15, child: Text('1')),
                        ),
                        const DataCell(
                          SizedBox(width: 50, child: Text('Row 1, Column 1')),
                        ),
                        const DataCell(
                          SizedBox(width: 50, child: Text('NS000012')),
                        ),
                        const DataCell(
                          SizedBox(width: 150, child: Text('Lương Đức Mạnh')),
                        ),
                        const DataCell(
                          SizedBox(width: 30, child: Text('Nam')),
                        ),
                        const DataCell(
                          SizedBox(width: 80, child: Text('11-07-2001')),
                        ),
                        const DataCell(
                          SizedBox(width: 90, child: Text('09876453263')),
                        ),
                        const DataCell(
                          SizedBox(
                              width: 120,
                              child: Text('luongmanh3222@gmail.com')),
                        ),
                        const DataCell(
                          SizedBox(width: 80, child: Text('01-11-2022')),
                        ),
                        const DataCell(
                          SizedBox(width: 50, child: Text('Hoạt động')),
                        ),
                        DataCell(
                            Row(
                              children: [
                                IconButton(
                                    onPressed: () => {},
                                    icon: const Icon(Icons.edit)),
                                IconButton(
                                    onPressed: () => {},
                                    icon: const Icon(Icons.delete)),
                                IconButton(
                                    onPressed: () => {},
                                    icon: const Icon(Icons.details))
                              ],
                            ),
                            showEditIcon: false),
                      ]),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
