import 'package:beautify_app/screens/app/account/register/RegisterResponsive/registerMobileLayout.dart';
import 'package:beautify_app/screens/app/account/register/RegisterResponsive/registerTableDesktopLayout.dart';
import 'package:flutter/material.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  @override
  Widget build(BuildContext context) {
    if (MediaQuery.of(context).size.width < 850) {
      return const registerMobileLayout();
    }
    return const registerTableDesktopLayout();
  }
}
