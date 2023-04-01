// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'comon_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

ParamSearch _$ParamSearchFromJson(Map<String, dynamic> json) => ParamSearch(
      json['textSearch'] as String? ?? '',
      json['currentPage'] as int? ?? 0,
      json['pageSize'] as int? ?? 10,
      json['columnSort'] as String? ?? 'CreationTime',
      json['typeSort'] as String? ?? 'DESC',
    );

Map<String, dynamic> _$ParamSearchToJson(ParamSearch instance) =>
    <String, dynamic>{
      'textSearch': instance.textSearch,
      'currentPage': instance.currentPage,
      'pageSize': instance.pageSize,
      'columnSort': instance.columnSort,
      'typeSort': instance.typeSort,
    };

InputFilter _$InputFilterFromJson(Map<String, dynamic> json) => InputFilter(
      json['key'] as String? ?? '',
      json['value'] as String? ?? '',
    );

Map<String, dynamic> _$InputFilterToJson(InputFilter instance) =>
    <String, dynamic>{
      'key': instance.key,
      'value': instance.value,
    };
