#include "StdAfx.h"

#include "FieldTripDriverNative.h"

#include "FunctionCallback.h"

using namespace System;
using namespace System::Collections;
using namespace OpenViBEAcquisitionServer;
using namespace System::Runtime::InteropServices;

public ref class FieldTripChangeEventArgs: public System::EventArgs
	{
	public:
		property System::DateTime Time;
		property cli::array<double> ^Channels;
	};

public delegate void CallbackFuncDelegate(float* samples,int samplesCount);

public delegate void FieldTripEventHandler(System::Object ^sender,
		FieldTripChangeEventArgs ^e);

public ref class FieldTripDriver {

private:

    FieldTripDriverNative* pUnmanaged;

	void Context(float* samples,int samplesCount)
	{
		//throw event 
	}

	CallbackFuncDelegate^ del;


public:

    FieldTripDriver() { 
		
		pUnmanaged = new FieldTripDriverNative();

		del = gcnew CallbackFuncDelegate(this, &FieldTripDriver::Context);    
	}
    
	~FieldTripDriver() { 
		pUnmanaged->uninitialize();
		delete pUnmanaged; pUnmanaged = 0; 
	}
    /*!FieldTripDriver() { ?????????
		pUnmanaged->uninitialize();
		delete pUnmanaged; 
	}*/

	event FieldTripEventHandler^ AnalogChanged;

    void initialize() { 

		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		pUnmanaged->configure();
		bool b=pUnmanaged->initialize(45);//how many samples??
		
    }

	void start()
	{
		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		bool s = pUnmanaged->start();

		if (s)
		for (int i=0;i<10;i++)
		{
		   float* result; 
		   int sampleCount;

		   //CallbackType t = &FieldTripDriver::Context;

		   //var del = &FieldTripDriver::Context;

		   pUnmanaged->loop((CallbackType)Marshal::GetFunctionPointerForDelegate(del).ToPointer());

		  /* ArrayList list=gcnew ArrayList();

		   for(int i=0;i<90;i++)
		   {
			   double[] d=gcnew double();
			   list.Add(result[i]);
		   }*/
		}
	}
};


