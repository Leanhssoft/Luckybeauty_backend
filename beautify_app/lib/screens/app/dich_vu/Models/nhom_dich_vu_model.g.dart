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
    json['id'] as String,
    json['maNhomHang'] as String? ?? '',
    json['tenNhomHang'] as String,
    json['laNhomHangHoa'] as bool? ?? false,
    json['color'] as String? ?? '',
    json['moTa'] as String?,
    json['isDeleted'] as bool? ?? false,
    json['isSelected'] as bool? ?? false,
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
