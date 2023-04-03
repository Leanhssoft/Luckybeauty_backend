import 'package:json_annotation/json_annotation.dart';

part 'nhom_dich_vu_model.g.dart';

@JsonSerializable()
class NhomDichVuDto {
  @JsonKey(required: true)
  final String id;

  @JsonKey(defaultValue: '')
  final String? maNhomHang;

  @JsonKey(required: true)
  final String tenNhomHang;

  @JsonKey(defaultValue: false)
  final bool? laNhomHangHoa;

  @JsonKey(defaultValue: '')
  final String? color;

  final String? moTa;

  @JsonKey(defaultValue: false)
  final bool? isDeleted;

  @JsonKey(defaultValue: false)
  bool isSelected;

  NhomDichVuDto(this.id, this.maNhomHang, this.tenNhomHang, this.laNhomHangHoa,
      this.color, this.moTa, this.isDeleted, this.isSelected);

  factory NhomDichVuDto.fromJson(Map<String, dynamic> json) =>
      _$NhomDichVuDtoFromJson(json);

  Map<String, dynamic> toJson() => _$NhomDichVuDtoToJson(this);
}
