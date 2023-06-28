import 'package:beautify_app/screens/app/admin/role/models/RoleListDto.dart';
import 'package:beautify_app/screens/app/admin/role/models/permissionViewModel.dart';
import 'package:flutter/material.dart';

class RoleList extends StatefulWidget {
  List<RoleListDto> allRole;
  List<String> roleNames;
  Function(List<String>) onSelectedPermissionsChanged;
  RoleList({
    Key? key,
    required this.allRole,
    required this.roleNames,
    required this.onSelectedPermissionsChanged,
  }) : super(key: key);

  @override
  State<RoleList> createState() => _PermissionListState();
}

class _PermissionListState extends State<RoleList>
    with AutomaticKeepAliveClientMixin {
  TextEditingController searchController = TextEditingController();
  bool isSelectAll = false;
  late List<RoleListDto> filterAllRole = widget.allRole;
  @override
  Widget build(BuildContext context) {
    super.build(context);
    return AutomaticKeepAlive(
      child: ListView.builder(
        itemCount: filterAllRole.length,
        itemBuilder: (context, index) {
          return Row(
            children: [
              Checkbox(
                value: widget.roleNames
                        .contains(filterAllRole[index].name.toUpperCase())
                    ? true
                    : false,
                onChanged: (bool? value) {
                  setState(() {
                    if (value == true) {
                      widget.roleNames
                          .add(filterAllRole[index].name.toUpperCase());
                    } else {
                      widget.roleNames
                          .remove(filterAllRole[index].name.toUpperCase());
                      isSelectAll = false;
                    }
                  });
                },
              ),
              Text(filterAllRole[index].name.toString()),
            ],
          );
        },
      ),
    );
  }

  @override
  bool get wantKeepAlive => true;
}
