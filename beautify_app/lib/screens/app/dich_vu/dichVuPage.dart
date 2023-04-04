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
import 'package:http/http.dart';
import 'package:syncfusion_flutter_datagrid/datagrid.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:beautify_app/screens/app/dich_vu/modal_add_nhom_dich_vu.dart';

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
              child: Padding(
                padding: EdgeInsets.only(left: 16),
                child: const Text(
                  'Danh mục dịch vụ',
                  style: TextStyle(fontSize: 20),
                ),
              ),
            ),
            Padding(
              padding: EdgeInsets.fromLTRB(8, 0, 8, 0),
              child: SizedBox(
                height: 50,
                width: 40,
                child: Container(
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    color: ClassAppColor.bgSecondBtnColor,
                    borderRadius: BorderRadius.circular(5),
                    border: Border.all(color: ClassAppColor.boderBtnColor),
                  ),
                  child: IconButton(
                    padding: EdgeInsets.all(4),
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
      padding: EdgeInsets.fromLTRB(8, 5, 8, 5),
      // decoration: BoxDecoration(
      //   borderRadius: BorderRadius.circular(8),
      // ),
      child: Row(
        children: [
          Expanded(
            child: SizedBox(
              width: 200, // notworking in expand
              child: ConstrainedBox(
                constraints: BoxConstraints(maxWidth: 100, maxHeight: 35),
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
          Padding(
            padding: EdgeInsets.only(left: 8),
            child: SizedBox(
              width: 40,
              height: 32,
              child: Container(
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(5),
                  border: Border.all(color: ClassAppColor.boderBtnColor),
                ),
                child: IconButton(
                  padding: EdgeInsets.all(5),
                  onPressed: () {},
                  icon: Icon(
                    Icons.filter_alt_rounded,
                    color: ClassAppColor.iconColor,
                  ),
                  // style: const ButtonStyle(
                  //   backgroundColor:
                  //       MaterialStatePropertyAll(ClassAppColor.bgSecondBtnColor),
                  // ),
                ),
              ),
            ),
          ),
          SizedBox(
            width: 116, // 100+16
            child: Padding(
              padding: EdgeInsets.fromLTRB(8, 5, 8, 5),
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
        color: ClassAppColor.bgApp,
        child: Row(
          textDirection: TextDirection.ltr,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // left content
            SizedBox(
              height: screenHeight * 0.4,
              width: 300,
              child: Container(
                padding: EdgeInsets.only(left: 8),
                child: Column(
                  children: [
                    // titile nhom
                    SizedBox(
                      height: 40,
                      child: Container(
                        padding: EdgeInsets.fromLTRB(16, 5, 8, 5),
                        alignment: Alignment.topLeft,
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.start,
                          children: [
                            Expanded(
                              child: Text(
                                'Nhóm dịch vụ',
                                style: TextStyle(fontSize: 15),
                              ),
                            ),
                            IconButton(
                              onPressed: () {
                                showDialog(
                                    context: context,
                                    builder: (BuildContext context) {
                                      return ModalAddNhomDichVu();
                                    });
                              },
                              icon: Icon(Icons.add),
                            ),
                          ],
                        ),
                      ),
                    ),
                    const Divider(),
                    // list nhomDV
                    Expanded(
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
                                color: (_nhomDichVu[index].isSelected ?? false)
                                    ? Colors.blueAccent
                                    : Colors.white,
                                child: ListTile(
                                  // leading: Icon(Icons.check),
                                  title: Text(
                                      _nhomDichVu[index].tenNhomHang ?? ''),
                                  // trailing: Icon(Icons.check),
                                ),
                              ),
                            );
                          },
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
                      height: 50,
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
                          columns: [
                            GridColumn(
                              width: columnWidths['maHangHoa']!,
                              columnName: 'maHangHoa',
                              label: Container(
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 8.0),
                                alignment: Alignment.center,
                                child: Text(
                                  'Mã dịch vụ',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['tenHangHoa']!,
                              columnName: 'tenHangHoa',
                              label: Container(
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 8.0),
                                alignment: Alignment.center,
                                child: Text(
                                  'Tên dịch vụ',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['tenNhomHang']!,
                              columnName: 'tenNhomHang',
                              label: Container(
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 8.0),
                                alignment: Alignment.center,
                                child: Text(
                                  'Nhóm',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['giaBan']!,
                              columnName: 'giaBan',
                              label: Container(
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 8.0),
                                alignment: Alignment.center,
                                child: Text(
                                  'Giá bán',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['soPhutThucHien']!,
                              columnName: 'soPhutThucHien',
                              label: Container(
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 8.0),
                                alignment: Alignment.center,
                                child: Text(
                                  'Thời gian',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
                              ),
                            ),
                            GridColumn(
                              width: columnWidths['txtTrangThaiHang']!,
                              columnName: 'txtTrangThaiHang',
                              label: Container(
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 8.0),
                                alignment: Alignment.center,
                                child: Text(
                                  'Trạng thái',
                                  style: TextStyle(fontWeight: FontWeight.bold),
                                ),
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
