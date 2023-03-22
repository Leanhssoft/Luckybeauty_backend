import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class PagedUserResultRequestDto {
  String keyWord;
  bool? isActive;
  int skipCount = 0;
  int maxResultCount = 10;
  PagedUserResultRequestDto({
    required this.keyWord,
    this.isActive,
    required this.skipCount,
    required this.maxResultCount,
  });

  @override
  String toString() =>
      'PagedRoleResultRequestDto(keyWord: $keyWord, skipCount: $skipCount, maxResultCount: $maxResultCount,isActive:$isActive)';

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'keyWord': keyWord,
      'skipCount': skipCount,
      'maxResultCount': maxResultCount,
      'isActive':isActive
    };
  }

  factory PagedUserResultRequestDto.fromMap(Map<String, dynamic> map) {
    return PagedUserResultRequestDto(
      keyWord: map['keyWord'] as String,
      skipCount: map['skipCount'] as int,
      maxResultCount: map['maxResultCount'] as int,
      isActive: map['isActive']
    );
  }

  String toJson() => json.encode(toMap());

  factory PagedUserResultRequestDto.fromJson(String source) =>
      PagedUserResultRequestDto.fromMap(
          json.decode(source) as Map<String, dynamic>);

  PagedUserResultRequestDto copyWith({
    String? keyWord,
    int? skipCount,
    int? maxResultCount,
    bool? isActive
  }) {
    return PagedUserResultRequestDto(
      keyWord: keyWord ?? this.keyWord,
      skipCount: skipCount ?? this.skipCount,
      maxResultCount: maxResultCount ?? this.maxResultCount,
      isActive: isActive??this.isActive
    );
  }
}
