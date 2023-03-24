// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/screens/app/admin/role/models/RoleListDto.dart';
import 'package:beautify_app/screens/app/admin/user/SelectedListRole.dart';
import 'package:beautify_app/screens/app/admin/user/CreateUpdateUserForm.dart';
import 'package:beautify_app/screens/app/admin/user/service/userServices.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/screens/app/admin/role/models/permissionViewModel.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';

import 'models/CreateUserDto.dart';

class CreateOrUpdateUserModal extends StatefulWidget {
  const CreateOrUpdateUserModal({
    Key? key,
  }) : super(key: key);

  @override
  _CreateOrUpdateUserModalState createState() =>
      _CreateOrUpdateUserModalState();
}

class _CreateOrUpdateUserModalState extends State<CreateOrUpdateUserModal>
    with SingleTickerProviderStateMixin {
  Map<String, dynamic> userData = {
    'userName': '',
    'name': '',
    'surname': '',
    'emailAddress': '',
    'isActive': '',
    'roleNames': [],
    'password': '',
    'nhanSuId': ''
  };
  List<String> _roleNames = [];
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  Future<void> _saveData() async {
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save();

      //Lưu trữ dữ liệu của tab 'Role'
      print(userData);
      CreateUserDto user = CreateUserDto(
          isActive: true,
          name: userData['name'],
          password: userData['password'],
          surname: userData['surname'],
          userName: userData['userName'],
          emailAddress: userData['emailAddress'],
          roleNames: _roleNames,
          nhanSuId: userData['nhanSuId']);
      print(user);
      await UserServices().createUser(user);
    }
  }

  late TabController _tabController;
  late List<RoleListDto> _fullRole = [];
  Future<void> getFullPermission() async {
    var lstRole = await RoleService().getRoles();
    setState(() {
      _fullRole = lstRole;
      if (kDebugMode) {
        print(_fullRole);
      }
    });
  }

  @override
  void initState() {
    _tabController = TabController(length: 2, vsync: this);
    getFullPermission();
    super.initState();
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (BuildContext context, BoxConstraints constraints) {
        return AlertDialog(
          title: const Text('Tạo tài khoản người dùng'),
          content: SizedBox(
            height: constraints.maxHeight * 0.8, // set height
            width: 640,
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                children: [
                  TabBar(
                    controller: _tabController,
                    tabs: [
                      Tab(
                        child: Text("Thông tin người dùng",
                            style: GoogleFonts.roboto(
                                color: Colors.black.withOpacity(.7))),
                      ),
                      Tab(
                        child: Text("Vai trò",
                            style: GoogleFonts.roboto(
                                color: Colors.black.withOpacity(.7))),
                      )
                    ],
                    indicatorColor: Colors.blue,
                    // Set the label style for the active tab
                    labelStyle: const TextStyle(color: Colors.blue),
                  ),
                  Expanded(
                    child: TabBarView(
                      controller: _tabController,
                      children: [
                        UserForm(
                          formKey: _formKey,
                          onUserSave: (Map<String, dynamic> data) {
                            userData.addAll(data);
                          },
                        ),
                        RoleList(
                          allRole: _fullRole,
                          roleNames: _roleNames,
                          onSelectedPermissionsChanged: (p0) =>
                              {_roleNames = p0},
                        )
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
          actions: [
            ElevatedButton.icon(
              icon: const Icon(Icons.cancel),
              label: const Text("Hủy"),
              onPressed: () {
                Navigator.of(context).pop();
              },
              style: const ButtonStyle(
                  backgroundColor: MaterialStatePropertyAll(Colors.red)),
            ),
            ElevatedButton.icon(
              icon: const Icon(Icons.save),
              label: const Text("Lưu"),
              onPressed: () {
                // Đóng form
                _saveData();
                Navigator.of(context).pop();
              },
            ),
          ],
        );
      },
    );
  }
}
