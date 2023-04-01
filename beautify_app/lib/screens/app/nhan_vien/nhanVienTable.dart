import 'package:beautify_app/components/CustomDeleteDialog.dart';
import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuFilter.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuItemDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanVienHeader.dart';
import 'package:beautify_app/screens/app/nhan_vien/services/nhanVienServices.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class NhanVienTable extends StatefulWidget {
  const NhanVienTable({super.key});

  @override
  State<NhanVienTable> createState() => _NhanVienTableState();
}

class _NhanVienTableState extends State<NhanVienTable> {
  bool checkAll = false;
  int _currentPage = 1;
  int perPage = 10;
  String searchText = '';
  List<NhanSuItemDto> _data = [];
  Future<void> _loadData() async {
    NhanSuFilter input = NhanSuFilter(
        keyWord: searchText,
        skipCount: _currentPage == 1 ? 0 : _currentPage * perPage,
        maxResultCount: perPage);
    var data = await NhanVienService().getAllNhanVien(input);
    setState(() {
      _data = data;
    });
  }

  @override
  void initState() {
    super.initState();
    _currentPage = 1;
    _loadData();
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
                  flex: 1,
                  child: Padding(
                    padding: const EdgeInsets.only(left: 16, top: 8, bottom: 8),
                    child: SizedBox(
                      height: 32,
                      width: 194,
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
                    flex: 2,
                    child: Padding(
                        padding: const EdgeInsets.only(right: 16),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.end,
                          children: [
                            Padding(
                              padding: const EdgeInsets.all(4.0),
                              child: Container(
                                decoration:
                                    const BoxDecoration(color: Colors.white),
                                width: 32,
                                height: 32,
                                child: IconButton(
                                  style: const ButtonStyle(
                                    backgroundColor:
                                        MaterialStatePropertyAll(Colors.white),
                                  ),
                                  onPressed: () {},
                                  iconSize: 16,
                                  icon: const Icon(
                                    Icons.filter_alt_rounded,
                                    color: Color(0xFF666466),
                                    size: 16,
                                  ),
                                ),
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.all(4),
                              child: Container(
                                height: 32,
                                width: 85,
                                child: ElevatedButton.icon(
                                  style: ElevatedButton.styleFrom(
                                    backgroundColor: Colors.white,
                                  ),
                                  onPressed: () {},
                                  icon: const Icon(
                                    Icons.download_rounded,
                                    color: Color(0xFF666466),
                                    size: 16,
                                  ),
                                  label: Text(
                                    "Nhập",
                                    style: GoogleFonts.roboto(
                                      color: const Color(0xFF666466),
                                      fontSize: 12,
                                    ),
                                  ),
                                ),
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.all(4.0),
                              child: Container(
                                height: 32,
                                width: 85,
                                child: ElevatedButton.icon(
                                  style: ElevatedButton.styleFrom(
                                    backgroundColor: Colors.white,
                                  ),
                                  onPressed: () {},
                                  icon: const Icon(
                                    Icons.upload,
                                    color: Color(0xFF666466),
                                    size: 16,
                                  ),
                                  label: Text(
                                    "Xuất",
                                    style: GoogleFonts.roboto(
                                      color: const Color(0xFF666466),
                                      fontSize: 12,
                                    ),
                                  ),
                                ),
                              ),
                            ),
                          ],
                        )))
              ],
            ),
          ),
          Container(
            alignment: Alignment.topCenter,
            padding: const EdgeInsets.all(2),
            height: MediaQuery.of(context).size.height - 270,
            width: MediaQuery.of(context).size.width,
            child: Scrollbar(
              thumbVisibility: true,
              controller: _scrollController,
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                controller: _scrollController,
                child: Row(
                  children: [
                    SingleChildScrollView(
                      scrollDirection: Axis.vertical,
                      child: Column(
                        children: [
                          DataTable(
                            dividerThickness: 1,
                            headingTextStyle: const TextStyle(
                              color: Color(0xFFB2AFB2),
                            ),
                            columns: viewColumn,
                            rows: dataRows(_data, context),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
          Container(
            height: 48,
            decoration: BoxDecoration(
                color: const Color(0xFFF2EBF0),
                borderRadius: BorderRadius.circular(2)),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                const Spacer(
                  flex: 2,
                ),
                Expanded(
                  flex: 1,
                  child: Row(
                    children: [
                      Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: Text(
                            "Hiển thị ${(_currentPage * perPage) - 9}-${_currentPage * perPage} của ${_data.length} mục",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF666466), fontSize: 14)),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(8.0),
                        child: CustomPaginator(
                          itemCount: _data.length,
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
            ),
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

List<DataRow> dataRows(List<NhanSuItemDto> items, BuildContext buildContext) {
  int i = 0;
  List<DataRow> dataRow = [];
  for (var item in items) {
    i += 1;
    DataRow row = DataRow(
      cells: [
        DataCell(
          Container(
            alignment: Alignment.center,
            child: Text(
              i.toString(),
              textAlign: TextAlign.center,
            ),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text(item.tenNhanVien.toString()),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text(item.soDienThoai.toString()),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text(item.gioiTinh == 0
                ? 'Nam'
                : item.gioiTinh == 1
                    ? 'Nữ'
                    : 'Khác'),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text(item.tenChucVu.toString()),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text(item.ngaySinh.toString()),
          ),
        ),
        DataCell(
          Container(
            alignment: Alignment.centerLeft,
            child: Text("Hoạt động"),
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
                    onPressed: () {
                      showDialog(
                          context: buildContext,
                          builder: (BuildContext context) {
                            return CreateOrEditNhanVienModal(
                              idNhanVien: item.id,
                            );
                          });
                    },
                    child: const Icon(Icons.edit),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(2.0),
                  child: ElevatedButton(
                    style: const ButtonStyle(
                        backgroundColor:
                            MaterialStatePropertyAll(Color(0xFFFF5252))),
                    onPressed: () {
                      showDialog(
                          context: buildContext,
                          builder: (BuildContext context) {
                            return CustomDeleteDialog(
                              onDelete: () async {
                                var isDelete = await NhanVienService()
                                    .deleteNhanSu(item.id.toString());
                                if (isDelete) {
                                  // ignore: use_build_context_synchronously
                                  ScaffoldMessenger.of(context).showSnackBar(
                                      const SnackBar(
                                          backgroundColor: Color(0xFF90CAF9),
                                          content:
                                              Text("Xóa dữ liệu thành công!")));
                                } else {
                                  // ignore: use_build_context_synchronously
                                  ScaffoldMessenger.of(context).showSnackBar(
                                      const SnackBar(
                                          backgroundColor:
                                              Color.fromARGB(255, 233, 53, 22),
                                          content: Text(
                                              "Có lỗ sảy ra vui lòng thử lại sau!")));
                                }
                                // ignore: use_build_context_synchronously
                                Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                      builder: (context) =>
                                          const NhanVienHeader()),
                                );
                              },
                            );
                          });
                    },
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
