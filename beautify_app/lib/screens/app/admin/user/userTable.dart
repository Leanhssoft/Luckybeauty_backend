import 'package:beautify_app/components/CustomPagination.dart';
import 'package:beautify_app/screens/app/admin/role/models/RoleDto.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';
import 'package:beautify_app/screens/app/admin/user/create_or_update_user_modal.dart';
import 'package:beautify_app/screens/app/admin/user/models/userDto.dart';
import 'package:beautify_app/screens/app/admin/user/service/userServices.dart';
import 'package:flutter/material.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'models/PagedResultRequestDto.dart';

class UserTable extends StatefulWidget {
  const UserTable({super.key});

  @override
  State<UserTable> createState() => _UserTableState();
}

class _UserTableState extends State<UserTable> {
  List<UserDto> _data = [];
  int _currentPage = 1;
  int perPage = 10;
  String _searchText = '';

  Future<void> _getUser() async {
    var skipCount = _currentPage == 1 ? 0 : _currentPage * perPage;
    var maxResult = perPage;
    PagedUserResultRequestDto input = PagedUserResultRequestDto(
        keyWord: _searchText, skipCount: skipCount, maxResultCount: maxResult);
    var data = await UserServices().GetAllUser(input);
    print(data);
    setState(() {
      _data = data;
    });
  }

  @override
  void initState() {
    super.initState();
    _currentPage = 1;
    _getUser();
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
            height: MediaQuery.of(context).size.height - 270,
            child: Scrollbar(
              thumbVisibility: true,
              controller: _scrollController,
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                controller: _scrollController,
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SingleChildScrollView(
                      scrollDirection: Axis.vertical,
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
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
                            perPage: 10,
                            pagesVisible: 5,
                            onPageChanged: (curentPage) {
                              setState(() {
                                _currentPage = curentPage;
                              });
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
            'Tên truy cập',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
        label: Container(
          alignment: Alignment.center,
          child: const Text(
            'Họ và tên',
            textAlign: TextAlign.center,
          ),
        ),
      ),
      DataColumn(
          label: Container(
            alignment: Alignment.center,
            child: const Text(
              'Vai trò',
              textAlign: TextAlign.center,
            ),
          ),
          numeric: true),
      DataColumn(
          label: Container(
            alignment: Alignment.center,
            child: const Text(
              'Địa chỉ email',
              textAlign: TextAlign.center,
            ),
          ),
          numeric: true),
      DataColumn(
          label: Container(
            alignment: Alignment.center,
            child: const Text(
              'Trạng thái',
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

List<DataRow> dataRows(List<UserDto> items, BuildContext parentContext) {
  int i = 0;
  List<DataRow> dataRow = [];
  for (var item in items) {
    i += 1;
    DataRow row = DataRow(
      cells: [
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 50),
            alignment: Alignment.center,
            child: Text(
              i.toString(),
              textAlign: TextAlign.center,
            ),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 120),
            alignment: Alignment.centerLeft,
            child: Text(item.userName),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 150),
            alignment: Alignment.centerLeft,
            child: Text(item.fullName),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 200),
            alignment: Alignment.centerLeft,
            child: Text(item.roleNames.toString()),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 200),
            alignment: Alignment.centerLeft,
            child: Text(item.emailAddress.toString()),
          ),
        ),
        DataCell(
          Container(
            constraints: const BoxConstraints(maxWidth: 120),
            alignment: Alignment.centerLeft,
            child: Text(item.isActive == true ? "Hoạt động" : "Đã khóa",
                style: GoogleFonts.roboto(
                    color: item.isActive == true
                        ? const Color(0xFF009EF7)
                        : Colors.red[200],
                    fontSize: 12)),
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
                          context: parentContext,
                          builder: (BuildContext context) {
                            return CreateOrUpdateUserModal(
                              id: item.id,
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
                          context: parentContext,
                          builder: (BuildContext context) {
                            return DeleteUserDialog(id: item.id);
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

class DeleteUserDialog extends StatelessWidget {
  const DeleteUserDialog({
    super.key,
    required this.id,
  });

  final int id;

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
        builder: (BuildContext context, BoxConstraints constraints) {
      return AlertDialog(
        shape:
            RoundedRectangleBorder(borderRadius: BorderRadius.circular(10.0)),
        title: const Text("Bạn có chắc muốn xóa bản ghi này"),
        content: const SizedBox(
          width: 450,
          height: 200,
          child: Center(
            child: Text("Bạn có chắc muốn xóa bản ghi này"),
          ),
        ),
        actions: [
          ElevatedButton(
            onPressed: () => Navigator.of(context).pop(),
            child: const Text(
              'Cancel',
              style: TextStyle(
                  color: Color(0xFFC41A3B), fontWeight: FontWeight.bold),
            ),
          ),
          ElevatedButton(
            style: const ButtonStyle(
                backgroundColor: MaterialStatePropertyAll(Color(0xFFFF5252))),
            onPressed: () async {
              bool delete = await UserServices().deleteUser(id);
              if (delete == true) {
                // ignore: use_build_context_synchronously
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(
                      content: Text(
                        'Delete success !',
                        textAlign: TextAlign.center,
                      ),
                      backgroundColor: Color.fromARGB(255, 241, 68, 68)),
                );
                // ignore: use_build_context_synchronously
                Navigator.of(context).pop();
              } else {
                // ignore: use_build_context_synchronously
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(
                      content: Text(
                        'Have error for delete !',
                        textAlign: TextAlign.center,
                      ),
                      backgroundColor: Color.fromARGB(255, 241, 68, 68)),
                );
                // ignore: use_build_context_synchronously
                Navigator.of(context).pop();
              }
            },
            child: const Text(
              'Confirm',
              style:
                  TextStyle(color: Colors.white, fontWeight: FontWeight.w700),
            ),
          )
        ],
      );
    });
  }
}
