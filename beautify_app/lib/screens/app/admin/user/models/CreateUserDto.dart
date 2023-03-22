import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class CreateUserDto {
  String userName;
  String name;
  String surname;
  String? emailAddress;
  bool isActive;
  List<String>? roleNames;
  String password;
  String? nhanSuId;
  CreateUserDto({
    required this.userName,
    required this.name,
    required this.surname,
    this.emailAddress,
    required this.isActive,
    this.roleNames,
    required this.password,
    this.nhanSuId,
  });

  factory CreateUserDto.fromJson(String source) =>
      CreateUserDto.fromMap(json.decode(source) as Map<String, dynamic>);

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['userName'] = userName;
    data['name'] = name;
    data['surname'] = surname;
    data['emailAddress'] = emailAddress;
    data['isActive'] = isActive;
    data['roleNames'] = roleNames;
    data['password'] = password;
    data['nhanSuId'] = nhanSuId;
    return data;
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'userName': userName,
      'name': name,
      'surname': surname,
      'emailAddress': emailAddress,
      'isActive': isActive,
      'roleNames': roleNames,
      'password': password,
      'nhanSuId': nhanSuId,
    };
  }

  factory CreateUserDto.fromMap(Map<String, dynamic> map) {
    return CreateUserDto(
      userName: map['userName'],
      name: map['name'],
      surname: map['surname'],
      emailAddress: map['emailAddress'],
      isActive: map['isActive'],
      roleNames: map['roleNames'],
      password: map['password'],
      nhanSuId: map['nhanSuId'],
    );
  }

  //String toJson() => json.encode(toMap());
}
