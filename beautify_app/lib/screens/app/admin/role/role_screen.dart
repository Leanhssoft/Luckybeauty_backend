import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/admin/role/models/PagedRoleResultRequestDto.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';
import 'package:data_table_2/data_table_2.dart';
import 'package:flutter/material.dart';

import 'RoleDataSource.dart';
import 'models/RoleDto.dart';
import 'roleDialog.dart';

class RoleScreen extends StatefulWidget {
  const RoleScreen({super.key});

  @override
  State<RoleScreen> createState() => _RoleScreenState();
}

class _RoleScreenState extends State<RoleScreen> {
  List<RoleDto> _data = [];

  int _rowIndex = 1;
  int _rowsPerPage = PaginatedDataTable.defaultRowsPerPage;
  int _sortColumnIndex = 0;
  bool _sortAscending = true;
  String _searchText = '';
  Future<void> _getRole() async {
    var skipCount = _rowIndex == 1 ? 0 : _rowIndex;
    var maxResult = _rowsPerPage;
    PagedRoleResultRequestDto input = PagedRoleResultRequestDto(
        keyWord: _searchText, skipCount: skipCount, maxResultCount: maxResult);
    var data = await RoleService().getAll(input);
    setState(() {
      _data = data;
    });
  }

  @override
  void initState() {
    _getRole();
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
                Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: Row(
                    children: [
                      InkWell(
                        onTap: () {
                          Navigator.pop(context);
                        },
                        child: Container(
                          padding: const EdgeInsets.all(10),
                          decoration: BoxDecoration(
                              color: Colors.grey.withOpacity(0.40),
                              borderRadius: BorderRadius.circular(30)),
                          child: const Icon(
                            Icons.arrow_back_ios_new,
                            size: 22,
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.all(16),
                  child: Align(
                    alignment: Alignment.topRight,
                    child: ElevatedButton(
                      onPressed: () async {
                        // showDialog(
                        //     context: context,
                        //     builder: (BuildContext context) {
                        //       return const MyAlertDialog();
                        //     });
                        var result = await showDialog<RoleDto>(
                          context: context,
                          builder: (BuildContext context) {
                            return const MyAlertDialog();
                          },
                        );

                        // If the result is not null, add the new role to the data list
                        if (result != null) {
                          setState(() {
                            _data.add(result);
                          });
                        }
                      },
                      child: const Text("Thêm mới"),
                    ),
                  ),
                ),
                SizedBox(
                  height: MediaQuery.of(context).size.height * 0.6,
                  child: PaginatedDataTable2(
                    rowsPerPage: _rowsPerPage,
                    sortColumnIndex: _sortColumnIndex,
                    sortAscending: _sortAscending,
                    availableRowsPerPage: const [5, 10, 20],
                    onPageChanged: (int rowIndex) {
                      setState(() {
                        _rowIndex = rowIndex;
                      });
                      _getRole();
                    },
                    onRowsPerPageChanged: (int? value) {
                      setState(() {
                        _rowsPerPage =
                            value ?? PaginatedDataTable.defaultRowsPerPage;
                      });
                      _getRole();
                    },
                    headingRowColor: MaterialStateColor.resolveWith(
                        (states) => Color(0xFFB2AFB2)),
                    columns: const [
                      DataColumn(label: Text('STT'), numeric: true),
                      DataColumn(label: Text('Vai trò'), numeric: true),
                      DataColumn(label: Text('Tên vai trò'), numeric: true),
                      DataColumn(label: Text('Mô tả'), numeric: true),
                      DataColumn(label: Text("Hành động"))
                    ],
                    source: RoleTableSource(data: _data),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
