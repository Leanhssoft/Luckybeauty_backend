import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class KhachHangTable extends StatefulWidget {
  const KhachHangTable({super.key});

  @override
  State<KhachHangTable> createState() => _KhachHangTableState();
}

class _KhachHangTableState extends State<KhachHangTable> {
  bool checkAll = false;
  List<String> khachHang = [
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

  @override
  Widget build(BuildContext context) {
    final ScrollController _scrollController = ScrollController();
    return SingleChildScrollView(
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
                    padding: const EdgeInsets.only(left: 16, top: 8, bottom: 8),
                    child: SizedBox(
                      height: 32,
                      child: TextField(
                        decoration: InputDecoration(
                            hintText: "Tìm kiếm...",
                            prefixIcon: const Icon(Icons.search),
                            border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(8))),
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
                            style: const ButtonStyle(
                                backgroundColor: MaterialStatePropertyAll(
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
                            style: const ButtonStyle(
                                backgroundColor: MaterialStatePropertyAll(
                                    Color(0xFFFFFFFF))),
                            onPressed: () {},
                            icon: const Icon(Icons.download_rounded,
                                color: Color(0xFF666466)),
                            label: Text(
                              "Nhập",
                              style: GoogleFonts.roboto(
                                  color: const Color(0xFF666466), fontSize: 12),
                            )),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(4.0),
                        child: ElevatedButton.icon(
                            style: const ButtonStyle(
                                backgroundColor: MaterialStatePropertyAll(
                                    Color(0xFFFFFFFF))),
                            onPressed: () {},
                            icon: const Icon(Icons.upload,
                                color: Color(0xFF666466)),
                            label: Text(
                              "Xuất",
                              style: GoogleFonts.roboto(
                                  color: const Color(0xFF666466), fontSize: 12),
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
                      headingTextStyle: const TextStyle(
                        color: Color(0xFFB2AFB2),
                      ),
                      columns: viewColumn,
                      rows: dataRows(khachHang),
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
                          "Hiển thị ${(_currentPage * perPage) - 9}-${_currentPage * perPage} của ${khachHang.length} mục",
                          style: GoogleFonts.roboto(
                              color: const Color(0xFF666466), fontSize: 14)),
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
    );
  }

  List<DataColumn> get viewColumn {
    return [
      const DataColumn(
          label: Center(
            child: Text(
              'STT',
              textAlign: TextAlign.center,
            ),
          ),
          numeric: true),
      const DataColumn(
        label: Center(
          child: Text(
            'Tên khách hàng',
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
            'Nhóm khách',
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
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Nhân viên phục vụ',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Tổng chi tiêu',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Cuộc hẹn gần nhất',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      const DataColumn(
        label: Center(
          child: Text(
            'Nguồn',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      const DataColumn(
        label: Center(
          child: Text(
            'Hành động',
            textAlign: TextAlign.center,
          ),
        ),
      ),
    ];
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
            child: const Text(
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
            child: const Text('VIP'),
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
            child: const Text('Lương đức mạnh'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('5.000.000'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: const Text('22-03-2023'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text(
              'Online',
              style: GoogleFonts.roboto(
                  color: const Color(0xFF009EF7), fontSize: 12),
            ),
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
