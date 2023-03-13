import 'package:flutter/material.dart';

class DichVuScreen extends StatefulWidget {
  const DichVuScreen({super.key});

  @override
  State<DichVuScreen> createState() => _DichVuScreenState();
}

class _DichVuScreenState extends State<DichVuScreen> {
  TextEditingController _search = new TextEditingController();
  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: const BoxDecoration(color: Colors.white),
    );
  }
}
