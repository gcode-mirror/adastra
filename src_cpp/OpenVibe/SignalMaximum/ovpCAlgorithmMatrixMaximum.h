#ifndef __OpenViBEPlugins_Algorithm_MatrixMaximum_H__
#define __OpenViBEPlugins_Algorithm_MatrixMaximum_H__
 
#include "ovp_defines.h"
#include <openvibe/ov_all.h>
#include <openvibe-toolkit/ovtk_all.h>
 
namespace OpenViBEPlugins
{
	namespace SignalProcessing
	{
                class CAlgorithmMatrixMaximum : public OpenViBEToolkit::TAlgorithm < OpenViBE::Plugins::IAlgorithm >
                {
                public:
 
                        virtual void release(void) { delete this; }
 
                        virtual OpenViBE::boolean initialize(void);
                        virtual OpenViBE::boolean uninitialize(void);
                        virtual OpenViBE::boolean process(void);
 
                        _IsDerivedFromClass_Final_(OpenViBEToolkit::TAlgorithm < OpenViBE::Plugins::IAlgorithm >, OVP_ClassId_Algorithm_MatrixMaximum);
 
                protected:
 
                        OpenViBE::Kernel::TParameterHandler < OpenViBE::IMatrix* > ip_pMatrix; // input matrix
                        OpenViBE::Kernel::TParameterHandler < OpenViBE::IMatrix* > op_pMatrix; // output matrix
                };
 
                class CAlgorithmMatrixMaximumDesc : public OpenViBE::Plugins::IAlgorithmDesc
                {
                public:
 
                        virtual void release(void) { }
 
                        virtual OpenViBE::CString getName(void) const                { return OpenViBE::CString("Signal maximum algorithm"); }
                        virtual OpenViBE::CString getAuthorName(void) const          { return OpenViBE::CString("Anton Andreev"); }
                        virtual OpenViBE::CString getAuthorCompanyName(void) const   { return OpenViBE::CString("GIPSA-lab"); }
                        virtual OpenViBE::CString getShortDescription(void) const    { return OpenViBE::CString("Computes the maximum vector of a 2-dimension matrix"); }
                        virtual OpenViBE::CString getDetailedDescription(void) const { return OpenViBE::CString(""); }
                        virtual OpenViBE::CString getCategory(void) const            { return OpenViBE::CString("Samples"); }
                        virtual OpenViBE::CString getVersion(void) const             { return OpenViBE::CString("1.0"); }
                        virtual OpenViBE::CString getStockItemName(void) const       { return OpenViBE::CString("gtk-execute"); }
 
                        virtual OpenViBE::CIdentifier getCreatedClass(void) const    { return OVP_ClassId_Algorithm_MatrixMaximum; }
                        virtual OpenViBE::Plugins::IPluginObject* create(void)       { return new OpenViBEPlugins::SignalProcessing::CAlgorithmMatrixMaximum; }
 
                        virtual OpenViBE::boolean getAlgorithmPrototype(
                                OpenViBE::Kernel::IAlgorithmProto& rAlgorithmPrototype) const
                        {
                                rAlgorithmPrototype.addInputParameter (OVP_Algorithm_MatrixMaximum_InputParameterId_Matrix,     "Matrix", OpenViBE::Kernel::ParameterType_Matrix);
                                rAlgorithmPrototype.addOutputParameter(OVP_Algorithm_MatrixMaximum_OutputParameterId_Matrix,    "Matrix", OpenViBE::Kernel::ParameterType_Matrix);
 
                                rAlgorithmPrototype.addInputTrigger   (OVP_Algorithm_MatrixMaximum_InputTriggerId_Initialize,   "Initialize");
                                rAlgorithmPrototype.addInputTrigger   (OVP_Algorithm_MatrixMaximum_InputTriggerId_Process,      "Process");
                                rAlgorithmPrototype.addOutputTrigger  (OVP_Algorithm_MatrixMaximum_OutputTriggerId_ProcessDone, "Process done");
 
                                return true;
                        }
 
                        _IsDerivedFromClass_Final_(OpenViBE::Plugins::IAlgorithmDesc, OVP_ClassId_Algorithm_MatrixMaximumDesc);
                };
        };
};
 
#endif // __OpenViBEPlugins_Algorithm_MatrixMaximum_H__