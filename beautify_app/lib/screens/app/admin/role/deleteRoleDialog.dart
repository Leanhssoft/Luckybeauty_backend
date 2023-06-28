import 'package:beautify_app/screens/app/admin/role/rolePage.dart';
import 'package:beautify_app/screens/app/admin/role/roleService.dart';
import 'package:flutter/material.dart';

class DeleteRoleDialog extends StatelessWidget {
  const DeleteRoleDialog({
    super.key,
    required this.id,
  });

  final int id;

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(builder: (BuildContext context,
        BoxConstraints constraints) {
      return AlertDialog(
        shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10.0)),
        title: const Text(
            "Bạn có chắc muốn xóa bản ghi này"),
        content: const SizedBox(
          width: 450,
          height: 200,
          child: Center(
            child: Text(
                "Bạn có chắc muốn xóa bản ghi này"),
          ),
        ),
        actions: [
          ElevatedButton(
            onPressed: () =>
                Navigator.of(context).pop(),
            child: const Text(
              'Cancel',
              style: TextStyle(
                  color: Color(0xFFC41A3B),
                  fontWeight: FontWeight.bold),
            ),
          ),
          ElevatedButton(
            style: const ButtonStyle(
                backgroundColor:
                    MaterialStatePropertyAll(
                        Color(0xFFFF5252))),
            onPressed: () async {
              bool delete = await RoleService()
                  .deleteRole(id);
              if (delete == true) {
                // ignore: use_build_context_synchronously
                ScaffoldMessenger.of(context)
                    .showSnackBar(
                  const SnackBar(
                      content: Text(
                        'Delete success !',
                        textAlign: TextAlign.center,
                      ),
                      backgroundColor: Color.fromARGB(
                          255, 241, 68, 68)),
                );
                // ignore: use_build_context_synchronously
                Navigator.of(context).pop();
              } else {
                // ignore: use_build_context_synchronously
                ScaffoldMessenger.of(context)
                    .showSnackBar(
                  const SnackBar(
                      content: Text(
                        'Have error for delete !',
                        textAlign: TextAlign.center,
                      ),
                      backgroundColor: Color.fromARGB(
                          255, 241, 68, 68)),
                );
                 // ignore: use_build_context_synchronously
                                Navigator.push(
                                  context,
                                  MaterialPageRoute(
                                      builder: (context) =>
                                          const RolePage()),
                                );
              }
            },
            child: const Text(
              'Confirm',
              style: TextStyle(
                  color: Colors.white,
                  fontWeight: FontWeight.w700),
            ),
          )
        ],
      );
    });
  }
}
