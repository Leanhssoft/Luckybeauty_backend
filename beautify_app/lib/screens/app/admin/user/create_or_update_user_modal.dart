// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/screens/app/admin/user/models/userDto.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/screens/app/admin/role/models/RoleListDto.dart';
import 'package:beautify_app/screens/app/admin/role/models/permissionViewModel.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';
import 'package:beautify_app/screens/app/admin/user/CreateUpdateUserForm.dart';
import 'package:beautify_app/screens/app/admin/user/SelectedListRole.dart';
import 'package:beautify_app/screens/app/admin/user/service/userServices.dart';

import 'models/CreateUserDto.dart';

class CreateOrUpdateUserModal extends StatefulWidget {
  final int? id;
  const CreateOrUpdateUserModal({
    Key? key,
    this.id,
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
  late UserDto _user = UserDto(
      userName: "",
      name: "",
      surname: "",
      emailAddress: "",
      isActive: false,
      fullName: "",
      creationTime: "",
      roleNames: [],
      id: 0);
  List<String> _roleNames = [];
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  Future<void> _saveData() async {
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save();
      if (widget.id == null) {
        CreateUserDto user = CreateUserDto(
            isActive: true,
            name: userData['name'],
            password: userData['password'],
            surname: userData['surname'],
            userName: userData['userName'],
            emailAddress: userData['emailAddress'],
            roleNames: _roleNames,
            nhanSuId: userData['nhanSuId']);
        await UserServices().createUser(user);
      } else {
        _user.id = _user.id;
        _user.userName = _user.userName;
        _user.surname = userData['surname'];
        _user.name = userData['name'];
        _user.emailAddress = userData['emailAddress'];
        _user.creationTime = _user.creationTime;
        _user.isActive = true;
        _user.fullName = userData['name'] + userData['surname'];
        _user.lastLoginTime = null;
        _user.roleNames = _roleNames;
        _user.nhanSuId = _user.nhanSuId;
        await UserServices().updateUser(_user);
      }
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
    super.initState();
    _tabController = TabController(length: 2, vsync: this);
    getFullPermission();
    getUser();
  }

  Future<void> getUser() async {
    if (widget.id != null) {
      var user = await UserServices().GetUser(widget.id ?? 1);
      userData = {
        'userName': user.userName,
        'name': user.name,
        'surname': user.surname,
        'emailAddress': user.emailAddress,
        'isActive': user.isActive,
        'roleNames': user.roleNames,
        'password': '',
        'nhanSuId': user.nhanSuId
      };
      setState(() {
        _user = user;
        _roleNames = _user.roleNames.map((e) => e as String).toList();
      });
    }
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
          title: Text(widget.id == null
              ? 'Tạo tài khoản người dùng'
              : "Sửa thông tin người dùng"),
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
                          user: _user,
                          id: widget.id,
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
