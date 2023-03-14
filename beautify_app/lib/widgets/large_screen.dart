// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/helper/local_navigator.dart';
import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';

import '../components/sideMenu.dart';

class LargeScreen extends StatefulWidget {
  
  const LargeScreen({
    Key? key,
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
        Expanded(flex: 5,child: localNavigator())
      ],
    );
  }
}
