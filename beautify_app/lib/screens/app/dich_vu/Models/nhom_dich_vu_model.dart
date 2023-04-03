import 'package:json_annotation/json_annotation.dart';
import 'package:beautify_app/helper/common_func.dart';

part 'nhom_dich_vu_model.g.dart';

@JsonSerializable()
class NhomDichVuDto {
  @JsonKey(required: true)
  late String? id;

  @JsonKey(defaultValue: '')
  late String? maNhomHang;

  @JsonKey(required: true)
  late String? tenNhomHang;

  @JsonKey(defaultValue: false)
  late bool? laNhomHangHoa;

  @JsonKey(defaultValue: '')
  late String? color;

  late String? moTa;

  @JsonKey(defaultValue: false)
  late bool? isDeleted;

  @JsonKey(defaultValue: false)
  late bool? isSelected;

  String get tenNhomHangKhongDau {
    return convertVietNamtoEng(tenNhomHang);
  }

  NhomDichVuDto(
      {this.id,
      this.maNhomHang,
      this.tenNhomHang,
      this.laNhomHangHoa,
      this.color,
      this.moTa,
      this.isDeleted,
      this.isSelected});

  factory NhomDichVuDto.fromJson(Map<String, dynamic> json) =>
      _$NhomDichVuDtoFromJson(json);

  Map<String, dynamic> toJson() => _$NhomDichVuDtoToJson(this);
}
