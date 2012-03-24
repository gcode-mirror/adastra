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

OpenViBE::boolean OpenViBEAcquisitionServer::FieldTripDriverNative::start(void)
{
	//if (!m_rDriverContext.isConnected()) return false;
	//if (m_rDriverContext.isStarted()) return false;

	// ...
	// request hardware to start
	// sending data
	// ...

    m_bFirstGetDataRequest = true;
    m_ui32WaitingTimeMs = (m_oHeader.getSamplingFrequency()>1000 ? 1:(1000/m_oHeader.getSamplingFrequency()));  //time for 1 sample if >= 1ms //(1000*m_ui32SampleCountPerSentBlock)
    m_ui32TotalSampleCount=0;

    m_f64DiffPerSample = (m_f64RealSamplingRate - m_oHeader.getSamplingFrequency()) / m_f64RealSamplingRate;
    if ( m_f64DiffPerSample <= 0.0 )
        m_f64DiffPerSample = 0.0;
    m_f64DriftSinceLastCorrection = 0.0;

	return true;
}

OpenViBE::boolean OpenViBEAcquisitionServer::FieldTripDriverNative::loop(void)
{
	//if (!m_rDriverContext.isConnected()) return false;
	//if (!m_rDriverContext.isStarted()) return true;

	//OpenViBE::CStimulationSet l_oStimulationSet;
 //   l_oStimulationSet.setStimulationCount(0);

	//// ...
	//// receive samples from hardware
	//// put them the correct way in the sample array
	//// whether the buffer is full, send it to the acquisition server
	////...
 //   int32 l_iSampleCount = requestChunk(l_oStimulationSet);
 //   if ( l_iSampleCount < 0 )
 //   {
 //       return false;
 //   }
 //   else if ( l_iSampleCount == 0 )
 //   {
 //       return true;
 //   }
	//m_pCallback->setSamples(m_pSample, l_iSampleCount);

 //   if (m_bGetCpuTime)
 //   {
 //       m_ui32mesureNumber++;
 //       float64 clocktime1 = GetCPUTimeInMilliseconds();
 //       for (uint32 i = 0; i < l_iSampleCount; i++)
 //       {
 //           if (m_pSample[m_ui32DetectionChannel*l_iSampleCount + i]!=FLT_MAX)
 //           {
 //               if (!m_bWasDetected)
 //               {
 //                   if ( ( m_bDetectionHigher  && m_pSample[m_ui32DetectionChannel*l_iSampleCount + i] >= m_f64DetectionThreshold )
 //                     ||( !m_bDetectionHigher && m_pSample[m_ui32DetectionChannel*l_iSampleCount + i] <= m_f64DetectionThreshold ))
 //                   {
 //                       float64 clocktime = GetCPUTimeInMilliseconds();
 //                       fprintf(m_myfile, "%.6f \n", clocktime);
 //                       m_bWasDetected = true;
 //                   }
 //               }
 //               else
 //               {
 //                   if ( ( m_bDetectionHigher  && m_pSample[m_ui32DetectionChannel*l_iSampleCount + i] < m_f64DetectionThreshold )
 //                     ||( !m_bDetectionHigher && m_pSample[m_ui32DetectionChannel*l_iSampleCount + i] > m_f64DetectionThreshold ))
 //                   {
 //                       m_bWasDetected = false;
 //                   }
 //               }
 //           }
 //       }//end for

 //       float64 clocktime2 = GetCPUTimeInMilliseconds();
 //       m_f64mesureLostTime += (clocktime2 - clocktime1);
 //   } // end get cpu time

	//m_pCallback->setStimulationSet(l_oStimulationSet);

 //   m_rDriverContext.correctDriftSampleCount(m_rDriverContext.getSuggestedDriftCorrectionSampleCount());

    return true;
}

OpenViBE::boolean OpenViBEAcquisitionServer::FieldTripDriverNative::stop(void)
{
	//if (!m_rDriverContext.isConnected()) return false;
	//if (!m_rDriverContext.isStarted()) return false;

	return true;
}

OpenViBE::boolean OpenViBEAcquisitionServer::FieldTripDriverNative::uninitialize(void)
{
	//if (!m_rDriverContext.isConnected()) return false;
	//if (m_rDriverContext.isStarted()) return false;

    if ( close_connection(m_i32ConnectionID) != 0 )
    {
        //m_rDriverContext.getLogManager() << LogLevel_Error << "Failed to disconnect correctly from Fieldtrip buffer\n";
    }
    m_i32ConnectionID = -1;

	delete [] m_pSample;
	m_pSample=NULL;
	//m_pCallback=NULL;
    
    if (m_myfile)
    {
        /*if (m_bGetCpuTime)
        {
            fprintf(m_myfile, "*** mesure lost time : %.6f \n", m_f64mesureLostTime);
            fprintf(m_myfile, "*** mesure number : %.6f \n", (float64) m_ui32mesureNumber);
            fprintf(m_myfile, "*** average : %.6f ms per mesure\n", m_f64mesureLostTime/m_ui32mesureNumber);
        } */
        fclose(m_myfile);
        m_myfile = NULL;
    }

	return true;
}

OpenViBE::boolean OpenViBEAcquisitionServer::FieldTripDriverNative::configure(void)
{
	/*CConfigurationFieldtrip l_oConfiguration("../share/openvibe-applications/acquisition-server/interface-Fieldtrip.ui");
    l_oConfiguration.setMinSamples(m_ui32MinSamples);
    l_oConfiguration.setHostPort(m_ui32PortNumber);
    l_oConfiguration.setHostName(m_sHostName);
    l_oConfiguration.setSRCorrection(m_bCorrectNonIntegerSR);

	if (l_oConfiguration.configure(m_oHeader))
	{
        m_ui32MinSamples = l_oConfiguration.getMinSamples();
        m_ui32PortNumber = l_oConfiguration.getHostPort();
        m_sHostName = l_oConfiguration.getHostName();
        m_bCorrectNonIntegerSR = l_oConfiguration.getSRCorrection();
        return true;
	}
	return false;*/
	return true;
}

bool OpenViBEAcquisitionServer::FieldTripDriverNative::requestHeader()
{
	return false;
}

