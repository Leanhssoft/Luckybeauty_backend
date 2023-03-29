// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'package:beautify_app/components/CustomTextFormField.dart';
import 'package:beautify_app/components/CustomTextFormFieldValidate.dart';
import 'package:beautify_app/screens/app/admin/user/models/SuggestNhanSuDto.dart';
import 'package:beautify_app/screens/app/admin/user/models/userDto.dart';
import 'package:beautify_app/screens/app/admin/user/service/userServices.dart';

class UserForm extends StatefulWidget {
  UserDto user;
  int? id;
  final GlobalKey<FormState> formKey;
  final Function(Map<String, dynamic> userData) onUserSave;

  UserForm({
    Key? key,
    required this.user,
    this.id,
    required this.formKey,
    required this.onUserSave,
  }) : super(key: key);

  @override
  State<UserForm> createState() => _UserFormState();
}

class _UserFormState extends State<UserForm>
    with AutomaticKeepAliveClientMixin {
  @override
  bool get wantKeepAlive => true;
  void handleSave(String value) {}
  List<SuggestNhanSuDto> _suggestNhanSu = [];
  void suggestNhanSu() async {
    var nhanSu = await UserServices().suggestNhanSu();
    setState(() {
      _suggestNhanSu = nhanSu;
    });
  }

  @override
  void initState() {
    super.initState();
    suggestNhanSu();
  }

  @override
  Widget build(BuildContext context) {
    return AutomaticKeepAlive(
      child: Form(
        key: widget.formKey,
        child: SingleChildScrollView(
          child: Column(
            children: <Widget>[
              Row(
                children: [
                  Expanded(
                    flex: 2,
                    child: Padding(
                        padding: const EdgeInsets.all(16.0),
                        child: Container(
                            height: 200,
                            decoration: BoxDecoration(
                              color: Colors.amber,
                              border: Border.all(),
                              borderRadius: BorderRadius.circular(8),
                              image: const DecorationImage(
                                image: AssetImage('images/avatarProfile.jpg'),
                                fit: BoxFit.cover,
                              ),
                            ))),
                  ),
                  Expanded(
                      flex: 5,
                      child: Column(
                        children: [
                          Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                "Nhân sự đã có",
                                textAlign: TextAlign.left,
                                style: GoogleFonts.roboto(
                                  fontSize: 14,
                                  fontWeight: FontWeight.w400,
                                  color: const Color(0xff666466),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.only(left: 4),
                                child: Text(
                                  "*",
                                  textAlign: TextAlign.left,
                                  style: GoogleFonts.roboto(
                                    fontSize: 14,
                                    fontWeight: FontWeight.w400,
                                    color: const Color(0xffD70000),
                                  ),
                                ),
                              ),
                            ],
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
                              errorBorder: OutlineInputBorder(
                                  borderRadius: BorderRadius.circular(8),
                                  borderSide:
                                      const BorderSide(color: Colors.red)),
                              enabledBorder: OutlineInputBorder(
                                  borderRadius: BorderRadius.circular(8),
                                  borderSide:
                                      const BorderSide(color: Colors.black)),
                              disabledBorder: OutlineInputBorder(
                                  borderRadius: BorderRadius.circular(8),
                                  borderSide:
                                      const BorderSide(color: Colors.black)),
                            ),

                            items: _suggestNhanSu
                                .map<DropdownMenuItem<SuggestNhanSuDto>>(
                                    (SuggestNhanSuDto value) {
                              return DropdownMenuItem(
                                value: value,
                                child: Text(
                                  value.tenNhanVien.toString(),
                                  style: const TextStyle(fontSize: 16),
                                ),
                              );
                            }).toList(),
                            onSaved: (value) {
                              setState(() {
                                widget.onUserSave(
                                    {'nhanSuId': value!.id.toString()});
                              });
                            },
                            onChanged: (value) {},
                            //value: widget.user.fullName,
                          ),
                          Padding(
                            padding: const EdgeInsets.only(
                                top: 4, bottom: 2, right: 16, left: 16),
                            child: Row(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  "Họ",
                                  textAlign: TextAlign.left,
                                  style: GoogleFonts.roboto(
                                    fontSize: 14,
                                    fontWeight: FontWeight.w400,
                                    color: const Color(0xff666466),
                                  ),
                                ),
                                Padding(
                                  padding: const EdgeInsets.only(left: 4),
                                  child: Text(
                                    "*",
                                    textAlign: TextAlign.left,
                                    style: GoogleFonts.roboto(
                                      fontSize: 14,
                                      fontWeight: FontWeight.w400,
                                      color: const Color(0xffD70000),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                          ),
                          CustomTextFormFieldValidate(
                            textValidate: "Họ không được để trống",
                            controller:
                                TextEditingController(text: widget.user.name),
                            onSave: (value) => {
                              widget.onUserSave({'name': value})
                            },
                          ),
                          Padding(
                            padding: const EdgeInsets.only(
                                top: 4, bottom: 2, right: 16, left: 16),
                            child: Row(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                Text(
                                  "Tên",
                                  textAlign: TextAlign.left,
                                  style: GoogleFonts.roboto(
                                    fontSize: 14,
                                    fontWeight: FontWeight.w400,
                                    color: const Color(0xff666466),
                                  ),
                                ),
                                Padding(
                                  padding: const EdgeInsets.only(left: 4),
                                  child: Text(
                                    "*",
                                    textAlign: TextAlign.left,
                                    style: GoogleFonts.roboto(
                                      fontSize: 14,
                                      fontWeight: FontWeight.w400,
                                      color: const Color(0xffD70000),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                          ),
                          CustomTextFormFieldValidate(
                            textValidate: "Tên không được để trống",
                            controller: TextEditingController(
                                text: widget.user.surname),
                            onSave: (value) => {
                              widget.onUserSave({'surname': value})
                            },
                          ),
                        ],
                      ))
                ],
              ),
              Padding(
                padding: const EdgeInsets.only(
                    top: 4, bottom: 2, right: 16, left: 16),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Email",
                      textAlign: TextAlign.left,
                      style: GoogleFonts.roboto(
                        fontSize: 14,
                        fontWeight: FontWeight.w400,
                        color: const Color(0xff666466),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(left: 4),
                      child: Text(
                        "*",
                        textAlign: TextAlign.left,
                        style: GoogleFonts.roboto(
                          fontSize: 14,
                          fontWeight: FontWeight.w400,
                          color: const Color(0xffD70000),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              CustomTextFormFieldValidate(
                textValidate: "Email không được để trống",
                controller:
                    TextEditingController(text: widget.user.emailAddress),
                onSave: (value) => {
                  widget.onUserSave({'emailAddress': value})
                },
              ),
              Padding(
                padding: const EdgeInsets.only(
                    top: 4, bottom: 2, right: 16, left: 16),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Số điện thoại",
                      textAlign: TextAlign.left,
                      style: GoogleFonts.roboto(
                        fontSize: 14,
                        fontWeight: FontWeight.w400,
                        color: const Color(0xff666466),
                      ),
                    ),
                  ],
                ),
              ),
              CustomTextFormField(
                controller: TextEditingController(),
              ),
              Padding(
                padding: const EdgeInsets.only(
                    top: 4, bottom: 2, right: 16, left: 16),
                child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Tên truy cập",
                      textAlign: TextAlign.left,
                      style: GoogleFonts.roboto(
                        fontSize: 14,
                        fontWeight: FontWeight.w400,
                        color: const Color(0xff666466),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.only(left: 4),
                      child: Text(
                        "*",
                        textAlign: TextAlign.left,
                        style: GoogleFonts.roboto(
                          fontSize: 14,
                          fontWeight: FontWeight.w400,
                          color: const Color(0xffD70000),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              CustomTextFormFieldValidate(
                textValidate: "Tên truy cập không được để trống",
                controller: TextEditingController(text: widget.user.userName),
                onSave: (value) => {
                  widget.onUserSave({'userName': value})
                },
              ),
              if (widget.id == null)
                Padding(
                  padding: const EdgeInsets.only(
                      top: 4, bottom: 2, right: 16, left: 16),
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        "Mật khẩu",
                        textAlign: TextAlign.left,
                        style: GoogleFonts.roboto(
                          fontSize: 14,
                          fontWeight: FontWeight.w400,
                          color: const Color(0xff666466),
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(left: 4),
                        child: Text(
                          "*",
                          textAlign: TextAlign.left,
                          style: GoogleFonts.roboto(
                            fontSize: 14,
                            fontWeight: FontWeight.w400,
                            color: const Color(0xffD70000),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              if (widget.id == null)
                CustomTextFormFieldValidate(
                  controller: TextEditingController(),
                  onSave: (value) => {
                    widget.onUserSave({'password': value})
                  },
                  textValidate: "Mật khảu không được để trống",
                )
            ],
          ),
        ),
      ),
    );
  }
}
