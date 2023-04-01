import 'package:json_annotation/json_annotation.dart';

import 'package:beautify_app/Models/comon_model.dart';

part 'dich_vu_filter.g.dart';

@JsonSerializable()
class DichVuFilter {
  String? idNhomHangHoas;
  ParamSearch? paramSearch;

  DichVuFilter(this.idNhomHangHoas, this.paramSearch);

  factory DichVuFilter.fromJson(Map<String, dynamic> json) =>
      _$DichVuFilterFromJson(json);

  Map<String, dynamic> toJson() => _$DichVuFilterToJson(this);
}
