
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import 'models/BanHangItemDto.dart';

class BanHangContent extends StatefulWidget {
  const BanHangContent({Key? key}) : super(key: key);

  @override
  State<BanHangContent> createState() => _BanHangContentState();
}

class _BanHangContentState extends State<BanHangContent> {
  ScrollController _scrollController = ScrollController();
  List<BanHangItemDto> itemList = [
    BanHangItemDto(id: "1",diemTichLuy: "100",ngayHen: "03/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 8",tenVietTat: "XM",soDieThoai: "0830293939",trangThai: 1),
    BanHangItemDto(id: "2",diemTichLuy: "100",ngayHen: "04/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 7",tenVietTat: "XM",soDieThoai: "0730293939",trangThai: 1),
    BanHangItemDto(id: "3",diemTichLuy: "100",ngayHen: "05/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 6",tenVietTat: "XM",soDieThoai: "0630293939",trangThai: 1),
    BanHangItemDto(id: "4",diemTichLuy: "100",ngayHen: "06/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 5",tenVietTat: "XM",soDieThoai: "0530293939",trangThai: 1),
    BanHangItemDto(id: "5",diemTichLuy: "100",ngayHen: "07/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 4",tenVietTat: "XM",soDieThoai: "0430293939",trangThai: 1),
    BanHangItemDto(id: "6",diemTichLuy: "100",ngayHen: "08/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 3",tenVietTat: "XM",soDieThoai: "0330293939",trangThai: 1),
    BanHangItemDto(id: "7",diemTichLuy: "100",ngayHen: "09/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 2",tenVietTat: "XM",soDieThoai: "0230293939",trangThai: 1),
    BanHangItemDto(id: "8",diemTichLuy: "100",ngayHen: "10/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 1",tenVietTat: "XM",soDieThoai: "0130293939",trangThai: 1),
    BanHangItemDto(id: "9",diemTichLuy: "100",ngayHen: "11/04/2023",thoiGianHen: "9h30",tenKhachHang: "Xuân Mai 9",tenVietTat: "XM",soDieThoai: "0930293939",trangThai: 1),
  ];
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(top: 8, bottom: 8),
      child: SizedBox(
        child: Center(
          child: Padding(
            padding: const EdgeInsets.symmetric(vertical: 8.0,horizontal: 32),
            child: SingleChildScrollView(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: buildItems(itemList, context),
              ),
            ),
          ),
        ),
      ),
    );
  }
}

List<Widget> buildItems(List<BanHangItemDto> items, BuildContext parentContext) {
  final int itemCount = items.length;
  final int itemView = MediaQuery.of(parentContext).size.width>1100 ?4:1;
  final int rows = (itemCount / itemView).ceil(); // Số lượng hàng cần hiển thị
  final int itemsInLastRow = itemCount % itemView;
  List<Widget> widgets = [];
  for (int i = 0; i < rows; i++) {
    final int startIndex = i * itemView;
    final int endIndex =
        (startIndex + itemView) > itemCount ? itemCount : (startIndex + itemView);
    final List<BanHangItemDto> rowItems = items.sublist(startIndex, endIndex);
    widgets.add(Row(
      mainAxisAlignment: MainAxisAlignment.center,
      crossAxisAlignment: CrossAxisAlignment.start,
      children: rowItems
          .map((item) => Padding(
                padding: const EdgeInsets.all(12),
                child: Expanded(
                  child: Container(
                    width: 326,
                    height: 165,
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(15),
                      color: Colors.white,
                      boxShadow: [
                        BoxShadow(
                          color: Colors.grey.withOpacity(0.5),
                          spreadRadius: 1,
                          blurRadius: 7,
                          offset: const Offset(0, 3),
                        ),
                      ],
                    ),
                    child: Padding(
                      padding: const EdgeInsets.only(
                          right: 12, left: 12, top: 24, bottom: 24),
                      child: SingleChildScrollView(
                        child: Column(
                            mainAxisAlignment: MainAxisAlignment.start,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              SizedBox(
                                height: 40,
                                child: ListTile(
                                  leading: Container(
                                    height: 40,
                                    width: 40,
                                    decoration: BoxDecoration(
                                        borderRadius: BorderRadius.circular(50),
                                        color: const Color(0xFfE5D6E1)),
                                    child: Center(
                                      child: Text(
                                        item.tenVietTat.toString(),
                                        style: GoogleFonts.roboto(
                                            fontSize: 16,
                                            color: const Color(0xFF7C3367)),
                                        textAlign: TextAlign.center,
                                      ),
                                    ),
                                  ),
                                  title: Column(
                                    crossAxisAlignment:
                                        CrossAxisAlignment.start,
                                    mainAxisAlignment: MainAxisAlignment.start,
                                    children: [
                                      Text(
                                        item.tenKhachHang.toString(),
                                        style: GoogleFonts.roboto(
                                            fontSize: 16,
                                            color: const Color(0xFF333233)),
                                      ),
                                      Text(
                                       item.soDieThoai.toString(),
                                        style: GoogleFonts.roboto(
                                            fontSize: 12,
                                            color: const Color(0xFF999699)),
                                      )
                                    ],
                                  ),
                                  trailing: const Icon(Icons.more_horiz),
                                ),
                              ),
                              Padding(
                                padding:
                                    const EdgeInsets.only(right: 12, left: 12),
                                child: SizedBox(
                                  height: 38,
                                  child: Row(
                                    children: [
                                      Text(
                                        "Điểm tích lũy:",
                                        style: GoogleFonts.roboto(
                                            fontSize: 16,
                                            color: const Color(0xFF4C4B4C),
                                            fontWeight: FontWeight.w400),
                                      ),
                                      const SizedBox(
                                        width: 5,
                                      ),
                                      Text(
                                        item.diemTichLuy.toString(),
                                        style: GoogleFonts.roboto(
                                            fontSize: 16,
                                            color: const Color(0xFF4C4B4C),
                                            fontWeight: FontWeight.w700),
                                      )
                                    ],
                                  ),
                                ),
                              ),
                              Padding(
                                padding:
                                    const EdgeInsets.only(right: 12, left: 12),
                                child: SizedBox(
                                  height: 38,
                                  child: Row(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                    children: [
                                      Expanded(
                                        child: SizedBox(
                                          child: Row(children: [
                                            Text(
                                              item.ngayHen.toString(),
                                              style: GoogleFonts.roboto(
                                                  fontSize: 14,
                                                  color:
                                                      const Color(0xFF4C4B4C),
                                                  fontWeight: FontWeight.w400),
                                            ),
                                            const SizedBox(
                                              width: 5,
                                            ),
                                            Row(
                                              children: [
                                                const Icon(Icons.access_time),
                                                const SizedBox(
                                                  width: 5,
                                                ),
                                                Text(
                                                  item.thoiGianHen.toString(),
                                                  style: GoogleFonts.roboto(
                                                      fontSize: 12,
                                                      color: const Color(
                                                          0xFF4C4B4C),
                                                      fontWeight:
                                                          FontWeight.w700),
                                                ),
                                              ],
                                            )
                                          ]),
                                        ),
                                      ),
                                      Expanded(
                                          child: Row(
                                        children: [
                                          const Spacer(),
                                          Expanded(
                                            child: Container(
                                              alignment: Alignment.centerRight,
                                              width: 75,
                                              height: 24,
                                              decoration: BoxDecoration(
                                                  borderRadius:
                                                      BorderRadius.circular(15),
                                                  color:
                                                      const Color(0xFFFFF8DD)),
                                              child: Center(
                                                child: Text(
                                                  "Đang chờ",
                                                  style: GoogleFonts.roboto(
                                                      color: const Color(
                                                          0xFFFFC700),
                                                      fontSize: 12),
                                                  textAlign: TextAlign.center,
                                                ),
                                              ),
                                            ),
                                          ),
                                        ],
                                      ))
                                    ],
                                  ),
                                ),
                              )
                            ]),
                      ),
                    ),
                  ),
                ),
              ))
          .toList(),
    ));
  }

  if (itemsInLastRow != 0) {
    final int emptyContainers = 4 - itemsInLastRow;
    final emptyContainerList = List.generate(
        emptyContainers, (_) => Container(width: 326, height: 165));
    widgets.addAll(emptyContainerList);
  }
  return widgets;
}
