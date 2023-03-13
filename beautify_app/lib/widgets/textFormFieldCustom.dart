// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';

class textFormFieldCustom extends StatelessWidget {
  final TextEditingController controller;
  final String? hintText;
  final Icon? prefixIcon;
  const textFormFieldCustom({
    Key? key,
    required this.controller,
    this.hintText,
    this.prefixIcon,
  }) : super(key: key);
  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 45,
      child: TextFormField(
        controller: controller,
        decoration: InputDecoration(
          prefixIcon: prefixIcon,
          hintText: hintText,
          contentPadding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
          //labelText: "Username",
          labelStyle: const TextStyle(
              color: Colors.blue, fontSize: 20, fontWeight: FontWeight.bold),
          border: OutlineInputBorder(borderRadius: BorderRadius.circular(15)),
          errorBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(15),
              borderSide: const BorderSide(color: Colors.red)),
          enabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(15),
              borderSide: const BorderSide(color: Colors.black)),
          disabledBorder: OutlineInputBorder(
              borderRadius: BorderRadius.circular(15),
              borderSide: const BorderSide(color: Colors.black)),
        ),
        validator: (value) {
          if (value == null || value.isEmpty) {
            return 'Không được để trống dữ liệu';
          }
          return null;
        },
      ),
    );
  }
}
