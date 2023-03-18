// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';

import '../components/sideMenu.dart';

class LargeScreen extends StatefulWidget {
  Widget child;
  LargeScreen({
    Key? key,
    required this.child,
  }) : super(key: key);

  @override
  State<LargeScreen> createState() => _LargeScreenState();
}

class _LargeScreenState extends State<LargeScreen> {
  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        const Expanded(child: SideMenu()),
        Expanded(flex: 5, child: widget.child)
      ],
    );
  }
}
