import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/admin/user/models/userDto.dart';
import 'package:beautify_app/screens/app/admin/user/service/userServices.dart';
import 'package:beautify_app/screens/app/admin/user/userDataTableSource.dart';
import 'package:data_table_2/data_table_2.dart';
import 'package:flutter/material.dart';

import 'create_or_update_user_modal.dart';
import 'models/PagedResultRequestDto.dart';

class UserScreen extends StatefulWidget {
  const UserScreen({Key? key}) : super(key: key);

  @override
  State<UserScreen> createState() => _UserScreenState();
}

class _UserScreenState extends State<UserScreen> {
  List<UserDto> _data = [];

  int _rowIndex = 0;
  int _rowsPerPage = PaginatedDataTable.defaultRowsPerPage;
  int _sortColumnIndex = 0;
  bool _sortAscending = true;
  String _searchText = '';

  Future<void> _getUser() async {
    var skipCount = _rowIndex * _rowsPerPage;
    var maxResult = _rowsPerPage;
    PagedUserResultRequestDto input = PagedUserResultRequestDto(
        keyWord: _searchText, skipCount: skipCount, maxResultCount: maxResult);
    var data = await UserServices().GetAllUser(input);
    setState(() {
      _data = data;
    });
  }

  @override
  void initState() {
    super.initState();
    _getUser();
  }

  @override
  Widget build(BuildContext context) {
    final double _width = MediaQuery.of(context).size.width;
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: [
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
                        var result = await showDialog<UserDto>(
                          context: context,
                          builder: (BuildContext context) {
                            return const CreateOrUpdateUserModal();
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
                  height: MediaQuery.of(context).size.height * 0.8,
                  child: PaginatedDataTable2(
                    rowsPerPage: _rowsPerPage,
                    sortColumnIndex: _sortColumnIndex,
                    sortAscending: _sortAscending,
                    availableRowsPerPage: [5, 10, 20],
                    onPageChanged: (int rowIndex) {
                      setState(() {
                        _rowIndex = rowIndex;
                      });
                      _getUser();
                    },
                    onRowsPerPageChanged: (int? value) {
                      setState(() {
                        _rowsPerPage =
                            value ?? PaginatedDataTable.defaultRowsPerPage;
                      });
                      _getUser();
                    },
                    headingRowColor: MaterialStateColor.resolveWith(
                        (states) => const Color(0xFFB2AFB2)),
                    columns: [
                      DataColumn(
                        label: SizedBox(
                            width: _width * 0.05,
                            child: const Text(
                              'STT',
                              textAlign: TextAlign.center,
                            )),
                      ),
                      DataColumn(
                          label: SizedBox(
                              width: _width * 0.1,
                              child: const Text(
                                'Tên truy cập',
                                textAlign: TextAlign.center,
                              ))),
                      DataColumn(
                          label: SizedBox(
                              width: _width * 0.1,
                              child: const Text(
                                'Họ và tên',
                                textAlign: TextAlign.center,
                              ))),
                      DataColumn(
                          label: SizedBox(
                              width: _width * 0.1,
                              child: const Text(
                                'Vai trò',
                                textAlign: TextAlign.center,
                              ))),
                      DataColumn(
                          label: SizedBox(
                              width: _width * 0.2,
                              child: const Text(
                                'Địa chỉ email',
                                textAlign: TextAlign.center,
                              ))),
                      DataColumn(
                          label: SizedBox(
                              width: _width * 0.1,
                              child: const Text(
                                'Trạng thái',
                                textAlign: TextAlign.center,
                              ))),
                      DataColumn(
                          label: SizedBox(
                              width: _width * 0.2,
                              child: const Text(
                                "Hành động",
                                textAlign: TextAlign.center,
                              )))
                    ],
                    source: UserTableSource(data: _data),
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
