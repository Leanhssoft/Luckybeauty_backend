import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class PermissionViewModel {
  late String name;
  late String displayName;
  String? description;
  late int id;
  List<String>? grantedPermissions;
  PermissionViewModel({
    required this.name,
    required this.displayName,
    this.description,
    required this.id,
    this.grantedPermissions,
  });
  PermissionViewModel.fromJson(Map<String, dynamic> json) {
    id = json['id'];
    name = json['name'];
    displayName = json['displayName'];
    description = json['description'];
    grantedPermissions = json['grantedPermissions'];
    id = json['id'];
  }

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'name': name,
      'displayName': displayName,
      'description': description,
      'grantedPermissions': grantedPermissions,
      'id': id,
    };
  }

  factory PermissionViewModel.fromMap(Map<String, dynamic> map) {
    return PermissionViewModel(
      name: map['name'],
      displayName: map['displayName'],
      description: map['description'],
      grantedPermissions: map['grantedPermissions'],
      id: map['id'],
    );
  }

  String toJson() => json.encode(toMap());

  //factory PermissionViewModel.fromJson(dynamic source) => PermissionViewModel.fromMap(json.decode(source) as Map<String, dynamic>);

  @override
  String toString() {
    return 'PermissionViewModel(name: $name, displayName: $displayName, description: $description, id: $id,grantedPermissions:$grantedPermissions)';
  }
}
