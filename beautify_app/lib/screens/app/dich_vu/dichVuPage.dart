// ignore_for_file: prefer_const_constructors

import 'package:beautify_app/Models/comon_model.dart';
import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/loai_dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/nhom_dich_vu_model.dart';
import 'package:beautify_app/screens/app/dich_vu/service/dichVuService.dart';
import 'package:flutter/material.dart';
import 'package:beautify_app/constants/styles.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/dich_vu_filter.dart';
import 'package:syncfusion_flutter_datagrid/datagrid.dart';

class DichVuPage extends StatefulWidget {
  const DichVuPage({super.key});

  @override
  State<DichVuPage> createState() => _DichVuPageState();
}

class _DichVuPageState extends State<DichVuPage> {
  List<DichVuViewModel> _data = [];
  late DichVuDataSource _dvDataSource = DichVuDataSource(products: []);

  List<LoaiDichVuDto> _loaiDichVu = [];
  List<NhomDichVuDto> _nhomDichVu = [];
  // int _rowsPerPage = PaginatedDataTable.defaultRowsPerPage;
  // int _sortColumnIndex = 0;
  // bool _sortAscending = true;

  Future<void> _loadData() async {
    final input = DichVuFilter('', ParamSearch('', 0, 10, '', ''));
    List<DichVuViewModel> data = await DichVuService().getDichVu(input);
    var loaiDichVu = await DichVuService().getLoaiDichVu();
    var nhomDichVu = await DichVuService().getNhomDichVu();
    setState(() {
      _data = data;
      _dvDataSource = DichVuDataSource(products: _data);
      _loaiDichVu = loaiDichVu;
      _nhomDichVu = nhomDichVu;
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

    late Map<String, double> columnWidths = {
      'maHangHoa': double.nan,
      'tenHangHoa': double.nan,
      'giaBan': double.nan,
      'tenNhomHang': double.nan,
      'soPhutThucHien': double.nan,
      'txtTrangThaiHang': double.nan,
    };

    var title = Expanded(
      flex: 1,
      child: Container(
        alignment: Alignment.center,
        color: ClassAppColor.bgApp,
        padding: EdgeInsets.all(8),
        child: Row(
          children: [
            Expanded(
              child: const Text(
                'Danh mục dịch vụ',
                style: TextStyle(fontSize: 20),
              ),
            ),
            Padding(
              padding: EdgeInsets.fromLTRB(8, 0, 8, 0),
              child: SizedBox(
                height: 50,
                width: 50,
                child: Container(
                  padding: EdgeInsets.only(bottom: 8),
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    color: Colors.red,
                    borderRadius: BorderRadius.circular(5),
                    border: Border.all(color: ClassAppColor.boderBtnColor),
                  ),
                  child: IconButton(
                    icon: Icon(
                      Icons.menu,
                      color: ClassAppColor.iconColor,
                    ),
                    onPressed: () {},
                    style: IconButton.styleFrom(
                      backgroundColor: Colors.yellow,
                    ),
                  ),
                ),
              ),
            ),
            SizedBox(
              height: 50,
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
              width: screenWidth * 0.3, // notworking in expand
              child: ConstrainedBox(
                constraints: BoxConstraints(maxWidth: 300),
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
                          mainAxisAlignment: MainAxisAlignment.start,
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
                        child: ConstrainedBox(
                          constraints: BoxConstraints(maxHeight: 400),
                          child: ListView.builder(
                            itemCount: _nhomDichVu.length,
                            itemBuilder: (_, index) {
                              return GestureDetector(
                                onTap: () {
                                  for (var i = 0; i < _nhomDichVu.length; i++) {
                                    setState(() {
                                      if (index == i) {
                                        _nhomDichVu[index].isSelected = true;
                                      } else {
                                        //the condition to change the highlighted item
                                        _nhomDichVu[i].isSelected = false;
                                      }
                                    });
                                  }
                                },
                                child: Container(
                                  color: _nhomDichVu[index].isSelected
                                      ? Colors.blueAccent
                                      : Colors.white,
                                  child: ListTile(
                                    // leading: Icon(Icons.check),
                                    title: Text(_nhomDichVu[index].tenNhomHang),
                                    // trailing: Icon(Icons.check),
                                  ),
                                ),
                              );
                            },
                          ),
                        ),
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
                        color: ClassAppColor.bgApp,
                        padding: EdgeInsets.all(8),
                        child: SfDataGrid(
                          source: _dvDataSource,
                          allowSorting: true,
                          allowMultiColumnSorting: true,
                          allowColumnsResizing: true,
                          columnResizeMode: ColumnResizeMode.onResizeEnd,
                          columnWidthMode: ColumnWidthMode.auto,
                          onColumnResizeUpdate:
                              (ColumnResizeUpdateDetails details) {
                            setState(() {
                              columnWidths[details.column.columnName] =
                                  details.width;
                            });
                            return true;
                          },
                          columns: <GridColumn>[
                            GridColumn(
                              width: columnWidths['maHangHoa']!,
                              columnName: 'maHangHoa',
                              label: Container(
                                padding: EdgeInsets.all(10),
                                alignment: Alignment.center,
                                child: Text(
                                  'Mã dịch vụ',
                                  style: TextStyle(fontWeight: FontWeight.w500),
                                ),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['tenHangHoa']!,
                              columnName: 'tenHangHoa',
                              label: Container(
                                padding: EdgeInsets.all(10),
                                alignment: Alignment.center,
                                child: Text('Tên dịch vụ'),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['tenNhomHang']!,
                              columnName: 'tenNhomHang',
                              label: Container(
                                padding: EdgeInsets.all(10),
                                alignment: Alignment.center,
                                child: Text('Nhóm'),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['giaBan']!,
                              columnName: 'giaBan',
                              label: Container(
                                padding: EdgeInsets.all(10),
                                alignment: Alignment.center,
                                child: Text('Giá bán'),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['soPhutThucHien']!,
                              columnName: 'soPhutThucHien',
                              label: Container(
                                padding: EdgeInsets.all(10),
                                alignment: Alignment.center,
                                child: Text('Thời gian'),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['txtTrangThaiHang']!,
                              columnName: 'txtTrangThaiHang',
                              label: Container(
                                padding: EdgeInsets.all(10),
                                alignment: Alignment.center,
                                child: Text('Trạng thái'),
                              ),
                            ),
                          ],
                        ),
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
