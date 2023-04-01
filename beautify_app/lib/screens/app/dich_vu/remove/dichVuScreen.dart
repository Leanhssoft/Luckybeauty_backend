import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/loai_dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/service/dichVuService.dart';
import 'package:flutter/material.dart';
import 'package:data_table_2/data_table_2.dart';
import 'dichVuDataSource.dart';

class DichVuScreen extends StatefulWidget {
  const DichVuScreen({super.key});

  @override
  State<DichVuScreen> createState() => _DichVuScreenState();
}

class _DichVuScreenState extends State<DichVuScreen> {
  List<dynamic> _data = [];
  List<LoaiDichVuDto> _loaiDichVu = [];
  int _rowsPerPage = PaginatedDataTable.defaultRowsPerPage;
  int _sortColumnIndex = 0;
  bool _sortAscending = true;
  Future<void> _loadData() async {
    var data = await DichVuService().getLoaiDichVu();
    var loaiDichVu = await DichVuService().getLoaiDichVu();
    setState(() {
      // _data = data;
      _loaiDichVu = loaiDichVu;
    });
  }

  @override
  void initState() {
    _loadData();
    super.initState();
  }

  TextEditingController _search = new TextEditingController();
  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
          body: SafeArea(
        child: SingleChildScrollView(
          child: Column(children: [
            Padding(
              padding: const EdgeInsets.all(2),
              child: Container(
                height: 90,
                decoration: const BoxDecoration(
                  color: Colors.white,
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: const [
                          Text("Trang chủ", style: TextStyle(fontSize: 20)),
                          Text(
                            "Tổng quan",
                            style: TextStyle(fontSize: 28),
                          ),
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(16),
                      child: Row(
                        children: [
                          SizedBox(
                            width: 40,
                            height: 40,
                            child: Container(
                              decoration: BoxDecoration(
                                  borderRadius: BorderRadius.circular(5),
                                  color: Colors.white,
                                  border: Border.all(
                                      color: const Color(0xFFD0D5DD))),
                              child: IconButton(
                                icon: const Icon(Icons.menu),
                                onPressed: () {},
                              ),
                            ),
                          ),
                          Padding(
                            padding:
                                const EdgeInsets.only(right: 8.0, left: 8.0),
                            child: SizedBox(
                              height: 40,
                              child: ElevatedButton.icon(
                                onPressed: () {},
                                icon: const Icon(Icons.add),
                                label: const Text("Thêm"),
                                style: ElevatedButton.styleFrom(
                                  backgroundColor:
                                      const Color(0xFF7C3367), // background
                                  foregroundColor: Colors.white, // foreground
                                ),
                              ),
                            ),
                          )
                        ],
                      ),
                    )
                  ],
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.all(2),
              child: Container(
                height: MediaQuery.of(context).size.height - 90,
                decoration: const BoxDecoration(
                  color: Color(0xFF7C3367),
                ),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Expanded(
                        child: Container(
                      color: const Color(0xFFFFFFFF),
                      child: Column(children: [
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            const Padding(
                              padding: EdgeInsets.only(left: 16.0),
                              child: Text("Nhóm dịch vụ"),
                            ),
                            Padding(
                              padding: const EdgeInsets.only(right: 8.0),
                              child: IconButton(
                                  onPressed: () {},
                                  icon: const Icon(Icons.add)),
                            )
                          ],
                        ),
                        const Divider(),
                        for (var item in _loaiDichVu)
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(item.tenLoai.toString()),
                              if (item.dichVus?.isNotEmpty == false)
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    for (var child in item.dichVus!)
                                      Padding(
                                        padding:
                                            const EdgeInsets.only(left: 8.0),
                                        child: Text(child),
                                      ),
                                  ],
                                ),
                            ],
                          )
                      ]),
                    )),
                    Expanded(
                        flex: 3,
                        child: Container(
                          color: const Color(0xFFFFFFFF),
                          child: PaginatedDataTable2(
                            rowsPerPage: _rowsPerPage,
                            sortColumnIndex: _sortColumnIndex,
                            sortAscending: _sortAscending,
                            availableRowsPerPage: const [5, 10, 20],
                            onPageChanged: (int rowIndex) {
                              print('Page changed: $rowIndex');
                            },
                            onRowsPerPageChanged: (int? value) {
                              setState(() {
                                _rowsPerPage = value ??
                                    PaginatedDataTable.defaultRowsPerPage;
                              });
                            },
                            showCheckboxColumn: true,
                            headingRowColor: MaterialStateColor.resolveWith(
                                (states) => Color(0xFFB2AFB2)),
                            columns: const [
                              DataColumn(label: Text('STT'), numeric: true),
                              DataColumn(label: Text('Tên'), numeric: true),
                              DataColumn(label: Text('Nhóm'), numeric: true),
                              DataColumn(label: Text('Giá'), numeric: true),
                              DataColumn(
                                  label: Text('Thời gian'), numeric: true),
                              DataColumn(
                                  label: Text('Trạng thái'), numeric: true),
                            ],
                            source: DichVuTableSource(data: _data),
                          ),
                        ))
                  ],
                ),
              ),
            ),
          ]),
        ),
      )),
    );
  }
}

List<Widget> loaiDichVuItems(List<LoaiDichVuDto> items) {
  List<Widget> widgets = [];
  for (var item in items) {
    List<Widget> children = [
      Text(item.tenLoai.toString()),
    ];
    if (item.dichVus!.isNotEmpty) {
      children.addAll(item.dichVus!.toList().map((child) {
        return Padding(
          padding: const EdgeInsets.only(top: 1, bottom: 1, right: 5, left: 10),
          child: Text(child),
        );
      }));
    }
    widgets.add(Column(
      children: children,
    ));
  }
  return widgets;
}
