class RoleListDto {
  late String name;
  late String displayName;
  late bool isStatic;
  late bool isDefault;
  late String creationTime;
  late int id;

  RoleListDto(
      {required this.name,
      required this.displayName,
      required this.isStatic,
      required this.isDefault,
      required this.creationTime,
      required this.id});

  RoleListDto.fromJson(Map<String, dynamic> json) {
    name = json['name'];
    displayName = json['displayName'];
    isStatic = json['isStatic'];
    isDefault = json['isDefault'];
    creationTime = json['creationTime'];
    id = json['id'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['name'] = name;
    data['displayName'] = displayName;
    data['isStatic'] = isStatic;
    data['isDefault'] = isDefault;
    data['creationTime'] = creationTime;
    data['id'] = id;
    return data;
  }
}
