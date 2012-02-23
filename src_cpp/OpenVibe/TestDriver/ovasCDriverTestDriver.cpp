#include "ovasCDriverTestDriver.h"
//#include "ovasCConfigurationTestDriver.h"

#include <openvibe-toolkit/ovtk_all.h>

#include <assert.h>

using namespace OpenViBEAcquisitionServer;
using namespace OpenViBE;
using namespace OpenViBE::Kernel;
using namespace std;

//___________________________________________________________________//
//                                                                   //

CDriverTestDriver::CDriverTestDriver(IDriverContext& rDriverContext)
	:IDriver(rDriverContext)
	,m_pCallback(NULL)
	,m_ui32SampleCountPerSentBlock(0)
	,m_pSample(NULL)
{
	m_oHeader.setSamplingFrequency(128);
	m_oHeader.setChannelCount(2);
}

CDriverTestDriver::~CDriverTestDriver(void)
{
}

const char* CDriverTestDriver::getName(void)
{
	return "My Test Driver";
}

//___________________________________________________________________//
//                                                                   //

boolean CDriverTestDriver::initialize(
	const uint32 ui32SampleCountPerSentBlock,
	IDriverCallback& rCallback)
{
	assert(&m_rDriverContext!=NULL);

	if(m_rDriverContext.isConnected()) return false;
	if(!m_oHeader.isChannelCountSet()||!m_oHeader.isSamplingFrequencySet()) return false;
	
	// Builds up a buffer to store
	// acquired samples. This buffer
	// will be sent to the acquisition
	// server later...
	m_pSample=new float32[m_oHeader.getChannelCount()*ui32SampleCountPerSentBlock];
	if(!m_pSample)
	{
		delete [] m_pSample;
		m_pSample=NULL;
		return false;
	}
	
	// ...
	// initialize hardware and get
	// available header information
	// from it
	// Using for example the connection ID provided by the configuration (m_ui32ConnectionID)
	// ...

	// Saves parameters
	m_pCallback=&rCallback;
	m_ui32SampleCountPerSentBlock=ui32SampleCountPerSentBlock;
	return true;
}

boolean CDriverTestDriver::start(void)
{
	if(!m_rDriverContext.isConnected()) return false;
	if(m_rDriverContext.isStarted()) return false;

	// ...
	// request hardware to start
	// sending data
	// ...

	return true;
}

boolean CDriverTestDriver::loop(void)
{
	if(!m_rDriverContext.isConnected()) return false;
	if(!m_rDriverContext.isStarted()) return true;

	OpenViBE::CStimulationSet l_oStimulationSet;

	// ...
	// receive samples from hardware
	// put them the correct way in the sample array
	// whether the buffer is full, send it to the acquisition server
	//...
	m_pCallback->setSamples(m_pSample);
	
	// When your sample buffer is fully loaded, 
	// it is advised to ask the acquisition server 
	// to correct any drift in the acquisition automatically.
	m_rDriverContext.correctDriftSampleCount(m_rDriverContext.getSuggestedDriftCorrectionSampleCount());

	// ...
	// receive events from hardware
	// and put them the correct way in a CStimulationSet object
	//...
	m_pCallback->setStimulationSet(l_oStimulationSet);

	return true;
}

boolean CDriverTestDriver::stop(void)
{
	if(!m_rDriverContext.isConnected()) return false;
	if(!m_rDriverContext.isStarted()) return false;

	// ...
	// request the hardware to stop
	// sending data
	// ...

	return true;
}

boolean CDriverTestDriver::uninitialize(void)
{
	if(!m_rDriverContext.isConnected()) return false;
	if(m_rDriverContext.isStarted()) return false;

	// ...
	// uninitialize hardware here
	// ...

	delete [] m_pSample;
	m_pSample=NULL;
	m_pCallback=NULL;

	return true;
}

//___________________________________________________________________//
//                                                                   //
boolean CDriverTestDriver::isConfigurable(void)
{
	return true; // change to false if your device is not configurable
}

boolean CDriverTestDriver::configure(void)
{
	return false;
	// Change this line if you need to specify some references to your driver attribute that need configuration, e.g. the connection ID.
	/*CConfigurationTestDriver m_oConfiguration(m_rDriverContext,"D:\\Work\\OpenVibe_src\\openvibe-applications\\acquisition-server\\trunc\\src\\TestDriver\\interface-TestDriver.ui");
	if(!m_oConfiguration.configure(m_oHeader))
	{
		return false;
	}
	return true;*/
}
