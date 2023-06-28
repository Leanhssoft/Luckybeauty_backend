import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class PagedRoleResultRequestDto {
  String keyWord;
  int skipCount = 0;
  int maxResultCount = 10;
  PagedRoleResultRequestDto({
    required this.keyWord,
    required this.skipCount,
    required this.maxResultCount,
  });

  @override
  String toString() => 'PagedRoleResultRequestDto(keyWord: $keyWord, skipCount: $skipCount, maxResultCount: $maxResultCount)';

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'keyWord': keyWord,
      'skipCount': skipCount,
      'maxResultCount': maxResultCount,
    };
  }

  factory PagedRoleResultRequestDto.fromMap(Map<String, dynamic> map) {
    return PagedRoleResultRequestDto(
      keyWord: map['keyWord'] as String,
      skipCount: map['skipCount'] as int,
      maxResultCount: map['maxResultCount'] as int,
    );
  }

  String toJson() => json.encode(toMap());

  factory PagedRoleResultRequestDto.fromJson(String source) => PagedRoleResultRequestDto.fromMap(json.decode(source) as Map<String, dynamic>);

  PagedRoleResultRequestDto copyWith({
    String? keyWord,
    int? skipCount,
    int? maxResultCount,
  }) {
    return PagedRoleResultRequestDto(
      keyWord: keyWord ?? this.keyWord,
      skipCount: skipCount ?? this.skipCount,
      maxResultCount: maxResultCount ?? this.maxResultCount,
    );
  }
}
