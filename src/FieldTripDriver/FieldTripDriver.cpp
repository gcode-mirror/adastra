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
		property cli::array<float> ^Channels;
	};

public delegate void CallbackFuncDelegate(float* samples,int samplesCount);

public delegate void FieldTripEventHandler(System::Object ^sender,
		FieldTripChangeEventArgs ^e);

public ref class FieldTripDriver {

private:

    FieldTripDriverNative* pUnmanaged;

	void Context(float* samples,int samplesCount)
	{
		//if (FieldTripChanged!=nullptr) //todo: check for null
		{
			
			
	       /* pin_ptr<double> clrChannels = &e->Channels[0];

	        ::memcpy_s(clrChannels, info.num_channel * sizeof(double), 
		    info.channel, info.num_channel * sizeof(double));*/
			
			

			//todo: optimize code
			for (int i=0;i<45;i++)
			{
				FieldTripChangeEventArgs ^e = gcnew FieldTripChangeEventArgs();
				//todo: add time
	            //e->Time = VrpnUtils::ConvertTimeval(info.msg_time);
	            e->Channels = gcnew array<float>(2);//todo: fix number of channels

                e->Channels[0] = samples[i];
				e->Channels[1] = samples[i+45];

	            FieldTripChanged(this, e);
			}
		}
	}

	CallbackFuncDelegate^ del;

	bool initialized;
	bool started;

public:

    FieldTripDriver() { 
		
		pUnmanaged = new FieldTripDriverNative();

		del = gcnew CallbackFuncDelegate(this, &FieldTripDriver::Context);    
	}
    
	~FieldTripDriver() { 
		pUnmanaged->uninitialize();
		delete pUnmanaged; pUnmanaged = 0; 
	}
	//todo: is this needed
    /*!FieldTripDriver() { ?????????
		pUnmanaged->uninitialize();
		delete pUnmanaged; 
	}*/

	event FieldTripEventHandler^ FieldTripChanged;

    void initialize() { 

		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		pUnmanaged->configure();
		initialized=pUnmanaged->initialize(45);//todo: how many samples??
		
    }

	void start()
	{
		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		
		if (initialized)
		{
		started = pUnmanaged->start();

		//if (s)
		//for (int i=0;i<10;i++)
		//{
		//   float* result; 
		//   int sampleCount;

		//   //CallbackType t = &FieldTripDriver::Context;

		//   //var del = &FieldTripDriver::Context;

		//   pUnmanaged->loop((CallbackType)Marshal::GetFunctionPointerForDelegate(del).ToPointer());

		//  /* ArrayList list=gcnew ArrayList();

		//   for(int i=0;i<90;i++)
		//   {
		//	   double[] d=gcnew double();
		//	   list.Add(result[i]);
		//   }*/
		}
	}

	void loop()
	{
		if (started)
		   pUnmanaged->loop((CallbackType)Marshal::GetFunctionPointerForDelegate(del).ToPointer());
	}
};


