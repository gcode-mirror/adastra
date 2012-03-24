// This is the main DLL file.

#include "stdafx.h"

#include "FieldTripDriverNative.h"
#include "fieldtrip_buffer\src\buffer.h"

boolean OpenViBEAcquisitionServer::FieldTripDriverNative::initialize(
		const unsigned int ui32SampleCountPerSentBlock)
{
	signed int m_i32ConnectionID = open_connection("localhost", 4564);

	return true;
}

