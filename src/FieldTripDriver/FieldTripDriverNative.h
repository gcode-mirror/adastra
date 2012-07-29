/* This driver uses the FieldTrip buffer open source library. 
 * See http://www.ru.nl/fcdonders/fieldtrip for details.
 */

#ifndef __OpenViBE_AcquisitionServer_CDriverFieldtrip_H__
#define __OpenViBE_AcquisitionServer_CDriverFieldtrip_H__

#include "FieldTripDriverNative.h"
#include "ovasCHeader.h"
#include "ov_types.h"
#include <string>
#include "fieldtrip_buffer\src\message.h"
#include "FunctionCallback.h"

using namespace std;

namespace OpenViBEAcquisitionServer
{
	/**
	 * \class CDriverFieldtrip
	 * \author Amelie Serpollet (CEA/LETI/CLINATEC)
	 * \date Mon May 23 09:48:21 2011
	 * \brief The FieldTripDriverNative allows the acquisition of data from a Fieldtrip buffer.
	 *
	 * Converted from OpenVibe fieldtrip driver
	 *
	 */
	class FieldTripDriverNative
	{
	public:

		FieldTripDriverNative();
		FieldTripDriverNative(string hostname, int port);
		~FieldTripDriverNative(void);

		virtual OpenViBE::boolean initialize(
			const OpenViBE::uint32 ui32SampleCountPerSentBlock);
			//OpenViBEAcquisitionServer::IDriverCallback& rCallback);
		virtual OpenViBE::boolean uninitialize(void);

		virtual OpenViBE::boolean start(void);
		virtual OpenViBE::boolean stop(void);
		virtual OpenViBE::boolean loop(CallbackType);

		virtual OpenViBE::boolean isConfigurable(void);
		virtual OpenViBE::boolean configure(void);
		virtual const OpenViBEAcquisitionServer::IHeader* getHeader(void) { return &m_oHeader; }
		
		//added 3 new
		bool FoundChannelNames(){ return l_bFoundChannelNames;}

		std::string GetChannelName(const OpenViBE::uint32 index)
		{
			if (index>=0 && index<m_oHeader.getChannelCount())
			{
				return m_oHeader.getChannelName(index);
			}
			return "";
		}

		double GetSamplingFrequency()
		{
			if (m_oHeader.isSamplingFrequencySet())
			return static_cast<double>(m_oHeader.getSamplingFrequency());
			else return -1;
		}

	protected:

//		OpenViBEAcquisitionServer::IDriverCallback* m_pCallback;
		OpenViBEAcquisitionServer::CHeader m_oHeader;

		OpenViBE::uint32 m_ui32SampleCountPerSentBlock;
		OpenViBE::float32* m_pSample;

		OpenViBE::boolean requestHeader();
		OpenViBE::int32 requestChunk(/*OpenViBE::CStimulationSet& oStimulationSet*/);

		OpenViBE::uint32 m_ui32DataType;

	private:
		// Connection to Fieldtrip buffer
		string m_sHostName;
		OpenViBE::uint32  m_ui32PortNumber;
		OpenViBE::int32   m_i32ConnectionID;
		OpenViBE::uint32  m_ui32MinSamples;

		// Avoid frequent memory allocation
		message_t* m_pWaitData_Request;
		message_t* m_pGetData_Request;

		OpenViBE::uint32 m_ui32TotalSampleCount;
		OpenViBE::uint32 m_ui32WaitingTimeMs;

		OpenViBE::boolean m_bFirstGetDataRequest;

		OpenViBE::boolean m_bCorrectNonIntegerSR; // ???
		OpenViBE::float64 m_f64RealSamplingRate;
		OpenViBE::float64 m_f64DiffPerSample; // ???
		OpenViBE::float64 m_f64DriftSinceLastCorrection;

		// count time lost for "get cpu time" :
		OpenViBE::float64 m_f64mesureLostTime;
		OpenViBE::uint32 m_ui32mesureNumber;

		OpenViBE::boolean l_bFoundChannelNames;

	};
};

#endif // __OpenViBE_AcquisitionServer_CDriverFieldtrip_H__
