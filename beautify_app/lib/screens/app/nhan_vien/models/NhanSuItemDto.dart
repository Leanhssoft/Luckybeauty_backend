// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class NhanSuItemDto {
  String? id;
  String? maNhanVien;
  String? tenNhanVien;
  String? diaChi;
  String? soDienThoai;
  String? cccd;
  String? ngaySinh;
  int? kieuNgaySinh;
  int? gioiTinh;
  String? ngayCap;
  String? noiCap;
  String? ngayVaoLam;
  String? avatar;
  String? tenChucVu;

  NhanSuItemDto(
      {this.id,
      this.maNhanVien,
      this.tenNhanVien,
      this.diaChi,
      this.soDienThoai,
      this.cccd,
      this.ngaySinh,
      this.kieuNgaySinh,
      this.gioiTinh,
      this.ngayCap,
      this.noiCap,
      this.ngayVaoLam,
      this.avatar,
      this.tenChucVu});

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'maNhanVien': maNhanVien,
      'tenNhanVien': tenNhanVien,
      'diaChi': diaChi,
      'soDienThoai': soDienThoai,
      'cccd': cccd,
      'ngaySinh': ngaySinh,
      'kieuNgaySinh': kieuNgaySinh,
      'gioiTinh': gioiTinh,
      'ngayCap': ngayCap,
      'noiCap': noiCap,
      'ngayVaoLam': ngayVaoLam,
      'avatar': avatar,
      'tenChucVu': tenChucVu,
    };
  }

  factory NhanSuItemDto.fromMap(Map<String, dynamic> map) {
    return NhanSuItemDto(
      id: map['id'] != null ? map['id'] as String : null,
      maNhanVien:
          map['maNhanVien'] != null ? map['maNhanVien'] as String : null,
      tenNhanVien:
          map['tenNhanVien'] != null ? map['tenNhanVien'] as String : null,
      diaChi: map['diaChi'] != null ? map['diaChi'] as String : null,
      soDienThoai:
          map['soDienThoai'] != null ? map['soDienThoai'] as String : null,
      cccd: map['cccd'] != null ? map['cccd'] as String : null,
      ngaySinh: map['ngaySinh'] != null ? map['ngaySinh'] as String : null,
      kieuNgaySinh:
          map['kieuNgaySinh'] != null ? map['kieuNgaySinh'] as int : null,
      gioiTinh: map['gioiTinh'] != null ? map['gioiTinh'] as int : null,
      ngayCap: map['ngayCap'] != null ? map['ngayCap'] as String : null,
      noiCap: map['noiCap'] != null ? map['noiCap'] as String : null,
      ngayVaoLam:
          map['ngayVaoLam'] != null ? map['ngayVaoLam'] as String : null,
      avatar: map['avatar'] != null ? map['avatar'] as String : null,
      tenChucVu: map['tenChucVu'] != null ? map['tenChucVu'] as String : null,
    );
  }

  String toJson() => json.encode(toMap());

  factory NhanSuItemDto.fromJson(String source) =>
      NhanSuItemDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
