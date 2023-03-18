class TenantViewModel {
  late int id;
  late String tenantName;
  late String storeName;
  String? chuSoHuu;
  String? address;
  String? email;
  String? soDienThoai;
  String? nganhKinhDoanh;
  String? goiDichVu;
  String? connectionStrings;
  DateTime? ngayDangKy;
  DateTime? ngayHetHan;
  bool? trangThai;

  TenantViewModel(
      {required this.id,
      required this.tenantName,
      required this.storeName,
      this.chuSoHuu,
      this.address,
      this.email,
      this.soDienThoai,
      this.nganhKinhDoanh,
      this.goiDichVu,
      this.connectionStrings,
      this.ngayDangKy,
      this.ngayHetHan,
      this.trangThai});

  TenantViewModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    tenantName = json['tenantName'];
    storeName = json['name'];
    chuSoHuu = json['chuSoHuu'];
    address = json['address'];
    email = json['email'];
    soDienThoai = json['soDienThoai'];
    nganhKinhDoanh = json['nganhKinhDoanh'];
    goiDichVu = json['goiDichVu'];
    connectionStrings = json['connectionStrings'];
    ngayDangKy = json['ngayDangKy'];
    ngayHetHan = json['ngayHetHan'];
    trangThai = json['trangThai'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['id'] = id;
    data['tenantName'] = tenantName;
    data['storeName'] = storeName;
    data['chuSoHuu'] = chuSoHuu;
    data['address'] = address;
    data['email'] = email;
    data['soDienThoai'] = soDienThoai;
    data['nganhKinhDoanh'] = nganhKinhDoanh;
    data['goiDichVu'] = goiDichVu;
    data['connectionStrings'] = connectionStrings;
    data['ngayDangKy'] = ngayDangKy;
    data['ngayHetHan'] = ngayHetHan;
    data['trangThai'] = trangThai;
    return data;
  }
}