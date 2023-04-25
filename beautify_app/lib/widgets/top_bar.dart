import 'package:beautify_app/helper/responsivesLayout.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

import '../constants/styles.dart';

PreferredSize TopBarNavigation(
        BuildContext context, GlobalKey<ScaffoldState> key) =>
    PreferredSize(
      preferredSize: const Size.fromHeight(70),
      child: Center(
        child: AppBar(
          leading: !ResponsiveWidget.isSmallScreen(context)
              ? Container(
                  color: Colors.white,
                  child: Padding(
                    padding: const EdgeInsets.all(16.0),
                    child: Row(
                      children: [
                        Padding(
                          padding: const EdgeInsets.only(left: 18),
                          child: SizedBox(
                            width: 24,
                            height: 24,
                            child: Image.asset('assets/images/Lucky_beauty.jpg',
                                fit: BoxFit.cover),
                          ),
                        ),
                        Padding(
                          padding: const EdgeInsets.only(left: 16),
                          child: TextButton(
                              onPressed: () {
                                Navigator.pushNamed(context, "/home");
                              },
                              child: Text("Lucky Beauty",
                                  style: GoogleFonts.roboto(
                                    color: const Color(0xFF1F0D1A),
                                    fontSize: 18,
                                  ))),
                        ),
                      ],
                    ),
                  ),
                )
              : IconButton(
                  icon: const Icon(Icons.menu),
                  color: Colors.black.withOpacity(.7),
                  onPressed: () {
                    key.currentState!.openDrawer();
                  },
                ),
          title: Center(
            child: Row(
              children: [
                !ResponsiveWidget.isSmallScreen(context)
                    ? const Spacer(
                        flex: 1,
                      )
                    : const SizedBox(
                        width: 8,
                      ),
                Expanded(
                    flex: !ResponsiveWidget.isSmallScreen(context) ? 5 : 1,
                    child: SizedBox(
                      height: 40,
                      child: Center(
                        child: Row(
                          children: [
                            Expanded(
                              flex: 1,
                              child: TextField(
                                decoration: InputDecoration(
                                    hintText: "Tìm kiếm...",
                                    prefixIcon: const Icon(Icons.search),
                                    border: OutlineInputBorder(
                                        borderRadius:
                                            BorderRadius.circular(15))),
                              ),
                            ),
                            Expanded(
                                flex: 2,
                                child: Row(
                                  mainAxisAlignment: MainAxisAlignment.end,
                                  children: [
                                    SizedBox(
                                      width: 40,
                                      height: 40,
                                      child: IconButton(
                                        icon: Icon(
                                          Icons.settings,
                                          color: dark,
                                        ),
                                        onPressed: () {},
                                      ),
                                    ),
                                    SizedBox(
                                      width: 40,
                                      height: 40,
                                      child: Stack(
                                        children: [
                                          IconButton(
                                            icon: Icon(
                                              Icons.notifications,
                                              color: dark.withOpacity(.7),
                                            ),
                                            onPressed: () {},
                                          ),
                                          Positioned(
                                            top: 7,
                                            right: 7,
                                            child: Container(
                                              width: 12,
                                              height: 12,
                                              padding: EdgeInsets.all(4),
                                              decoration: BoxDecoration(
                                                color: active,
                                                borderRadius:
                                                    BorderRadius.circular(30),
                                                border: Border.all(
                                                    color: light, width: 2),
                                              ),
                                            ),
                                          ),
                                        ],
                                      ),
                                    ),
                                    Container(
                                      width: 1,
                                      height: 30,
                                      color: lightGrey,
                                    ),
                                    if (!ResponsiveWidget.isSmallScreen(
                                        context))
                                      Padding(
                                        padding: const EdgeInsets.only(
                                            right: 4, left: 4),
                                        child: Text(
                                          "Admin",
                                          style: GoogleFonts.roboto(
                                            color: lightGrey,
                                            fontSize: 20,
                                            fontWeight: FontWeight.bold,
                                          ),
                                        ),
                                      )
                                    else
                                      const SizedBox(
                                        width: 1,
                                      ),
                                    Padding(
                                      padding: const EdgeInsets.only(
                                          left: 4, right: 4),
                                      child: Container(
                                        width: 40,
                                        height: 40,
                                        decoration: BoxDecoration(
                                          color: active.withOpacity(.5),
                                          borderRadius:
                                              BorderRadius.circular(30),
                                        ),
                                        child: Container(
                                          decoration: BoxDecoration(
                                            color: Colors.white,
                                            borderRadius:
                                                BorderRadius.circular(30),
                                          ),
                                          padding: EdgeInsets.all(2),
                                          margin: EdgeInsets.all(2),
                                          child: CircleAvatar(
                                            backgroundColor: light,
                                            child: Icon(
                                              Icons.person_outline,
                                              color: dark,
                                            ),
                                          ),
                                        ),
                                      ),
                                    ),
                                  ],
                                )),
                          ],
                        ),
                      ),
                    ))
              ],
            ),
          ),
          iconTheme: IconThemeData(color: dark),
          elevation: 0,
          backgroundColor: Colors.transparent,
        ),
      ),
    );
