import 'package:json_annotation/json_annotation.dart';
part 'dich_vu_model.g.dart';

@JsonSerializable()
class DonViQuiDoiDto {
  @JsonKey(name: 'IdDonViQuyDoi', defaultValue: null)
  String? idDonViQuyDoi;

  @JsonKey(name: 'IdHangHoa', defaultValue: null)
  String? idHangHoa;

  @JsonKey(name: 'MaHangHoa', defaultValue: '')
  String? maHangHoa;

  @JsonKey(name: 'GiaBan', defaultValue: 0)
  double? giaBan;

  @JsonKey(name: 'TenDonViTinh', defaultValue: '')
  String? tenDonViTinh;

  @JsonKey(name: 'TyLeChuyenDoi', defaultValue: 1)
  double? tyLeChuyenDoi;

  @JsonKey(name: 'LaDonViTinhChuan', defaultValue: 1)
  int? laDonViTinhChuan;

  @JsonKey(name: 'IsDeleted', defaultValue: false)
  bool? isDeleted;

  DonViQuiDoiDto(
    this.idDonViQuyDoi,
    this.idHangHoa,
    this.maHangHoa,
    this.giaBan,
    this.tenDonViTinh,
    this.tyLeChuyenDoi,
    this.laDonViTinhChuan,
  );

  factory DonViQuiDoiDto.fromJson(Map<String, dynamic> json) =>
      _$DonViQuiDoiDtoFromJson(json);

  Map<String, dynamic> toJson() => _$DonViQuiDoiDtoToJson(this);
}

@JsonSerializable()
class DichVuViewModel {
  @JsonKey(name: 'Id', required: true)
  String id;

  @JsonKey(name: 'TenHangHoa', required: true)
  String tenHangHoa;

  @JsonKey(name: 'TenHangHoa_KhongDau', defaultValue: '')
  String? tenHangHoaKhongDau;

  @JsonKey(name: 'IdLoaiHangHoa', required: true, defaultValue: 2)
  int? idLoaiHangHoa;

  @JsonKey(name: 'IdNhomHangHoa', defaultValue: null)
  String? idNhomHangHoa;

  @JsonKey(name: 'SoPhutThucHien', defaultValue: 0)
  double? soPhutThucHien;

  @JsonKey(name: 'TrangThai', defaultValue: 1)
  int? trangThai;

  @JsonKey(name: 'MoTa', defaultValue: '')
  String? moTa;

  late DonViQuiDoiDto? donViChuan;
  late final List<DonViQuiDoiDto>? listDonViTinh;

  DichVuViewModel({
    required this.id,
    required this.tenHangHoa,
    this.idLoaiHangHoa,
    this.donViChuan,
  });

  factory DichVuViewModel.fromJson(Map<String, dynamic> json) =>
      _$DichVuViewModelFromJson(json);

  Map<String, dynamic> toJson() => _$DichVuViewModelToJson(this);
}
