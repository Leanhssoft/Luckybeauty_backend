import 'package:flutter/material.dart';

import '../../../../../widgets/textFormFieldCustom.dart';

// ignore: camel_case_types
class registerTableDesktopLayout extends StatefulWidget {
  const registerTableDesktopLayout({super.key});

  @override
  State<registerTableDesktopLayout> createState() =>
      _registerTableDesktopLayoutState();
}

// ignore: camel_case_types
class _registerTableDesktopLayoutState
    extends State<registerTableDesktopLayout> {
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
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Expanded(
                  flex: 1,
                  child: Column(
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
                      const SizedBox(height: 30),
                      const Text(
                        "Đăng ký",
                        style: TextStyle(
                            fontSize: 32,
                            color: Color(0xff192F54),
                            fontWeight: FontWeight.bold),
                      ),
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
                              child: SizedBox(
                                width: MediaQuery.of(context).size.width * 0.45,
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
                            ),
                            Padding(
                                padding: const EdgeInsets.only(
                                    top: 2, bottom: 8, right: 16, left: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.45,
                                  child: textFormFieldCustom(
                                    controller: _hoTenController,
                                    hintText: "Nhập họ và tên",
                                  ),
                                )),
                            Padding(
                              padding: const EdgeInsets.only(
                                  top: 4, bottom: 8, right: 16, left: 16),
                              child: SizedBox(
                                width: MediaQuery.of(context).size.width * 0.45,
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
                            ),
                            Padding(
                                padding: const EdgeInsets.only(
                                    top: 2, bottom: 8, right: 16, left: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.45,
                                  child: textFormFieldCustom(
                                    controller: _emailController,
                                    hintText: "Nhập địa chỉ email",
                                  ),
                                )),
                            SizedBox(
                              width: MediaQuery.of(context).size.width * 0.45,
                              child: Row(
                                children: [
                                  Expanded(
                                    child: Column(
                                      children: [
                                        Padding(
                                          padding: const EdgeInsets.only(
                                              top: 4,
                                              bottom: 8,
                                              right: 0,
                                              left: 0),
                                          child: Row(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: const [
                                              Text(
                                                "Tên cửa hàng",
                                                textAlign: TextAlign.left,
                                                style: TextStyle(
                                                    color: Color(0xff4C4B4C),
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
                                                right: 9,
                                                left: 0),
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
                                              right: 0,
                                              left: 8),
                                          child: Row(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: const [
                                              Text(
                                                "Số điện thoại",
                                                textAlign: TextAlign.left,
                                                style: TextStyle(
                                                    color: Color(0xff4C4B4C),
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
                                                right: 0,
                                                left: 8),
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
                            ),
                            SizedBox(
                              width: MediaQuery.of(context).size.width * 0.45,
                              child: Row(
                                children: [
                                  Expanded(
                                    child: Column(
                                      children: [
                                        Padding(
                                          padding: const EdgeInsets.only(
                                              top: 4,
                                              bottom: 8,
                                              right: 0,
                                              left: 0),
                                          child: Row(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: const [
                                              Text(
                                                "Quận/Huyện",
                                                textAlign: TextAlign.left,
                                                style: TextStyle(
                                                    color: Color(0xff4C4B4C),
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
                                                right: 8,
                                                left: 0),
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
                                              right: 0,
                                              left: 8),
                                          child: Row(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: const [
                                              Text(
                                                "Tỉnh/Thành phố",
                                                textAlign: TextAlign.left,
                                                style: TextStyle(
                                                    color: Color(0xff4C4B4C),
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
                                                right: 0,
                                                left: 8),
                                            child: textFormFieldCustom(
                                              controller: _sdtController,
                                              hintText: "Tỉnh/Thành phố",
                                            )),
                                      ],
                                    ),
                                  )
                                ],
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.only(
                                  top: 4, bottom: 8, right: 16, left: 16),
                              child: SizedBox(
                                width: MediaQuery.of(context).size.width * 0.45,
                                child: Row(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: const [
                                    Text(
                                      "Lĩnh vực quan tâm",
                                      textAlign: TextAlign.left,
                                      style: TextStyle(
                                          color: Color(0xff4C4B4C),
                                          fontSize: 14,
                                          fontWeight: FontWeight.bold),
                                    ),
                                  ],
                                ),
                              ),
                            ),
                            Padding(
                                padding: const EdgeInsets.only(
                                    top: 2, bottom: 8, right: 16, left: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.45,
                                  child: textFormFieldCustom(
                                    controller: _emailController,
                                    hintText: "Lĩnh vực quan tâm",
                                  ),
                                )),
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(16.0),
                        child: SizedBox(
                          width: MediaQuery.of(context).size.width * 0.45,
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
                              backgroundColor: MaterialStateProperty.all<Color>(
                                  const Color(0xFF0078D7)),
                              //backgroundColor: MaterialStateProperty.all<Color>(Colors.red),
                            ),
                            child: const Text('Đăng ký',
                                style: TextStyle(color: Color(0xFFFFFFFF))),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
