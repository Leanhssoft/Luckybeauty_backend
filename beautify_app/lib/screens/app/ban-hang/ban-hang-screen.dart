import 'package:beautify_app/screens/app/ban-hang/ban-hang-content.dart';
import 'package:beautify_app/screens/app/ban-hang/ban-hang-header.dart';
import 'package:beautify_app/widgets/top_bar.dart';
import 'package:flutter/material.dart';

class BanHangScreen extends StatefulWidget {
  const BanHangScreen({super.key});

  @override
  State<BanHangScreen> createState() => _BanHangScreenState();
}

class _BanHangScreenState extends State<BanHangScreen> {
  GlobalKey<ScaffoldState> scaffoldKey = GlobalKey();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      key: scaffoldKey,
      appBar: TopBarNavigation(context, scaffoldKey),
      body: SafeArea(
        child: Container(
          color: Colors.white,
          height: MediaQuery.of(context).size.height,
          child: SingleChildScrollView(
            scrollDirection: Axis.vertical,
            child: Column(
              children: [
                const Divider(),
                const BanHangHeader(),
                Container(
                  alignment: Alignment.center,
                  color: const Color(0xFFF4F4F4),
                    height: MediaQuery.of(context).size.height - 200,
                    child: const BanHangContent()),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
