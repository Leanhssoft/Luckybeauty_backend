import 'package:beautify_app/components/routing/router.dart';
import 'package:beautify_app/components/routing/routes.dart';
import 'package:flutter/material.dart';
import 'package:flutter/cupertino.dart';

import '../constants/controllers.dart';
Navigator localNavigator() =>   Navigator(
      key: navigationController.navigatorKey,
      onGenerateRoute: generateRoute,
      initialRoute: overviewPageRoute,
    );