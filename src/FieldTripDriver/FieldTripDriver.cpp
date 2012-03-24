#include "StdAfx.h"

#include "FieldTripDriverNative.h"

using namespace System;
using namespace OpenViBEAcquisitionServer;

public ref class FieldTripDriver {
private:

    FieldTripDriverNative* pUnmanaged;

public:

    FieldTripDriver() { pUnmanaged = new FieldTripDriverNative(); }
    ~FieldTripDriver() { delete pUnmanaged; pUnmanaged = 0; }
    !FieldTripDriver() { delete pUnmanaged; }

    void initialize() { 

		pUnmanaged->initialize(45);

/*        if (!pUnmanaged) throw gcnew ObjectDisposedException("Wrapper");
        pUnmanaged->sampleMethod();*/ 
    }
};


