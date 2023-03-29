import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/admin/role/roleHeader.dart';
import 'package:beautify_app/screens/app/admin/role/roleTable.dart';
import 'package:beautify_app/screens/app/admin/tenant/CreateOrUpdateTenantModal.dart';
import 'package:beautify_app/screens/app/admin/tenant/tenantHeader.dart';
import 'package:beautify_app/screens/app/admin/tenant/tenantTable.dart';
import 'package:flutter/material.dart';
import 'package:responsive_table/responsive_table.dart';

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
    return SiteLayout(
      child: Scaffold(
        body: SafeArea(
          child: Row(
            children: [
              Expanded(
                flex: 5,
                child: SingleChildScrollView(
                  child: Column(
                    children: [
                      const TenantHeader(),
                      TenantTable(
                        parentContext: context,
                      )
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
