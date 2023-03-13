import 'package:flutter/material.dart';

import 'OverViewWidget.dart';

class Overview extends StatelessWidget {
  const Overview({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    List<OverViewWidget> items = lstTongQuan();
    return SizedBox(
      height: MediaQuery.of(context).size.width > 1100 ? 80 : 160,
      child: GridView.builder(
        padding: const EdgeInsets.only(left: 2, right: 2, top: 2, bottom: 2),
        gridDelegate: SliverGridDelegateWithFixedCrossAxisCount(
          crossAxisCount: MediaQuery.of(context).size.width > 1100 ? 4 : 2,
          childAspectRatio: 4/1
        ),
        itemCount: items.length,
        itemBuilder: (BuildContext context, int index) {
          return Padding(
            padding: const EdgeInsets.only(top:2,bottom:2,right: 5,left: 5),
            child: Center(
              child: Container(
                decoration: BoxDecoration(borderRadius: BorderRadius.circular(8),color: Colors.white),
                child: OverViewWidget(
                  lable: items[index].lable,
                  icon: items[index].icon,
                  value: items[index].value,
                  percen: items[index].percen,
                  bgIconColor: items[index].bgIconColor,
                ),
              ),
            ),
          );
        },
      ),
    );
  }

  List<OverViewWidget> lstTongQuan() {
    return [
      OverViewWidget(
          lable: "Tổng số khách hàng",
          icon: Icons.person_3_outlined,
          value: "100",
          percen: "10",
          bgIconColor: const Color(0xFF009EF7)),
      OverViewWidget(
          lable: "Tổng cuộc hẹn",
          icon: Icons.calendar_month_outlined,
          value: "100",
          percen: "10",
          bgIconColor: const Color(0xFFFFC700)),
      OverViewWidget(
          lable: "Tổng số",
          icon: Icons.person_3_outlined,
          value: "100",
          percen: "10",
          bgIconColor: const Color(0xFFF1416C)),
      OverViewWidget(
          lable: "Doanh thu",
          icon: Icons.person_3_outlined,
          value: "100",
          percen: "10",
          bgIconColor: const Color(0xFF50CD89)),
    ];
  }
  // List<Widget> lstTongQuan() {
  //   return [
  //     Padding(
  //       padding: const EdgeInsets.only(left: 2, right: 5, top: 2, bottom: 2),
  //       child: Container(
  //         decoration: BoxDecoration(
  //             borderRadius: BorderRadius.circular(8), color: Colors.white),
  //         child: Padding(
  //           padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
  //           child: OverViewWidget(
  //               lable: "Tổng số khách hàng",
  //               icon: Icons.person_3_outlined,
  //               value: "100",
  //               percen: "10",
  //               bgIconColor: const Color(0xFF009EF7)),
  //         ),
  //       ),
  //     ),
  //     Padding(
  //       padding: const EdgeInsets.only(left: 2, right: 5, top: 2, bottom: 2),
  //       child: Container(
  //         decoration: BoxDecoration(
  //             borderRadius: BorderRadius.circular(8), color: Colors.white),
  //         child: Padding(
  //             padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
  //             child: OverViewWidget(
  //                 lable: "Tổng cuộc hẹn",
  //                 icon: Icons.calendar_month_outlined,
  //                 value: "100",
  //                 percen: "10",
  //                 bgIconColor: const Color(0xFFFFC700))),
  //       ),
  //     ),
  //     Padding(
  //       padding: const EdgeInsets.only(left: 2, right: 5, top: 2, bottom: 2),
  //       child: Container(
  //         decoration: BoxDecoration(
  //             borderRadius: BorderRadius.circular(8), color: Colors.white),
  //         child: Padding(
  //           padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
  //           child: OverViewWidget(
  //               lable: "Tổng số",
  //               icon: Icons.person_3_outlined,
  //               value: "100",
  //               percen: "10",
  //               bgIconColor: const Color(0xFFF1416C)),
  //         ),
  //       ),
  //     ),
  //     Padding(
  //       padding: const EdgeInsets.only(left: 2, right: 5, top: 2, bottom: 2),
  //       child: Container(
  //         decoration: BoxDecoration(
  //             borderRadius: BorderRadius.circular(8), color: Colors.white),
  //         child: Padding(
  //           padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
  //           child: OverViewWidget(
  //               lable: "Doanh thu",
  //               icon: Icons.person_3_outlined,
  //               value: "100",
  //               percen: "10",
  //               bgIconColor: const Color(0xFF50CD89)),
  //         ),
  //       ),
  //     ),
  //   ];
  // }
}
