// ignore_for_file: public_member_api_docs, sort_constructors_first, curly_braces_in_flow_control_structures, use_build_context_synchronously
import 'package:beautify_app/Service/Models/SuggestNguonKhachHangDto.dart';
import 'package:beautify_app/Service/Models/SuggestNhomKhachHang.dart';
import 'package:beautify_app/Service/SuggestServices.dart';
import 'package:beautify_app/screens/app/customer/Models/CreateOrEditCustomerModel.dart';
import 'package:beautify_app/screens/app/customer/Service/KhachHangServices.dart';
import 'package:beautify_app/screens/app/customer/customerScreen.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:beautify_app/components/CustomTextFormField.dart';
import 'package:beautify_app/components/CustomTextFormFieldValidate.dart';

class CreateOrEditCustomerModal extends StatefulWidget {
  final String? idKhachHang;
  const CreateOrEditCustomerModal({
    Key? key,
    this.idKhachHang = '',
  }) : super(key: key);

  @override
  State<CreateOrEditCustomerModal> createState() =>
      _CreateOrEditCustomerModalState();
}

class _CreateOrEditCustomerModalState extends State<CreateOrEditCustomerModal> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final DateTime _dateTime = DateTime.now();
  late DateTime birthdaySelected = DateTime.now();

  late int selectedGenderValue = 1;
  late List<SuggestNhomKhachHangDto> suggestNhomKhachHang = [
    SuggestNhomKhachHangDto(id: '', tenNhomKhach: "Chọn nhóm khách")
  ];
  late List<SuggestNguonKhachHang> suggestNguonKhachHang = [
    SuggestNguonKhachHang(id: '', tenNguonKhach: "Chọn nguồn khách")
  ];
  late String? nhomKhachHangSelected;
  late String? nguonKhachHangSelected;
  late CreateOrEditCustomerModel customer = CreateOrEditCustomerModel();
  final List<Map<String, dynamic>> genderOptions = [
    {"display": "Nam", "value": 1},
    {"display": "Nữ", "value": 2},
    {"display": "Khác", "value": 0}
  ];
  void getData() async {
    var nhomKhach = await SuggestServices().suggestNhomKhachHang();
    var nguonKhach = await SuggestServices().suggestNguonKhachHang();
    setState(() {
      suggestNhomKhachHang = nhomKhach;
      suggestNguonKhachHang = nguonKhach;
    });
    if (widget.idKhachHang != '') {
      var getCustomer =
          await KhachHangServices().getKhachHang(widget.idKhachHang.toString());
      setState(() {
        customer = getCustomer;
        birthdaySelected = DateTime.parse(customer.ngaySinh.toString());
        selectedGenderValue = customer.gioiTinh!;
        nhomKhachHangSelected = customer.idNhomKhach;
        nguonKhachHangSelected = customer.idNguonKhach;
      });
    }
    print(customer.toJson());
  }

  @override
  void initState() {
    super.initState();
    getData();
  }

  Future<void> _saveData() async {
    if (widget.idKhachHang == '') {
      final kq = await KhachHangServices().CreateKhachHang(customer);
      if (kq == true) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Thêm mới thành công")),
        );
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => const KhachHangScreen()),
        );
      }
    } else {
      customer.id = widget.idKhachHang;
      final kq = await KhachHangServices().UpdateKhachHang(customer);
      if (kq == true) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Cập nhật thành công")),
        );
        Navigator.push(
          context,
          MaterialPageRoute(builder: (context) => const KhachHangScreen()),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
        builder: (BuildContext context, BoxConstraints constraints) {
      return Container(
        decoration: BoxDecoration(borderRadius: BorderRadius.circular(15)),
        child: AlertDialog(
          title: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Text(
                widget.idKhachHang == ''
                    ? "Thêm khách hàng mới"
                    : "Cập nhật thông tin khách hàng",
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
              width: 720,
              height: 600,
              child: Form(
                key: _formKey,
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Padding(
                        padding: const EdgeInsets.only(top: 15, bottom: 20),
                        child: Center(
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
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Text(
                          "Thông tin chi tiết",
                          style: GoogleFonts.roboto(
                              fontSize: 16, color: const Color(0xFF999699)),
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(right: 4.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Row(
                                      children: [
                                        Text(
                                          "Họ và tên",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFF999699)),
                                        ),
                                        Text(
                                          "*",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFFFD4027)),
                                        ),
                                      ],
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormFieldValidate(
                                      controller: TextEditingController(
                                          text: customer.tenKhachHang),
                                      hintText: "Nhập tên khách hàng",
                                      textValidate:
                                          "Tên khách hàng không được bỏ trống",
                                      onSave: (value) {
                                        setState(() {
                                          customer.tenKhachHang = value;
                                        });
                                      },
                                      onChanged: (value) {
                                        customer.tenKhachHang = value;
                                      },
                                    )
                                  ],
                                ),
                              ),
                            ),
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(left: 4),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Row(
                                      children: [
                                        Text(
                                          "Số điện thoại",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFF999699)),
                                        ),
                                        Text(
                                          "*",
                                          style: GoogleFonts.roboto(
                                              fontSize: 14,
                                              color: const Color(0xFFFD4027)),
                                        )
                                      ],
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormFieldValidate(
                                      controller: TextEditingController(
                                          text: customer.soDienThoai),
                                      hintText: "Nhập số điện thoại khách hàng",
                                      textValidate:
                                          "Số điện thoại không được bỏ trống",
                                      onSave: (value) {
                                        setState(() {
                                          customer.soDienThoai = value;
                                        });
                                      },
                                      onChanged: (value) {
                                        customer.soDienThoai = value;
                                      },
                                    )
                                  ],
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(right: 4.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Email",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      controller: TextEditingController(
                                          text: customer.email),
                                      hintText: "Nhập địa chỉ email",
                                      onSaved: (value) {
                                        setState(() {
                                          customer.email = value;
                                        });
                                      },
                                      onChanged: (value) {
                                        customer.email = value;
                                      },
                                    )
                                  ],
                                ),
                              ),
                            ),
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(left: 4),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Địa chỉ",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      controller: TextEditingController(
                                          text: customer.diaChi),
                                      hintText: "Nhập địa chỉ khách hàng",
                                      onSaved: (value) {
                                        setState(() {
                                          customer.diaChi = value;
                                        });
                                      },
                                      onChanged: (value) {
                                        customer.diaChi = value;
                                      },
                                    )
                                  ],
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(right: 4.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
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
                                      leftIcon:
                                          const Icon(Icons.date_range_outlined),
                                      controller: TextEditingController(
                                          text:
                                              "${birthdaySelected.year}-${birthdaySelected.month}-${birthdaySelected.day}"),
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
                                          customer.ngaySinh =
                                              '${birthdaySelected.year}-${birthdaySelected.month}-${birthdaySelected.day}';
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
                                  crossAxisAlignment: CrossAxisAlignment.start,
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
                                                BorderRadius.circular(8)),
                                        enabledBorder: OutlineInputBorder(
                                            borderRadius:
                                                BorderRadius.circular(8),
                                            borderSide: const BorderSide(
                                                color: Colors.black)),
                                      ),
                                      value: selectedGenderValue,
                                      onChanged: (value) => {
                                        setState(() {
                                          selectedGenderValue = value ?? 0;
                                          customer.gioiTinh = value ?? 0;
                                        })
                                      },
                                      onSaved: (int? newValue) {
                                        setState(() {
                                          selectedGenderValue = newValue ?? 0;
                                          customer.gioiTinh = newValue ?? 0;
                                        });
                                      },
                                      items: genderOptions
                                          .map<DropdownMenuItem<int>>(
                                              (Map<String, dynamic> gender) {
                                        return DropdownMenuItem<int>(
                                          value: gender['value'] ?? 0,
                                          child: Text(
                                              gender['display'].toString()),
                                        );
                                      }).toList(),
                                    )
                                  ],
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Nhóm khách",
                              style: GoogleFonts.roboto(
                                  fontSize: 14, color: const Color(0xFF999699)),
                            ),
                            const SizedBox(
                              height: 8,
                            ),
                            DropdownButtonFormField(
                              decoration: InputDecoration(
                                contentPadding:
                                    const EdgeInsets.fromLTRB(10, 10, 10, 10),
                                labelStyle: const TextStyle(
                                    color: Colors.blue,
                                    fontSize: 20,
                                    fontWeight: FontWeight.bold),
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8)),
                                enabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.black)),
                              ),
                              value: suggestNhomKhachHang[0].id ?? 'Not found',
                              onChanged: (value) => {
                                setState(() {
                                  nhomKhachHangSelected = value.toString();
                                  customer.idNhomKhach = value.toString();
                                })
                              },
                              onSaved: (String? newValue) {
                                setState(() {
                                  nhomKhachHangSelected = newValue.toString();
                                  customer.idNhomKhach = newValue.toString();
                                });
                              },
                              items: suggestNhomKhachHang
                                  .map<DropdownMenuItem<String>>(
                                      (SuggestNhomKhachHangDto item) {
                                return DropdownMenuItem<String>(
                                  value: item.id,
                                  child: Text(item.tenNhomKhach.toString()),
                                );
                              }).toList(),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Nguồn khách",
                              style: GoogleFonts.roboto(
                                  fontSize: 14, color: const Color(0xFF999699)),
                            ),
                            const SizedBox(
                              height: 8,
                            ),
                            DropdownButtonFormField(
                              decoration: InputDecoration(
                                contentPadding:
                                    const EdgeInsets.fromLTRB(10, 10, 10, 10),
                                labelStyle: const TextStyle(
                                    color: Colors.blue,
                                    fontSize: 20,
                                    fontWeight: FontWeight.bold),
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8)),
                                enabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.black)),
                              ),
                              value: suggestNguonKhachHang[0].id ?? 'not found',
                              onChanged: (value) => {
                                setState(() {
                                  nguonKhachHangSelected = value.toString();
                                  customer.idNguonKhach = value.toString();
                                })
                              },
                              onSaved: (String? newValue) {
                                setState(() {
                                  nguonKhachHangSelected = newValue.toString();
                                  customer.idNguonKhach = newValue.toString();
                                });
                              },
                              items: suggestNguonKhachHang
                                  .map<DropdownMenuItem<String>>(
                                      (SuggestNguonKhachHang item) {
                                return DropdownMenuItem<String>(
                                  value: item.id,
                                  child: Text(item.tenNguonKhach.toString()),
                                );
                              }).toList(),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Ghi chú",
                              style: GoogleFonts.roboto(
                                  fontSize: 14, color: const Color(0xFF999699)),
                            ),
                            const SizedBox(
                              height: 8,
                            ),
                            CustomTextFormField(
                              heightForm: 120,
                              controller:
                                  TextEditingController(text: customer.moTa),
                              hintText: "Ghi chú",
                              onSaved: (value) {
                                setState(() {
                                  customer.moTa = value;
                                });
                              },
                              onChanged: (value) {
                                customer.moTa = value;
                              },
                            )
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              )),
          actions: [
            Padding(
              padding: const EdgeInsets.only(right: 4, bottom: 28),
              child: SizedBox(
                width: 66,
                height: 32,
                child: ElevatedButton(
                  onPressed: () {
                    if (_formKey.currentState!.validate()) {
                      _formKey.currentState!.save();
                      _saveData();
                      Navigator.of(context).pop();
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
                  ),
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(right: 8.0, left: 4.0, bottom: 28),
              child: SizedBox(
                width: 66,
                height: 32,
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
                            fontSize: 12, color: const Color(0xFF7C3367)))),
              ),
            ),
          ],
        ),
      );
    });
  }

  Future<DateTime?> pickDate() => showDatePicker(
      context: context,
      firstDate: DateTime(1000),
      lastDate: DateTime(3000),
      locale: const Locale('vi', 'VN'),
      initialDate: _dateTime);
}
