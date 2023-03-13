import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'chart/dashboard_column_char.dart';
import 'chart/dashboard_line_chart.dart';
import 'package:percent_indicator/percent_indicator.dart';

class Sales extends StatefulWidget {
  const Sales({
    super.key,
  });

  @override
  State<Sales> createState() => _SalesState();
}

class _SalesState extends State<Sales> {
  @override
  Widget build(BuildContext context) {
    String dropdownvalue = 'Tháng này';
    var items = [
      'Tháng này',
      'Tháng trước',
      'Tuần này',
      'Tuần trước',
      'Hôm nay',
      'Hôm qua',
    ];
    return Container(
      padding: const EdgeInsets.only(top: 5, right: 2.0, left: 2, bottom: 5),
      height: 400,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Expanded(
              flex: 3,
              child: SingleChildScrollView(
                physics: const NeverScrollableScrollPhysics(),
                child: Container(
                  height: 400,
                  decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(2)),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      SizedBox(
                        height: 64,
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Padding(
                              padding: const EdgeInsets.only(
                                  left: 24, top: 12, bottom: 12),
                              child: Column(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text("Doanh thu",
                                      style: GoogleFonts.roboto(
                                          color: const Color(0xFF191919),
                                          fontSize: 18)),
                                  Text("Cuộc hẹn mới nhất",
                                      style: GoogleFonts.roboto(
                                        color: const Color(0xFF191919),
                                        fontSize: 12,
                                      )),
                                ],
                              ),
                            ),
                            Padding(
                              padding: const EdgeInsets.only(
                                  left: 24, top: 12, bottom: 12, right: 14),
                              child: SizedBox(
                                height: 40,
                                child: DropdownButton(
                                  icon: const Icon(Icons.keyboard_arrow_down,size: 20,),
                                  value: dropdownvalue,
                                  onChanged: (value) => {
                                    setState(() {
                                      dropdownvalue = value!;
                                    })
                                  },
                                  items: items.map((String items) {
                                    return DropdownMenuItem(
                                      value: items,
                                      child: Text(items,
                                          style: GoogleFonts.roboto(
                                              color: const Color(0xFF191919),
                                              fontSize: 14)),
                                    );
                                  }).toList(),
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      const Divider(),
                      SizedBox(child: MyColumnChart()),
                    ],
                  ),
                ),
              )),
          const SizedBox(
            width: 10,
          ),
          Expanded(
              flex: 1,
              child: SingleChildScrollView(
                child: Container(
                  height: 400,
                  decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(2),
                      color: Colors.white),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 12, bottom: 8, right: 10),
                        child: SizedBox(
                          height: 64,
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                             Text("Top 5 dịch vụ hot",
                                  style: GoogleFonts.roboto(
                                              color: const Color(0xFF191919),
                                              fontSize: 18)),
                              Padding(
                                padding: const EdgeInsets.all(5),
                                child: SizedBox(
                                  width: 30,
                                  height: 30,
                                  child: Container(
                                    decoration: BoxDecoration(
                                        borderRadius: BorderRadius.circular(5),
                                        color: Colors.white,
                                        border: Border.all(
                                            color: const Color(0xFFD0D5DD))),
                                    child: const Icon(Icons.more_horiz),
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 4, bottom: 14, right: 10),
                        child: SizedBox(
                          child: Column(
                            children: [
                              Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                children: const [
                                  Text("Dịch vụ 1"),
                                  Text("9000000")
                                ],
                              ),
                              LinearPercentIndicator(
                                padding: const EdgeInsets.only(top: 10),
                                animation: true,
                                lineHeight: 20.0,
                                animationDuration: 2500,
                                percent: 0.8,
                                progressColor: const Color(0xFFFFC700),
                              ),
                            ],
                          ),
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 8, bottom: 14, right: 10),
                        child: Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: const [
                                Text("Dịch vụ 2"),
                                Text("9000000")
                              ],
                            ),
                            LinearPercentIndicator(
                              padding: const EdgeInsets.only(top: 10),
                              animation: true,
                              lineHeight: 20.0,
                              animationDuration: 2500,
                              percent: 0.8,
                              progressColor: const Color(0xFF7C3367),
                            ),
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 8, bottom: 14, right: 10),
                        child: Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: const [
                                Text("Dịch vụ 3"),
                                Text("9000000")
                              ],
                            ),
                            LinearPercentIndicator(
                              padding: const EdgeInsets.only(top: 10),
                              animation: true,
                              lineHeight: 20.0,
                              animationDuration: 2500,
                              percent: 0.8,
                              progressColor: const Color(0xFF009EF7),
                            ),
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 8, bottom: 14, right: 10),
                        child: Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: const [
                                Text("Dịch vụ 4"),
                                Text("9000000")
                              ],
                            ),
                            LinearPercentIndicator(
                              padding: const EdgeInsets.only(top: 10),
                              animation: true,
                              lineHeight: 20.0,
                              animationDuration: 2500,
                              percent: 0.8,
                              progressColor: const Color(0xFFF1416C),
                            ),
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 8, bottom: 14, right: 10),
                        child: Column(
                          children: [
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: const [
                                Text("Dịch vụ 5"),
                                Text("9000000")
                              ],
                            ),
                            LinearPercentIndicator(
                              padding: const EdgeInsets.only(top: 10),
                              animation: true,
                              lineHeight: 20.0,
                              animationDuration: 2500,
                              percent: 0.8,
                              progressColor: const Color(0xFF50CD89),
                            ),
                          ],
                        ),
                      )
                    ],
                  ),
                ),
              )),
        ],
      ),
    );
  }
}
