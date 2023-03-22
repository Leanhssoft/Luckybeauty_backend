// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'package:beautify_app/Models/PermissionAndMenu/DrawerItem.dart';
import 'package:beautify_app/routing/routes.dart';

import '../helper/responsivesLayout.dart';

class ListTileMenu extends StatefulWidget {
  BuildContext buildContext;
  DrawerItem item;
  ListTileMenu({Key? key, required this.buildContext, required this.item})
      : super(key: key);

  @override
  State<ListTileMenu> createState() => _ListTileMenuState();
}

class _ListTileMenuState extends State<ListTileMenu> {
  bool _isHovering = false;
  String routeSelected = overviewPageRoute;
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
          color: (routeSelected == widget.item.route || _isHovering)
              ? const Color(0xFF7C3367)
              : const Color(0xFF666466),
        ),
        title: (MediaQuery.of(widget.buildContext).size.width >= 1100 ||
                MediaQuery.of(widget.buildContext).size.width <= 850)
            ? Text(
                widget.item.title.toString(),
                style: TextStyle(
                    color: (routeSelected == widget.item.route || _isHovering)
                        ? const Color(0xFF7C3367)
                        : const Color(0xFF344054),
                    fontWeight: FontWeight.w600),
              )
            : null,
        onTap: () {
          if (kDebugMode) {
            print(routeSelected);
          }
          setState(() {
            routeSelected = widget.item.route.toString();
          });
          Navigator.pushNamed(context, widget.item.route.toString());
          if (kDebugMode) {
            print(routeSelected);
          }
        },
        hoverColor: const Color(0xFFF2EBF0),
      ),
    );
  }
}
