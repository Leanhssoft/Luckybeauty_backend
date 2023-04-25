import 'package:beautify_app/screens/app/ban-hang/add-cutomer-to-check-out.dart';
import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter/src/widgets/placeholder.dart';
import 'package:google_fonts/google_fonts.dart';

class BanHangHeader extends StatelessWidget {
  const BanHangHeader({super.key});

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
              padding: const EdgeInsets.all(16),
              child: Row(
                children: [
                  SizedBox(
                    width: 32,
                    height: 32,
                    child: Container(
                      decoration: BoxDecoration(
                          borderRadius: const BorderRadius.only(
                              bottomLeft: Radius.circular(5),
                              topLeft: Radius.circular(5)),
                          color: Colors.white,
                          border: Border.all(color: const Color(0xFFD0D5DD))),
                      child: Center(
                        child: IconButton(
                          icon: const Icon(
                            Icons.window_sharp,
                            size: 14,
                          ),
                          onPressed: () {},
                        ),
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 32,
                    height: 32,
                    child: Container(
                      decoration: BoxDecoration(
                          borderRadius: const BorderRadius.only(
                              bottomRight: Radius.circular(5),
                              topRight: Radius.circular(5)),
                          color: Colors.white,
                          border: Border.all(color: const Color(0xFFD0D5DD))),
                      child: Center(
                        child: IconButton(
                          icon: const Icon(
                            Icons.density_small_outlined,
                            size: 14,
                          ),
                          onPressed: () {},
                        ),
                      ),
                    ),
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
                        icon: const Icon(
                          Icons.menu,
                          size: 24,
                        ),
                        onPressed: () {},
                      ),
                    ),
                  ),
                  SizedBox(
                    width: 2,
                  ),
                  SizedBox(
                    width: 40,
                    height: 40,
                    child: Container(
                      decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(5),
                          color: Colors.white,
                          border: Border.all(color: const Color(0xFFD0D5DD))),
                      child: IconButton(
                        icon: const Icon(
                          Icons.calendar_month,
                          size: 24,
                        ),
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
                                return const AddCustomerToCheckOutModal();
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
