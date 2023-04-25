

import 'dart:convert';

class Constants {
  // ignore: constant_identifier_names
  //static const String BASE_URL = 'http://192.168.1.63:7000';
  //static const String BASE_URL = 'https://localhost:44311';
  // static const String BASE_URL = 'http://localhost:59548';

  static const String apiUrl =
      "aHR0cDovLzE5Mi4xNjguMS42Mzo3MDAw"; // base64 encoded URL http://192.168.1.63:7000

  static String get BASE_URL => utf8.decode(base64.decode(apiUrl));
}
