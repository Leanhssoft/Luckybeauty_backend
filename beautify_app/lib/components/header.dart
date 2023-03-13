// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
class HeaderOfPage extends StatelessWidget {
  final VoidCallback openDrawerCallback;
  const HeaderOfPage({
    Key? key,
    required this.openDrawerCallback,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 70,
      width: MediaQuery.of(context).size.width - 64,
      child: Row(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Expanded(
              flex: 1,
              child: Padding(
                padding: const EdgeInsets.only(
                    top: 8.0, bottom: 6.0, left: 16.0, right: 16),
                child: SizedBox(
                  height: 45,
                  child: Row(
                    children: [
                      if (MediaQuery.of(context).size.width < 850)
                        InkWell(
                          onTap: openDrawerCallback,
                          child: Container(
                            padding: const EdgeInsets.all(8),
                            decoration: BoxDecoration(
                                color: Colors.grey.withOpacity(0.40),
                                borderRadius: BorderRadius.circular(30)),
                            child: const Icon(
                              Icons.menu,
                              size: 22,
                            ),
                          ),
                        ),
                      Expanded(
                        child: Padding(
                          padding: const EdgeInsets.only(left: 8),
                          child: TextField(
                            decoration: InputDecoration(
                                hintText: "Tìm kiếm...",
                                prefixIcon: const Icon(Icons.search),
                                border: OutlineInputBorder(
                                    borderRadius: BorderRadius.circular(15))),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
              )),
          Expanded(
              child: Row(
            mainAxisAlignment: MainAxisAlignment.end,
            children: [
              Padding(
                padding: const EdgeInsets.only(
                    right: 4, left: 16, top: 8, bottom: 8),
                child: InkWell(
                  onTap: () {},
                  child: Container(
                    padding: const EdgeInsets.all(10),
                    decoration: BoxDecoration(
                        color: Colors.grey.withOpacity(0.40),
                        borderRadius: BorderRadius.circular(30)),
                    child: const Icon(
                      Icons.message,
                      size: 14,
                    ),
                  ),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(
                    right: 16, left: 4, top: 8, bottom: 8),
                child: InkWell(
                  onTap: () {},
                  child: Container(
                    padding: const EdgeInsets.all(10),
                    decoration: BoxDecoration(
                        color: Colors.grey.withOpacity(0.40),
                        borderRadius: BorderRadius.circular(30)),
                    child: const Icon(
                      Icons.notifications,
                      size: 14,
                    ),
                  ),
                ),
              ),
              const Padding(
                padding: EdgeInsets.only(right: 8, left: 4, top: 8, bottom: 8),
                child: CircleAvatar(
                  backgroundImage: AssetImage('assets/images/avatarProfle.jpg'),
                ),
              ),
            ],
          )),
        ],
      ),
    );
  }
}
