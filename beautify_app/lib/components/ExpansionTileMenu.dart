// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/routing/routes.dart';
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';

import 'package:beautify_app/Models/PermissionAndMenu/DrawerItem.dart';

import 'ListTitleMenu.dart';

class ExpansionTileMenu extends StatefulWidget {
  BuildContext buildContext;
  DrawerItem item;
  ExpansionTileMenu({Key? key, required this.buildContext, required this.item})
      : super(key: key);

  @override
  State<ExpansionTileMenu> createState() => _ExpansionTileMenuState();
}

class _ExpansionTileMenuState extends State<ExpansionTileMenu> {
  bool _isHovering = false;
  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {},
      onHover: (value) {
        setState(() {
          _isHovering = value;
        });
      },
      child: ExpansionTile(
        backgroundColor: const Color(0xFFF2EBF0),
        leading: Icon(
          widget.item.icon,
          color:
              _isHovering ? const Color(0xFF7C3367) : const Color(0xFF666466),
        ),
        title: (MediaQuery.of(widget.buildContext).size.width >= 1100 ||
                MediaQuery.of(widget.buildContext).size.width <= 850)
            ? Text(
                widget.item.title.toString(),
                style: TextStyle(
                    color: _isHovering
                        ? const Color(0xFF7C3367)
                        : const Color(0xFF344054),
                    fontWeight: FontWeight.w600),
              )
            : const Text(""),
        children: [
          for (var listTitle in widget.item.children!)
            ListTileMenu(buildContext: widget.buildContext, item: listTitle)
        ],
      ),
    );
  }
}
