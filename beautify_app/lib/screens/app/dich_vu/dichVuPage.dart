// ignore_for_file: prefer_const_constructors

import 'dart:developer';

import 'package:beautify_app/Models/comon_model.dart';
import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/customer/customerHeader.dart';
import 'package:beautify_app/screens/app/customer/customerTable.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/loai_dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuHeader.dart';
import 'package:beautify_app/screens/app/dich_vu/dichVuTable.dart';
import 'package:beautify_app/screens/app/dich_vu/service/dichVuService.dart';
import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:beautify_app/constants/styles.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_filter.dart';
// import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';

class DichVuPage extends StatefulWidget {
  const DichVuPage({super.key});

  @override
  State<DichVuPage> createState() => _DichVuPageState();
}

class _DichVuPageState extends State<DichVuPage> {
  List<DichVuViewModel> _data = [];
  List<LoaiDichVuDto> _loaiDichVu = [];
  int _rowsPerPage = PaginatedDataTable.defaultRowsPerPage;
  int _sortColumnIndex = 0;
  bool _sortAscending = true;

  Future<void> _loadData() async {
    final input = DichVuFilter('', ParamSearch('', 0, 10, '', ''));
    var data = await DichVuService().getDichVu(input);
    var loaiDichVu = await DichVuService().getLoaiDichVu();
    setState(() {
      _data = data;
      _loaiDichVu = loaiDichVu;
    });
  }

  @override
  void initState() {
    _loadData();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    double screenHeight = MediaQuery.of(context).size.height;
    double screenWidth = MediaQuery.of(context).size.width;

    var title = Expanded(
      flex: 1,
      child: Container(
        alignment: Alignment.center,
        color: Colors.red,
        padding: EdgeInsets.all(8),
        child: Row(
          children: [
            Expanded(
              child: const Text(
                'Danh mục dịch vụ',
                style: TextStyle(fontSize: 20),
              ),
            ),
            SizedBox(
              height: 40,
              width: 40,
              child: IconButton(
                icon: Icon(
                  Icons.menu,
                  color: ClassAppColor.iconColor,
                ),
                onPressed: () {},
                style: const ButtonStyle(
                  backgroundColor:
                      MaterialStatePropertyAll(ClassAppColor.bgSecondBtnColor),
                ),
              ),
            ),
            SizedBox(
              height: 40,
              width: 100,
              child: ElevatedButton.icon(
                onPressed: () {},
                icon: Icon(Icons.add),
                label: Text('Thêm'),
                style: ButtonStyle(
                  backgroundColor:
                      MaterialStateProperty.all(ClassAppColor.bgMainBtnColor),
                ),
              ),
            ),
          ],
        ),
      ),
    );

    var partSearch = Container(
      padding: EdgeInsets.all(8),
      child: Row(
        children: [
          Expanded(
            child: SizedBox(
              width: screenWidth * 0.5, // notworking in expand
              child: ConstrainedBox(
                constraints: BoxConstraints(maxWidth: screenWidth * 0.5),
                child: TextField(
                  decoration: InputDecoration(
                    prefixIcon: Icon(Icons.search),
                    hintText: 'Tìm kiếm',
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(8),
                    ),
                  ),
                ),
              ),
            ),
          ),
          SizedBox(
            width: 40,
            child: IconButton(
              onPressed: () {},
              icon: Icon(
                Icons.filter_alt_rounded,
                color: ClassAppColor.iconColor,
              ),
              style: const ButtonStyle(
                backgroundColor:
                    MaterialStatePropertyAll(ClassAppColor.bgSecondBtnColor),
              ),
            ),
          ),
          SizedBox(
            width: 116, // 100+16
            child: Padding(
              padding: EdgeInsets.all(8),
              child: ElevatedButton.icon(
                onPressed: () {},
                icon: Icon(
                  Icons.download_rounded,
                  color: ClassAppColor.iconColor,
                ),
                label: Text(
                  'Nhập',
                  style: TextStyle(color: ClassAppColor.iconColor),
                ),
                style: ElevatedButton.styleFrom(
                    backgroundColor: ClassAppColor.bgSecondBtnColor),
              ),
            ),
          ),
          SizedBox(
            width: 100,
            child: ElevatedButton.icon(
              onPressed: () {},
              icon: Icon(
                Icons.upload,
                color: ClassAppColor.iconColor,
              ),
              label: Text(
                'Xuất',
                style: TextStyle(color: ClassAppColor.iconColor),
              ),
              style: ElevatedButton.styleFrom(
                  backgroundColor: ClassAppColor.bgSecondBtnColor),
            ),
          ),
        ],
      ),
    );
    var body = Expanded(
      flex: 11,
      child: Container(
        alignment: Alignment.center,
        color: Colors.yellow,
        child: Row(
          textDirection: TextDirection.ltr,
          children: [
            // left content
            SizedBox(
              height: screenHeight * 0.4,
              width: 300,
              child: Container(
                color: Colors.amber,
                padding: EdgeInsets.only(left: 8),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // titile nhom
                    SizedBox(
                      height: 60,
                      child: Container(
                        color: Colors.amberAccent,
                        padding: EdgeInsets.all(8),
                        child: Row(
                          children: const [
                            Expanded(
                              child: Text(
                                'Nhóm dịch vụ',
                                style: TextStyle(fontSize: 15),
                              ),
                            ),
                            Icon(Icons.add),
                          ],
                        ),
                      ),
                    ),
                    // list nhomDV
                    Expanded(
                      child: Container(
                        color: Colors.greenAccent,
                      ),
                    ),
                  ],
                ),
              ),
            ),
            // right content
            Expanded(
              child: Container(
                color: ClassAppColor.bgTitleColor,
                child: Column(
                  children: [
                    // icon search
                    SizedBox(
                      height: 60,
                      child: partSearch,
                    ),
                    // lst Ds
                    Expanded(
                      child: Container(
                        color: Colors.deepPurpleAccent,
                      ),
                    )
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: Column(
            children: [
              title,
              body,
            ],
          ),
        ),
      ),
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
