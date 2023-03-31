import 'package:json_annotation/json_annotation.dart';

part 'nhom-dich-vu-model.g.dart';

@JsonSerializable()
class NhomDichVuDto {
  @JsonKey(name: 'Id', required: true)
  final String id;

  @JsonKey(name: 'MaNhomHang', defaultValue: '')
  final String? maNhomHang;

  @JsonKey(name: 'TenNhomHang', required: true)
  final String tenNhomHang;

  @JsonKey(name: 'LaNhomHangHoa', defaultValue: false)
  final bool? laNhomHangHoa;

  @JsonKey(name: 'Color', defaultValue: '')
  final String? color;

  @JsonKey(name: 'MoTa')
  final String? moTa;

  @JsonKey(name: 'IsDeleted', defaultValue: false)
  final bool? isDeleted;

  NhomDichVuDto(this.id, this.maNhomHang, this.tenNhomHang, this.laNhomHangHoa,
      this.color, this.moTa, this.isDeleted);

  factory NhomDichVuDto.fromJson(Map<String, dynamic> json) =>
      _$NhomDichVuDtoFromJson(json);

  Map<String, dynamic> toJson() => _$NhomDichVuDtoToJson(this);
}
