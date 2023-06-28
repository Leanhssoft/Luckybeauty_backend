import 'package:beautify_app/layout.dart';
import 'package:beautify_app/screens/app/admin/tenant/tenantHeader.dart';
import 'package:beautify_app/screens/app/admin/tenant/tenantTable.dart';
import 'package:flutter/material.dart';

class TenantScreen extends StatefulWidget {
  const TenantScreen({super.key});

  @override
  State<TenantScreen> createState() => _TenantScreenState();
}

class _TenantScreenState extends State<TenantScreen> {
  @override
  void initState() {
    super.initState();
  }
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
