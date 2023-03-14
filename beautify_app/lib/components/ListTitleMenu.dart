// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/components/routing/routes.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';

import 'package:beautify_app/Models/PermissionAndMenu/DrawerItem.dart';
import 'package:get/get.dart';

import '../constants/controllers.dart';
import '../helper/responsivesLayout.dart';

class ListTileMenu extends StatefulWidget {
  BuildContext buildContext;
  DrawerItem item;
   final bool selected;
  ListTileMenu({
    Key? key,
    required this.buildContext,
    required this.item,
    this.selected = false
  }) : super(key: key);

  @override
  State<ListTileMenu> createState() => _ListTileMenuState();
}

class _ListTileMenuState extends State<ListTileMenu> {
  bool _isHovering = false;
  bool _selected = false;
  @override
  void initState() {
    super.initState();
    _selected = widget.selected;
  }
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
          color: (_selected || _isHovering)
              ? const Color(0xFF7C3367)
              : const Color(0xFF666466),
        ),
        title: (MediaQuery.of(widget.buildContext).size.width >= 1100 ||
                MediaQuery.of(widget.buildContext).size.width <= 850)
            ? Text(
                widget.item.title.toString(),
                style: TextStyle(
                    color: (_selected == true || _isHovering)
                        ? const Color(0xFF7C3367)
                        : const Color(0xFF666466),
                    fontWeight: FontWeight.w600),
              )
            : null,
        onTap: () {
          if (!_selected) {
            setState(() {
              _selected = true;
              menuBeautyController
                  .changeActiveItemTo(widget.item.title.toString());
              if (ResponsiveWidget.isSmallScreen(context)) Get.back();
              navigationController.navigateTo(widget.item.route.toString());
            });
          }
        },
        hoverColor: const Color(0xFFF2EBF0),
      ),
    );
  }
}
