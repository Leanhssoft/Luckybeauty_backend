import 'package:flutter/material.dart';

class OverViewWidget extends StatelessWidget {
  String lable;
  IconData icon;
  String value;
  String percen;
  Color bgIconColor;
  OverViewWidget({
    Key? key,
    required this.lable,
    required this.icon,
    required this.value,
    required this.percen,
    required this.bgIconColor,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Center(
      child: ListTile(
          leading: SizedBox(
            width: 56,
            height: 56,
            child: Container(
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(10),
                color: bgIconColor,
              ),
              child: Center(
                child: Icon(icon),
              ),
            ),
          ),
          title: Text(lable),
          subtitle: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              Padding(
                padding: const EdgeInsets.all(8.0),
                child: Text(value),
              ),
              Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: Text(
                    "$percen %",
                  ))
            ],
          )),
    );
  }
}
