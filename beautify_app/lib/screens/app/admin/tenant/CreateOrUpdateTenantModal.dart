// ignore_for_file: use_build_context_synchronously

import 'package:beautify_app/Models/TenanlModels/CreateTenantDto.dart';
import 'package:flutter/material.dart';

import 'TenantService.dart';

class CreateTenantModal extends StatefulWidget {
  final String headerModel;
  const CreateTenantModal({super.key, required this.headerModel});

  @override
  State<CreateTenantModal> createState() => _CreateTenantModalState();
}

class _CreateTenantModalState extends State<CreateTenantModal> {
  final _formKey = GlobalKey<FormState>();
  final _tenantNameController = TextEditingController();
  final _nameController = TextEditingController();
  final _connectionStringController = TextEditingController();
  final _adminEmailController = TextEditingController();
  final _createTenantInput = CreateTenantDto(
      tenancyName: '',
      name: '',
      connectionString: '',
      adminEmailAddress: '',
      isActive: true);
  @override
  Widget build(BuildContext context) {
    return GridTile(
      child: AlertDialog(
        title: Row(
            crossAxisAlignment: CrossAxisAlignment.center,
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Text(widget.headerModel.toString()),
              InkWell(
                onTap: () {
                  Navigator.pop(context);
                },
                child: Container(
                  padding: const EdgeInsets.all(6),
                  decoration: BoxDecoration(
                      color: Colors.red.withOpacity(0.8),
                      borderRadius: BorderRadius.circular(30)),
                  child: const Icon(
                    Icons.close,
                    color: Colors.white,
                    size: 16,
                  ),
                ),
              )
            ]),
        content: Stack(
          children: <Widget>[
            SizedBox(
              width: 540,
              child: Form(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(
                          top: 4, bottom: 8, right: 16, left: 16),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: const [
                          Text(
                            "Tenant Id",
                            textAlign: TextAlign.left,
                            style: TextStyle(
                                color: Color(0xff4C4B4C),
                                fontSize: 14,
                                fontWeight: FontWeight.bold),
                          ),
                          Padding(
                            padding: EdgeInsets.only(left: 4),
                            child: Text(
                              "*",
                              textAlign: TextAlign.left,
                              style: TextStyle(
                                  color: Color(0xffD70000),
                                  fontSize: 14,
                                  fontWeight: FontWeight.bold),
                            ),
                          ),
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: SizedBox(
                        height: 45,
                        child: TextFormField(
                          controller: _tenantNameController,
                          decoration: InputDecoration(
                            contentPadding:
                                const EdgeInsets.fromLTRB(10, 10, 10, 10),
                            //labelText: "Username",
                            labelStyle: const TextStyle(
                                color: Colors.blue,
                                fontSize: 20,
                                fontWeight: FontWeight.bold),
                            border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15)),
                            errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.red)),
                            enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                            disabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                          ),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Không được để trống dữ liệu';
                            }
                            return null;
                          },
                        ),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(
                          top: 4, bottom: 8, right: 16, left: 16),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: const [
                          Text(
                            "Tên cửa hàng",
                            textAlign: TextAlign.left,
                            style: TextStyle(
                                color: Color(0xff4C4B4C),
                                fontSize: 14,
                                fontWeight: FontWeight.bold),
                          ),
                          Padding(
                            padding: EdgeInsets.only(left: 4),
                            child: Text(
                              "*",
                              textAlign: TextAlign.left,
                              style: TextStyle(
                                  color: Color(0xffD70000),
                                  fontSize: 14,
                                  fontWeight: FontWeight.bold),
                            ),
                          ),
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: SizedBox(
                        height: 45,
                        child: TextFormField(
                          controller: _nameController,
                          decoration: InputDecoration(
                            contentPadding:
                                const EdgeInsets.fromLTRB(10, 10, 10, 10),
                            //labelText: "Username",
                            labelStyle: const TextStyle(
                                color: Colors.blue,
                                fontSize: 20,
                                fontWeight: FontWeight.bold),
                            border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15)),
                            errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.red)),
                            enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                            disabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                          ),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Không được để trống dữ liệu';
                            }
                            return null;
                          },
                        ),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(
                          top: 4, bottom: 8, right: 16, left: 16),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: const [
                          Text(
                            "Database conection strings",
                            textAlign: TextAlign.left,
                            style: TextStyle(
                                color: Color(0xff4C4B4C),
                                fontSize: 14,
                                fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: SizedBox(
                        height: 45,
                        child: TextFormField(
                          controller: _connectionStringController,
                          decoration: InputDecoration(
                            contentPadding:
                                const EdgeInsets.fromLTRB(10, 10, 10, 10),
                            //labelText: "Username",
                            labelStyle: const TextStyle(
                                color: Colors.blue,
                                fontSize: 20,
                                fontWeight: FontWeight.bold),
                            border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15)),
                            errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.red)),
                            enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                            disabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                          ),
                        ),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(
                          top: 4, bottom: 8, right: 16, left: 16),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: const [
                          Text(
                            "Admin email",
                            textAlign: TextAlign.left,
                            style: TextStyle(
                                color: Color(0xff4C4B4C),
                                fontSize: 14,
                                fontWeight: FontWeight.bold),
                          ),
                          Padding(
                            padding: EdgeInsets.only(left: 4),
                            child: Text(
                              "*",
                              textAlign: TextAlign.left,
                              style: TextStyle(
                                  color: Color(0xffD70000),
                                  fontSize: 14,
                                  fontWeight: FontWeight.bold),
                            ),
                          ),
                        ],
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(8.0),
                      child: SizedBox(
                        height: 45,
                        child: TextFormField(
                          controller: _adminEmailController,
                          decoration: InputDecoration(
                            contentPadding:
                                const EdgeInsets.fromLTRB(10, 10, 10, 10),
                            //labelText: "Username",
                            labelStyle: const TextStyle(
                                color: Colors.blue,
                                fontSize: 20,
                                fontWeight: FontWeight.bold),
                            border: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15)),
                            errorBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.red)),
                            enabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                            disabledBorder: OutlineInputBorder(
                                borderRadius: BorderRadius.circular(15),
                                borderSide:
                                    const BorderSide(color: Colors.black)),
                          ),
                          validator: (value) {
                            if (value == null || value.isEmpty) {
                              return 'Không được để trống dữ liệu';
                            }
                            return null;
                          },
                        ),
                      ),
                    ),
                    const Padding(
                      padding: EdgeInsets.only(
                          top: 4, bottom: 8, right: 16, left: 16),
                      child: Text(
                        "Mật khẩu mặc định là : 123qwe",
                        textAlign: TextAlign.left,
                        style: TextStyle(
                            color: Color(0xff4C4B4C),
                            fontSize: 14,
                            fontWeight: FontWeight.bold),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
        actions: <Widget>[
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
              _createTenantInput.adminEmailAddress = _adminEmailController.text;
              _createTenantInput.connectionString =
                  _connectionStringController.text;
              _createTenantInput.name = _nameController.text;
              _createTenantInput.tenancyName = _tenantNameController.text;
              print(_createTenantInput.toString());
              TenantService().CreateTenant(_createTenantInput);
              // Đóng form
              Navigator.of(context).pop();
            },
          ),
        ],
      ),
    );
  }
}
