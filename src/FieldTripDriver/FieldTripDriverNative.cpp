// This is the main DLL file.

#include "stdafx.h"
#include <string>
using namespace std;

#include "FieldTripDriverNative.h"
#include "fieldtrip_buffer\src\buffer.h"
#include "ov_types.h"

OpenViBEAcquisitionServer::FieldTripDriverNative::FieldTripDriverNative()
	:
    //IDriver(rDriverContext)
	//m_pCallback(NULL)
	m_ui32SampleCountPerSentBlock(0)
	,m_pSample(NULL)
    ,m_sHostName("localhost")
    ,m_ui32PortNumber(1979)
    ,m_i32ConnectionID(-1)
    ,m_ui32DataType(DATATYPE_UNKNOWN)
    ,m_bFirstGetDataRequest(false)
    ,m_myfile(NULL)
    ,m_ui32MinSamples(1)
    ,m_bCorrectNonIntegerSR(true)
{
    m_oHeader.setSamplingFrequency(0);
	m_oHeader.setChannelCount(0);

    m_pWaitData_Request = new message_t();
    m_pWaitData_Request->def = new messagedef_t();
    m_pWaitData_Request->buf = NULL;

    m_pGetData_Request = new message_t();
    m_pGetData_Request->def = new messagedef_t();
    m_pGetData_Request->buf = NULL;
}

bool OpenViBEAcquisitionServer::FieldTripDriverNative::
	initialize(const OpenViBE::uint32 ui32SampleCountPerSentBlock)
{
	// ...
	// initialize hardware and get available header information
	// from it :

    // connect to buffer
    if ( m_i32ConnectionID != -1 )
    {
        //m_rDriverContext.getLogManager() << LogLevel_Error << "Already connected to Fieldtrip buffer "
           // <<  m_sHostName.toASCIIString() << ":" << m_ui32PortNumber <<"\n";
        return false;
    }

    m_i32ConnectionID = open_connection(m_sHostName.c_str(), (int)m_ui32PortNumber);
    if ( m_i32ConnectionID < 0 )
    {
       /* m_rDriverContext.getLogManager() << LogLevel_Error << "Failed to connect to Fieldtrip buffer :\n"
            <<  m_sHostName.toASCIIString() << ":" << m_ui32PortNumber <<"\n";*/
        m_i32ConnectionID = -1;
        return false;
    }
    else
    {
        //m_rDriverContext.getLogManager() << LogLevel_Info << "Connection to Fieldtrip buffer succeeded !\n";
    }

    // request header
    if ( !requestHeader() )
    {
        //m_rDriverContext.getLogManager() << LogLevel_Error << "Request header failed, disconnecting.\n";
        if ( close_connection(m_i32ConnectionID) != 0 )
        {
            //m_rDriverContext.getLogManager() << LogLevel_Error << "Failed to disconnect correctly from Fieldtrip buffer\n";
        }
        m_i32ConnectionID = -1;
        return false;
    }


    if (!m_oHeader.isChannelCountSet()||!m_oHeader.isSamplingFrequencySet()) return false;

    // Builds up a buffer to store acquired samples. This buffer
	// will be sent to the acquisition server later...
	m_pSample = new OpenViBE::float32[m_oHeader.getChannelCount()*ui32SampleCountPerSentBlock];
	if (!m_pSample)
	{
		delete [] m_pSample;
		m_pSample = NULL;
		return false;
	}

	// Saves parameters
	//m_pCallback = &rCallback;
	m_ui32SampleCountPerSentBlock = ui32SampleCountPerSentBlock;

    if ( m_ui32MinSamples < 1 )
    {
        m_ui32MinSamples = 1;
    }
    if ( m_ui32MinSamples > m_ui32SampleCountPerSentBlock )
    {
        m_ui32MinSamples = m_ui32SampleCountPerSentBlock;
    }

	return true;
}

OpenViBEAcquisitionServer::FieldTripDriverNative::~FieldTripDriverNative(void)
{
    if ( m_pWaitData_Request )
    {
        //m_pWaitData_Request->buf deleted with m_pWaitData_Request->def
        if ( m_pWaitData_Request->def ) delete m_pWaitData_Request->def;
        delete m_pWaitData_Request;
    }

    if ( m_pGetData_Request )
    {
        //m_pGetData_Request->buf deleted with m_pGetData_Request->def
        if ( m_pGetData_Request->def ) delete m_pGetData_Request->def;
        delete m_pGetData_Request;
    }
}

bool OpenViBEAcquisitionServer::FieldTripDriverNative::requestHeader()
{
	return false;
}

