#ifndef __OpenViBE_AcquisitionServer_CDriverTestDriver_H__
#define __OpenViBE_AcquisitionServer_CDriverTestDriver_H__

#include "../ovasIDriver.h"
#include "../ovasCHeader.h"
#include <openvibe/ov_all.h>

namespace OpenViBEAcquisitionServer
{
	/**
	 * \class CDriverTestDriver
	 * \author Anton (Andreev)
	 * \date Tue Feb 21 09:42:04 2012
	 * \erief The CDriverTestDriver allows the acquisition server to acquire data from a My Test Driver device.
	 *
	 * TODO: details
	 *
	 * \sa CConfigurationTestDriver
	 */
	class CDriverTestDriver : public OpenViBEAcquisitionServer::IDriver
	{
	public:

		CDriverTestDriver(OpenViBEAcquisitionServer::IDriverContext& rDriverContext);
		virtual ~CDriverTestDriver(void);
		virtual const char* getName(void);

		virtual OpenViBE::boolean initialize(
			const OpenViBE::uint32 ui32SampleCountPerSentBlock,
			OpenViBEAcquisitionServer::IDriverCallback& rCallback);
		virtual OpenViBE::boolean uninitialize(void);

		virtual OpenViBE::boolean start(void);
		virtual OpenViBE::boolean stop(void);
		virtual OpenViBE::boolean loop(void);

		virtual OpenViBE::boolean isConfigurable(void);
		virtual OpenViBE::boolean configure(void);
		virtual const OpenViBEAcquisitionServer::IHeader* getHeader(void) { return &m_oHeader; }
		
		virtual OpenViBE::boolean isFlagSet(
			const OpenViBEAcquisitionServer::EDriverFlag eFlag) const
		{
			return eFlag==DriverFlag_IsUnstable;
		}

	protected:

		OpenViBEAcquisitionServer::IDriverCallback* m_pCallback;

		// Replace this generic Header with any specific header you might have written
		OpenViBEAcquisitionServer::CHeader m_oHeader;

		OpenViBE::uint32 m_ui32SampleCountPerSentBlock;
		OpenViBE::float32* m_pSample;
	
	private:

		/*
		 * Insert here all specific attributes, such as USB port number or device ID.
		 * Example :
		 */
		// OpenViBE::uint32 m_ui32ConnectionID;
	};
};

#endif // __OpenViBE_AcquisitionServer_CDriverTestDriver_H__
