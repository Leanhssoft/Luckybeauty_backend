import 'package:flutter/material.dart';

class CustomDeleteDialog extends StatelessWidget {
  final VoidCallback onDelete;

  const CustomDeleteDialog({
    Key? key,
    required this.onDelete,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      backgroundColor: Colors.blueGrey, // Change background color
      title: const Text("Confirm Delete",
          style: TextStyle(color: Colors.white)), // Change title color
      content: const Text("Are you sure you want to delete this item?",
          style: TextStyle(color: Colors.white)),
      actions: [
        TextButton(
          child: const Text("Cancel",
              style:
                  TextStyle(color: Colors.white)), // Change cancel button color
          onPressed: () {
            Navigator.of(context).pop();
          },
        ),
        TextButton(
          onPressed: onDelete,
          child: const Text("Delete",
              style:
                  TextStyle(color: Colors.red))
        ),
      ],
    );
  }
}
