import 'package:beautify_app/screens/app/nhan_vien/create-or-edit-nhan-vien.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class NhanVienHeader extends StatelessWidget {
  const NhanVienHeader({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(4.0),
      child: SizedBox(
        height: 90,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      TextButton(
                          onPressed: () {},
                          child: Text(
                            "Trang chủ",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF4C4B4C), fontSize: 14),
                          )),
                      const Padding(
                        padding: EdgeInsets.only(
                            right: 3, left: 3, top: 4, bottom: 4),
                        child: Text(
                          ">",
                          textAlign: TextAlign.center,
                        ),
                      ),
                      TextButton(
                          onPressed: () {},
                          child: Text(
                            "Nhân viên",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF4C4B4C), fontSize: 14),
                          )),
                      const Padding(
                        padding: EdgeInsets.only(
                            right: 3, left: 3, top: 4, bottom: 4),
                        child: Text(">"),
                      ),
                      TextButton(
                          onPressed: () {},
                          child: Text(
                            "Thông tin nhân viên",
                            style: GoogleFonts.roboto(
                                color: const Color(0xFF4C4B4C), fontSize: 14),
                          )),
                    ],
                  ),
                  Text(
                    "Nhân viên",
                    style: GoogleFonts.roboto(
                        color: const Color(0xFF4C4B4C), fontSize: 32),
                  ),
                ],
              ),
            ),
            Padding(
              padding: const EdgeInsets.all(16),
              child: Row(
                children: [
                  SizedBox(
                    width: 40,
                    height: 40,
                    child: Container(
                      decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(5),
                          color: Colors.white,
                          border: Border.all(color: const Color(0xFFD0D5DD))),
                      child: IconButton(
                        icon: const Icon(Icons.menu),
                        onPressed: () {},
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(right: 8.0, left: 8.0),
                    child: SizedBox(
                      height: 40,
                      child: ElevatedButton.icon(
                        onPressed: () {
                          showDialog(
                              context: context,
                              builder: (BuildContext context) {
                                return CreateOrEditNhanVienModal();
                              });
                        },
                        icon: const Icon(Icons.add),
                        label: const Text("Thêm"),
                        style: ElevatedButton.styleFrom(
                          backgroundColor:
                              const Color(0xFF7C3367), // background
                          foregroundColor: Colors.white, // foreground
                        ),
                      ),
                    ),
                  )
                ],
              ),
            )
          ],
        ),
      ),
    );
  }
}
