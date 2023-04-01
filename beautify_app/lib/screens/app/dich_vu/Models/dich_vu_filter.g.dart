// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'dich_vu_filter.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

DichVuFilter _$DichVuFilterFromJson(Map<String, dynamic> json) => DichVuFilter(
      json['idNhomHangHoas'] as String?,
      json['paramSearch'] == null
          ? null
          : ParamSearch.fromJson(json['paramSearch'] as Map<String, dynamic>),
    );

Map<String, dynamic> _$DichVuFilterToJson(DichVuFilter instance) =>
    <String, dynamic>{
      'idNhomHangHoas': instance.idNhomHangHoas,
      'paramSearch': instance.paramSearch,
    };
