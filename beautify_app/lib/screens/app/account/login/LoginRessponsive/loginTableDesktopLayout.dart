// ignore_for_file: use_build_context_synchronously

import 'package:beautify_app/screens/main/HomeScreen.dart';
import 'package:flutter/material.dart';

import '../../../../../Service/LoginService.dart';

// ignore: camel_case_types
class loginTableDesktopLayout extends StatefulWidget {
  const loginTableDesktopLayout({super.key});

  @override
  State<loginTableDesktopLayout> createState() => _loginMobileLayoutState();
}

// ignore: camel_case_types
class _loginMobileLayoutState extends State<loginTableDesktopLayout> {
  final _formKey = GlobalKey<FormState>();
  bool rememberMe = false;
  final _tenantNameController = TextEditingController();
  final _userNameController = TextEditingController();
  final _passwordController = TextEditingController();
  String findTenantResult = "";
  bool _passwordVisible = true;
  double? heightUser = 45;
  double? heightTenantId = 45;
  double? heightPassword = 45;
  final _loginService = LoginService();

  void _login() async {
    if (_formKey.currentState!.validate()) {
      // If the form is valid, display a snackbar. In the real world,
      // you'd often call a server or save the information in a database.
      var result = await LoginService().login(_tenantNameController.text,
          _userNameController.text, _passwordController.text, rememberMe);

      if (result == true) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
              content: Text(
                'Đăng nhập thành công !',
                textAlign: TextAlign.center,
              ),
              backgroundColor: Color(0xFF64B5F6)),
        );
        Navigator.pushNamed(context, "/home");
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
              content: Text(
                'Tài khoản hoặc mật khẩu không chính xác !',
                textAlign: TextAlign.center,
              ),
              backgroundColor: Color(0xFFA80707)),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: SingleChildScrollView(
          child: SafeArea(
            child: Row(children: [
              Expanded(
                child: Padding(
                  padding: const EdgeInsets.all(32),
                  child: Center(
                    child: Column(
                      children: [
                        // image
                        Container(
                          width: 64,
                          height: 64,
                          decoration: BoxDecoration(
                            image: const DecorationImage(
                              image:
                                  AssetImage("assets/images/Lucky_beauty.jpg"),
                              fit: BoxFit.fill,
                            ),
                            borderRadius: BorderRadius.circular(12),
                          ),
                        ),

                        const SizedBox(
                          height: 20,
                        ),
                        //From
                        Form(
                          key: _formKey,
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              const Padding(
                                padding: EdgeInsets.only(
                                    top: 16, bottom: 8, right: 16, left: 16),
                                child: Text(
                                  "Đăng nhập",
                                  textAlign: TextAlign.left,
                                  style: TextStyle(
                                      color: Color(0xff192F54),
                                      fontSize: 32,
                                      fontWeight: FontWeight.bold),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.only(
                                    top: 16, bottom: 2, right: 16, left: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: Row(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: const [
                                      Text(
                                        "ID Cửa hàng",
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
                                  height: heightTenantId,
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: TextFormField(
                                    keyboardType: TextInputType.text,
                                    controller: _tenantNameController,
                                    decoration: InputDecoration(
                                      //prefixIcon: const Icon(Icons.store),
                                      hintText: "Nhập tên Id",
                                      //labelText: "Tenant Name",
                                      labelStyle: const TextStyle(
                                          color: Colors.blue,
                                          fontSize: 20,
                                          fontWeight: FontWeight.bold),
                                      border: OutlineInputBorder(
                                          borderRadius:
                                              BorderRadius.circular(15)),
                                    ),
                                    validator: (value) {
                                      if (findTenantResult.isNotEmpty) {
                                        setState(() {
                                          heightTenantId = 65;
                                        });
                                        return findTenantResult;
                                      }
                                      return null;
                                    },
                                    onEditingComplete: _login,
                                  ),
                                ),
                              ),
                              // Padding(
                              //   padding: const EdgeInsets.only(top: 4),
                              //   child: Text(findTenantResult,
                              //       textAlign: TextAlign.left,
                              //       style: const TextStyle(
                              //           color: Colors.red, fontSize: 12)),
                              // ),
                              Padding(
                                padding: const EdgeInsets.only(
                                    top: 2, bottom: 2, right: 16, left: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: Row(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: const [
                                      Text(
                                        "Tên đăng nhập",
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
                                  height: heightUser,
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: TextFormField(
                                    controller: _userNameController,
                                    decoration: InputDecoration(
                                        //prefixIcon: const Icon(Icons.account_circle),
                                        hintText: "Nhập tên đăng nhập",
                                        border: OutlineInputBorder(
                                            borderRadius:
                                                BorderRadius.circular(15)),
                                        errorBorder: OutlineInputBorder(
                                            borderRadius:
                                                BorderRadius.circular(15),
                                            borderSide: const BorderSide(
                                                color: Colors.red))),
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        setState(() {
                                          heightUser = 65;
                                        });
                                        return 'Tên đăng nhập không được để trống';
                                      }
                                      return null;
                                    },
                                    onEditingComplete: _login,
                                  ),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.only(
                                    top: 2, bottom: 2, right: 16, left: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: Row(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    children: const [
                                      Text(
                                        "Mật khẩu",
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
                                    top: 2, bottom: 2, right: 16, left: 16),
                                child: SizedBox(
                                  height: heightPassword,
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: TextFormField(
                                    controller: _passwordController,
                                    obscureText: _passwordVisible,
                                    decoration: InputDecoration(
                                        //prefixIcon: const Icon(Icons.vpn_key),
                                        suffixIcon: SizedBox(
                                          width: 10,
                                          height: 10,
                                          child: IconButton(
                                            icon: _passwordVisible
                                                ? const Icon(Icons.visibility)
                                                : const Icon(
                                                    Icons.visibility_off),
                                            onPressed: () {
                                              setState(() {
                                                _passwordVisible =
                                                    !_passwordVisible;
                                              });
                                            },
                                          ),
                                        ),
                                        hintText: "Nhập mật khẩu",
                                        border: OutlineInputBorder(
                                            borderRadius:
                                                BorderRadius.circular(15)),
                                        errorBorder: OutlineInputBorder(
                                            borderRadius:
                                                BorderRadius.circular(15),
                                            borderSide: const BorderSide(
                                                color: Colors.red))),
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        setState(() {
                                          heightPassword = 65;
                                        });
                                        return 'Mật khẩu không được để trống';
                                      } else if (value.length < 6) {
                                        setState(() {
                                          heightPassword = 65;
                                        });
                                        return 'Mật khẩu không được ít hơn 6 ký tự';
                                      }
                                      return null;
                                    },
                                    onEditingComplete: _login,
                                  ),
                                ),
                              ),
                              Padding(
                                padding:
                                    const EdgeInsets.only(left: 16, right: 16),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  child: Row(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                    children: [
                                      Row(
                                        mainAxisAlignment:
                                            MainAxisAlignment.start,
                                        children: [
                                          Checkbox(
                                            value: rememberMe,
                                            onChanged: (value) {
                                              setState(() {
                                                rememberMe = value!;
                                              });
                                            },
                                          ),
                                          const Text(
                                            "Ghi nhớ ?",
                                            style: TextStyle(
                                                color: Color(0xff999699)),
                                          )
                                        ],
                                      ),
                                      Row(
                                        mainAxisAlignment:
                                            MainAxisAlignment.end,
                                        children: const [
                                          Text(
                                            "Quên mật khẩu ?",
                                            style: TextStyle(
                                                color: Color(0xff0078D7),
                                                fontSize: 14,
                                                fontWeight: FontWeight.bold),
                                          )
                                        ],
                                      )
                                    ],
                                  ),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.all(16.0),
                                child: SizedBox(
                                  width:
                                      MediaQuery.of(context).size.width * 0.35,
                                  height: 45,
                                  child: OutlinedButton(
                                    onPressed: _login,
                                    style: ButtonStyle(
                                      shape: MaterialStateProperty.all<
                                          RoundedRectangleBorder>(
                                        RoundedRectangleBorder(
                                          borderRadius:
                                              BorderRadius.circular(15),
                                        ),
                                      ),
                                      backgroundColor:
                                          MaterialStateProperty.all<Color>(
                                              const Color(0xFF7C3367)),
                                      //backgroundColor: MaterialStateProperty.all<Color>(Colors.red),
                                    ),
                                    child: const Text('Đăng nhập',
                                        style: TextStyle(
                                            color: Color(0xFFFFFFFF))),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                        Padding(
                          padding: const EdgeInsets.all(16),
                          child: Text(
                            "Tổng đài hỗ trợ : 0247 303 9333 - 0936 363 069",
                            style: TextStyle(
                                color: Colors.grey[800],
                                fontWeight: FontWeight.bold),
                          ),
                        ),
                        Padding(
                          padding: const EdgeInsets.all(16),
                          child: SizedBox(
                            width: MediaQuery.of(context).size.width * 0.35,
                            child: Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              children: [
                                Text(
                                  "Bạn chưa có tài khoản ? ",
                                  style: TextStyle(
                                      color: Colors.grey[600],
                                      fontWeight: FontWeight.bold),
                                ),
                                TextButton(
                                  onPressed: () => {
                                    Navigator.pushNamed(context, "/register")
                                  },
                                  child: const Text(
                                    "Đăng ký",
                                    style: TextStyle(
                                        color: Color(0xff0078D7),
                                        fontWeight: FontWeight.bold),
                                  ),
                                )
                              ],
                            ),
                          ),
                        )
                      ],
                    ),
                  ),
                ),
              ),
            ]),
          ),
        ),
      ),
    );
  }
}
