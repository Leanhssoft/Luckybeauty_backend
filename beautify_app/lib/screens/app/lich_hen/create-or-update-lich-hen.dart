import 'package:beautify_app/components/CustomTextFormField.dart';
import 'package:beautify_app/components/CustomTextFormFieldValidate.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

class CreateOrEditLichHen extends StatefulWidget {
  const CreateOrEditLichHen({super.key});

  @override
  State<CreateOrEditLichHen> createState() => _CreateOrEditLichHenState();
}

class _CreateOrEditLichHenState extends State<CreateOrEditLichHen> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  late DateTime _dateTime = DateTime.now();
  late TimeOfDay timeSelected = TimeOfDay.now();
  late DateTime dateSelect = DateTime.now();
  late int selectedTypeBooking = 1;
  late int selectedStatusBooking = 1;
  @override
  Widget build(BuildContext context) {
    final List<Map<String, dynamic>> typeBooking = [
      {"typeName": "Booking online", "value": 0},
      {"typeName": "Nhờ cửa hàng", "value": 1},
      {"typeName": "Booking trực tiếp", "value": 2},
    ];
    final List<Map<String, dynamic>> statusBooking = [
      {"statusName": "Đặt lịch", "value": 1},
      {"statusName": "Đã gọi(nhắn tin) cho khách", "value": 2},
      {"statusName": "Checkin", "value": 3},
      {"statusName": "Xóa", "value": 0},
    ];
    return LayoutBuilder(
        builder: (BuildContext context, BoxConstraints constraints) {
      return Container(
        decoration: BoxDecoration(borderRadius: BorderRadius.circular(15)),
        child: AlertDialog(
          title: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Text(
                "Thêm cuộc hẹn",
                style: GoogleFonts.roboto(
                    color: const Color(0xFF333233), fontSize: 24),
              ),
              Container(
                width: 32,
                height: 32,
                decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(8),
                    border:
                        Border.all(color: const Color(0xFF999699), width: 2)),
                child: InkWell(
                  onTap: () {
                    Navigator.pop(context);
                  },
                  child: const Icon(
                    Icons.close,
                    color: Color(0xFF999699),
                    size: 16,
                  ),
                ),
              )
            ],
          ),
          content: SizedBox(
              width: 720,
              height: 600,
              child: Form(
                key: _formKey,
                child: SingleChildScrollView(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Text(
                          "Chi tiết cuộc hẹn",
                          style: GoogleFonts.roboto(
                              fontSize: 16, color: const Color(0xFF999699)),
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(right: 4.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Khách hàng",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormFieldValidate(
                                        controller: TextEditingController(),
                                        hintText: "Nhập thông tin khách hàng",
                                        textValidate:
                                            "Tên khách hàng không được bỏ trống")
                                  ],
                                ),
                              ),
                            ),
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(left: 4),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Số điện thoại",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormFieldValidate(
                                        controller: TextEditingController(),
                                        hintText:
                                            "Nhập số điện thoại khách hàng",
                                        textValidate:
                                            "Số điện thoại không được bỏ trống")
                                  ],
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(right: 4.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Dịch vụ",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      controller: TextEditingController(),
                                      hintText: "Chọn dịch vụ",
                                    )
                                  ],
                                ),
                              ),
                            ),
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(left: 4),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Nhân viên",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      controller: TextEditingController(),
                                      hintText: "Chọn nhân viên",
                                    )
                                  ],
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(right: 4.0),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Ngày",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      leftIcon:
                                          const Icon(Icons.date_range_outlined),
                                      controller: TextEditingController(
                                          text:
                                              '${dateSelect.day}/${dateSelect.month}/${dateSelect.year}'),
                                      hintText: "Chọn ngày",
                                      onTab: () async {
                                        final date = await pickDate();
                                        if (date != null) {
                                          setState(() {
                                            dateSelect = date;
                                          });
                                        }
                                      },
                                    ),
                                  ],
                                ),
                              ),
                            ),
                            Expanded(
                              child: Padding(
                                padding: const EdgeInsets.only(left: 4),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      "Giờ",
                                      style: GoogleFonts.roboto(
                                          fontSize: 14,
                                          color: const Color(0xFF999699)),
                                    ),
                                    const SizedBox(
                                      height: 8,
                                    ),
                                    CustomTextFormField(
                                      leftIcon: const Icon(Icons.access_time),
                                      controller: TextEditingController(
                                          text:
                                              '${timeSelected.hour}:${timeSelected.minute.toString().padLeft(2, '0')}'),
                                      hintText: "Chọn giờ",
                                      onTab: () async {
                                        final time = await pickTime();
                                        if (time != null) {
                                          setState(() {
                                            timeSelected = time;
                                          });
                                        }
                                      },
                                    ),
                                  ],
                                ),
                              ),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Loại booking",
                              style: GoogleFonts.roboto(
                                  fontSize: 14, color: const Color(0xFF999699)),
                            ),
                            const SizedBox(
                              height: 8,
                            ),
                            DropdownButtonFormField(
                              decoration: InputDecoration(
                                contentPadding:
                                    const EdgeInsets.fromLTRB(10, 10, 10, 10),
                                labelStyle: const TextStyle(
                                    color: Colors.blue,
                                    fontSize: 20,
                                    fontWeight: FontWeight.bold),
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8)),
                                errorBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.red)),
                                enabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.black)),
                                disabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.black)),
                              ),
                              value: selectedTypeBooking,
                              onChanged: (value) => {},
                              onSaved: (int? newValue) {
                                setState(() {
                                  selectedTypeBooking = newValue ?? 1;
                                });
                              },
                              items: typeBooking.map<DropdownMenuItem<int>>(
                                  (Map<String, dynamic> item) {
                                return DropdownMenuItem<int>(
                                  value: item['value'] ?? 0,
                                  child: Text(item['typeName'].toString()),
                                );
                              }).toList(),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Trạng thái",
                              style: GoogleFonts.roboto(
                                  fontSize: 14, color: const Color(0xFF999699)),
                            ),
                            const SizedBox(
                              height: 8,
                            ),
                            DropdownButtonFormField(
                              decoration: InputDecoration(
                                contentPadding:
                                    const EdgeInsets.fromLTRB(10, 10, 10, 10),
                                labelStyle: const TextStyle(
                                    color: Colors.blue,
                                    fontSize: 20,
                                    fontWeight: FontWeight.bold),
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8)),
                                errorBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.red)),
                                enabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.black)),
                                disabledBorder: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(8),
                                    borderSide:
                                        const BorderSide(color: Colors.black)),
                              ),
                              value: selectedStatusBooking,
                              onChanged: (value) => {},
                              onSaved: (int? newValue) {
                                setState(() {
                                  selectedStatusBooking = newValue ?? 1;
                                });
                              },
                              items: statusBooking.map<DropdownMenuItem<int>>(
                                  (Map<String, dynamic> item) {
                                return DropdownMenuItem<int>(
                                  value: item['value'] ?? 0,
                                  child: Text(item['statusName'].toString()),
                                );
                              }).toList(),
                            )
                          ],
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.only(top: 4, bottom: 20),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(
                              "Ghi chú",
                              style: GoogleFonts.roboto(
                                  fontSize: 14, color: const Color(0xFF999699)),
                            ),
                            const SizedBox(
                              height: 8,
                            ),
                            CustomTextFormField(
                              heightForm: 120,
                              controller: TextEditingController(),
                              hintText: "Ghi chú",
                            )
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              )),
          actions: [
            Padding(
              padding: const EdgeInsets.only(right: 4, bottom: 28),
              child: SizedBox(
                width: 66,
                height: 32,
                child: ElevatedButton(
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                  style: const ButtonStyle(
                    backgroundColor:
                        MaterialStatePropertyAll(Color(0xFF7C3367)),
                  ),
                  child: Text(
                    "Lưu",
                    style: GoogleFonts.roboto(
                        fontSize: 12, color: const Color(0xFFFFFFFF)),
                  ),
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(right: 8.0, left: 4.0, bottom: 28),
              child: SizedBox(
                width: 66,
                height: 32,
                child: ElevatedButton(
                    onPressed: () {
                      // Đóng form
                    },
                    style: ButtonStyle(
                        backgroundColor:
                            const MaterialStatePropertyAll(Colors.white),
                        shape:
                            MaterialStateProperty.all<RoundedRectangleBorder>(
                                const RoundedRectangleBorder(
                                    side:
                                        BorderSide(color: Color(0xFF7C3367))))),
                    child: Text("Hủy",
                        style: GoogleFonts.roboto(
                            fontSize: 12, color: const Color(0xFF7C3367)))),
              ),
            ),
          ],
        ),
      );
    });
  }

  Future<TimeOfDay?> pickTime() => showTimePicker(
      context: context,
      initialTime: TimeOfDay(hour: _dateTime.hour, minute: _dateTime.minute));
  Future<DateTime?> pickDate() => showDatePicker(
      context: context,
      firstDate: DateTime.now(),
      lastDate: DateTime(2100),
      locale: const Locale('vi', 'VN'),
      initialDate: _dateTime);
}
