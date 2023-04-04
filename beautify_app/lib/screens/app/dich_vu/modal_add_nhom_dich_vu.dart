import 'package:flutter/material.dart';
import 'package:beautify_app/screens/app/dich_vu/Models/nhom_dich_vu_model.dart';
import 'package:http/http.dart';

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
          constraints: const BoxConstraints(minHeight: 850, minWidth: 500),
          child: Column(
            children: [
              // image
              Expanded(
                child: Container(
                  constraints: const BoxConstraints(maxHeight: 100),
                  child: IconButton(
                    onPressed: () {},
                    icon: const Icon(
                      Icons.image,
                      size: 50,
                    ),
                  ),
                ),
              ),
              // text form
              Container(
                alignment: Alignment.topLeft,
                constraints: const BoxConstraints(maxHeight: 40),
                child: const Text('Thông tin chi tiết'),
              ),
              Container(
                constraints: const BoxConstraints(maxHeight: 75),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Padding(
                      padding: EdgeInsets.fromLTRB(0, 15, 0, 4),
                      child:
                          Text('Tên dịch vụ', style: TextStyle(fontSize: 13)),
                    ),
                    Expanded(
                        child: TextField(
                      decoration: InputDecoration(
                        hintText: 'Nhập tên dịch vụ',
                        hintStyle: const TextStyle(fontSize: 13),
                        border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(4)),
                      ),
                    )),
                  ],
                ),
              ),
              Container(
                constraints: const BoxConstraints(maxHeight: 75),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Padding(
                      padding: EdgeInsets.fromLTRB(0, 15, 0, 4),
                      child:
                          Text('Nhóm dịch vụ', style: TextStyle(fontSize: 13)),
                    ),
                    Expanded(
                        child: TextField(
                      decoration: InputDecoration(
                        hintText: 'Chọn nhóm dịch vụ',
                        hintStyle: const TextStyle(fontSize: 13),
                        border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(4)),
                      ),
                    )),
                  ],
                ),
              ),
              Container(
                constraints: const BoxConstraints(maxHeight: 75),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.start,
                  children: [
                    Expanded(
                      child: Padding(
                        padding: const EdgeInsets.only(right: 8),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: const [
                            Padding(
                              padding: EdgeInsets.fromLTRB(0, 15, 0, 4),
                              child: Text(
                                'Giá',
                                style: TextStyle(fontSize: 13),
                              ),
                            ),
                            Expanded(
                              child: TextField(
                                decoration: InputDecoration(
                                  hintText: '0.00',
                                  hintStyle: TextStyle(fontSize: 13),
                                  border: OutlineInputBorder(),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                    Expanded(
                      child: Padding(
                        padding: const EdgeInsets.only(left: 8),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: const [
                            Padding(
                              padding: EdgeInsets.fromLTRB(0, 15, 0, 4),
                              child: Text(
                                'Số phút thực hiện',
                                style: TextStyle(fontSize: 13),
                              ),
                            ),
                            Expanded(
                              child: TextField(
                                decoration: InputDecoration(
                                  hintText: '20',
                                  hintStyle: TextStyle(fontSize: 13),
                                  border: OutlineInputBorder(),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ],
                ),
              ),
              Container(
                constraints: const BoxConstraints(maxHeight: 120),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.start,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const Padding(
                      padding: EdgeInsets.fromLTRB(0, 15, 0, 4),
                      child: Text('Mô tả', style: TextStyle(fontSize: 13)),
                    ),
                    Expanded(
                      child: TextField(
                        minLines: 3,
                        maxLines: 3,
                        decoration: InputDecoration(
                          hintText: 'Mô tả thêm về dịch vụ',
                          hintStyle: const TextStyle(fontSize: 13),
                          border: OutlineInputBorder(
                              borderRadius: BorderRadius.circular(4)),
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
        actions: [
          ElevatedButton(
            onPressed: () {},
            style: const ButtonStyle(
                backgroundColor: MaterialStatePropertyAll(Colors.blue)),
            child: const Text('Lưu'),
          ),
          Padding(
            padding: const EdgeInsets.only(right: 20),
            child: ElevatedButton(
              style: const ButtonStyle(
                  backgroundColor: MaterialStatePropertyAll(Colors.red)),
              onPressed: () {},
              child: const Text('Hủy'),
            ),
          ),
        ],
      );
    });
  }
}
