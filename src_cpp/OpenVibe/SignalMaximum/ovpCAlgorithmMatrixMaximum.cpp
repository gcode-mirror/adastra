#include "ovpCAlgorithmMatrixMaximum.h"

using namespace OpenViBE;
using namespace OpenViBE::Kernel;
using namespace OpenViBE::Plugins;

using namespace OpenViBEPlugins;
using namespace OpenViBEPlugins::SignalProcessing;

boolean CAlgorithmMatrixMaximum::initialize(void)
{
	//bind input matrix parameters to actual matrix objects(below) that we are going to use
	ip_pMatrix.initialize(this->getInputParameter(OVP_Algorithm_MatrixMaximum_InputParameterId_Matrix));
	op_pMatrix.initialize(this->getOutputParameter(OVP_Algorithm_MatrixMaximum_OutputParameterId_Matrix));
	return true;
}

boolean CAlgorithmMatrixMaximum::uninitialize(void)
{
	op_pMatrix.uninitialize();
	ip_pMatrix.uninitialize();
	return true;
}

boolean CAlgorithmMatrixMaximum::process(void)
{
	//seems like triggers(flags) raised by default

	//checks the list of triggers (flags) whether OVP_Algorithm_MatrixMaximum_InputTriggerId_Initialize is raised
	if(this->isInputTriggerActive(OVP_Algorithm_MatrixMaximum_InputTriggerId_Initialize))
	{
		int dimCount = ip_pMatrix->getDimensionCount();
		if( dimCount != 2)
		{
			this->getLogManager() << LogLevel_Error << "The input matrix must have 2 dimensions";
			return false;
		}

		//set output matrix
		op_pMatrix->setDimensionCount(2);
		op_pMatrix->setDimensionSize(0,1); // only one - maximum - vector
		op_pMatrix->setDimensionSize(1,ip_pMatrix->getDimensionSize(1)); // same number of elements in the vector as in the input matrix
	}

	if(this->isInputTriggerActive(OVP_Algorithm_MatrixMaximum_InputTriggerId_Process))
	{
		// we iterate over the columns (second dimension)
		for(uint32 i=0; i<ip_pMatrix->getDimensionSize(1); i++)
		{
			float64 l_f64Maximum = ip_pMatrix->getBuffer()[i];
			// and try to find the maximum among the values on column i
			for(uint32 j=1; j<ip_pMatrix->getDimensionSize(0); j++)
			{
				if(l_f64Maximum < ip_pMatrix->getBuffer()[i+j*ip_pMatrix->getDimensionSize(1)])
				{
					l_f64Maximum = ip_pMatrix->getBuffer()[i+j*ip_pMatrix->getDimensionSize(1)];
				}
			}

			//we intialized this output matrix already with 1 dimensional vector
			op_pMatrix->getBuffer()[i]= l_f64Maximum;
		}

		//raise trigger(flag) "Process Done"
		this->activateOutputTrigger(OVP_Algorithm_MatrixMaximum_OutputTriggerId_ProcessDone, true);
	}

	return true;
}