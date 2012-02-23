#ifndef __OpenViBE_AcquisitionServer_CConfigurationTestDriver_H__
#define __OpenViBE_AcquisitionServer_CConfigurationTestDriver_H__

#include "../ovasCConfigurationBuilder.h"
#include "../ovasIDriver.h"

//#include <gtk/gtk.h>

namespace OpenViBEAcquisitionServer
{
	/**
	 * \class CConfigurationTestDriver
	 * \author Anton (Andreev)
	 * \date Tue Feb 21 09:42:04 2012
	 * \erief The CConfigurationTestDriver handles the configuration dialog specific to the My Test Driver device.
	 *
	 * TODO: details
	 *
	 * \sa CDriverTestDriver
	 */
	class CConfigurationTestDriver : public OpenViBEAcquisitionServer::CConfigurationBuilder
	{
	public:

		// you may have to add to your constructor some reference parameters
		// for example, a connection ID:
		//CConfigurationTestDriver(OpenViBEAcquisitionServer::IDriverContext& rDriverContext, const char* sGtkBuilderFileName, OpenViBE::uint32& rConnectionId);
		CConfigurationTestDriver(OpenViBEAcquisitionServer::IDriverContext& rDriverContext, const char* sGtkBuilderFileName);

		virtual OpenViBE::boolean preConfigure(void);
		virtual OpenViBE::boolean postConfigure(void);

	protected:

		OpenViBEAcquisitionServer::IDriverContext& m_rDriverContext;

	private:

		/*
		 * Insert here all specific attributes, such as a connection ID.
		 * use references to directly modify the corresponding attribute of the driver
		 * Example:
		 */
		// OpenViBE::uint32& m_ui32ConnectionID;
	};
};

#endif // __OpenViBE_AcquisitionServer_CConfigurationTestDriver_H__
