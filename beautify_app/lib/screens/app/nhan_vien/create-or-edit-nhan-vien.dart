// ignore_for_file: public_member_api_docs, sort_constructors_first, use_build_context_synchronously
import 'package:beautify_app/screens/app/nhan_vien/models/CreateOrEditNhanSuDto.dart';
import 'package:beautify_app/screens/app/nhan_vien/models/NhanSuDto.dart';
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
    this.idNhanVien,
  }) : super(key: key);

  @override
  _CreateOrEditNhanVienModalState createState() =>
      _CreateOrEditNhanVienModalState();
}

class _CreateOrEditNhanVienModalState extends State<CreateOrEditNhanVienModal> {
  int? _genderGroup;
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  List<SuggestChucVu> _suggestChucVu = [];

  String selectedChucVu = '';
  Map<String, dynamic> nhanSuData = {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "maNhanVien": "",
    "tenNhanVien": "",
    "diaChi": "",
    "soDienThoai": "",
    "cccd": "",
    "ngaySinh": "",
    "kieuNgaySinh": 0,
    "gioiTinh": 0,
    "ngayCap": "",
    "noiCap": "",
    "avatar": null,
    "idChucVu": "3fa85f64-5858-4562-b3fc-2c963f66afa6"
  };

  void suggestChucVu() async {
    var chucVu = await NhanVienService().suggestChucVu();
    setState(() {
      _suggestChucVu = chucVu;
    });
  }

  @override
  void initState() {
    super.initState();
    suggestChucVu();
  }

  @override
  void dispose() {
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final List<Map<String, dynamic>> genderOptions = [
      {"display": "Nam", "value": 0},
      {"display": "Nữ", "value": 1},
      {"display": "Khác", "value": 2}
    ];
    NhanSuDto nhanSuDto = NhanSuDto(
        id: '',
        maNhanVien: '',
        tenNhanVien: '',
        ngaySinh: '',
        gioiTinh: 0,
        idChucVu: '');
    CreateOrEditNhanSuDto createNhanSu;
    Future<void> _saveData() async {
      if (_formKey.currentState!.validate()) {
        _formKey.currentState!.save();
        // Lưu trữ dữ liệu của tab 'Role'
        createNhanSu = CreateOrEditNhanSuDto(
            cccd: '',
            diaChi: '',
            gioiTinh: 0,
            idChucVu: '',
            maNhanVien: '',
            ngayCap: '',
            id: '',
            kieuNgaySinh: 0,
            ngaySinh: '',
            noiCap: '',
            soDienThoai: '',
            tenNhanVien: '');
        if (widget.idNhanVien == null) {
          createNhanSu.id = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        } else {
          createNhanSu.id = widget.idNhanVien.toString();
        }
        createNhanSu.avatar = null;
        createNhanSu.kieuNgaySinh = 0;
        createNhanSu.maNhanVien = "NS000002";
        createNhanSu.tenNhanVien = nhanSuData['tenNhanVien'];
        createNhanSu.diaChi = nhanSuData['diaChi'];
        createNhanSu.soDienThoai = nhanSuData['soDienThoai'];
        createNhanSu.cccd = nhanSuData['cccd'];
        createNhanSu.ngaySinh = nhanSuData['ngaySinh'];
        createNhanSu.gioiTinh = nhanSuData['gioiTinh'];
        createNhanSu.ngayCap = nhanSuData['ngayCap'];
        createNhanSu.noiCap = nhanSuData['noiCap'];
        createNhanSu.idChucVu = nhanSuData['idChucVu'].toString();
        print(createNhanSu.toJson());
        var kq = await NhanVienService().createOrEditNhanVien(createNhanSu);
        if (kq == true) {
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text("Cập nhật thành công")),
          );
          Navigator.of(context).pop();
        }
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Có lỗi xảy ra, vui lòng thử lại sau")),
        );
      }
    }

    int selectedGenderValue = genderOptions[0]['value'];
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
                                  child: Text(
                                    "Tên nhân viên",
                                    style: GoogleFonts.roboto(
                                        fontSize: 14,
                                        color: const Color(0xFF53545C),
                                        fontWeight: FontWeight.bold),
                                  )),
                              CustomTextFormFieldValidate(
                                controller: TextEditingController(
                                    text: nhanSuDto.tenNhanVien),
                                textValidate:
                                    "Tên nhân viên không được bỏ trống",
                                onSave: (value) {
                                  setState(() {
                                    nhanSuData.addAll({'tenNhanVien': value});
                                  });
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
                                            "Ngày Sinh",
                                            style: GoogleFonts.roboto(
                                                fontSize: 14,
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
                                          ),
                                          InputDatePickerFormField(
                                            firstDate: DateTime(0001),
                                            lastDate: DateTime(3000),
                                            keyboardType:
                                                TextInputType.datetime,
                                            onDateSaved: (value) => {
                                              nhanSuData.addAll({
                                                'ngaySinh': value.toString()
                                              })
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
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
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
                                                      BorderRadius.circular(8)),
                                              errorBorder: OutlineInputBorder(
                                                  borderRadius:
                                                      BorderRadius.circular(8),
                                                  borderSide: const BorderSide(
                                                      color: Colors.red)),
                                              enabledBorder: OutlineInputBorder(
                                                  borderRadius:
                                                      BorderRadius.circular(8),
                                                  borderSide: const BorderSide(
                                                      color: Colors.black)),
                                              disabledBorder:
                                                  OutlineInputBorder(
                                                      borderRadius:
                                                          BorderRadius.circular(
                                                              8),
                                                      borderSide:
                                                          const BorderSide(
                                                              color: Colors
                                                                  .black)),
                                            ),
                                            value: selectedGenderValue,
                                            onChanged: (value) => {},
                                            onSaved: (int? newValue) {
                                              setState(() {
                                                selectedGenderValue =
                                                    newValue ?? 0;
                                                nhanSuData.addAll(
                                                    {'gioiTinh': newValue});
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
                                                      const Color(0xFF53545C),
                                                  fontWeight: FontWeight.bold),
                                            ),
                                            CustomTextFormField(
                                              keyBoardType: TextInputType.phone,
                                              controller: TextEditingController(
                                                  text: nhanSuDto.soDienThoai),
                                              onSaved: (value) {
                                                setState(() {
                                                  nhanSuData.addAll(
                                                      {'soDienThoai': value});
                                                });
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
                                                color: const Color(0xFF53545C),
                                                fontWeight: FontWeight.bold),
                                          ),
                                          CustomTextFormField(
                                            keyBoardType:
                                                TextInputType.emailAddress,
                                            controller: TextEditingController(),
                                            onSaved: (value) {
                                              setState(() {});
                                            },
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
                                                    fontWeight:
                                                        FontWeight.bold),
                                              ),
                                              CustomTextFormField(
                                                keyBoardType:
                                                    TextInputType.streetAddress,
                                                controller:
                                                    TextEditingController(
                                                        text: nhanSuDto.diaChi),
                                                onSaved: (value) {
                                                  setState(() {
                                                    nhanSuData.addAll(
                                                        {'diaChi': value});
                                                  });
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
                                                      const Color(0xFF53545C),
                                                  fontWeight: FontWeight.bold),
                                            ),
                                            CustomTextFormField(
                                              keyBoardType:
                                                  TextInputType.number,
                                              controller: TextEditingController(
                                                  text: nhanSuDto.cccd),
                                              onSaved: (value) {
                                                setState(() {
                                                  nhanSuData
                                                      .addAll({'cccd': value});
                                                });
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
                                                      const Color(0xFF53545C),
                                                  fontWeight: FontWeight.bold),
                                            ),
                                            CustomTextFormField(
                                              keyBoardType:
                                                  TextInputType.streetAddress,
                                              controller: TextEditingController(
                                                  text: nhanSuDto.noiCap),
                                              onSaved: (value) {
                                                setState(() {
                                                  nhanSuData.addAll(
                                                      {'noiCap': value});
                                                });
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
                                                      const Color(0xFF53545C),
                                                  fontWeight: FontWeight.bold),
                                            ),
                                            InputDatePickerFormField(
                                              keyboardType:
                                                  TextInputType.datetime,
                                              firstDate: DateTime(0001),
                                              lastDate: DateTime(3000),
                                              onDateSaved: (value) => {
                                                setState(() {
                                                  nhanSuData.addAll({
                                                    'ngayCap': value.toString()
                                                  });
                                                })
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
                                              color: const Color(0xFF53545C),
                                              fontWeight: FontWeight.bold),
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
                                                    BorderRadius.circular(8)),
                                            errorBorder: OutlineInputBorder(
                                                borderRadius:
                                                    BorderRadius.circular(8),
                                                borderSide: const BorderSide(
                                                    color: Colors.red)),
                                            enabledBorder: OutlineInputBorder(
                                                borderRadius:
                                                    BorderRadius.circular(8),
                                                borderSide: const BorderSide(
                                                    color: Colors.black)),
                                            disabledBorder: OutlineInputBorder(
                                                borderRadius:
                                                    BorderRadius.circular(8),
                                                borderSide: const BorderSide(
                                                    color: Colors.black)),
                                          ),
                                          items: _suggestChucVu.map<
                                                  DropdownMenuItem<
                                                      SuggestChucVu>>(
                                              (SuggestChucVu value) {
                                            return DropdownMenuItem(
                                              value: value,
                                              child: Text(
                                                value.tenChucVu.toString(),
                                                style: const TextStyle(
                                                    fontSize: 16),
                                              ),
                                            );
                                          }).toList(),
                                          onSaved: (value) {
                                            setState(() {
                                              nhanSuData.addAll({
                                                'idChucVu':
                                                    value!.idChucVu.toString()
                                              });
                                            });
                                          },
                                          onChanged: (value) {},
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
                                      keyBoardType: TextInputType.multiline,
                                      controller: TextEditingController(),
                                      onSaved: (value) {
                                        setState(() {});
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
                  print(nhanSuData.toString());
                  _saveData();
                },
              ),
            ),
          ],
        );
      },
    );
  }
}
