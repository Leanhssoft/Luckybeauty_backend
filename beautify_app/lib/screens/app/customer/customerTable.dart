// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/screens/app/customer/customerScreen.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/components/CustomDeleteDialog.dart';
import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/components/CustomSuccessDialog.dart';
import 'package:beautify_app/screens/app/customer/Models/PagedKhachHangRequestDto.dart';
import 'package:beautify_app/screens/app/customer/Service/KhachHangServices.dart';
import 'package:beautify_app/screens/app/customer/create-or-edit-customer-modal.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';

import 'Models/KhachHangItemDto.dart';

class KhachHangTable extends StatefulWidget {
  final Function? refreshData;
  const KhachHangTable({
    Key? key,
    this.refreshData,
  }) : super(key: key);

  @override
  State<KhachHangTable> createState() => _KhachHangTableState();
}

class _KhachHangTableState extends State<KhachHangTable> {
  bool checkAll = false;
  List<KhachHangItemDto> _khachHang = [];
  int _currentPage = 1;
  int _perPage = 10;
  String _keyword = '';
  int totalCount = 0;
  @override
  void initState() {
    super.initState();
    _currentPage = 1;
    getData();
  }

  void getData() async {
    PagedKhachHangRequestDto input = PagedKhachHangRequestDto();
    input.keyword = _keyword;
    input.maxResultCount = _perPage;
    input.skipCount = _currentPage == 1 ? 0 : _currentPage;
    var data = await KhachHangServices().getAllKhachHang(input);
    setState(() {
      _khachHang = data.items;
      totalCount = data.totalCount;
    });
    widget.refreshData!();
  }

  void refreshData() async {
    PagedKhachHangRequestDto input = PagedKhachHangRequestDto();
    input.keyword = _keyword;
    input.maxResultCount = _perPage;
    input.skipCount = (_currentPage - 1) * _perPage;
    var data = await KhachHangServices().getAllKhachHang(input);
    setState(() {
      _khachHang = data.items;
      totalCount = data.totalCount;
    });
  }

  @override
  Widget build(BuildContext context) {
    final scrollController = ScrollController();
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
            padding: const EdgeInsets.all(2),
            height: MediaQuery.of(context).size.height - 270,
            child: Scrollbar(
              controller: scrollController,
              thumbVisibility: true,
              child: SingleChildScrollView(
                controller: scrollController,
                scrollDirection: Axis.horizontal,
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SingleChildScrollView(
                      scrollDirection: Axis.vertical,
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          DataTable(
                            dividerThickness: 1,
                            headingTextStyle: const TextStyle(
                              color: Color(0xFFB2AFB2),
                            ),
                            columns: viewColumn,
                            rows: dataRows(_khachHang, context, refreshData),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),
          Row(
            mainAxisAlignment: MainAxisAlignment.end,
            children: [
              const Spacer(
                flex: 3,
              ),
              Expanded(
                flex: 2,
                child: Row(
                  children: [
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: Text(
                          "Hiển thị ${(_currentPage * _perPage) - 9}-${_currentPage * _perPage} của $totalCount mục",
                          style: GoogleFonts.roboto(
                              color: const Color(0xFF666466), fontSize: 14)),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: CustomPaginator(
                        itemCount: totalCount,
                        perPage: _perPage,
                        pagesVisible: 5,
                        onPageChanged: (curentPage) {
                          if (curentPage * _perPage < totalCount) {
                            setState(() {
                              _currentPage = curentPage;
                            });
                          }
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

  List<DataRow> dataRows(List<KhachHangItemDto> items, BuildContext context,
      Function refreshData) {
    int i = 0;

    List<DataRow> dataRow = [];
    for (var item in items) {
      i += 1;
      DateTime cuocHenGanNhat = DateTime.parse(item.cuocHenGanNhat.toString());
      DataRow row = DataRow(
        cells: [
          DataCell(
            Container(
              alignment: Alignment.center,
              child: Checkbox(
                onChanged: (value) {},
                value: false,
              ),
            ),
          ),
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
              child: Text(item.tenKhachHang.toString()),
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
              child: Text(item.tenNhomKhach.toString()),
            ),
          ),
          DataCell(
            Container(
              alignment: Alignment.centerLeft,
              child: Text(item.gioiTinh.toString()),
            ),
          ),
          DataCell(
            Container(
              alignment: Alignment.centerLeft,
              child: Text(item.nhanVienPhuTrach.toString()),
            ),
          ),
          DataCell(
            Container(
              alignment: Alignment.centerLeft,
              child: Text(item.tongChiTieu.toString()),
            ),
          ),
          DataCell(
            Container(
              alignment: Alignment.centerLeft,
              child: Text(
                  "${cuocHenGanNhat.day}/${cuocHenGanNhat.month}/${cuocHenGanNhat.year} ${cuocHenGanNhat.hour}:${cuocHenGanNhat.minute}"),
            ),
          ),
          DataCell(
            Container(
              alignment: Alignment.centerLeft,
              child: Text(
                item.tenNguonKhach.toString(),
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
                      onPressed: () {
                        showDialog(
                          context: context,
                          builder: (context) => CreateOrEditCustomerModal(
                            idKhachHang: item.id.toString(),
                          ),
                        );
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
                          context: context,
                          builder: (BuildContext buildContext) {
                            return CustomDeleteDialog(
                              onDelete: () async {
                                var isDelete = await KhachHangServices()
                                    .deleteKhachHang(item.id.toString());
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
                                          const KhachHangScreen()),
                                ).then((value) => refreshData());
                              },
                            );
                          },
                        );
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

  List<DataColumn> get viewColumn {
    return [
      DataColumn(
          label: Center(
            child: Checkbox(
              value: false,
              onChanged: (value) {},
            ),
          ),
          numeric: true),
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
