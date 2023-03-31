// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'dich_vu_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

DonViQuiDoiDto _$DonViQuiDoiDtoFromJson(Map<String, dynamic> json) =>
    DonViQuiDoiDto(
      json['IdDonViQuyDoi'] as String?,
      json['IdHangHoa'] as String?,
      json['MaHangHoa'] as String? ?? '',
      (json['GiaBan'] as num?)?.toDouble() ?? 0,
      json['TenDonViTinh'] as String? ?? '',
      (json['TyLeChuyenDoi'] as num?)?.toDouble() ?? 1,
      json['LaDonViTinhChuan'] as int? ?? 1,
    )..isDeleted = json['IsDeleted'] as bool? ?? false;

Map<String, dynamic> _$DonViQuiDoiDtoToJson(DonViQuiDoiDto instance) =>
    <String, dynamic>{
      'IdDonViQuyDoi': instance.idDonViQuyDoi,
      'IdHangHoa': instance.idHangHoa,
      'MaHangHoa': instance.maHangHoa,
      'GiaBan': instance.giaBan,
      'TenDonViTinh': instance.tenDonViTinh,
      'TyLeChuyenDoi': instance.tyLeChuyenDoi,
      'LaDonViTinhChuan': instance.laDonViTinhChuan,
      'IsDeleted': instance.isDeleted,
    };

DichVuViewModel _$DichVuViewModelFromJson(Map<String, dynamic> json) {
  $checkKeys(
    json,
    requiredKeys: const ['Id', 'TenHangHoa', 'IdLoaiHangHoa'],
  );
  return DichVuViewModel(
    id: json['Id'] as String,
    tenHangHoa: json['TenHangHoa'] as String,
    idLoaiHangHoa: json['IdLoaiHangHoa'] as int? ?? 2,
    donViChuan: json['donViChuan'] == null
        ? null
        : DonViQuiDoiDto.fromJson(json['donViChuan'] as Map<String, dynamic>),
  )
    ..tenHangHoaKhongDau = json['TenHangHoa_KhongDau'] as String? ?? ''
    ..idNhomHangHoa = json['IdNhomHangHoa'] as String?
    ..soPhutThucHien = (json['SoPhutThucHien'] as num?)?.toDouble() ?? 0
    ..trangThai = json['TrangThai'] as int? ?? 1
    ..moTa = json['MoTa'] as String? ?? ''
    ..listDonViTinh = (json['listDonViTinh'] as List<dynamic>?)
        ?.map((e) => DonViQuiDoiDto.fromJson(e as Map<String, dynamic>))
        .toList();
}

Map<String, dynamic> _$DichVuViewModelToJson(DichVuViewModel instance) =>
    <String, dynamic>{
      'Id': instance.id,
      'TenHangHoa': instance.tenHangHoa,
      'TenHangHoa_KhongDau': instance.tenHangHoaKhongDau,
      'IdLoaiHangHoa': instance.idLoaiHangHoa,
      'IdNhomHangHoa': instance.idNhomHangHoa,
      'SoPhutThucHien': instance.soPhutThucHien,
      'TrangThai': instance.trangThai,
      'MoTa': instance.moTa,
      'donViChuan': instance.donViChuan,
      'listDonViTinh': instance.listDonViTinh,
    };
