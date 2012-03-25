#include "StdAfx.h"

#include "FieldTripDriverNative.h"

using namespace System;
using namespace OpenViBEAcquisitionServer;

public ref class FieldTripDriver {
private:

    FieldTripDriverNative* pUnmanaged;

public:

    FieldTripDriver() { pUnmanaged = new FieldTripDriverNative(); }
    
	~FieldTripDriver() { 
		pUnmanaged->uninitialize();
		delete pUnmanaged; pUnmanaged = 0; 
	}
    /*!FieldTripDriver() { 
		pUnmanaged->uninitialize();
		delete pUnmanaged; 
	}*/

    void initialize() { 

		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		bool b=pUnmanaged->initialize(45);//how many samples??
		//pUnmanaged->configure();
    }

	void start()
	{
		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		bool s = pUnmanaged->start();

		if (s)
		for (int i=0;i<10;i++)
		{
		   pUnmanaged->loop();
		}
	}

	/*void loop()
	{
		for (int i=0;i<10;i++)
		{
		   pUnmanaged->loop();
		}
	}*/
};


