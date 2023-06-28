// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class CreateBookingDto {
  int? tenantId;
  String? tenKhachHang;
  String? soDienThoai;
  String? bookingDate;
  String? startTime;
  String? endTime;
  int? loaiBooking;
  int? trangThai;
  String? ghiChu;

  CreateBookingDto(
      {this.tenantId = 0,
      this.tenKhachHang,
      this.soDienThoai,
      this.bookingDate,
      this.startTime,
      this.endTime,
      this.loaiBooking,
      this.trangThai,
      this.ghiChu});

  CreateBookingDto.fromJson(Map<String, dynamic> json) {
    tenantId = json['tenantId'];
    tenKhachHang = json['tenKhachHang'];
    soDienThoai = json['soDienThoai'];
    bookingDate = json['bookingDate'];
    startTime = json['startTime'];
    endTime = json['endTime'];
    loaiBooking = json['loaiBooking'];
    trangThai = json['trangThai'];
    ghiChu = json['ghiChu'];
  }
  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'tenantId': tenantId,
      'tenKhachHang': tenKhachHang,
      'soDienThoai': soDienThoai,
      'bookingDate': bookingDate,
      'startTime': startTime,
      'endTime': endTime,
      'loaiBooking': loaiBooking,
      'trangThai': trangThai,
      'ghiChu': ghiChu,
    };
  }

  factory CreateBookingDto.fromMap(Map<String, dynamic> map) {
    return CreateBookingDto(
      tenantId: map['tenantId'],
      tenKhachHang: map['tenKhachHang'],
      soDienThoai: map['soDienThoai'],
      bookingDate: map['bookingDate'],
      startTime: map['startTime'],
      endTime: map['endTime'],
      loaiBooking: map['loaiBooking'],
      trangThai: map['trangThai'],
      ghiChu: map['ghiChu'],
    );
  }

  String toJson() => json.encode(toMap());
}
