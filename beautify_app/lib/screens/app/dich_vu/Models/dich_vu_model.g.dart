// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'dich_vu_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

DonViQuiDoiDto _$DonViQuiDoiDtoFromJson(Map<String, dynamic> json) =>
    DonViQuiDoiDto(
      json['idDonViQuyDoi'] as String?,
      json['maHangHoa'] as String? ?? '',
      (json['giaBan'] as num?)?.toDouble() ?? 0,
      json['tenDonViTinh'] as String? ?? '',
      (json['tyLeChuyenDoi'] as num?)?.toDouble() ?? 1,
      json['laDonViTinhChuan'] as int? ?? 1,
    )
      ..idHangHoa = json['idHangHoa'] as String?
      ..isDeleted = json['isDeleted'] as bool? ?? false;

Map<String, dynamic> _$DonViQuiDoiDtoToJson(DonViQuiDoiDto instance) =>
    <String, dynamic>{
      'idDonViQuyDoi': instance.idDonViQuyDoi,
      'idHangHoa': instance.idHangHoa,
      'maHangHoa': instance.maHangHoa,
      'giaBan': instance.giaBan,
      'tenDonViTinh': instance.tenDonViTinh,
      'tyLeChuyenDoi': instance.tyLeChuyenDoi,
      'laDonViTinhChuan': instance.laDonViTinhChuan,
      'isDeleted': instance.isDeleted,
    };

DichVuViewModel _$DichVuViewModelFromJson(Map<String, dynamic> json) {
  $checkKeys(
    json,
    requiredKeys: const ['id', 'tenHangHoa', 'idLoaiHangHoa'],
  );
  return DichVuViewModel(
    id: json['id'] as String,
    tenHangHoa: json['tenHangHoa'] as String,
    idLoaiHangHoa: json['idLoaiHangHoa'] as int? ?? 2,
  )
    ..idDonViQuyDoi = json['idDonViQuyDoi'] as String?
    ..idHangHoa = json['idHangHoa'] as String?
    ..maHangHoa = json['maHangHoa'] as String? ?? ''
    ..giaBan = (json['giaBan'] as num?)?.toDouble() ?? 0
    ..tenDonViTinh = json['tenDonViTinh'] as String? ?? ''
    ..tyLeChuyenDoi = (json['tyLeChuyenDoi'] as num?)?.toDouble() ?? 1
    ..laDonViTinhChuan = json['laDonViTinhChuan'] as int? ?? 1
    ..isDeleted = json['isDeleted'] as bool? ?? false
    ..idNhomHangHoa = json['idNhomHangHoa'] as String? ?? ''
    ..soPhutThucHien = (json['soPhutThucHien'] as num?)?.toDouble() ?? 0
    ..trangThai = json['trangThai'] as int? ?? 1
    ..moTa = json['moTa'] as String? ?? '';
}

Map<String, dynamic> _$DichVuViewModelToJson(DichVuViewModel instance) =>
    <String, dynamic>{
      'idDonViQuyDoi': instance.idDonViQuyDoi,
      'idHangHoa': instance.idHangHoa,
      'maHangHoa': instance.maHangHoa,
      'giaBan': instance.giaBan,
      'tenDonViTinh': instance.tenDonViTinh,
      'tyLeChuyenDoi': instance.tyLeChuyenDoi,
      'laDonViTinhChuan': instance.laDonViTinhChuan,
      'isDeleted': instance.isDeleted,
      'id': instance.id,
      'tenHangHoa': instance.tenHangHoa,
      'idLoaiHangHoa': instance.idLoaiHangHoa,
      'idNhomHangHoa': instance.idNhomHangHoa,
      'soPhutThucHien': instance.soPhutThucHien,
      'trangThai': instance.trangThai,
      'moTa': instance.moTa,
    };
