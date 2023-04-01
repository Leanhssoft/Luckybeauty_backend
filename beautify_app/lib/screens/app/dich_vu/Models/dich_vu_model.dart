import 'package:json_annotation/json_annotation.dart';
import 'package:beautify_app/helper/common_func.dart';

part 'dich_vu_model.g.dart';

@JsonSerializable()
class DonViQuiDoiDto {
  @JsonKey(defaultValue: null)
  String? idDonViQuyDoi;

  @JsonKey(defaultValue: null)
  String? idHangHoa;

  @JsonKey(defaultValue: '')
  String? maHangHoa;

  @JsonKey(defaultValue: 0)
  double? giaBan;

  @JsonKey(defaultValue: '')
  String? tenDonViTinh;

  @JsonKey(defaultValue: 1)
  double? tyLeChuyenDoi;

  @JsonKey(defaultValue: 1)
  int? laDonViTinhChuan;

  @JsonKey(defaultValue: false)
  bool? isDeleted;

  DonViQuiDoiDto(
    this.idDonViQuyDoi,
    // this.idHangHoa,
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

@JsonSerializable(explicitToJson: true)
class DichVuViewModel extends DonViQuiDoiDto {
  @JsonKey(required: true)
  String id;

  @JsonKey(required: true)
  String tenHangHoa;

  @JsonKey(required: true, defaultValue: 2)
  int? idLoaiHangHoa;

  @JsonKey(defaultValue: '')
  String? idNhomHangHoa;

  @JsonKey(defaultValue: 0)
  double? soPhutThucHien;

  @JsonKey(defaultValue: 1)
  int? trangThai;

  @JsonKey(defaultValue: '')
  String? moTa;

  String? get tenHangHoaKhongDau {
    return convertVietNamtoEng(tenHangHoa);
  }

  // late final List<DonViQuiDoiDto>? donViTinhs;

  DichVuViewModel({
    required this.id,
    required this.tenHangHoa,
    this.idLoaiHangHoa,
  }) : super(null, '', 0, '', 1, 1);

  factory DichVuViewModel.fromJson(Map<String, dynamic> json) =>
      _$DichVuViewModelFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$DichVuViewModelToJson(this);
}
