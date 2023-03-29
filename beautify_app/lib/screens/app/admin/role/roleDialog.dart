// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/components/CustomTextFormField.dart';
import 'package:beautify_app/components/CustomTextFormFieldValidate.dart';
import 'package:beautify_app/screens/app/admin/role/models/RoleDto.dart';
import 'package:beautify_app/screens/app/admin/role/models/permissionViewModel.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';

import 'models/createRoleDto.dart';

class CreateOrUpdateRoleModal extends StatefulWidget {
  String? id;
  Function? reload;
  CreateOrUpdateRoleModal({
    Key? key,
    this.id,
    this.reload,
  }) : super(key: key);

  @override
  _CreateOrUpdateRoleModalState createState() =>
      _CreateOrUpdateRoleModalState();
}

class _CreateOrUpdateRoleModalState extends State<CreateOrUpdateRoleModal>
    with SingleTickerProviderStateMixin {
  RoleDto roleEdit = RoleDto(
      id: 0,
      name: '',
      displayName: '',
      normalizedName: '',
      description: '',
      grantedPermissions: []);
  Map<String, dynamic> roleData = {
    'roleName': '',
    'displayName': '',
    'normalizedName': '',
    'description': '',
    'grantedPermissions': []
  };
  double _heightDialog = 380;
  List<String> _permissionsCurent = [];
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  Future<void> _saveData() async {
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save();

      if (widget.id == '') {
        // Lưu trữ dữ liệu của tab 'Role'
        CreateRoleDto role = CreateRoleDto(
            name: roleData['roleName'],
            displayName: roleData['displayName'],
            description: roleData['description'],
            normalizedName: roleData['roleName'].toString().toUpperCase(),
            grantedPermissions: _permissionsCurent);
        await RoleService().createRole(role); // add await here
      } else {
        // Lưu trữ dữ liệu của tab 'Role'
        RoleDto role = RoleDto(
            id: int.parse(widget.id.toString()),
            name: roleData['roleName'],
            displayName: roleData['displayName'],
            description: roleData['description'],
            normalizedName: roleData['roleName'].toString().toUpperCase(),
            grantedPermissions: _permissionsCurent);
        await RoleService().updateRole(role); // add await here
      }
    }
  }

  late TabController _tabController;
  late List<PermissionViewModel> _fullPermissions = [];
  Future<void> getFullPermission() async {
    var lstPermission = await RoleService().getAllPermission();
    setState(() {
      _fullPermissions = lstPermission;
      if (kDebugMode) {
        print(_fullPermissions);
      }
    });
  }

  Future<RoleDto> getRole(int id) async {
    var role = await RoleService().getRole(id);
    setState(() {
      roleEdit = role;
      _permissionsCurent = role.grantedPermissions!.cast<String>();
      if (kDebugMode) {
        print(_fullPermissions);
      }
    });
    return role;
  }

  @override
  void initState() {
    _tabController = TabController(length: 2, vsync: this);
    getRole(int.parse(widget.id.toString()));
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
          title:
              Text(widget.id == '' ? 'Thêm mới vai trò' : 'Chỉnh sửa vai trò'),
          content: SizedBox(
            height: _heightDialog,
            width: 560,
            child: Column(
              children: [
                TabBar(
                  controller: _tabController,
                  tabs: [
                    Tab(
                        child: TextButton(
                      onPressed: () {
                        setState(() {
                          _heightDialog = 380;
                        });
                      },
                      child: Text("Tên vai trò",
                          style: GoogleFonts.roboto(
                              color: Colors.black.withOpacity(.7))),
                    )),
                    TextButton(
                      onPressed: () {
                        setState(() {
                          _heightDialog = 800;
                        });
                      },
                      child: Tab(
                        child: Text("Quyền",
                            style: GoogleFonts.roboto(
                                color: Colors.black.withOpacity(.7))),
                      ),
                    ),
                  ],
                  indicatorColor: Colors.blue,
                  // Set the label style for the active tab
                  labelStyle: const TextStyle(color: Colors.blue),
                ),
                Expanded(
                  child: TabBarView(
                    controller: _tabController,
                    children: [
                      RoleForm(
                        role: roleEdit,
                        formKey: _formKey,
                        onRoleSave: (Map<String, dynamic> data) {
                          roleData.addAll(data);
                        },
                      ),
                      PermissionList(
                        fullPermissions: _fullPermissions,
                        selectedPermissions: _permissionsCurent,
                        onSelectedPermissionsChanged: (p0) => {
                          _permissionsCurent = p0,
                        },
                      )
                    ],
                  ),
                ),
              ],
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

class PermissionList extends StatefulWidget {
  List<PermissionViewModel> fullPermissions;
  List<String> selectedPermissions;
  Function(List<String>) onSelectedPermissionsChanged;
  PermissionList({
    Key? key,
    required this.fullPermissions,
    required this.selectedPermissions,
    required this.onSelectedPermissionsChanged,
  }) : super(key: key);

  @override
  State<PermissionList> createState() => _PermissionListState();
}

class _PermissionListState extends State<PermissionList>
    with AutomaticKeepAliveClientMixin {
  TextEditingController searchController = TextEditingController();
  bool isSelectAll = false;
  late List<PermissionViewModel> filterFullPermissions = widget.fullPermissions;
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return AutomaticKeepAlive(
      child: Column(
        children: [
          TextField(
            controller: searchController,
            onChanged: (value) {
              // Filter the fullPermissions list based on the search query
              if (value.isNotEmpty) {
                List<PermissionViewModel> filteredPermissions = widget
                    .fullPermissions
                    .where((permission) => permission.displayName
                        .toLowerCase()
                        .contains(value.toLowerCase()))
                    .toList();
                setState(() {
                  filterFullPermissions = filteredPermissions;
                });
              } else {
                setState(() {
                  filterFullPermissions = widget.fullPermissions;
                });
              }
            },
            decoration: const InputDecoration(
              hintText: 'Search permissions',
            ),
          ),
          Row(
            children: [
              Checkbox(
                value: isSelectAll,
                onChanged: (bool? value) {
                  setState(() {
                    isSelectAll = value!;
                    if (value == true) {
                      widget.selectedPermissions =
                          filterFullPermissions.map((permission) {
                        return permission.name;
                      }).toList();
                    } else {
                      widget.selectedPermissions = [];
                    }
                  });
                },
              ),
              const Text('Select all'),
            ],
          ),
          Expanded(
            child: ListView.builder(
              itemCount: filterFullPermissions.length,
              itemBuilder: (context, index) {
                return Row(
                  children: [
                    Checkbox(
                      value: widget.selectedPermissions
                              .contains(filterFullPermissions[index].name)
                          ? true
                          : false,
                      onChanged: (bool? value) {
                        setState(() {
                          if (value == true) {
                            widget.selectedPermissions
                                .add(filterFullPermissions[index].name);
                          } else {
                            widget.selectedPermissions
                                .remove(filterFullPermissions[index].name);
                            isSelectAll = false;
                          }
                        });
                      },
                    ),
                    Text(filterFullPermissions[index].displayName.toString()),
                  ],
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  @override
  bool get wantKeepAlive => true;
}

class RoleForm extends StatefulWidget {
  final GlobalKey<FormState> formKey;
  final RoleDto? role;
  final Function(Map<String, dynamic> roleData) onRoleSave;

  const RoleForm({
    Key? key,
    required this.formKey,
    this.role,
    required this.onRoleSave,
  }) : super(key: key);

  @override
  State<RoleForm> createState() => _RoleFormState();
}

class _RoleFormState extends State<RoleForm>
    with AutomaticKeepAliveClientMixin {
  @override
  bool get wantKeepAlive => true;

  @override
  Widget build(BuildContext context) {
    return AutomaticKeepAlive(
      child: Center(
        child: Form(
          key: widget.formKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: <Widget>[
              Padding(
                padding: const EdgeInsets.only(top: 24),
                child: Text(
                  "Vai trò",
                  style: GoogleFonts.roboto(
                      fontSize: 14,
                      color: const Color(0xFF53545C),
                      fontWeight: FontWeight.bold),
                ),
              ),
              CustomTextFormFieldValidate(
                controller: TextEditingController(text: widget.role!.name),
                textValidate: 'Tên vai trò không được để trống',
                onSave: (value) {
                  widget.onRoleSave({'roleName': value});
                },
              ),
              Padding(
                padding: const EdgeInsets.only(top: 24),
                child: Text(
                  "Tên hiển thị",
                  style: GoogleFonts.roboto(
                      fontSize: 14,
                      color: const Color(0xFF53545C),
                      fontWeight: FontWeight.bold),
                ),
              ),
              CustomTextFormFieldValidate(
                controller:
                    TextEditingController(text: widget.role!.displayName),
                textValidate: 'Tên vai trò không được để trống',
                onSave: (value) {
                  widget.onRoleSave({'displayName': value});
                },
              ),
              Padding(
                padding: const EdgeInsets.only(top: 24),
                child: Text(
                  "Mô tả",
                  style: GoogleFonts.roboto(
                      fontSize: 14,
                      color: const Color(0xFF53545C),
                      fontWeight: FontWeight.bold),
                ),
              ),
              CustomTextFormField(
                controller:
                    TextEditingController(text: widget.role!.description),
                onSaved: (value) {
                  widget.onRoleSave({'description': value}); // change to this
                },
              ),
            ],
          ),
        ),
      ),
    );
  }
}
