import 'package:beautify_app/helper/responsivesLayout.dart';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

// ignore: non_constant_identifier_names
AppBar TopBarNavigation(BuildContext context, GlobalKey<ScaffoldState> key) =>
    AppBar(
        elevation: 0,
        backgroundColor: Colors.white,
        leading: !ResponsiveWidget.isSmallScreen(context)
            ? Row(
              children: [
                Center(
                  child: Container(
                    padding:const EdgeInsets.only(left:24),
                    child: Image.asset('assets/images/Lucky_beauty.jpg',width: 24,height: 24,),
                  ),
                ),
                Padding(padding: const EdgeInsets.only(left:5),child: Text("Lucky Beauty",style: GoogleFonts.roboto(fontSize: 16 ,color:const Color(0xFF1F0D1A)),),)
              ],
            )
            : IconButton(
                onPressed: () {
                  key.currentState!.openDrawer();
                },
                icon: const Icon(Icons.menu,color:Color.fromARGB(99, 0, 0, 0))),
                
            actions: [
              
            ],);
