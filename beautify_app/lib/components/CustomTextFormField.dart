// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';

class CustomTextFormField extends StatelessWidget {
  final TextEditingController controller;
  final String? hintText;
  final TextInputType? keyBoardType;
  final double? heightForm;
  final Function(String?)? onSaved;
  final Function()? onTab;
  final Icon? leftIcon;
  final Icon? rightIcon;
  const CustomTextFormField({
    Key? key,
    required this.controller,
    this.hintText,
    this.keyBoardType,
    this.heightForm,
    this.onSaved,
    this.onTab,
    this.leftIcon,
    this.rightIcon,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: heightForm ?? 48,
      child: TextFormField(
        controller: controller,
        keyboardType: keyBoardType,
        decoration: InputDecoration(
          prefixIcon: leftIcon,
          suffixIcon: rightIcon,
          hintText: hintText,
          contentPadding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
          labelStyle: const TextStyle(
              color: Colors.blue, fontSize: 20, fontWeight: FontWeight.bold),
          border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
          errorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: Colors.red)),
          enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: Colors.black)),
          disabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(8),
              borderSide: const BorderSide(color: Colors.black)),
        ),
        onSaved: onSaved,
        onTap: onTab,
      ),
    );
  }
}
