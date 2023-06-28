// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'dart:convert';

class MeetingCalendar {
  int id;
  DateTime startTime;
  DateTime endTime;
  String? userId;
  String? userName;
  String? employeeFullName;
  String customerId;
  String? customerName;
  String? customerNumber;
  String? serviceName;
  String? noiDung;
  bool isDelete;
  String? lyDoHuy;
  String? color;
  MeetingCalendar({
    required this.id,
    required this.startTime,
    required this.endTime,
    required this.userId,
    required this.userName,
    required this.employeeFullName,
    required this.customerId,
    required this.customerName,
    required this.customerNumber,
    required this.serviceName,
    required this.noiDung,
    required this.isDelete,
    required this.lyDoHuy,
    required this.color,
  });

  MeetingCalendar copyWith({
    int? id,
    DateTime? startTime,
    DateTime? endTime,
    String? userId,
    String? userName,
    String? employeeFullName,
    String? customerId,
    String? customerName,
    String? customerNumber,
    String? serviceName,
    String? noiDung,
    bool? isDelete,
    String? lyDoHuy,
    String? color,
  }) {
    return MeetingCalendar(
      id: id ?? this.id,
      startTime: startTime ?? this.startTime,
      endTime: endTime ?? this.endTime,
      userId: userId ?? this.userId,
      userName: userName ?? this.userName,
      employeeFullName: employeeFullName ?? this.employeeFullName,
      customerId: customerId ?? this.customerId,
      customerName: customerName ?? this.customerName,
      customerNumber: customerNumber ?? this.customerNumber,
      serviceName: serviceName ?? this.serviceName,
      noiDung: noiDung ?? this.noiDung,
      isDelete: isDelete ?? this.isDelete,
      lyDoHuy: lyDoHuy ?? this.lyDoHuy,
      color: color ?? this.color,
    );
  }

  Map<dynamic, dynamic> toMap() {
    return <dynamic, dynamic>{
      'id': id,
      'startTime': startTime,
      'endTime': endTime,
      'userId': userId,
      'userName': userName,
      'employeeFullName': employeeFullName,
      'customerId': customerId,
      'customerName': customerName,
      'customerNumber': customerNumber,
      'serviceName': serviceName,
      'noiDung': noiDung,
      'isDelete': isDelete,
      'lyDoHuy': lyDoHuy,
      'color': color,
    };
  }

  factory MeetingCalendar.fromMap(Map<dynamic, dynamic> map) {
    return MeetingCalendar(
      id: map['id'],
      startTime: DateTime.parse(map['startTime']),
      endTime: DateTime.parse(map['endTime']),
      userId: map['userId'],
      userName: map['userName'],
      employeeFullName: map['employeeFullName'],
      customerId: map['customerId'],
      customerName: map['customerName'],
      customerNumber: map['customerNumber'],
      serviceName: map['serviceName'],
      noiDung: map['noiDung'],
      isDelete: map['isDelete'],
      lyDoHuy: map['lyDoHuy'],
      color: map['color'],
    );
  }

  dynamic toJson() => json.encode(toMap());

  factory MeetingCalendar.fromJson(Map<String, dynamic> json) {
    return MeetingCalendar(
      id: json['id'] as int,
      startTime: DateTime.parse(json['startTime']),
      endTime: DateTime.parse(json['endTime']),
      userId: json['userId'],
      userName: json['userName'],
      employeeFullName: json['employeeFullName'],
      customerId: json['customerId'],
      customerName: json['customerName'],
      customerNumber: json['customerNumber'],
      serviceName: json['serviceName'],
      noiDung: json['noiDung'],
      isDelete: json['isDelete'],
      lyDoHuy: json['lyDoHuy'],
      color: json['color'],
    );
  }

  @override
  String toString() {
    return 'meeting(id: $id, startTime: $startTime, endTime: $endTime, userId: $userId, userName: $userName, employeeFullName: $employeeFullName, customerId: $customerId, customerName: $customerName, customerNumber: $customerNumber, serviceName: $serviceName, noiDung: $noiDung, isDelete: $isDelete, lyDoHuy: $lyDoHuy, color: $color)';
  }

  @override
  bool operator ==(covariant MeetingCalendar other) {
    if (identical(this, other)) return true;

    return other.id == id &&
        other.startTime == startTime &&
        other.endTime == endTime &&
        other.userId == userId &&
        other.userName == userName &&
        other.employeeFullName == employeeFullName &&
        other.customerId == customerId &&
        other.customerName == customerName &&
        other.customerNumber == customerNumber &&
        other.serviceName == serviceName &&
        other.noiDung == noiDung &&
        other.isDelete == isDelete &&
        other.lyDoHuy == lyDoHuy &&
        other.color == color;
  }

  @override
  int get hashCode {
    return id.hashCode ^
        startTime.hashCode ^
        endTime.hashCode ^
        userId.hashCode ^
        userName.hashCode ^
        employeeFullName.hashCode ^
        customerId.hashCode ^
        customerName.hashCode ^
        customerNumber.hashCode ^
        serviceName.hashCode ^
        noiDung.hashCode ^
        isDelete.hashCode ^
        lyDoHuy.hashCode ^
        color.hashCode;
  }
}
