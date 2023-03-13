// ignore_for_file: use_build_context_synchronously

import 'package:beautify_app/screens/app/account/login/LoginRessponsive/loginMobileLayout.dart';
import 'package:beautify_app/screens/app/account/login/LoginRessponsive/loginTableDesktopLayout.dart';
import 'package:flutter/material.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  // @override
  // void initState() {
  //   super.initState();
  //   destroyAllSession();
  // }

  // Future<void> destroyAllSession() async {
  //   await SessionManager().destroy();
  // }

  @override
  Widget build(BuildContext context) {
    if (MediaQuery.of(context).size.width < 850) {
      return const loginMobileLayout();
    }
    return const loginTableDesktopLayout();
  }
}
