import vrpn.*;

public class AnalogTest 
	implements vrpn.AnalogRemote.AnalogChangeListener
{
	public void analogUpdate( vrpn.AnalogRemote.AnalogUpdate u,
							  vrpn.AnalogRemote tracker )
	{
		
	    double[] channels = u.channel;
	    System.out.println(channels[10]);
	
		
	}
	

	public static void main( String[] args )
	{
		String analogName = "openvibe-vrpn@localhost";
		AnalogRemote analog = null;
		AnalogOutputRemote ao = null;
		try
		{
			analog = new AnalogRemote( analogName, null, null, null, null );
			ao = new AnalogOutputRemote( analogName, null, null, null, null );
		}
		catch( InstantiationException e )
		{
			// do something b/c you couldn't create the analog
			System.out.println( "We couldn't connect to analog " + analogName + "." );
			System.out.println( e.getMessage( ) );
			return;
		}
		
		AnalogTest test = new AnalogTest( );
		analog.addAnalogChangeListener( test );
		
		ao.requestValueChange( 2, 5 );
		
	}
}
