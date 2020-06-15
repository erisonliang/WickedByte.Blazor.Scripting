( function () {

   "use strict";

   window.WickedByte = window.WickedByte || {};

   function loadjscssfile( filename, filetype ) {
      console.log( "ScriptLoader: file " + filename );
      if( filetype == "js" ) { //if filename is a external JavaScript file
         var fileref = document.createElement( 'script' );
         fileref.setAttribute( "type", "text/javascript" );
         fileref.setAttribute( "src", filename );
      }
      else if( filetype == "css" ) { //if filename is an external CSS file
         var fileref = document.createElement( "link" );
         fileref.setAttribute( "rel", "stylesheet" );
         fileref.setAttribute( "type", "text/css" );
         fileref.setAttribute( "href", filename );
      }

      if( typeof fileref != "undefined" && filetype === "css" )
         document.getElementsByTagName( "head" )[0].appendChild( fileref );
      else document.getElementsByTagName( "body" )[0].appendChild( fileref );
   }

   function loadJavascript( fileName )
   {
      try {
         loadjscssfile( fileName, "js" );
      }
      catch( e ) {
         console.error( e.message );
		}
   }

   function loadCss( fileName ) {
      try {
         loadjscssfile( fileName, "css" );
      }
      catch( e ) {
         console.error( e.message );
      }
	}

   WickedByte.ScriptLoader = WickedByte.ScriptLoader || {
      loadScript: loadJavascript,
      loadCss: loadCss
   };

} )();