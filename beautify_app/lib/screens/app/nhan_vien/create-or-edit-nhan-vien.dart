// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/components/CustomTextFormFieldValidate.dart';

import '../../../components/CustomTextFormField.dart';

class CreateOrEditNhanVienModal extends StatefulWidget {
  String? idNhanVien;
  CreateOrEditNhanVienModal({
    Key? key,
    this.idNhanVien,
  }) : super(key: key);

  @override
  _CreateOrEditNhanVienModalState createState() =>
      _CreateOrEditNhanVienModalState();
}

class _CreateOrEditNhanVienModalState extends State<CreateOrEditNhanVienModal> {
  int? _genderGroup;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  Future<void> _saveData() async {
    if (_formKey.currentState!.validate()) {
      _formKey.currentState!.save();
      // Lưu trữ dữ liệu của tab 'Role'
    }
  }

  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (BuildContext context, BoxConstraints constraints) {
        return AlertDialog(
          title: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              const Text('Thêm nhân viên'),
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
            ],
          ),
          content: SizedBox(
            width: 660,
            child: SingleChildScrollView(
              child: Column(
                children: [
                  Padding(
                    padding: const EdgeInsets.only(top: 28, bottom: 28),
                    child: Container(
                      width: 80,
                      height: 80,
                      decoration: BoxDecoration(
                        borderRadius: BorderRadius.circular(50),
                        image: const DecorationImage(
                          image: AssetImage('images/avatarProfile.jpg'),
                          fit: BoxFit.cover,
                        ),
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.all(8),
                    child: SingleChildScrollView(
                      child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Thông tin chi tiết",
                              style: GoogleFonts.roboto(
                                  fontSize: 16,
                                  color: const Color(0xFF999699),
                                  fontWeight: FontWeight.bold),
                            ),
                            Padding(
                                padding:
                                    const EdgeInsets.only(top: 24, bottom: 2),
                                child: Text(
                                  "Tên nhân viên",
                                  style: GoogleFonts.roboto(
                                      fontSize: 14,
                                      color: const Color(0xFF53545C),
                                      fontWeight: FontWeight.bold),
                                )),
                            CustomTextFormFieldValidate(
                                controller: TextEditingController(),
                                textValidate:
                                    "Tên nhân viên không được bỏ trống"),
                            Padding(
                              padding: const EdgeInsets.only(top: 24),
                              child: Row(
                                children: [
                                  Expanded(
                                      child: Padding(
                                    padding: const EdgeInsets.only(right: 4),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: [
                                        Text(
                                          "Ngày Sinh",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFF53545C),
                                              fontWeight: FontWeight.bold),
                                        ),
                                        InputDatePickerFormField(
                                            firstDate: DateTime(0001),
                                            lastDate: DateTime(3000)),
                                      ],
                                    ),
                                  )),
                                  Expanded(
                                      child: Padding(
                                    padding: const EdgeInsets.only(left: 4),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: [
                                        Text(
                                          "Giới tính",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFF53545C),
                                              fontWeight: FontWeight.bold),
                                        ),
                                        Row(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Expanded(
                                                child: RadioListTile(
                                              value: 0,
                                              groupValue: _genderGroup,
                                              onChanged: (value) => {
                                                setState(() {
                                                  _genderGroup = value;
                                                })
                                              },
                                              title: Text(
                                                "Nam",
                                                style: GoogleFonts.roboto(
                                                    fontSize: 10,
                                                    color:
                                                        const Color(0xFF53545C),
                                                    fontWeight:
                                                        FontWeight.bold),
                                              ),
                                            )),
                                            Expanded(
                                                child: RadioListTile(
                                              value: 1,
                                              groupValue: _genderGroup,
                                              onChanged: (value) => {
                                                setState(() {
                                                  _genderGroup = value;
                                                })
                                              },
                                              title: Text(
                                                "Nữ",
                                                style: GoogleFonts.roboto(
                                                    fontSize: 10,
                                                    color:
                                                        const Color(0xFF53545C),
                                                    fontWeight:
                                                        FontWeight.bold),
                                              ),
                                            )),
                                            Expanded(
                                                child: RadioListTile(
                                              value: 2,
                                              groupValue: _genderGroup,
                                              onChanged: (value) => {
                                                setState(() {
                                                  _genderGroup = value;
                                                })
                                              },
                                              title: Text(
                                                "Khác",
                                                style: GoogleFonts.roboto(
                                                    fontSize: 10,
                                                    color:
                                                        const Color(0xFF53545C),
                                                    fontWeight:
                                                        FontWeight.bold),
                                              ),
                                            )),
                                          ],
                                        )
                                      ],
                                    ),
                                  ))
                                ],
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.only(top: 24),
                              child: Row(
                                children: [
                                  Expanded(
                                    child: Padding(
                                      padding: const EdgeInsets.only(right: 4),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            "Số điện thoại",
                                            style: GoogleFonts.roboto(
                                                fontSize: 14,
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
                                          ),
                                          CustomTextFormField(
                                            controller: TextEditingController(),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ),
                                  Expanded(
                                      child: Padding(
                                    padding: const EdgeInsets.only(left: 4),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: [
                                        Text(
                                          "E-mail",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFF53545C),
                                              fontWeight: FontWeight.bold),
                                        ),
                                        CustomTextFormField(
                                          controller: TextEditingController(),
                                        ),
                                      ],
                                    ),
                                  ))
                                ],
                              ),
                            ),
                            Padding(
                                padding: const EdgeInsets.only(top: 24),
                                child: Row(
                                  children: [
                                    Expanded(
                                      child: Padding(
                                        padding:
                                            const EdgeInsets.only(right: 4),
                                        child: Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              "Địa chỉ",
                                              style: GoogleFonts.roboto(
                                                  fontSize: 14,
                                                  color:
                                                      const Color(0xFF53545C),
                                                  fontWeight: FontWeight.bold),
                                            ),
                                            CustomTextFormField(
                                              controller:
                                                  TextEditingController(),
                                            ),
                                          ],
                                        ),
                                      ),
                                    ),
                                    Expanded(
                                        child: Padding(
                                      padding: const EdgeInsets.only(left: 4),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            "CCCD",
                                            style: GoogleFonts.roboto(
                                                fontSize: 14,
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
                                          ),
                                          CustomTextFormField(
                                            controller: TextEditingController(),
                                          ),
                                        ],
                                      ),
                                    ))
                                  ],
                                )),
                            Padding(
                              padding: const EdgeInsets.only(top: 24),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Expanded(
                                      child: Padding(
                                    padding: const EdgeInsets.only(right: 4),
                                    child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            "Nơi cấp",
                                            style: GoogleFonts.roboto(
                                                fontSize: 14,
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
                                          ),
                                          CustomTextFormField(
                                            controller: TextEditingController(),
                                          ),
                                        ]),
                                  )),
                                  Expanded(
                                      child: Padding(
                                    padding: const EdgeInsets.only(left: 4),
                                    child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Text(
                                            "Ngày cấp",
                                            style: GoogleFonts.roboto(
                                                fontSize: 14,
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
                                          ),
                                          CustomTextFormField(
                                            controller: TextEditingController(),
                                          ),
                                        ]),
                                  )),
                                ],
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.only(top: 24),
                              child: Padding(
                                padding: const EdgeInsets.only(right: 3),
                                child: Column(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: [
                                      Text(
                                        "Chức vụ",
                                        style: GoogleFonts.roboto(
                                            fontSize: 14,
                                            color: const Color(0xFF53545C),
                                            fontWeight: FontWeight.bold),
                                      ),
                                      CustomTextFormField(
                                        controller: TextEditingController(),
                                      ),
                                    ]),
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.only(top: 24),
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    "Ghi chú",
                                    style: GoogleFonts.roboto(
                                        fontSize: 14,
                                        color: const Color(0xFF53545C),
                                        fontWeight: FontWeight.bold),
                                  ),
                                  CustomTextFormField(
                                    controller: TextEditingController(),
                                  ),
                                ],
                              ),
                            )
                          ]),
                    ),
                  )
                ],
              ),
            ),
          ),
          actions: [
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: ElevatedButton.icon(
                icon: const Icon(Icons.cancel),
                label: const Text("Hủy"),
                onPressed: () {
                  Navigator.of(context).pop();
                },
                style: const ButtonStyle(
                    backgroundColor: MaterialStatePropertyAll(Colors.red)),
              ),
            ),
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: ElevatedButton.icon(
                icon: const Icon(Icons.save),
                label: const Text("Lưu"),
                onPressed: () {
                  // Đóng form
                  _saveData();
                  Navigator.of(context).pop();
                },
              ),
            ),
          ],
        );
      },
    );
  }
}
