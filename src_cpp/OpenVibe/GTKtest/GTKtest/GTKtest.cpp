#include <iostream>
#include <regex>
#include <windows.h>
#include <gtk/gtk.h>


using namespace std;
int main( int argc, char **argv );
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
LPSTR lpCmdLine,int nCmdShow){
return main (__argc, __argv);
}


int main( int argc, char **argv ) {

	GtkWidget *window;

	gtk_init( &argc, &argv );

	window = gtk_window_new( GTK_WINDOW_TOPLEVEL );

	g_signal_connect( G_OBJECT( window ), "destroy", G_CALLBACK( gtk_main_quit ), NULL );

	gtk_widget_show( window );

	gtk_main ();

	return 0;
}
