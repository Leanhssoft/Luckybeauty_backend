
import 'package:beautify_app/screens/app/admin/tenant/CreateOrUpdateTenantModal.dart';
import 'package:flutter/material.dart';
import 'package:responsive_table/responsive_table.dart';
import 'package:syncfusion_flutter_datagrid/datagrid.dart';

class TenantScreen extends StatefulWidget {
  const TenantScreen({super.key});

  @override
  State<TenantScreen> createState() => _TenantScreenState();
}

class _TenantScreenState extends State<TenantScreen> {
  late final List<Map<String, dynamic>> _source = [];
  @override
  void initState() {
    super.initState();
  }

  final List<int> _perPages = [10, 20, 50, 100];
  final int _total = 0;
  int? _currentPerPage = 10;
  List<bool>? _expanded;
  final String _searchKey = "id";

  int _currentPage = 1;
  final bool _isSearch = false;
  final String _selectableKey = "id";

  String? _sortColumn;
  final bool _sortAscending = true;
  final bool _isLoading = true;
  final bool _showSelect = true;
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Row(
          children: [
            Expanded(
              flex: 5,
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    Padding(
                      padding: const EdgeInsets.all(16),
                      child: SingleChildScrollView(
                          child: Column(
                              mainAxisAlignment: MainAxisAlignment.start,
                              mainAxisSize: MainAxisSize.max,
                              children: [
                            Container(
                              margin: const EdgeInsets.all(10),
                              padding: const EdgeInsets.all(0),
                              constraints: const BoxConstraints(
                                maxHeight: 700,
                              ),
                              child: Card(
                                elevation: 1,
                                shadowColor: Colors.black,
                                clipBehavior: Clip.none,
                                child: ResponsiveDatatable(
                                  title: const Text(""),
                                  actions: [
                                    ElevatedButton(
                                      onPressed: () {
                                        showDialog(
                                            context: context,
                                            builder: (BuildContext context) {
                                              return const CreateTenantModal(
                                                headerModel: "Thêm mới Tenant",
                                              );
                                            });
                                      },
                                      child: const Text("Thêm mới Tenant"),
                                    ),
                                  ],
                                  reponseScreenSizes: const [ScreenSize.xs],
                                  headers: [
                                    DatatableHeader(
                                        text: "STT",
                                        value: "",
                                        show: true,
                                        sortable: true,
                                        textAlign: TextAlign.center),
                                    DatatableHeader(
                                        text: "Tenant Name",
                                        value: "tenantName",
                                        show: true,
                                        flex: 2,
                                        sortable: true,
                                        textAlign: TextAlign.left),
                                    DatatableHeader(
                                        text: "Tên cửa hàng",
                                        value: "name",
                                        show: true,
                                        sortable: true,
                                        textAlign: TextAlign.center),
                                    DatatableHeader(
                                        text: "Địa chỉ",
                                        value: "address",
                                        show: true,
                                        sortable: true,
                                        textAlign: TextAlign.left),
                                    DatatableHeader(
                                        text: "Logo",
                                        value: "logo",
                                        show: true,
                                        sortable: true,
                                        textAlign: TextAlign.left),
                                  ],
                                  source: _source,
                                  showSelect: _showSelect,
                                  autoHeight: false,
                                  onChangedRow: (value, header) {
                                    /// print(value);
                                    /// print(header);
                                  },
                                  onSubmittedRow: (value, header) {
                                    /// print(value);
                                    /// print(header);
                                  },
                                  onTabRow: (data) {
                                    print(data);
                                  },
                                  expanded: _expanded,
                                  sortAscending: _sortAscending,
                                  sortColumn: _sortColumn,
                                  isLoading: _isLoading,
                                  footers: [
                                    Container(
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 15),
                                      child: const Text("Rows per page:"),
                                    ),
                                    if (_perPages.isNotEmpty)
                                      Container(
                                        padding: const EdgeInsets.symmetric(
                                            horizontal: 15),
                                        child: DropdownButton<int>(
                                          value: _currentPerPage,
                                          items: _perPages
                                              .map((e) => DropdownMenuItem<int>(
                                                    value: e,
                                                    child: Text("$e"),
                                                  ))
                                              .toList(),
                                          onChanged: (dynamic value) {
                                            setState(() {
                                              _currentPerPage = value;
                                              _currentPage = 1;
                                            });
                                          },
                                          isExpanded: false,
                                        ),
                                      ),
                                    Container(
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 15),
                                      child: Text(
                                          "$_currentPage - $_currentPerPage of $_total"),
                                    ),
                                    IconButton(
                                      icon: const Icon(
                                        Icons.arrow_back_ios,
                                        size: 16,
                                      ),
                                      onPressed: _currentPage == 1
                                          ? null
                                          : () {
                                              var _nextSet = _currentPage -
                                                  _currentPerPage!;
                                              setState(() {
                                                _currentPage =
                                                    _nextSet > 1 ? _nextSet : 1;
                                              });
                                            },
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 15),
                                    ),
                                    IconButton(
                                      icon: const Icon(Icons.arrow_forward_ios,
                                          size: 16),
                                      onPressed: _currentPage +
                                                  _currentPerPage! -
                                                  1 >
                                              _total
                                          ? null
                                          : () {
                                              var _nextSet = _currentPage +
                                                  _currentPerPage!;

                                              setState(() {
                                                _currentPage = _nextSet < _total
                                                    ? _nextSet
                                                    : _total - _currentPerPage!;
                                              });
                                            },
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 15),
                                    )
                                  ],
                                  headerDecoration: const BoxDecoration(
                                      color: Colors.grey,
                                      border: Border(
                                          bottom: BorderSide(
                                              color: Colors.red, width: 1))),
                                  selectedDecoration: BoxDecoration(
                                    border: Border(
                                        bottom: BorderSide(
                                            color: Colors.green[300]!,
                                            width: 1)),
                                    color: Colors.green,
                                  ),
                                  headerTextStyle:
                                      const TextStyle(color: Colors.white),
                                  rowTextStyle:
                                      const TextStyle(color: Colors.green),
                                  selectedTextStyle:
                                      const TextStyle(color: Colors.white),
                                ),
                              ),
                            ),
                          ])),
                    )
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

