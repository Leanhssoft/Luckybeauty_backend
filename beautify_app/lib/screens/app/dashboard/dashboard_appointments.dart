import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'chart/dashboard_line_chart.dart';

class Appointments extends StatelessWidget {
  const Appointments({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(top: 5, right: 2.0, left: 2, bottom: 5),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
              child: Container(
            height: 370,
            decoration: BoxDecoration(
                borderRadius: BorderRadiusDirectional.circular(8),
                color: Colors.white),
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Padding(
                    padding:
                        const EdgeInsets.only(left: 24, top: 20, bottom: 10),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: const [
                        Text("Danh sách cuộc hẹn hôm nay",
                            style: TextStyle(
                                fontSize: 24, color: Color(0xFF191919))),
                        Text(
                          "Cuộc hẹn mới nhất",
                          style:
                              TextStyle(fontSize: 12, color: Color(0xFF666466)),
                        ),
                      ],
                    ),
                  ),
                  const Divider(),
                  Padding(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 4, vertical: 4),
                    child: Column(
                      children: [
                        ListTile(
                          leading: SizedBox(
                            width: 56,
                            height: 56,
                            child: Container(
                              decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(50),
                              ),
                              child: const Icon(Icons.person),
                            ),
                          ),
                          title: Text(
                            "Khách hàng",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF666466), fontSize: 14),
                          ),
                          subtitle: Column(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Row(
                                children: [
                                  const Icon(
                                    Icons.access_time,
                                    size: 16,
                                  ),
                                  Padding(
                                    padding: const EdgeInsets.only(left: 5),
                                    child: Text("9h00 - 12h30",
                                        style: GoogleFonts.roboto(
                                            color: const Color(0xFF666466),
                                            fontSize: 14)),
                                  ),
                                ],
                              ),
                              Text(
                                "Cắt duỗi",
                                style: GoogleFonts.roboto(
                                    color: const Color(0xFF666466),
                                    fontSize: 16),
                              )
                            ],
                          ),
                          trailing: Column(
                            crossAxisAlignment: CrossAxisAlignment.end,
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Text(
                                "Đang chờ",
                                style: GoogleFonts.roboto(
                                    color: const Color(0xFF009EF7),
                                    fontSize: 12),
                              ),
                              Text("80000",
                                  style: GoogleFonts.roboto(
                                      color: const Color(0xFF191919),
                                      fontSize: 16))
                            ],
                          ),
                        ),
                        const Divider()
                      ],
                    ),
                  ),
                  Padding(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 4, vertical: 4),
                    child: Column(
                      children: [
                        ListTile(
                          leading: SizedBox(
                            width: 56,
                            height: 56,
                            child: Container(
                              decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(50),
                              ),
                              child: const Icon(Icons.person),
                            ),
                          ),
                          title: Text(
                            "Khách hàng",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF666466), fontSize: 14),
                          ),
                          subtitle: Column(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Row(
                                children: [
                                  const Icon(
                                    Icons.access_time,
                                    size: 16,
                                  ),
                                  Padding(
                                    padding: const EdgeInsets.only(left: 5),
                                    child: Text("9h00 - 12h30",
                                        style: GoogleFonts.roboto(
                                            color: const Color(0xFF666466),
                                            fontSize: 14)),
                                  ),
                                ],
                              ),
                              Text(
                                "Cắt duỗi",
                                style: GoogleFonts.roboto(
                                    color: const Color(0xFF666466),
                                    fontSize: 16),
                              )
                            ],
                          ),
                          trailing: Column(
                            crossAxisAlignment: CrossAxisAlignment.end,
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Text(
                                "Đang chờ",
                                style: GoogleFonts.roboto(
                                    color: const Color(0xFF009EF7),
                                    fontSize: 12),
                              ),
                              Text("80000",
                                  style: GoogleFonts.roboto(
                                      color: const Color(0xFF191919),
                                      fontSize: 16))
                            ],
                          ),
                        ),
                        const Divider()
                      ],
                    ),
                  ),
                  Padding(
                    padding:
                        const EdgeInsets.symmetric(horizontal: 4, vertical: 4),
                    child: Column(
                      children: [
                        ListTile(
                          leading: SizedBox(
                            width: 56,
                            height: 56,
                            child: Container(
                              decoration: BoxDecoration(
                                borderRadius: BorderRadius.circular(50),
                              ),
                              child: const Icon(Icons.person),
                            ),
                          ),
                          title: Text(
                            "Khách hàng",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF666466), fontSize: 14),
                          ),
                          subtitle: Column(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Row(
                                children: [
                                  const Icon(
                                    Icons.access_time,
                                    size: 16,
                                  ),
                                  Padding(
                                    padding: const EdgeInsets.only(left: 5),
                                    child: Text("9h00 - 12h30",
                                        style: GoogleFonts.roboto(
                                            color: const Color(0xFF666466),
                                            fontSize: 14)),
                                  ),
                                ],
                              ),
                              Text(
                                "Cắt duỗi",
                                style: GoogleFonts.roboto(
                                    color: const Color(0xFF666466),
                                    fontSize: 16),
                              )
                            ],
                          ),
                          trailing: Column(
                            crossAxisAlignment: CrossAxisAlignment.end,
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              Text(
                                "Đang chờ",
                                style: GoogleFonts.roboto(
                                    color: const Color(0xFF009EF7),
                                    fontSize: 12),
                              ),
                              Text("80000",
                                  style: GoogleFonts.roboto(
                                      color: const Color(0xFF191919),
                                      fontSize: 16))
                            ],
                          ),
                        ),
                        const Divider()
                      ],
                    ),
                  ),
                ],
              ),
            ),
          )),
          const SizedBox(width: 10),
          Expanded(
            child: Container(
                height: 370,
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(8),
                    color: Colors.white),
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Padding(
                        padding: const EdgeInsets.only(
                            left: 24, top: 20, bottom: 10),
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: const [
                            Text("Tổng số cuộc hẹn hàng tuần",
                                style: TextStyle(
                                    fontSize: 24, color: Color(0xFF191919))),
                            Text(
                              "Số lượng",
                              style: TextStyle(
                                  fontSize: 12, color: Color(0xFF666466)),
                            ),
                          ],
                        ),
                      ),
                      MyLineChart(),
                    ],
                  ),
                )),
          ),
        ],
      ),
    );
  }
}
