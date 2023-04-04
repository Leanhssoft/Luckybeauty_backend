// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'nhom_dich_vu_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

NhomDichVuDto _$NhomDichVuDtoFromJson(Map<String, dynamic> json) {
  $checkKeys(
    json,
    requiredKeys: const ['id', 'tenNhomHang'],
  );
  return NhomDichVuDto(
    id: json['id'] as String?,
    maNhomHang: json['maNhomHang'] as String? ?? '',
    tenNhomHang: json['tenNhomHang'] as String?,
    laNhomHangHoa: json['laNhomHangHoa'] as bool? ?? false,
    color: json['color'] as String? ?? '',
    moTa: json['moTa'] as String?,
    isDeleted: json['isDeleted'] as bool? ?? false,
    isSelected: json['isSelected'] as bool? ?? false,
  );
}

Map<String, dynamic> _$NhomDichVuDtoToJson(NhomDichVuDto instance) =>
    <String, dynamic>{
      'id': instance.id,
      'maNhomHang': instance.maNhomHang,
      'tenNhomHang': instance.tenNhomHang,
      'laNhomHangHoa': instance.laNhomHangHoa,
      'color': instance.color,
      'moTa': instance.moTa,
      'isDeleted': instance.isDeleted,
      'isSelected': instance.isSelected,
    };
