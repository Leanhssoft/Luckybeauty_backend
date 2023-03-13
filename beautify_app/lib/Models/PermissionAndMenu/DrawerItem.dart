// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';

class DrawerItem {
  final String? title;
  final IconData? icon;
  final String? permission;
  final String? route;
  final List<DrawerItem>? children; // nullable list of children

  DrawerItem({
    this.title,
    this.icon,
    this.permission,
    this.route,
    this.children,
  });
  DrawerItem copyWith({
    String? title,
    IconData? icon,
    String? permission,
    String? route,
    List<DrawerItem>? children,
  }) {
    return DrawerItem(
      title: title ?? this.title,
      icon: icon ?? this.icon,
      permission: permission ?? this.permission,
      route: route ?? this.route,
      children: children ?? this.children,
    );
  }
}

