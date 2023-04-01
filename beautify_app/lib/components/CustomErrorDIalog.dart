import 'package:flutter/material.dart';

class CustomErrorDialog extends StatelessWidget {
  const CustomErrorDialog({super.key});

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
          title: Row(
            children: const [
              Icon(Icons.error, color: Colors.red),
              SizedBox(width: 8.0),
              Text('Error'),
            ],
          ),
          content: const Text("Có lỗi sảy ra vui lòng thử lại sau!"),
          actions: <Widget>[
            TextButton(
              child:const Text('OK'),
              onPressed: () {
                Navigator.pop(context);
              },
            ),
          ],
        );
  }
}