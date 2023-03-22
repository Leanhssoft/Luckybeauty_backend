// ignore_for_file: prefer_const_constructors

import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class NhanVienScreen extends StatefulWidget {
  const NhanVienScreen({super.key});

  @override
  State<NhanVienScreen> createState() => _NhanVienScreenState();
}

class _NhanVienScreenState extends State<NhanVienScreen> {
  bool checkAll = false;
  List<String> nhanVien = [
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    "",
    ""
  ];
  int _currentPage = 1;
  int perPage = 10;
  @override
  void initState() {
    super.initState();
    _currentPage = 1;
  }

  void _handlePageChanged(int page) {
    setState(() {
      _currentPage = page;
    });
  }

  @override
  Widget build(BuildContext context) {
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: [
                NhanVienHeader(),
                SingleChildScrollView(
                  scrollDirection: Axis.vertical,
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Container(
                        height: 48,
                        decoration: BoxDecoration(
                            color: const Color(0xFFF2EBF0),
                            borderRadius: BorderRadius.circular(2)),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(
                                    left: 16, top: 8, bottom: 8),
                                child: SizedBox(
                                  height: 32,
                                  child: TextField(
                                    decoration: InputDecoration(
                                        hintText: "Tìm kiếm...",
                                        prefixIcon: const Icon(Icons.search),
                                        border: OutlineInputBorder(
                                            borderRadius:
                                                BorderRadius.circular(8))),
                                  ),
                                ),
                              ),
                            ),
                            const Spacer(),
                            Expanded(
                                child: Padding(
                              padding: const EdgeInsets.only(right: 16),
                              child: Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: [
                                  Padding(
                                    padding: const EdgeInsets.all(4.0),
                                    child: ElevatedButton.icon(
                                        style: ButtonStyle(
                                            backgroundColor:
                                                MaterialStatePropertyAll(
                                                    Color(0xFFFFFFFF))),
                                        onPressed: () {},
                                        icon: const Icon(
                                          Icons.filter_alt_outlined,
                                          color: Color(0xFF666466),
                                        ),
                                        label: const Text("")),
                                  ),
                                  Padding(
                                    padding: const EdgeInsets.all(4),
                                    child: Align(
                                      alignment: Alignment.center,
                                      child: ElevatedButton(
                                        onPressed: () {
                                          showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return const CreateOrEditNhanVienModal();
                                              });
                                        },
                                        child: const Text("Thêm mới"),
                                      ),
                                    ),
                                  ),
                                  Padding(
                                    padding: const EdgeInsets.all(4),
                                    child: ElevatedButton.icon(
                                        style: ButtonStyle(
                                            backgroundColor:
                                                MaterialStatePropertyAll(
                                                    Color(0xFFFFFFFF))),
                                        onPressed: () {},
                                        icon: const Icon(Icons.download_rounded,
                                            color: Color(0xFF666466)),
                                        label: Text(
                                          "Nhập",
                                          style: GoogleFonts.roboto(
                                              color: Color(0xFF666466),
                                              fontSize: 12),
                                        )),
                                  ),
                                  Padding(
                                    padding: const EdgeInsets.all(4.0),
                                    child: ElevatedButton.icon(
                                        style: ButtonStyle(
                                            backgroundColor:
                                                MaterialStatePropertyAll(
                                                    Color(0xFFFFFFFF))),
                                        onPressed: () {},
                                        icon: const Icon(Icons.upload,
                                            color: Color(0xFF666466)),
                                        label: Text(
                                          "Xuất",
                                          style: GoogleFonts.roboto(
                                              color: Color(0xFF666466),
                                              fontSize: 12),
                                        )),
                                  )
                                ],
                              ),
                            ))
                          ],
                        ),
                      ),
                      Container(
                        height: MediaQuery.of(context).size.height - 270,
                        width: MediaQuery.of(context).size.width,
                        child: SingleChildScrollView(
                          scrollDirection: Axis.horizontal,
                          child: SingleChildScrollView(
                            scrollDirection: Axis.vertical,
                            child: Column(
                              children: [
                                DataTable(
                                  dividerThickness: 1,
                                  headingTextStyle: TextStyle(
                                    color: Color(0xFFB2AFB2),
                                  ),
                                  columns: viewColumn,
                                  rows: dataRows(nhanVien),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          const Spacer(),
                          Expanded(
                            child: Row(
                              children: [
                                Padding(
                                  padding: const EdgeInsets.all(8.0),
                                  child: Text(
                                      "Hiển thị ${(_currentPage * perPage) - 9}-${_currentPage * perPage} của ${nhanVien.length} mục",
                                      style: GoogleFonts.roboto(
                                          color: Color(0xFF666466),
                                          fontSize: 14)),
                                ),
                                Padding(
                                  padding: const EdgeInsets.all(8.0),
                                  child: CustomPaginator(
                                    itemCount: 500,
                                    perPage: 10,
                                    pagesVisible: 5,
                                    onPageChanged: (curentPage) {
                                      setState(() {
                                        _currentPage = curentPage;
                                      });
                                    },
                                  ),
                                ),
                              ],
                            ),
                          )
                        ],
                      )
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  List<DataColumn> get viewColumn {
    return [
      DataColumn(
          label: Center(
            child: const Text(
              'STT',
              textAlign: TextAlign.center,
            ),
          ),
          numeric: true),
      DataColumn(
        label: Center(
          child: const Text(
            'Tên nhân viên',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Số điện thoại',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
          label: Container(
            alignment: Alignment.center,
            child: const Text(
              'Giới tính',
              textAlign: TextAlign.center,
            ),
          ),
          numeric: true),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Vị trí',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Ngày vào làm',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Trạng thái',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Center(
          child: const Text(
            'Hành động',
            textAlign: TextAlign.center,
          ),
        ),
      ),
    ];
  }
}

class NhanVienHeader extends StatelessWidget {
  const NhanVienHeader({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(4.0),
      child: SizedBox(
        height: 90,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      TextButton(
                          onPressed: () {},
                          child: Text(
                            "Trang chủ",
                            style: GoogleFonts.roboto(
                                color: Color(0xFF4C4B4C), fontSize: 14),
                          )),
                      Padding(
                        padding: const EdgeInsets.only(
                            right: 3, left: 3, top: 4, bottom: 4),
                        child: Text(
                          ">",
                          textAlign: TextAlign.center,
                        ),
                      ),
                      TextButton(
                          onPressed: () {},
                          child: Text(
                            "Nhân viên",
                            style: GoogleFonts.roboto(
                                color: Color(0xFF4C4B4C), fontSize: 14),
                          )),
                      Padding(
                        padding: const EdgeInsets.only(
                            right: 3, left: 3, top: 4, bottom: 4),
                        child: Text(">"),
                      ),
                      TextButton(
                          onPressed: () {},
                          child: Text(
                            "Thông tin nhân viên",
                            style: GoogleFonts.roboto(
                                color: Color(0xFF4C4B4C), fontSize: 14),
                          )),
                    ],
                  ),
                  Text(
                    "Nhân viên",
                    style: GoogleFonts.roboto(
                        color: Color(0xFF4C4B4C), fontSize: 32),
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
                          border: Border.all(color: const Color(0xFFD0D5DD))),
                      child: IconButton(
                        icon: const Icon(Icons.menu),
                        onPressed: () {},
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(right: 8.0, left: 8.0),
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
    );
  }
}

List<DataRow> dataRows(List<dynamic> items) {
  List<DataRow> dataRow = [];
  for (var item in items) {
    DataRow row = DataRow(
      cells: [
        DataCell(
          Container(
            alignment: Alignment.center,
            child: Text(
              '1',
              textAlign: TextAlign.center,
            ),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('Lương đức mạnh'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('0348016446'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('Nam'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('Admin'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('23/03/2023'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('Hoạt động'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.center,
            child: Row(
              children: [
                Padding(
                  padding: const EdgeInsets.all(2.0),
                  child: ElevatedButton(
                    onPressed: () {},
                    child: const Icon(Icons.remove_red_eye_outlined),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(2.0),
                  child: ElevatedButton(
                    onPressed: () {},
                    child: const Icon(Icons.edit),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(2.0),
                  child: ElevatedButton(
                    onPressed: () {},
                    child: const Icon(Icons.delete),
                  ),
                ),
              ],
            ),
          ),
        ),
      ],
    );

    dataRow.add(row);
  }
  return dataRow;
}
