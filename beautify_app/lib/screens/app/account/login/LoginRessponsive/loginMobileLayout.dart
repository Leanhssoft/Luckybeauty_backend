// ignore_for_file: use_build_context_synchronousl, use_build_context_synchronously
import 'package:flutter/material.dart';

import '../../../../../Service/LoginService.dart';

// ignore: camel_case_types
class loginMobileLayout extends StatefulWidget {
  const loginMobileLayout({super.key});

  @override
  State<loginMobileLayout> createState() => _loginMobileLayoutState();
}

// ignore: camel_case_types
class _loginMobileLayoutState extends State<loginMobileLayout> {
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
            child: Padding(
              padding: const EdgeInsets.all(32.0),
              child: Row(children: [
                Expanded(
                  child: Column(
                    children: [
                      const SizedBox(
                        height: 2,
                      ),
                      // image
                      Container(
                        width: 84,
                        height: 84,
                        decoration: BoxDecoration(
                          image: const DecorationImage(
                            image: AssetImage("assets/images/Lucky_beauty.jpg"),
                            fit: BoxFit.fill,
                          ),
                          borderRadius: BorderRadius.circular(12),
                        ),
                      ),
                      const SizedBox(
                        height: 10,
                      ),
                      //From
                      Form(
                        key: _formKey,
                        child: Column(
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
                                  top: 16, bottom: 8, right: 16, left: 16),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
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
                            Padding(
                              padding: const EdgeInsets.only(
                                  top: 2, bottom: 8, right: 16, left: 16),
                              child: SizedBox(
                                height: heightTenantId,
                                child: TextFormField(
                                  textInputAction: TextInputAction.done,
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
                                      errorBorder: OutlineInputBorder(
                                          borderRadius:
                                              BorderRadius.circular(15),
                                          borderSide: const BorderSide(
                                              color: Colors.red))),
                                  onTapOutside: (event) async {
                                    var result =
                                        await _loginService.isTenantAvailable(
                                            _tenantNameController.text);
                                    if (result == 0) {
                                      setState(() {
                                        findTenantResult =
                                            "Id cửa hàng không tồn tại";
                                        heightTenantId = 65;
                                      });
                                    } else {
                                      setState(() {
                                        findTenantResult = "";
                                      });
                                    }
                                  },
                                  onEditingComplete: _login,
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
                                    "Tên đăng nhập",
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
                              padding: const EdgeInsets.only(
                                  top: 2, bottom: 8, right: 16, left: 16),
                              child: SizedBox(
                                height: heightUser,
                                child: TextFormField(
                                  controller: _userNameController,
                                  decoration: InputDecoration(
                                      //prefixIcon: const Icon(Icons.account_circle),
                                      hintText: "Nhập tên đăng nhập",
                                      //labelText: "Username",
                                      labelStyle: const TextStyle(
                                          color: Colors.blue,
                                          fontSize: 20,
                                          fontWeight: FontWeight.bold),
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
                                  top: 8, bottom: 8, right: 16, left: 16),
                              child: Row(
                                crossAxisAlignment: CrossAxisAlignment.start,
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
                            Padding(
                              padding: const EdgeInsets.only(
                                  top: 2, bottom: 2, right: 16, left: 16),
                              child: SizedBox(
                                height: heightPassword,
                                child: TextFormField(
                                  controller: _passwordController,
                                  obscureText: _passwordVisible,
                                  decoration: InputDecoration(
                                      //prefixIcon: const Icon(Icons.vpn_key),
                                      suffixIcon: IconButton(
                                        icon: _passwordVisible
                                            ? const Icon(Icons.visibility)
                                            : const Icon(Icons.visibility_off),
                                        onPressed: () {
                                          setState(() {
                                            _passwordVisible =
                                                !_passwordVisible;
                                          });
                                        },
                                      ),
                                      hintText: "Nhập mật khẩu",
                                      //labelText: "Password",
                                      labelStyle: const TextStyle(
                                          color: Colors.blue,
                                          fontSize: 20,
                                          fontWeight: FontWeight.bold),
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
                                      return 'Mật khẩu không được để trống';
                                    } else if (value.length < 6) {
                                      setState(() {
                                        heightPassword = 65;
                                      });
                                      return 'Mật khẩu không được ít hơn 6 ký tự';
                                    }
                                    return null;
                                  },
                                  onEditingComplete: _login,
                                ),
                              ),
                            ),
                            Padding(
                              padding:
                                  const EdgeInsets.only(left: 2, right: 16),
                              child: Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                children: [
                                  Row(
                                    mainAxisAlignment: MainAxisAlignment.start,
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
                                        style:
                                            TextStyle(color: Color(0xff999699)),
                                      )
                                    ],
                                  ),
                                  Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
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
                            Padding(
                              padding: const EdgeInsets.only(
                                  right: 16.0, left: 16, bottom: 8, top: 2),
                              child: SizedBox(
                                width: MediaQuery.of(context).size.width,
                                height: 45,
                                child: OutlinedButton(
                                  onPressed: _login,
                                  style: ButtonStyle(
                                    shape: MaterialStateProperty.all<
                                        RoundedRectangleBorder>(
                                      RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(15),
                                      ),
                                    ),
                                    backgroundColor:
                                        MaterialStateProperty.all<Color>(
                                            const Color(0xFF7C3367)),
                                    //backgroundColor: MaterialStateProperty.all<Color>(Colors.red),
                                  ),
                                  child: const Text('Đăng nhập',
                                      style:
                                          TextStyle(color: Color(0xFFFFFFFF))),
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
                        padding: const EdgeInsets.only(
                            right: 16, left: 16, bottom: 8, top: 4),
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
                              onPressed: () =>
                                  {Navigator.pushNamed(context, "Register")},
                              child: const Text(
                                "Đăng ký",
                                style: TextStyle(
                                    color: Color(0xff0078D7),
                                    fontWeight: FontWeight.bold),
                              ),
                            )
                          ],
                        ),
                      )
                    ],
                  ),
                ),
              ]),
            ),
          ),
        ),
      ),
    );
  }
}
