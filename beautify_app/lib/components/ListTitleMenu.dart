// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:beautify_app/components/routing/routes.dart';
import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';

import 'package:beautify_app/Models/PermissionAndMenu/DrawerItem.dart';
import 'package:get/get.dart';

import '../constants/controllers.dart';
import '../helper/responsivesLayout.dart';

class ListTileMenu extends StatefulWidget {
  BuildContext buildContext;
  DrawerItem item;
  ListTileMenu({
    Key? key,
    required this.buildContext,
    required this.item,
  }) : super(key: key);

  @override
  State<ListTileMenu> createState() => _ListTileMenuState();
}

class _ListTileMenuState extends State<ListTileMenu> {
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
        onTap: () {
        if (widget.item.route.toString() == authenticationPageRoute) {}
        if (!menuController.isActive(widget.item.route.toString())) {
          menuController.changeActiveItemTo(widget.item.route.toString());
          if (ResponsiveWidget.isSmallScreen(widget.buildContext)) ;
          Get.back();
        }
      },
        hoverColor:const Color(0xFFF2EBF0),
      ),
    );
  }
}
