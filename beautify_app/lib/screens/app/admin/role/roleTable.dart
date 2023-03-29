// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/screens/app/admin/role/deleteRoleDialog.dart';
import 'package:beautify_app/screens/app/admin/role/roleDialog.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/screens/app/admin/role/models/RoleDto.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';

import 'models/PagedRoleResultRequestDto.dart';

class RoleTable extends StatefulWidget {
  late BuildContext parentContext;
  RoleTable({
    Key? key,
    required this.parentContext,
  }) : super(key: key);

  @override
  State<RoleTable> createState() => _RoleTableState();
}

class _RoleTableState extends State<RoleTable> {
  bool checkAll = false;
  int _currentPage = 1;
  int perPage = 10;
  int stt = 0;
  List<RoleDto> _data = [];
  String _searchText = '';
  Future<void> _getRole() async {
    var skipCount = _currentPage == 1 ? 0 : _currentPage * perPage;
    var maxResult = perPage;
    PagedRoleResultRequestDto input = PagedRoleResultRequestDto(
        keyWord: _searchText, skipCount: skipCount, maxResultCount: maxResult);
    var data = await RoleService().getAll(input);
    setState(() {
      _data = data;
      _searchText = '';
    });
  }

  @override
  void initState() {
    _getRole();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
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
                        onEditingComplete: () => {},
                        onSubmitted: (value) {
                          setState(() {
                            _searchText = value;
                          });
                          _getRole();
                        },
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
            height: MediaQuery.of(context).size.height - 270,
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: SingleChildScrollView(
                scrollDirection: Axis.vertical,
                child: Column(
                  children: [
                    DataTable(
                        columnSpacing: 170,
                        dividerThickness: 1,
                        headingTextStyle: const TextStyle(
                          color: Color(0xFFB2AFB2),
                        ),
                        columns: viewColumn,
                        rows: [
                          for (int i = 0; i < _data.length; i++)
                            DataRow(cells: [
                              DataCell(
                                Container(
                                  alignment: Alignment.center,
                                  child: Text(
                                    (i + 1).toString(),
                                    textAlign: TextAlign.center,
                                  ),
                                ),
                              ),
                              DataCell(
                                Container(
                                  alignment: Alignment.centerLeft,
                                  child: Text(_data[i].name),
                                ),
                              ),
                              DataCell(
                                Container(
                                  alignment: Alignment.centerLeft,
                                  child: Text(_data[i].displayName),
                                ),
                              ),
                              DataCell(
                                Container(
                                  alignment: Alignment.centerLeft,
                                  child: Text(_data[i].description.toString()),
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
                                          child: const Icon(
                                              Icons.remove_red_eye_outlined),
                                        ),
                                      ),
                                      Padding(
                                        padding: const EdgeInsets.all(2.0),
                                        child: ElevatedButton(
                                          onPressed: () {
                                            showDialog(
                                                context: context,
                                                builder:
                                                    (BuildContext context) {
                                                  return CreateOrUpdateRoleModal(
                                                    id: _data[i].id.toString(),
                                                    reload: () {
                                                      _getRole();
                                                    },
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
                                                  MaterialStatePropertyAll(
                                                      Color(0xFFFF5252))),
                                          onPressed: () {
                                            showDialog(
                                                context: context,
                                                builder:
                                                    (BuildContext context) {
                                                  return DeleteRoleDialog(
                                                      id: _data[i].id);
                                                });
                                          },
                                          child: const Icon(Icons.delete),
                                        ),
                                      ),
                                    ],
                                  ),
                                ),
                              ),
                            ])
                        ]),
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
                const Spacer(),
                Expanded(
                  child: Row(
                    children: [
                      Expanded(
                        child: Padding(
                          padding: const EdgeInsets.all(8.0),
                          child: Text(
                              "Hiển thị ${(_currentPage * perPage) - 9}-${_currentPage * perPage} của ${_data.length} mục",
                              style: GoogleFonts.roboto(
                                  color: const Color(0xFF666466),
                                  fontSize: 14)),
                        ),
                      ),
                      Expanded(
                        child: Padding(
                          padding: const EdgeInsets.all(8.0),
                          child: CustomPaginator(
                            itemCount: _data.length,
                            perPage: perPage,
                            pagesVisible: 5,
                            onPageChanged: (curentPage) {
                              setState(() {
                                _currentPage = curentPage;
                              });
                              _getRole();
                            },
                          ),
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
            'Vai trò',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Tên vai trò',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
          label: Container(
            alignment: Alignment.center,
            child: const Text(
              'Mô tả',
              textAlign: TextAlign.center,
            ),
          ),
          numeric: true),
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

List<DataRow> dataRows(List<RoleDto> items, BuildContext context) {
  int i = 0;
  List<DataRow> dataRow = [];
  for (var item in items) {
    i += 1;
    DataRow row = DataRow(
      cells: [
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 30),
            alignment: Alignment.center,
            child: Text(
              i.toString(),
              textAlign: TextAlign.center,
            ),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 300),
            width: 300,
            alignment: Alignment.centerLeft,
            child: Text(item.name),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 300),
            width: 300,
            alignment: Alignment.centerLeft,
            child: Text(item.displayName),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 300),
            width: 300,
            alignment: Alignment.centerLeft,
            child: Text(item.description.toString()),
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
                          builder: (BuildContext context) {
                            return CreateOrUpdateRoleModal(
                              id: item.id.toString(),
                              reload: () {},
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
                          context: context,
                          builder: (BuildContext context) {
                            return DeleteRoleDialog(id: item.id);
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
