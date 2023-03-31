// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'nhom-dich-vu-model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

NhomDichVuDto _$NhomDichVuDtoFromJson(Map<String, dynamic> json) {
  $checkKeys(
    json,
    requiredKeys: const ['Id', 'TenNhomHang'],
  );
  return NhomDichVuDto(
    json['Id'] as String,
    json['MaNhomHang'] as String? ?? '',
    json['TenNhomHang'] as String,
    json['LaNhomHangHoa'] as bool? ?? false,
    json['Color'] as String? ?? '',
    json['MoTa'] as String?,
    json['IsDeleted'] as bool? ?? false,
  );
}

Map<String, dynamic> _$NhomDichVuDtoToJson(NhomDichVuDto instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'MaNhomHang': instance.maNhomHang,
      'TenNhomHang': instance.tenNhomHang,
      'LaNhomHangHoa': instance.laNhomHangHoa,
      'Color': instance.color,
      'MoTa': instance.moTa,
      'IsDeleted': instance.isDeleted,
    };
