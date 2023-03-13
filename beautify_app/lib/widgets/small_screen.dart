// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';

class SmallScreen extends StatefulWidget {
  final Widget route;
  const SmallScreen({
    Key? key,
    required this.route,
  }) : super(key: key);

  @override
  State<SmallScreen> createState() => _SmallScreenState();
}

class _SmallScreenState extends State<SmallScreen> {
  @override
  Widget build(BuildContext context) {
    return Container(
      constraints:const BoxConstraints.expand(),
      color: Colors.white,
      child: widget.route,
    );
  }
}