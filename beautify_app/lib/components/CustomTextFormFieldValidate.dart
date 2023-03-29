// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';

class CustomTextFormFieldValidate extends StatelessWidget {
  final TextEditingController controller;
  final String textValidate;
  final String? hintText;
  final TextInputType? keyboardType;
  final double? heightForm;
  final Function(String?)? onSave;
  final Function()? onTab;
  final Icon? leftIcon;
  final Icon? rightIcon;
  const CustomTextFormFieldValidate({
    Key? key,
    required this.controller,
    required this.textValidate,
    this.hintText,
    this.keyboardType,
    this.heightForm,
    this.onSave,
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
        keyboardType: keyboardType,
        decoration: InputDecoration(
          hintText: hintText,
          prefixIcon: leftIcon,
          suffixIcon: rightIcon,
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
        validator: (value) {
          if (value == null || value.isEmpty) {
            return textValidate.toString();
          }
          return null;
        },
        onSaved: onSave,
        onTap: onTab,
      ),
    );
  }
}
