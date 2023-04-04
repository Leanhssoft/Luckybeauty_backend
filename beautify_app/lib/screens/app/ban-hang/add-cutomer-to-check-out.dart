import 'package:beautify_app/Service/Models/SuggestKhachHangDto.dart';
import 'package:beautify_app/Service/SuggestServices.dart';
import 'package:beautify_app/components/CustomTextFormField.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:searchfield/searchfield.dart';

class AddCustomerToCheckOutModal extends StatefulWidget {
  const AddCustomerToCheckOutModal({super.key});

  @override
  State<AddCustomerToCheckOutModal> createState() =>
      _AddCustomerToCheckOutModalState();
}

class _AddCustomerToCheckOutModalState
    extends State<AddCustomerToCheckOutModal> {
  late List<SuggestKhachHangDto> _suggestKhachHang = [];
  late String? _idKhachHangSelected;
  void getData() async {
    var suggestKhachHang = await SuggestServices().suggestKhachHang();
    setState(() {
      _suggestKhachHang = suggestKhachHang;
    });
  }

  @override
  void initState() {
    super.initState();
    getData();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
        builder: (BuildContext context, BoxConstraints constraints) {
      return AlertDialog(
        title: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text(
              "Thêm khách hàng checkout",
              style: GoogleFonts.roboto(
                  color: const Color(0xFF333233), fontSize: 24),
            ),
            Container(
              width: 32,
              height: 32,
              decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(8),
                  border: Border.all(color: const Color(0xFF999699), width: 2)),
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
          width: 840,
          height: 368,
          child: Padding(
            padding: const EdgeInsets.only(top: 28, bottom: 28),
            child: Row(
              children: [
                Expanded(
                  flex: 4,
                  child: Container(
                    child: Form(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Text(
                            "Họ và tên",
                            style: GoogleFonts.roboto(
                                fontSize: 14, color: const Color(0xFF999699)),
                          ),
                          const SizedBox(
                            height: 8,
                          ),
                          CustomTextFormField(
                            controller: TextEditingController(text: ""),
                            hintText: "Nhập họ tên khách hàng",
                            onSaved: (value) {
                              setState(() {});
                            },
                            onChanged: (value) {
                              setState(() {});
                            },
                          ),
                          const SizedBox(
                            height: 24,
                          ),
                          Text(
                            "Số điện thoại",
                            style: GoogleFonts.roboto(
                                fontSize: 14, color: const Color(0xFF999699)),
                          ),
                          const SizedBox(
                            height: 8,
                          ),
                          CustomTextFormField(
                            controller: TextEditingController(text: ""),
                            hintText: "Nhập số điện thoại khách hàng",
                            onSaved: (value) {
                              setState(() {});
                            },
                            onChanged: (value) {
                              setState(() {});
                            },
                          ),
                          const SizedBox(
                            height: 44,
                          ),
                          Row(
                            mainAxisAlignment: MainAxisAlignment.end,
                            crossAxisAlignment: CrossAxisAlignment.end,
                            children: [
                              Padding(
                                padding:
                                    const EdgeInsets.only(right: 4, bottom: 28),
                                child: SizedBox(
                                  width: 66,
                                  height: 32,
                                  child: ElevatedButton(
                                    onPressed: () {
                                      //if (_formKey.currentState!.validate()) {
                                      //_formKey.currentState!.save();
                                      //_saveData();
                                      Navigator.of(context).pop();
                                      //}
                                    },
                                    style: const ButtonStyle(
                                      backgroundColor: MaterialStatePropertyAll(
                                          Color(0xFF7C3367)),
                                    ),
                                    child: Text(
                                      "Lưu",
                                      style: GoogleFonts.roboto(
                                          fontSize: 12,
                                          color: const Color(0xFFFFFFFF)),
                                    ),
                                  ),
                                ),
                              ),
                              Padding(
                                padding: const EdgeInsets.only(
                                    right: 8.0, left: 4.0, bottom: 28),
                                child: SizedBox(
                                  width: 66,
                                  height: 32,
                                  child: ElevatedButton(
                                      onPressed: () {
                                        // Đóng form
                                      },
                                      style: ButtonStyle(
                                          backgroundColor:
                                              const MaterialStatePropertyAll(
                                                  Colors.white),
                                          shape: MaterialStateProperty.all<
                                                  RoundedRectangleBorder>(
                                              const RoundedRectangleBorder(
                                                  side: BorderSide(
                                                      color:
                                                          Color(0xFF7C3367))))),
                                      child: Text("Hủy",
                                          style: GoogleFonts.roboto(
                                              fontSize: 12,
                                              color: const Color(0xFF7C3367)))),
                                ),
                              )
                            ],
                          )
                        ],
                      ),
                    ),
                  ),
                ),
                Expanded(
                    flex: 3,
                    child: Container(
                        alignment: Alignment.topRight,
                        padding: const EdgeInsets.only(right: 16, left: 64),
                        child: SearchField(
                          suggestions: _suggestKhachHang
                              .map((e) => SearchFieldListItem(
                                  e.tenKhachHang.toString(),
                                  child: Padding(
                                    padding: const EdgeInsets.all(8.0),
                                    child: Text( e.tenKhachHang.toString(),style: GoogleFonts.roboto(fontSize: 16),),
                                  ),
                                  item: e))
                              .toList(),
                          hint: "Tìm tên",
                          itemHeight: 50,
                          maxSuggestionsInViewPort: 6,
                          searchInputDecoration: InputDecoration(
                              prefix: Icon(Icons.search),
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
                                      const BorderSide(color: Colors.black))),
                          suggestionsDecoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(8),
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
                          onSuggestionTap:
                              (SearchFieldListItem<SuggestKhachHangDto> item) {
                            setState(() {
                              _idKhachHangSelected = item.item!.id.toString();
                            });
                            
                          },
                        )))
              ],
            ),
          ),
        ),
      );
    });
  }
}
