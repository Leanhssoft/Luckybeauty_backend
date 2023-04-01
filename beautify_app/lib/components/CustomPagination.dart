import 'package:flutter/material.dart';

class CustomPaginator extends StatefulWidget {
  final int itemCount;
  final int perPage;
  final int pagesVisible;
  final void Function(int) onPageChanged;

  CustomPaginator({
    required this.itemCount,
    this.perPage = 10,
    this.pagesVisible = 5,
    required this.onPageChanged,
  });

  @override
  _CustomPaginatorState createState() => _CustomPaginatorState();
}

class _CustomPaginatorState extends State<CustomPaginator> {
  late int _currentPage;
  late int _lastPage;

  @override
  void initState() {
    super.initState();
    _currentPage = 1;
    _lastPage = (widget.itemCount / widget.perPage).ceil();
  }

  void _changePage(int page) {
    setState(() {
      _currentPage = page;
    });
    widget.onPageChanged(_currentPage);
  }

  List<int> _getPages() {
    final pages = <int>[];
    final half = (widget.pagesVisible / 2).floor();
    var start = _currentPage - half;
    if (start < 1) {
      start = 1;
    }
    var end = start + widget.pagesVisible - 1;
    if (end > _lastPage) {
      end = _lastPage;
      start = end - widget.pagesVisible + 1;
      if (start < 1) {
        start = 1;
      }
    }
    for (var i = start; i <= end; i++) {
      pages.add(i);
    }
    return pages;
  }

  @override
  Widget build(BuildContext context) {
    final pages = _getPages();
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        IconButton(
          onPressed: _currentPage == 1
              ? null
              : () {
                  _changePage(_currentPage - 1);
                },
          icon: Icon(Icons.arrow_left),
        ),
        for (final page in pages)
          TextButton(
            onPressed: () {
              _changePage(page);
            },
            style: ButtonStyle(
              backgroundColor: page == _currentPage
                  ? MaterialStateProperty.all(const Color(0xFF7C3367))
                  : null,
            ),
            child: Text(
              '$page',
              style: TextStyle(
                color: page == _currentPage
                    ? Colors.white
                    : const Color(0xFF666466),
              ),
            ),
          ),
        IconButton(
          onPressed: _currentPage == _lastPage ||
                  (_currentPage * widget.perPage) > widget.itemCount
              ? null
              : () {
                  _changePage(_currentPage + 1);
                },
          icon: Icon(Icons.arrow_right),
        ),
      ],
    );
  }
}
