// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';

import 'package:beautify_app/Models/PermissionAndMenu/DrawerItem.dart';
import 'package:flutter_session_manager/flutter_session_manager.dart';

class SignOut extends StatefulWidget {
  BuildContext buildContext;
  DrawerItem item;
  SignOut({
    Key? key,
    required this.buildContext,
    required this.item,
  }) : super(key: key);

  @override
  State<SignOut> createState() => _SignOutState();
}

class _SignOutState extends State<SignOut> {
  bool _isHovering = false;
  @override
  Widget build(BuildContext context) {
    return InkWell(
      onHover: (value) {
        setState(() {
          _isHovering = value;
        });
      },
      child: ListTile(
        leading: Icon(
          widget.item.icon,
          color: _isHovering ? const Color(0xFF7C3367) : const Color(0xFF666466),
        ),
        
        title:(MediaQuery.of(widget.buildContext).size.width>=1100 || MediaQuery.of(widget.buildContext).size.width<=850)?Text(
          widget.item.title.toString(),
          style: TextStyle(
            color: _isHovering ? const Color(0xFF7C3367) : const Color(0xFF344054),
            fontWeight: FontWeight.w600
          ),
        ):null,
        onTap: () async {
            await SessionManager().destroy();
            // ignore: use_build_context_synchronously
            Navigator.pushNamed(context, widget.item.route.toString());
          },
        hoverColor:const Color(0xFFF2EBF0),
      ),
    );
  }
}
