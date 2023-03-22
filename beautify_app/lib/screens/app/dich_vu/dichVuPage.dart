// ignore_for_file: prefer_const_constructors

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
import 'package:google_fonts/google_fonts.dart';

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
    //var data = await DichVuService().getDichVu();
    var loaiDichVu = await DichVuService().getLoaiDichVu();
    setState(() {
      //_data = data;
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
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: [
                const DichVuHeader(),
                Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Expanded(
                        child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                          Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
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
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
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
                        ])),
                    Expanded(flex: 3, child: DichVuTable()),
                  ],
                ),
              ],
            ),
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
