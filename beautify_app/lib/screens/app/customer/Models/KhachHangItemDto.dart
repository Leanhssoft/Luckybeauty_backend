import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class KhachHangItemDto {
  String? id;
  String? tenKhachHang;
  String? soDienThoai;
  String? gioiTinh;
  String? tenNhomKhach;
  String? tenNguonKhach;
  String? nhanVienPhuTrach;
  String? cuocHenGanNhat;
  num? tongChiTieu;
  KhachHangItemDto({
    this.id,
    this.tenKhachHang,
    this.soDienThoai,
    this.gioiTinh,
    this.tenNhomKhach,
    this.tenNguonKhach,
    this.nhanVienPhuTrach,
    this.cuocHenGanNhat,
    this.tongChiTieu,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'tenKhachHang': tenKhachHang,
      'soDienThoai': soDienThoai,
      'gioiTinh': gioiTinh,
      'tenNhomKhach': tenNhomKhach,
      'tenNguonKhach': tenNguonKhach,
      'nhanVienPhuTrach': nhanVienPhuTrach,
      'tongChiTieu': tongChiTieu,
      'cuocHenGanNhat': cuocHenGanNhat
    };
  }

  factory KhachHangItemDto.fromMap(Map<String, dynamic> map) {
    return KhachHangItemDto(
      id: map['id'] != null
          ? map['id'] as String
          : "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      tenKhachHang:
          map['tenKhachHang'] != null ? map['tenKhachHang'] as String : null,
      soDienThoai:
          map['soDienThoai'] != null ? map['soDienThoai'] as String : null,
      gioiTinh: map['gioiTinh'] != null ? map['gioiTinh'] as String : null,
      tenNhomKhach:
          map['tenNhomKhach'] != null ? map['tenNhomKhach'] as String : null,
      tenNguonKhach:
          map['tenNguonKhach'] != null ? map['tenNguonKhach'] as String : null,
      nhanVienPhuTrach: map['nhanVienPhuTrach'] != null
          ? map['nhanVienPhuTrach'] as String
          : null,
      tongChiTieu:
          map['tongChiTieu'] != null ? map['tongChiTieu'] as num : null,
      cuocHenGanNhat: map['cuocHenGanNhat']
    );
  }

  String toJson() => json.encode(toMap());

  factory KhachHangItemDto.fromJson(String source) =>
      KhachHangItemDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
