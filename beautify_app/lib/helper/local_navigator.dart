import 'package:flutter/material.dart';
import 'package:flutter/cupertino.dart';

import '../constants/controllers.dart';
Navigator localNavigator() =>   Navigator(
      key: navigationController.navigationKey,
      onGenerateRoute: (settings) {

      },
      initialRoute: '/home',
    );