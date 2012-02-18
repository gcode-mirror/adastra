#include "ovpCBoxAlgorithmSignalMaximum.h"
 
using namespace OpenViBE;
using namespace OpenViBE::Kernel;
using namespace OpenViBE::Plugins;
 
using namespace OpenViBEPlugins;
using namespace OpenViBEPlugins::SignalProcessing;
 
boolean CBoxAlgorithmSignalMaximum::initialize(void)
{
    m_oSignalDecoder.initialize(*this);

    m_pMatrixMaximumAlgorithm = NULL;
	m_pMatrixMaximumAlgorithm=&this->getAlgorithmManager().getAlgorithm(this->getAlgorithmManager().createAlgorithm(OVP_ClassId_Algorithm_MatrixMaximum));
	
	m_pMatrixMaximumAlgorithm->initialize();
    
	//bind concrete matrix to input and output parameters
	ip_pMatrixMaximumAlgorithm_Matrix.initialize(m_pMatrixMaximumAlgorithm->getInputParameter(OVP_Algorithm_MatrixMaximum_InputParameterId_Matrix));
    op_pMatrixMaximumAlgorithm_Matrix.initialize(m_pMatrixMaximumAlgorithm->getOutputParameter(OVP_Algorithm_MatrixMaximum_OutputParameterId_Matrix));
    //now we can use  ip_pMatrixMaximumAlgorithm_Matrix and op_pMatrixMaximumAlgorithm_Matrix
	//below we set the output from op_pMatrixMaximumAlgorithm_Matrix to the box final encoder
	//also the decoder sends its output to the algorithm through ip_pMatrixMaximumAlgorithm_Matrix

    m_oSignalEncoder.initialize(*this);
 
    // we connect the algorithms
    // the Signal Maximum algorithm will take the matrix coming from the signal decoder:
    ip_pMatrixMaximumAlgorithm_Matrix.setReferenceTarget(m_oSignalDecoder.getOutputMatrix());

    // The Signal Encoder will take the sampling rate from the Signal Decoder:
    m_oSignalEncoder.getInputSamplingRate().setReferenceTarget(m_oSignalDecoder.getOutputSamplingRate());

    // And the matrix from the Signal Maximum algorithm:
    m_oSignalEncoder.getInputMatrix().setReferenceTarget(op_pMatrixMaximumAlgorithm_Matrix);
 
    return true;
}
/*******************************************************************************/
 
boolean CBoxAlgorithmSignalMaximum::uninitialize(void)
{
    m_oSignalDecoder.uninitialize();
 
    op_pMatrixMaximumAlgorithm_Matrix.uninitialize();
    ip_pMatrixMaximumAlgorithm_Matrix.uninitialize();

	//if(m_pMatrixMaximumAlgorithm!=NULL){
         m_pMatrixMaximumAlgorithm->uninitialize();
         this->getAlgorithmManager().releaseAlgorithm(*m_pMatrixMaximumAlgorithm);
	//}
 
    m_oSignalEncoder.uninitialize();
 
    return true;
}
/*******************************************************************************/
 
boolean CBoxAlgorithmSignalMaximum::processInput(uint32 ui32InputIndex)
{
    getBoxAlgorithmContext()->markAlgorithmAsReadyToProcess();
 
    return true;
}
/*******************************************************************************/
 
boolean CBoxAlgorithmSignalMaximum::process(void)
{
 
    IBoxIO& l_rDynamicBoxContext=this->getDynamicBoxContext();
 
    //we decode the input signal chunks - we have only one input at index 0
    for(uint32 i=0; i<l_rDynamicBoxContext.getInputChunkCount(0); i++)
    {
        m_oSignalDecoder.decode(0,i);//get some data

        if(m_oSignalDecoder.isHeaderReceived())
        {
            ip_pMatrixMaximumAlgorithm_Matrix->setDimensionCount(2);//Anton: originaly 3
            if(!m_pMatrixMaximumAlgorithm->process(OVP_Algorithm_MatrixMaximum_InputTriggerId_Initialize)) return false;
 
			//forward
            m_oSignalEncoder.encodeHeader(0);
            l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i), l_rDynamicBoxContext.getInputChunkEndTime(0, i));
        }

		//when it is received then it is supplied to the m_pMatrixMaximumAlgorithm
        if(m_oSignalDecoder.isBufferReceived())
        {
            // we process the signal matrix with our algorithm
            m_pMatrixMaximumAlgorithm->process(OVP_Algorithm_MatrixMaximum_InputTriggerId_Process);
 
            // If the process is done successfully, we can encode the buffer
            if(m_pMatrixMaximumAlgorithm->isOutputTriggerActive(OVP_Algorithm_MatrixMaximum_OutputTriggerId_ProcessDone))
            {
				//forward
                m_oSignalEncoder.encodeBuffer(0);
                l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i), l_rDynamicBoxContext.getInputChunkEndTime(0, i));
            }
        }

		//forward 
        if(m_oSignalDecoder.isEndReceived())
        {
            m_oSignalEncoder.encodeEnd(0);
            l_rDynamicBoxContext.markOutputAsReadyToSend(0, l_rDynamicBoxContext.getInputChunkStartTime(0, i), l_rDynamicBoxContext.getInputChunkEndTime(0, i));
        }
    }
 
    return true;
}