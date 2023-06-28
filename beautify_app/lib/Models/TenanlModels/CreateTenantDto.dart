class CreateTenantDto {
  late String? tenancyName;
  late String? name;
  late String? adminEmailAddress;
  late String? connectionString;
  late String? password;
  late bool? isActive;

  CreateTenantDto(
      {this.tenancyName,
      this.name,
      this.adminEmailAddress,
      this.connectionString,
      this.password,
      this.isActive});

  CreateTenantDto.fromJson(Map<String, dynamic> json) {
    tenancyName = json['tenancyName'];
    name = json['name'];
    adminEmailAddress = json['adminEmailAddress'];
    connectionString = json['connectionString'];
    password = json['password'];
    isActive = json['isActive'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['tenancyName'] = tenancyName;
    data['name'] = name;
    data['adminEmailAddress'] = adminEmailAddress;
    data['connectionString'] = connectionString;
    data['password'] = password;
    data['isActive'] = isActive;
    return data;
  }
}
