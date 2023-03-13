import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';

import '../../../../../widgets/textFormFieldCustom.dart';

class registerMobileLayout extends StatefulWidget {
  const registerMobileLayout({super.key});

  @override
  State<registerMobileLayout> createState() => _registerMobileLayoutState();
}

class _registerMobileLayoutState extends State<registerMobileLayout> {
  final _formKey = GlobalKey<FormState>();
  final _hoTenController = TextEditingController();
  final _emailController = TextEditingController();
  final _sdtController = TextEditingController();
  final _storeNameController = TextEditingController();
  final _linhVucController = TextEditingController();
  final _diaChiController = TextEditingController();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: Center(
        child: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: SafeArea(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Padding(
                  padding: const EdgeInsets.only(
                      top: 10.0, left: 10.0, right: 10.0, bottom: 10.0),
                  child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      mainAxisSize: MainAxisSize.max,
                      children: [
                        InkWell(
                          onTap: () {
                            Navigator.pop(context);
                          },
                          child: Container(
                            padding: const EdgeInsets.all(10),
                            decoration: BoxDecoration(
                                color: Colors.grey.withOpacity(0.40),
                                borderRadius: BorderRadius.circular(30)),
                            child: const Icon(
                              Icons.arrow_back_ios_new,
                              size: 22,
                            ),
                          ),
                        ),
                        const Spacer(),
                        const Text(
                          "Đăng ký",
                          textAlign: TextAlign.center,
                          style: TextStyle(
                              fontSize: 22,
                              color: Color(0xff192F54),
                              fontWeight: FontWeight.bold),
                        ),
                        const Spacer(),
                      ]),
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Container(
                            width: 200,
                            height: 65,
                            decoration: BoxDecoration(
                              image: const DecorationImage(
                                image: AssetImage("assets/images/logo.jpg"),
                                fit: BoxFit.fill,
                              ),
                              borderRadius: BorderRadius.circular(12),
                            ),
                    ),
                  ],
                ),
                Row(
                  children: [
                    const Spacer(),
                    Expanded(
                      flex: 20,
                      child: Column(
                        children: [
                          const SizedBox(
                            height: 20,
                          ),
                          Form(
                            key: _formKey,
                            child: Column(
                              children: [
                                Padding(
                                  padding: const EdgeInsets.only(
                                      top: 4, bottom: 8, right: 16, left: 16),
                                  child: Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: const [
                                      Text(
                                        "Họ và tên",
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
                                    padding: const EdgeInsets.only(
                                        top: 2, bottom: 8, right: 16, left: 16),
                                    child: textFormFieldCustom(
                                      controller: _hoTenController,
                                      hintText: "Nhập họ và tên",
                                    )),
                                Padding(
                                  padding: const EdgeInsets.only(
                                      top: 4, bottom: 8, right: 16, left: 16),
                                  child: Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: const [
                                      Text(
                                        "Email",
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
                                    padding: const EdgeInsets.only(
                                        top: 2, bottom: 8, right: 16, left: 16),
                                    child: textFormFieldCustom(
                                      controller: _emailController,
                                      hintText: "Nhập địa chỉ email",
                                    )),
                                Row(
                                  children: [
                                    Expanded(
                                      child: Column(
                                        children: [
                                          Padding(
                                            padding: const EdgeInsets.only(
                                                top: 4,
                                                bottom: 8,
                                                right: 16,
                                                left: 16),
                                            child: Row(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.start,
                                              children: [
                                                Text(
                                                  "Tên cửa hàng",
                                                  textAlign: TextAlign.left,
                                                  style: TextStyle(
                                                      color: Colors.grey[800],
                                                      fontSize: 14,
                                                      fontWeight:
                                                          FontWeight.bold),
                                                ),
                                              ],
                                            ),
                                          ),
                                          Padding(
                                              padding: const EdgeInsets.only(
                                                  top: 2,
                                                  bottom: 8,
                                                  right: 16,
                                                  left: 16),
                                              child: textFormFieldCustom(
                                                controller: _storeNameController,
                                                hintText: "Nhập tên cửa hàng",
                                              )),
                                        ],
                                      ),
                                    ),
                                    Expanded(
                                      child: Column(
                                        children: [
                                          Padding(
                                            padding: const EdgeInsets.only(
                                                top: 4,
                                                bottom: 8,
                                                right: 16,
                                                left: 16),
                                            child: Row(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.start,
                                              children: [
                                                Text(
                                                  "Số điện thoại",
                                                  textAlign: TextAlign.left,
                                                  style: TextStyle(
                                                      color: Colors.grey[800],
                                                      fontSize: 14,
                                                      fontWeight:
                                                          FontWeight.bold),
                                                ),
                                              ],
                                            ),
                                          ),
                                          Padding(
                                              padding: const EdgeInsets.only(
                                                  top: 2,
                                                  bottom: 8,
                                                  right: 16,
                                                  left: 16),
                                              child: textFormFieldCustom(
                                                controller: _sdtController,
                                                hintText:
                                                    "Nhập số điện thoại",
                                              )),
                                        ],
                                      ),
                                    )
                                  ],
                                ),
                                Row(
                                  children: [
                                    Expanded(
                                      child: Column(
                                        children: [
                                          Padding(
                                            padding: const EdgeInsets.only(
                                                top: 4,
                                                bottom: 8,
                                                right: 16,
                                                left: 16),
                                            child: Row(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.start,
                                              children: [
                                                Text(
                                                  "Quận/Huyện",
                                                  textAlign: TextAlign.left,
                                                  style: TextStyle(
                                                      color: Colors.grey[800],
                                                      fontSize: 14,
                                                      fontWeight:
                                                          FontWeight.bold),
                                                ),
                                              ],
                                            ),
                                          ),
                                          Padding(
                                              padding: const EdgeInsets.only(
                                                  top: 2,
                                                  bottom: 8,
                                                  right: 16,
                                                  left: 16),
                                              child: textFormFieldCustom(
                                                controller: _storeNameController,
                                                hintText: "Quận/Huyện",
                                              )),
                                        ],
                                      ),
                                    ),
                                    Expanded(
                                      child: Column(
                                        children: [
                                          Padding(
                                            padding: const EdgeInsets.only(
                                                top: 4,
                                                bottom: 8,
                                                right: 16,
                                                left: 16),
                                            child: Row(
                                              crossAxisAlignment:
                                                  CrossAxisAlignment.start,
                                              children: [
                                                Text(
                                                  "Tỉnh/Thành phố",
                                                  textAlign: TextAlign.left,
                                                  style: TextStyle(
                                                      color: Colors.grey[800],
                                                      fontSize: 14,
                                                      fontWeight:
                                                          FontWeight.bold),
                                                ),
                                              ],
                                            ),
                                          ),
                                          Padding(
                                              padding: const EdgeInsets.only(
                                                  top: 2,
                                                  bottom: 8,
                                                  right: 16,
                                                  left: 16),
                                              child: textFormFieldCustom(
                                                controller: _sdtController,
                                                hintText: "Tỉnh/Thành phố",
                                              )),
                                        ],
                                      ),
                                    )
                                  ],
                                ),
                                Padding(
                                  padding: const EdgeInsets.only(
                                      top: 4, bottom: 8, right: 16, left: 16),
                                  child: Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: [
                                      Text(
                                        "Lĩnh vực quan tâm",
                                        textAlign: TextAlign.left,
                                        style: TextStyle(
                                            color: Colors.grey[800],
                                            fontSize: 14,
                                            fontWeight: FontWeight.bold),
                                      ),
                                    ],
                                  ),
                                ),
                                Padding(
                                    padding: const EdgeInsets.only(
                                        top: 2, bottom: 8, right: 16, left: 16),
                                    child: textFormFieldCustom(
                                      controller: _emailController,
                                      hintText: "Lĩnh vực quan tâm",
                                    )),
                              ],
                            ),
                          ),
                          Padding(
                            padding: const EdgeInsets.all(16.0),
                            child: SizedBox(
                              width: MediaQuery.of(context).size.width,
                              height: 45,
                              child: OutlinedButton(
                                onPressed: () async {},
                                style: ButtonStyle(
                                      shape: MaterialStateProperty.all<
                                          RoundedRectangleBorder>(
                                        RoundedRectangleBorder(
                                          borderRadius: BorderRadius.circular(15),
                                        ),
                                      ),
                                      backgroundColor:
                                          MaterialStateProperty.all<Color>(
                                              const Color(0xFF0078D7)),
                                      //backgroundColor: MaterialStateProperty.all<Color>(Colors.red),
                                    ),
                                child: const Text('Đăng ký',
                                    style: TextStyle(color: Color(0xFFFFFFFF))),
                              ),
                            ),
                          ),
                          const SizedBox(
                            height: 50,
                          )
                        ],
                      ),
                    ),
                    const Spacer(),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
