#include "ovpCBoxAlgorithmSignalFlattener.h"
#include "UnmanagedSimpleAverageFilter.h"

using namespace OpenViBE;
using namespace OpenViBE::Kernel;
using namespace OpenViBE::Plugins;

using namespace OpenViBEPlugins;
using namespace OpenViBEPlugins::SignalProcessing;

boolean CBoxAlgorithmSignalFlattener::initialize(void)
{
	// Signal stream decoder
	m_oAlgo0_SignalDecoder.initialize(*this);
	// Stimulation stream encoder
	m_oAlgo1_StimulationDecoder.initialize(*this);
	// Signal stream encoder
	m_oAlgo2_SignalEncoder.initialize(*this);

	m_oAlgo2_SignalEncoder.getInputMatrix().setReferenceTarget(m_oAlgo0_SignalDecoder.getOutputMatrix());
	m_oAlgo2_SignalEncoder.getInputSamplingRate().setReferenceTarget(m_oAlgo0_SignalDecoder.getOutputSamplingRate());

	// the "Level" setting is at index 0, we can auto cast it from CString to float64
	m_f64LevelValue = FSettingValueAutoCast(*this->getBoxAlgorithmContext(), 0);
	// the "Trigger" setting is at index 1, we can auto cast it from CString to uint64
	m_ui64Trigger = FSettingValueAutoCast(*this->getBoxAlgorithmContext(), 1);

	m_bFlatSignalRequested = false;

	m_window_elements = 0;

	return true;
}
/*******************************************************************************/

boolean CBoxAlgorithmSignalFlattener::uninitialize(void)
{
	m_oAlgo0_SignalDecoder.uninitialize();
	m_oAlgo1_StimulationDecoder.uninitialize();
	m_oAlgo2_SignalEncoder.uninitialize();

	return true;
}


boolean CBoxAlgorithmSignalFlattener::processInput(uint32 ui32InputIndex)
{
	getBoxAlgorithmContext()->markAlgorithmAsReadyToProcess();

	return true;
}
/*******************************************************************************/

boolean CBoxAlgorithmSignalFlattener::process(void)
{

	// the static box context describes the box inputs, outputs, settings structures
	//IBox& l_rStaticBoxContext=this->getStaticBoxContext();
	// the dynamic box context describes the current state of the box inputs and outputs (i.e. the chunks)
	IBoxIO& l_rDynamicBoxContext=this->getDynamicBoxContext();

	//we first process input 1 = simulation input because this one controls the behaviour of the first one 0 
	//1 is the second input of two declared (signal, simulation)
	//i is each chunk in the let's call it "simulation channel" 1
	for(uint32 i=0; i<l_rDynamicBoxContext.getInputChunkCount(1); i++)
	{
		//we decode the ith chunk on input 1 
        m_oAlgo1_StimulationDecoder.decode(1,i);//simulation input is decoded with simulation decoder

		if(m_oAlgo1_StimulationDecoder.isBufferReceived())
		{
			// A buffer has been received, lets' check the stimulations inside
			IStimulationSet* l_pStimulations = m_oAlgo1_StimulationDecoder.getOutputStimulationSet();

			//now we get information for the simulation itself from "l_pStimulations"
			//getStimulationCount() gives as how many simulations we are about to process
			for(uint32 j=0; j<l_pStimulations->getStimulationCount(); j++)
			{
				uint64 l_ui64StimulationCode = l_pStimulations->getStimulationIdentifier(j);
				uint64 l_ui64StimulationDate = l_pStimulations->getStimulationDate(j);

				CString l_sStimulationName   = this->getTypeManager().getEnumerationEntryNameFromValue(OV_TypeId_Stimulation, l_ui64StimulationCode);

				//If the trigger is received, we switch the mode
				if(l_pStimulations->getStimulationIdentifier(j) == m_ui64Trigger)//l_ui64StimulationCode == m_ui64Trigger
				{
					m_bFlatSignalRequested = !m_bFlatSignalRequested;
					this->getLogManager() << LogLevel_Info << "Flat mode is now ["
						<< (m_bFlatSignalRequested ? "ENABLED" : "DISABLED")
						<< "]\n";
				}

				else
				{
					this->getLogManager() << LogLevel_Warning << "Received an unrecognized trigger, with code ["
						<< l_ui64StimulationCode
						<< "], name ["
						<< l_sStimulationName
						<< "] and date ["
						<< time64(l_ui64StimulationDate)
						<< "]\n";
				}
			}
		}
	}

	//now process input 0 = signal 
	//0 is the first input of two declared (signal, simulation)
	//i is each chunk in the let's call it "signal channel" 0
	for(uint32 i=0; i<l_rDynamicBoxContext.getInputChunkCount(0); i++)
	{
		// decode the chunk i on input 0
		m_oAlgo0_SignalDecoder.decode(0,i); //signal input 0 is decoded with signal decoder

		//1. Is Header - simply forward it
		if(m_oAlgo0_SignalDecoder.isHeaderReceived())
		{
			// Pass the header to the next boxes, by encoding a header on the output 0:
			//they are already connected in initialize method, we have only one output so we index it with 0
			m_oAlgo2_SignalEncoder.encodeHeader(0); 

			// send the output chunk containing the header. The dates are the same as the input chunk:
			l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i),
				l_rDynamicBoxContext.getInputChunkEndTime(0, i));
		}

		//2. Buffer = data chunk received
		if(m_oAlgo0_SignalDecoder.isBufferReceived())
		{
			//actual data in contained in a IMatrix*
			IMatrix* l_pMatrix = m_oAlgo0_SignalDecoder.getOutputMatrix(); // the StreamedMatrix of samples.

			uint32 l_ui32ChannelCount = l_pMatrix->getDimensionSize(0);
			uint32 l_ui32SamplesPerChannel = l_pMatrix->getDimensionSize(1);
			
			//and the actual actual data is contained in this buffer
			float64* l_pBuffer = l_pMatrix->getBuffer();

			if(m_bFlatSignalRequested) //should we perform any actual work
			{

				for(uint32 j=0; j<l_pMatrix->getBufferElementCount(); j++)
				{
					l_pBuffer[j] = UnmanagedSimpleAverageFilter::calculate(l_pBuffer[j]);//replace incoming data with calculated in managed code
				}
			}

			//incoming object is changed (no deletion or new object creation has taken place)
			//so simply forward it
			m_oAlgo2_SignalEncoder.encodeBuffer(0);//encodes the changed chunk
			l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i),
				l_rDynamicBoxContext.getInputChunkEndTime(0, i));

		}

		//3. End header is received
		if(m_oAlgo0_SignalDecoder.isEndReceived())
		{
			//simply forward it 
			m_oAlgo2_SignalEncoder.encodeEnd(0);

			l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i),
				l_rDynamicBoxContext.getInputChunkEndTime(0, i));
		}
	}

	return true;
}
