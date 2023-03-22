import 'dart:convert';

// ignore_for_file: public_member_api_docs, sort_constructors_first
class DichVuViewModel {
  String id;
  String maHangHoa;
  String tenHangHoa;
  String giaBan;
  String loaiHangHoa;
  String trangThaiText;
  DichVuViewModel({
    required this.id,
    required this.maHangHoa,
    required this.tenHangHoa,
    required this.giaBan,
    required this.loaiHangHoa,
    required this.trangThaiText,
  });

  

  Map<String, dynamic> toMap() {
    return <String, dynamic>{
      'id': id,
      'maHangHoa': maHangHoa,
      'tenHangHoa': tenHangHoa,
      'giaBan': giaBan,
      'loaiHangHoa': loaiHangHoa,
      'trangThaiText': trangThaiText,
    };
  }

  factory DichVuViewModel.fromMap(Map<String, dynamic> map) {
    return DichVuViewModel(
      id: map['id'] as String,
      maHangHoa: map['maHangHoa'] as String,
      tenHangHoa: map['tenHangHoa'] as String,
      giaBan: map['giaBan'] as String,
      loaiHangHoa: map['loaiHangHoa'] as String,
      trangThaiText: map['trangThaiText'] as String,
    );
  }

  String toJson() => json.encode(toMap());

  factory DichVuViewModel.fromJson(String source) => DichVuViewModel.fromMap(json.decode(source) as Map<String, dynamic>);

  @override
  String toString() {
    return 'DichVuViewModel(id: $id, maHangHoa: $maHangHoa, tenHangHoa: $tenHangHoa, giaBan: $giaBan, loaiHangHoa: $loaiHangHoa, trangThaiText: $trangThaiText)';
  }
}
