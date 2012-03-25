#include "FieldTripDriverNative.h"
#include "FunctionCallback.h"
#include <vector>

using namespace System;
using namespace System::Collections;
using namespace System::Collections::Generic;
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

	int sampleCount;

	int m_ui32ChannelCount;

	array<double>^ m_vSwapBuffer;

	CallbackFuncDelegate^ del;

	bool initialized;

	bool started;

	void Context(float* samples,int samplesCount)
	{
		//if (FieldTripChanged!=nullptr) //todo: check if anyone registered
		{
			for(int i=0; i<samplesCount; i++)
		    {
				FieldTripChangeEventArgs ^e = gcnew FieldTripChangeEventArgs();
				e->Channels = gcnew array<double>(m_vSwapBuffer->Length);

				//drift correction should be performed here
				for(int j=0; j<m_ui32ChannelCount; j++)
				{
					//m_vSwapBuffer[j]=samples[j*samplesCount+i];
					e->Channels[j]=samples[j*samplesCount+i];
				}
				
				e->Time = System::DateTime::Now;
	            FieldTripChanged(this, e);
		    }
		}
	}

public:

	event FieldTripEventHandler^ FieldTripChanged;

    FieldTripDriver() 
	{ 
		pUnmanaged = new FieldTripDriverNative();

		del = gcnew CallbackFuncDelegate(this, &FieldTripDriver::Context);    

		m_ui32ChannelCount = -1;
		sampleCount=32;
	}
    
	~FieldTripDriver() 
	{ 
		pUnmanaged->uninitialize();
		delete pUnmanaged; 
		pUnmanaged = 0; 
	}

	
    !FieldTripDriver() //finalizer
	{
		pUnmanaged->uninitialize();
		delete pUnmanaged; 
	}

    void initialize() { 

		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		
		pUnmanaged->configure();
		initialized=pUnmanaged->initialize(sampleCount);

	    const IHeader& l_rHeader=*pUnmanaged->getHeader();
	    m_ui32ChannelCount=l_rHeader.getChannelCount();

		if (m_ui32ChannelCount>0)
		   m_vSwapBuffer = gcnew array<double>(m_ui32ChannelCount);
    }

	void start()
	{
		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
		
		if (initialized)
		{
		   started = pUnmanaged->start();
		}
	}

	void loop()
	{
		if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");

		if (started)
		   pUnmanaged->loop((CallbackType)Marshal::GetFunctionPointerForDelegate(del).ToPointer());
	}
};


