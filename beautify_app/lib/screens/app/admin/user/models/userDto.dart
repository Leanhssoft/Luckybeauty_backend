import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class UserDto {
  late String userName;
  late String name;
  late String surname;
  late String emailAddress;
  late bool isActive;
  late String fullName;
  late DateTime? lastLoginTime;
  late String creationTime;
  late List<dynamic> roleNames;
  late String? nhanSuId;
  late int id;
  UserDto({
    required this.userName,
    required this.name,
    required this.surname,
    required this.emailAddress,
    required this.isActive,
    required this.fullName,
    this.lastLoginTime,
    required this.creationTime,
    required this.roleNames,
    this.nhanSuId,
    required this.id,
  });
  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['userName'] = userName;
    data['name'] = name;
    data['surname'] = surname;
    data['emailAddress'] = emailAddress;
    data['isActive'] = isActive;
    data['fullName'] = fullName;
    data['lastLoginTime'] = lastLoginTime;
    data['creationTime'] = creationTime;
    data['roleNames'] = roleNames;
    data['nhanSuId'] = nhanSuId;
    data['id'] = id;
    return data;
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'userName': userName,
      'name': name,
      'surname': surname,
      'emailAddress': emailAddress,
      'isActive': isActive,
      'fullName': fullName,
      'lastLoginTime': lastLoginTime?.millisecondsSinceEpoch,
      'creationTime': creationTime,
      'roleNames': roleNames,
      'nhanSuId': nhanSuId,
      'id': id,
    };
  }

  factory UserDto.fromMap(Map<String, dynamic> map) {
    return UserDto(
      userName: map['userName'] as String,
      name: map['name'] as String,
      surname: map['surname'] as String,
      emailAddress: map['emailAddress'] as String,
      isActive: map['isActive'] as bool,
      fullName: map['fullName'] as String,
      lastLoginTime: map['lastLoginTime'] != null
          ? DateTime.fromMillisecondsSinceEpoch(map['lastLoginTime'] as int)
          : null,
      creationTime: map['creationTime'],
      roleNames: map['roleNames'],
      nhanSuId: map['nhanSuId'],
      id: map['id'],
    );
  }
  UserDto.fromJson(Map<String, dynamic> json) {
    userName = json['userName'];
    name = json['name'];
    surname = json['surname'];
    emailAddress = json['emailAddress'];
    isActive = json['isActive'];
    fullName = json['fullName'];
    lastLoginTime = json['lastLoginTime'];
    creationTime = json['creationTime'];
    roleNames = json['roleNames'];
    nhanSuId = json['nhanSuId'];
    id = json['id'];
  }
  //String toJson() => json.encode(toMap());

  //factory UserDto.fromJson(String source) => UserDto.fromMap(json.decode(source) as Map<String, dynamic>);
}
