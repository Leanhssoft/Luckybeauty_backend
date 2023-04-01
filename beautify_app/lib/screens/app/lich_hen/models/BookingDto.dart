import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class BookingDto {
  DateTime startTime;
  DateTime endTime;
  String noiDung;
  String color;
  BookingDto({
    required this.startTime,
    required this.endTime,
    required this.noiDung,
    required this.color,
  });

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'startTime': startTime.millisecondsSinceEpoch,
      'endTime': endTime.millisecondsSinceEpoch,
      'noiDung': noiDung,
      'color': color,
    };
  }

  factory BookingDto.fromMap(Map<String, dynamic> map) {
    return BookingDto(
      startTime: DateTime.fromMillisecondsSinceEpoch(map['startTime'] as int),
      endTime: DateTime.fromMillisecondsSinceEpoch(map['endTime'] as int),
      noiDung: map['noiDung'] as String,
      color: map['color'] as String,
    );
  }

  String toJson() => json.encode(toMap());

  factory BookingDto.fromJson(String source) => BookingDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
