import 'package:json_annotation/json_annotation.dart';
part 'comon_model.g.dart';

@JsonSerializable()
class ParamSearch {
  @JsonKey(defaultValue: '')
  String? textSearch;

  @JsonKey(defaultValue: 0)
  int? currentPage; // current page

  @JsonKey(defaultValue: 10)
  int? pageSize; // pagesize

  @JsonKey(defaultValue: 'CreationTime')
  String? columnSort;

  @JsonKey(defaultValue: 'DESC')
  String? typeSort;

  ParamSearch(this.textSearch, this.currentPage, this.pageSize, this.columnSort,
      this.typeSort);

  factory ParamSearch.fromJson(Map<String, dynamic> json) =>
      _$ParamSearchFromJson(json);

  Map<String, dynamic> toJson() => _$ParamSearchToJson(this);
}

@JsonSerializable()
class InputFilter {
  @JsonKey(name: 'key', defaultValue: '')
  String? key;

  @JsonKey(name: 'value', defaultValue: '')
  String? value;

  InputFilter(this.key, this.value);

  factory InputFilter.fromJson(Map<String, dynamic> json) =>
      _$InputFilterFromJson(json);

  Map<String, dynamic> toJson() => _$InputFilterToJson(this);
}
