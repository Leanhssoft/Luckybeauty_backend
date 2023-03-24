class CreateOrEditNhanSuDto {
  late String? id;
  late String? maNhanVien;
  late String tenNhanVien;
  late String? diaChi;
  late String soDienThoai;
  late String? cccd;
  late String? ngaySinh;
  late int? kieuNgaySinh;
  late int gioiTinh;
  late String? ngayCap;
  late String? noiCap;
  late String? avatar;
  late String? idChucVu;

  CreateOrEditNhanSuDto(
      {this.id,
      this.maNhanVien,
      required this.tenNhanVien,
      this.diaChi,
      required this.soDienThoai,
      this.cccd,
      this.ngaySinh,
      this.kieuNgaySinh,
      required this.gioiTinh,
      this.ngayCap,
      this.noiCap,
      this.avatar,
      this.idChucVu});

  CreateOrEditNhanSuDto.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    maNhanVien = json['maNhanVien'];
    tenNhanVien = json['tenNhanVien'];
    diaChi = json['diaChi'];
    soDienThoai = json['soDienThoai'];
    cccd = json['cccd'];
    ngaySinh = json['ngaySinh'];
    kieuNgaySinh = json['kieuNgaySinh'];
    gioiTinh = json['gioiTinh'];
    ngayCap = json['ngayCap'];
    noiCap = json['noiCap'];
    avatar = json['avatar'];
    idChucVu = json['idChucVu'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['id'] = id;
    data['maNhanVien'] = maNhanVien;
    data['tenNhanVien'] = tenNhanVien;
    data['diaChi'] = diaChi;
    data['soDienThoai'] = soDienThoai;
    data['cccd'] = cccd;
    data['ngaySinh'] = ngaySinh;
    data['kieuNgaySinh'] = kieuNgaySinh;
    data['gioiTinh'] = gioiTinh;
    data['ngayCap'] = ngayCap;
    data['noiCap'] = noiCap;
    data['avatar'] = avatar;
    data['idChucVu'] = idChucVu;
    return data;
  }
}