class UserPermission {
  late final String user;
  late final List<dynamic> permissions;
  
  UserPermission({required this.user, required this.permissions});
  
  UserPermission.fromJson(Map<String, dynamic> json) {
    user = json['user'];
    permissions = json['permissions'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['user'] = user;
    data['permissions'] = permissions;
    return data;
  }
}