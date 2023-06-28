// ignore_for_file: public_member_api_docs, sort_constructors_first, use_build_context_synchronously
import 'package:beautify_app/screens/app/nhan_vien/models/CreateOrEditNhanSuDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/nhanhVienScreen.dart';
import 'package:beautify_app/screens/app/nhan_vien/services/nhanVienServices.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/components/CustomTextFormFieldValidate.dart';

import '../../../components/CustomTextFormField.dart';
import 'models/SuggestChucVuDto.dart';

class CreateOrEditNhanVienModal extends StatefulWidget {
  String? idNhanVien;
  CreateOrEditNhanVienModal({
    Key? key,
    this.idNhanVien = '',
  }) : super(key: key);

  @override
  _CreateOrEditNhanVienModalState createState() =>
      _CreateOrEditNhanVienModalState();
}

class _CreateOrEditNhanVienModalState extends State<CreateOrEditNhanVienModal> {
  int? _genderGroup;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  List<SuggestChucVu> _suggestChucVu = [
    SuggestChucVu(idChucVu: '', tenChucVu: "Không có chức vụ được hiển thị")
  ];
  late DateTime birthdaySelected = DateTime.now();
  late DateTime ngayCapCCCD = DateTime.now();
  String selectedChucVu = '';
  final List<Map<String, dynamic>> genderOptions = [
    {"display": "Nam", "value": 1},
    {"display": "Nữ", "value": 2},
    {"display": "Khác", "value": 0}
  ];
  int selectedGenderValue = 0;
  CreateOrEditNhanSuDto createNhanVien = CreateOrEditNhanSuDto();

  void suggestChucVu() async {
    var chucVu = await NhanVienService().suggestChucVu();
    setState(() {
      _suggestChucVu = chucVu;
    });
  }

  void getNhanVien() async {
    var nhanVien =
        await NhanVienService().getNhanVien(widget.idNhanVien.toString());
    setState(() {
      createNhanVien = nhanVien;
      selectedGenderValue = nhanVien.gioiTinh ?? 0;
      selectedChucVu = nhanVien.idChucVu!;
      birthdaySelected = DateTime.parse(nhanVien.ngaySinh.toString());
      ngayCapCCCD = DateTime.parse(nhanVien.ngayCap.toString());
    });
  }

  @override
  void initState() {
    super.initState();
    suggestChucVu();
    if (widget.idNhanVien != '' || widget.idNhanVien != null) {
      getNhanVien();
    }
  }

  @override
  void dispose() {
    super.dispose();
  }

  Future<void> _saveData() async {
    var kq = await NhanVienService().createOrEditNhanVien(createNhanVien);
    if (kq == true) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(
            widget.idNhanVien == ''
                ? "Thêm mới nhân viên thành công!"
                : "Cập nhật thông tin nhân viên thành công!",
            style: const TextStyle(color: Colors.white),
          ),
          backgroundColor: const Color(0xFF90CAF9),
        ),
      );
      Navigator.of(context).pop();
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text("Có lỗi xảy ra, vui lòng thử lại sau")),
      );
    }
    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => const NhanVienScreen()),
    );
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (BuildContext context, BoxConstraints constraints) {
        return AlertDialog(
          title: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Text(
                widget.idNhanVien == ''
                    ? "Thêm nhân viên"
                    : "Cập nhật thông tin nhân viên",
                style: GoogleFonts.roboto(
                    color: const Color(0xFF333233), fontSize: 24),
              ),
              Container(
                width: 32,
                height: 32,
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(8),
                    border:
                        Border.all(color: const Color(0xFF999699), width: 2)),
                child: InkWell(
                  onTap: () {
                    Navigator.pop(context);
                  },
                  child: const Icon(
                    Icons.close,
                    color: Color(0xFF999699),
                    size: 16,
                  ),
                ),
              )
            ],
          ),
          content: SizedBox(
            width: 660,
            child: Form(
              key: _formKey,
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
                                  child: Text("Tên nhân viên",
                                      style: GoogleFonts.roboto(
                                        fontSize: 14,
                                        color: const Color(0xFF999699),
                                      ))),
                              const SizedBox(
                                height: 8,
                              ),
                              CustomTextFormFieldValidate(
                                controller: TextEditingController(
                                    text: createNhanVien.tenNhanVien),
                                hintText: "Nhập tên nhân viên",
                                textValidate:
                                    "Tên nhân viên không được bỏ trống",
                                onSave: (value) {
                                  setState(() {
                                    createNhanVien.tenNhanVien = value;
                                  });
                                },
                                onChanged: (value) {
                                  createNhanVien.tenNhanVien = value;
                                },
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
                                            "Ngày sinh",
                                            style: GoogleFonts.roboto(
                                                fontSize: 14,
                                                color: const Color(0xFF999699)),
                                          ),
                                          const SizedBox(
                                            height: 8,
                                          ),
                                          CustomTextFormField(
                                            leftIcon: const Icon(
                                                Icons.date_range_outlined),
                                            controller: TextEditingController(
                                                text:
                                                    '${birthdaySelected.day}/${birthdaySelected.month}/${birthdaySelected.year}'),
                                            hintText: "Chọn ngày",
                                            onTab: () async {
                                              final date = await pickDate();
                                              if (date != null) {
                                                setState(() {
                                                  birthdaySelected = date;
                                                });
                                              }
                                            },
                                            onSaved: (value) {
                                              setState(() {
                                                createNhanVien.ngaySinh = value;
                                              });
                                            },
                                          ),
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
                                                color: const Color(0xFF999699)),
                                          ),
                                          const SizedBox(
                                            height: 8,
                                          ),
                                          DropdownButtonFormField(
                                            decoration: InputDecoration(
                                              contentPadding:
                                                  const EdgeInsets.fromLTRB(
                                                      10, 10, 10, 10),
                                              labelStyle: const TextStyle(
                                                  color: Colors.blue,
                                                  fontSize: 20,
                                                  fontWeight: FontWeight.bold),
                                              border: OutlineInputBorder(
                                                  borderRadius:
                                                      BorderRadius.circular(8),
                                                  borderSide: const BorderSide(
                                                      color: Colors.black)),
                                              errorBorder: OutlineInputBorder(
                                                  borderRadius:
                                                      BorderRadius.circular(8),
                                                  borderSide: const BorderSide(
                                                      color: Colors.red)),
                                            ),
                                            value: selectedGenderValue,
                                            onChanged: (value) => {
                                              selectedGenderValue = value ?? 0,
                                              setState(
                                                () => createNhanVien.gioiTinh =
                                                    value,
                                              )
                                            },
                                            onSaved: (int? newValue) {
                                              setState(() {
                                                selectedGenderValue =
                                                    newValue ?? 0;
                                                createNhanVien.gioiTinh =
                                                    newValue;
                                              });
                                            },
                                            items: genderOptions.map<
                                                    DropdownMenuItem<int>>(
                                                (Map<String, dynamic> gender) {
                                              return DropdownMenuItem<int>(
                                                value: gender['value'] ?? 0,
                                                child: Text(gender['display']
                                                    .toString()),
                                              );
                                            }).toList(),
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
                                        padding:
                                            const EdgeInsets.only(right: 4),
                                        child: Column(
                                          crossAxisAlignment:
                                              CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              "Số điện thoại",
                                              style: GoogleFonts.roboto(
                                                  fontSize: 14,
                                                  color:
                                                      const Color(0xFF999699)),
                                            ),
                                            const SizedBox(
                                              height: 8,
                                            ),
                                            CustomTextFormField(
                                              keyBoardType: TextInputType.phone,
                                              hintText: "Nhập số điện thoại",
                                              controller: TextEditingController(
                                                  text: createNhanVien
                                                      .soDienThoai),
                                              onSaved: (value) {
                                                setState(() {
                                                  createNhanVien.soDienThoai =
                                                      value;
                                                });
                                              },
                                              onChanged: (value) {
                                                createNhanVien.soDienThoai =
                                                    value;
                                              },
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
                                                color: const Color(0xFF999699)),
                                          ),
                                          const SizedBox(
                                            height: 8,
                                          ),
                                          CustomTextFormField(
                                            keyBoardType:
                                                TextInputType.emailAddress,
                                            hintText: "Nhập địa chỉ email",
                                            controller: TextEditingController(),
                                            onSaved: (value) {},
                                            onChanged: (value) {},
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
                                                    color: const Color(
                                                        0xFF999699)),
                                              ),
                                              const SizedBox(
                                                height: 8,
                                              ),
                                              CustomTextFormField(
                                                keyBoardType:
                                                    TextInputType.streetAddress,
                                                hintText:
                                                    "Nhập địa chỉ khách hàng",
                                                controller:
                                                    TextEditingController(
                                                        text: createNhanVien
                                                            .diaChi),
                                                onSaved: (value) {
                                                  setState(() {
                                                    createNhanVien.diaChi =
                                                        value;
                                                  });
                                                },
                                                onChanged: (value) {
                                                  createNhanVien.diaChi = value;
                                                },
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
                                                  color:
                                                      const Color(0xFF999699)),
                                            ),
                                            const SizedBox(
                                              height: 8,
                                            ),
                                            CustomTextFormField(
                                              keyBoardType:
                                                  TextInputType.number,
                                              hintText: "Nhập số CCCD",
                                              controller: TextEditingController(
                                                  text: createNhanVien.cccd),
                                              onSaved: (value) {
                                                setState(() {
                                                  createNhanVien.cccd = value;
                                                });
                                              },
                                              onChanged: (value) {
                                                createNhanVien.cccd = value;
                                              },
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
                                                  color:
                                                      const Color(0xFF999699)),
                                            ),
                                            const SizedBox(
                                              height: 8,
                                            ),
                                            CustomTextFormField(
                                              keyBoardType:
                                                  TextInputType.streetAddress,
                                              hintText: "Nhập nơi cấp",
                                              controller: TextEditingController(
                                                  text: createNhanVien.noiCap),
                                              onSaved: (value) {
                                                setState(() {
                                                  createNhanVien.noiCap = value;
                                                });
                                              },
                                              onChanged: (value) {
                                                createNhanVien.noiCap = value;
                                              },
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
                                                  color:
                                                      const Color(0xFF999699)),
                                            ),
                                            const SizedBox(
                                              height: 8,
                                            ),
                                            CustomTextFormField(
                                              leftIcon: const Icon(
                                                  Icons.date_range_outlined),
                                              controller: TextEditingController(
                                                  text:
                                                      "${ngayCapCCCD.day}/${ngayCapCCCD.month}-${ngayCapCCCD.year}"),
                                              hintText: "Chọn ngày",
                                              onTab: () async {
                                                final date = await pickDate();
                                                setState(() {
                                                  ngayCapCCCD = date!;
                                                  createNhanVien.ngayCap =
                                                      date.toString();
                                                });
                                              },
                                              onSaved: (value) {
                                                setState(() {
                                                  createNhanVien.ngayCap =
                                                      value;
                                                });
                                              },
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
                                              color: const Color(0xFF999699)),
                                        ),
                                        const SizedBox(
                                          height: 8,
                                        ),
                                        DropdownButtonFormField(
                                          decoration: InputDecoration(
                                            contentPadding:
                                                const EdgeInsets.fromLTRB(
                                                    10, 10, 10, 10),
                                            labelStyle: const TextStyle(
                                                color: Colors.blue,
                                                fontSize: 20,
                                                fontWeight: FontWeight.bold),
                                            border: OutlineInputBorder(
                                                borderRadius:
                                                    BorderRadius.circular(8),
                                                borderSide: const BorderSide(
                                                    color: Colors.black)),
                                            errorBorder: OutlineInputBorder(
                                                borderRadius:
                                                    BorderRadius.circular(8),
                                                borderSide: const BorderSide(
                                                    color: Colors.red)),
                                          ),
                                          value: _suggestChucVu[0].idChucVu,
                                          items: _suggestChucVu
                                              .map<DropdownMenuItem<String>>(
                                                  (SuggestChucVu value) {
                                            return DropdownMenuItem(
                                              value: value.idChucVu.toString(),
                                              child: Text(
                                                value.tenChucVu.toString(),
                                                style: const TextStyle(
                                                    fontSize: 16),
                                              ),
                                            );
                                          }).toList(),
                                          onSaved: (value) {
                                            setState(() {
                                              createNhanVien.idChucVu = value;
                                            });
                                          },
                                          onChanged: (value) {
                                            createNhanVien.idChucVu = value;
                                          },
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
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      keyBoardType: TextInputType.multiline,
                                      hintText: "Nhập ghi chú",
                                      controller: TextEditingController(),
                                      onSaved: (value) {
                                        setState(() {
                                          createNhanVien.ghiChu = value;
                                        });
                                      },
                                      onChanged: (value) {
                                        createNhanVien.ghiChu = value;
                                      },
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
          ),
          actions: [
            Padding(
                padding: const EdgeInsets.all(8.0),
                child: ElevatedButton(
                    onPressed: () {
                      if (_formKey.currentState!.validate()) {
                        _formKey.currentState!.save();
                        _saveData();
                      }
                    },
                    style: const ButtonStyle(
                      backgroundColor:
                          MaterialStatePropertyAll(Color(0xFF7C3367)),
                    ),
                    child: Text(
                      "Lưu",
                      style: GoogleFonts.roboto(
                          fontSize: 12, color: const Color(0xFFFFFFFF)),
                    ))),
            Padding(
                padding: const EdgeInsets.all(8.0),
                child: ElevatedButton(
                    onPressed: () {
                      // Đóng form
                    },
                    style: ButtonStyle(
                        backgroundColor:
                            const MaterialStatePropertyAll(Colors.white),
                        shape:
                            MaterialStateProperty.all<RoundedRectangleBorder>(
                                const RoundedRectangleBorder(
                                    side:
                                        BorderSide(color: Color(0xFF7C3367))))),
                    child: Text("Hủy",
                        style: GoogleFonts.roboto(
                            fontSize: 12, color: const Color(0xFF7C3367))))),
          ],
        );
      },
    );
  }

  Future<DateTime?> pickDate() => showDatePicker(
      context: context,
      firstDate: DateTime(1000),
      lastDate: DateTime(3000),
      locale: const Locale('vi', 'VN'),
      initialDate: DateTime.now());
}
