import 'package:beautify_app/screens/app/dich_vu/Models/loai_dich_vu_model.dart';
import 'package:json_annotation/json_annotation.dart';
import 'package:beautify_app/helper/common_func.dart';
import 'package:syncfusion_flutter_datagrid/datagrid.dart';
import 'package:flutter/material.dart';

part 'dich_vu_model.g.dart';

@JsonSerializable()
class DonViQuiDoiDto {
  @JsonKey(defaultValue: null)
  String? idDonViQuyDoi;

  @JsonKey(defaultValue: null)
  String? idHangHoa;

  @JsonKey(defaultValue: '')
  String? maHangHoa;

  @JsonKey(defaultValue: 0)
  double? giaBan;

  @JsonKey(defaultValue: '')
  String? tenDonViTinh;

  @JsonKey(defaultValue: 1)
  double? tyLeChuyenDoi;

  @JsonKey(defaultValue: 1)
  int? laDonViTinhChuan;

  @JsonKey(defaultValue: false)
  bool? isDeleted;

  DonViQuiDoiDto(
    this.idDonViQuyDoi,
    // this.idHangHoa,
    this.maHangHoa,
    this.giaBan,
    this.tenDonViTinh,
    this.tyLeChuyenDoi,
    this.laDonViTinhChuan,
  );

  factory DonViQuiDoiDto.fromJson(Map<String, dynamic> json) =>
      _$DonViQuiDoiDtoFromJson(json);

  Map<String, dynamic> toJson() => _$DonViQuiDoiDtoToJson(this);
}

@JsonSerializable(explicitToJson: true)
class DichVuViewModel extends DonViQuiDoiDto {
  @JsonKey(required: true)
  @override
  String id;

  @JsonKey(required: true)
  String tenHangHoa;

  @JsonKey(required: true, defaultValue: 2)
  int? idLoaiHangHoa;

  @JsonKey(defaultValue: '')
  String? idNhomHangHoa;

  @JsonKey(defaultValue: 0)
  double? soPhutThucHien;

  @JsonKey(defaultValue: 1)
  int? trangThai;

  @JsonKey(defaultValue: '')
  String? moTa;

  @JsonKey(defaultValue: '')
  String? tenNhomHang;

  @JsonKey(defaultValue: '')
  String? txtTrangThaiHang;

  String? get tenHangHoaKhongDau {
    return convertVietNamtoEng(tenHangHoa);
  }

  // late final List<DonViQuiDoiDto>? donViTinhs;

  DichVuViewModel({
    required this.id,
    required this.tenHangHoa,
    this.idLoaiHangHoa,
  }) : super(null, '', 0, '', 1, 1);

  factory DichVuViewModel.fromJson(Map<String, dynamic> json) =>
      _$DichVuViewModelFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$DichVuViewModelToJson(this);
}

class DichVuDataSource extends DataGridSource {
  List<DataGridRow> dataGridRows = [];

  DichVuDataSource({required List<DichVuViewModel> products}) {
    dataGridRows = products
        .map<DataGridRow>((dataGridRow) => DataGridRow(
              cells: [
                DataGridCell<String>(
                    columnName: 'maHangHoa', value: dataGridRow.maHangHoa),
                DataGridCell<String>(
                    columnName: 'tenHangHoa', value: dataGridRow.tenHangHoa),
                DataGridCell<String>(
                    columnName: 'tenNhomHang',
                    value: dataGridRow.tenNhomHang ?? ''),
                DataGridCell<double>(
                    columnName: 'giaBan', value: dataGridRow.giaBan),
                DataGridCell<double>(
                    columnName: 'soPhutThucHien',
                    value: dataGridRow.soPhutThucHien),
                DataGridCell<String>(
                    columnName: 'txtTrangThaiHang',
                    value: dataGridRow.txtTrangThaiHang),
              ],
            ))
        .toList();
  }
  @override
  List<DataGridRow> get rows => dataGridRows;

  @override
  DataGridRowAdapter? buildRow(DataGridRow row) {
    return DataGridRowAdapter(
        cells: row.getCells().map<Widget>((dataGridCell) {
      return Container(
          alignment: (dataGridCell.columnName == "giaBan" ||
                  dataGridCell.columnName == 'soPhutThucHien')
              ? Alignment.centerRight
              : Alignment.centerLeft,
          padding: const EdgeInsets.symmetric(horizontal: 4),
          child: Text(
            dataGridCell.value.toString(),
            overflow: TextOverflow.ellipsis,
          ));
    }).toList());
  }
}
