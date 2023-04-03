import 'package:flutter/material.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/nhom_dich_vu_model.dart';

@immutable
class ModalAddNhomDichVu extends StatefulWidget {
  const ModalAddNhomDichVu({super.key});
  @override
  State<ModalAddNhomDichVu> createState() => _ModalAddNhomDichVu();
}

class _ModalAddNhomDichVu extends State<ModalAddNhomDichVu> {
  late final bool isNew;
  late NhomDichVuDto groupNew;

  @override
  void setState(fn) {}
  void innitState() {
    isNew = false;
    groupNew = NhomDichVuDto(
        id: null,
        maNhomHang: "",
        tenNhomHang: "",
        laNhomHangHoa: true,
        isDeleted: false,
        isSelected: false);
    super.initState();
  }

  @override
  void dispose() {
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
        builder: (BuildContext context, BoxConstraints constraints) {
      return AlertDialog(
        title: Container(
          padding: const EdgeInsets.all(0),
          child: Row(
            children: [
              const Expanded(
                child: Text('Thêm dịch vụ mới'),
              ),
              IconButton(
                onPressed: () {},
                icon: const Icon(Icons.close),
              ),
            ],
          ),
        ),
        content: Container(
          padding: const EdgeInsets.all(8),
          constraints: const BoxConstraints(minHeight: 800, minWidth: 600),
          child: Column(
            children: [
              // image
              Expanded(
                child: IconButton(
                  onPressed: () {},
                  icon: const Icon(Icons.image),
                ),
              ),
              // text form
              Container(
                alignment: Alignment.topLeft,
                padding: const EdgeInsets.all(8),
                constraints: const BoxConstraints(maxHeight: 80),
                child: const Text('Thông tin chi tiết'),
              ),
              Container(
                padding: const EdgeInsets.all(8),
                constraints: const BoxConstraints(maxHeight: 100),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  children: [
                    Expanded(
                      child: Container(
                        alignment: Alignment.topLeft,
                        child: const Text(
                          'Tên dịch vụ',
                          // style: TextStyle(fontSize: 12),
                        ),
                      ),
                    ),
                    Expanded(
                        child: TextField(
                      decoration: InputDecoration(
                        hintText: 'Tên dịch vụ',
                        border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(4)),
                      ),
                    )),
                  ],
                ),
              ),
            ],
          ),
        ),
        actions: [
          // bottom
          SizedBox(
            height: 100,
            child: Container(
              padding: const EdgeInsets.all(16),
              child: Row(
                children: [
                  const Expanded(child: Text('')),
                  SizedBox(
                    width: 100,
                    child: ElevatedButton(
                      onPressed: () {},
                      style: const ButtonStyle(
                          backgroundColor:
                              MaterialStatePropertyAll(Colors.blue)),
                      child: const Text('Lưu'),
                    ),
                  ),
                  SizedBox(
                    width: 100,
                    child: ElevatedButton(
                      style: const ButtonStyle(
                          backgroundColor:
                              MaterialStatePropertyAll(Colors.red)),
                      onPressed: () {},
                      child: const Text('Hủy'),
                    ),
                  ),
                ],
              ),
            ),
          )
        ],
      );
      // return Container(
      //   padding: const EdgeInsets.all(16),
      //   child: Column(
      //     children: [
      //       // title
      //       SizedBox(
      //         height: 80,
      //         child: Row(
      //           children: [
      //             const Expanded(
      //               child: Text('Thêm dịch vụ mới'),
      //             ),
      //             IconButton(
      //               onPressed: () {},
      //               icon: const Icon(Icons.close),
      //             ),
      //           ],
      //         ),
      //       ),
      //       //content
      //       Expanded(
      //         child: IconButton(
      //           onPressed: () {},
      //           icon: const Icon(Icons.image),
      //         ),
      //       ),
      //       Expanded(
      //         flex: 3,
      //         child: IconButton(
      //           onPressed: () {},
      //           icon: const Icon(Icons.image),
      //         ),
      //       ),
      //       // bottom
      //       SizedBox(
      //         height: 100,
      //         child: Container(
      //           padding: const EdgeInsets.fromLTRB(0, 10, 0, 10),
      //           child: Row(
      //             children: [
      //               const Expanded(child: Text('')),
      //               SizedBox(
      //                 width: 100,
      //                 child: ElevatedButton(
      //                   onPressed: () {},
      //                   style: const ButtonStyle(
      //                       backgroundColor:
      //                           MaterialStatePropertyAll(Colors.blue)),
      //                   child: const Text('Lưu'),
      //                 ),
      //               ),
      //               SizedBox(
      //                 width: 100,
      //                 child: ElevatedButton(
      //                   style: const ButtonStyle(
      //                       backgroundColor:
      //                           MaterialStatePropertyAll(Colors.red)),
      //                   onPressed: () {},
      //                   child: const Text('Hủy'),
      //                 ),
      //               ),
      //             ],
      //           ),
      //         ),
      //       ),
      //     ],
      //   ),
      // );
    });
  }
}
