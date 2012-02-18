#ifndef __OpenViBEPlugins_BoxAlgorithm_SignalMaximum_H__
#define __OpenViBEPlugins_BoxAlgorithm_SignalMaximum_H__
 
#include "ovp_defines.h"
 
#include <openvibe/ov_all.h>
#include <openvibe-toolkit/ovtk_all.h>
 
#define OVP_ClassId_BoxAlgorithm_SignalMaximum OpenViBE::CIdentifier(0x6A12346A, 0x6758940)
#define OVP_ClassId_BoxAlgorithm_SignalMaximumDesc OpenViBE::CIdentifier(0x6AAEAB12, 0x67124560)
 
namespace OpenViBEPlugins
{
    namespace SignalProcessing
    {
        class CBoxAlgorithmSignalMaximum : virtual public OpenViBEToolkit::TBoxAlgorithm < OpenViBE::Plugins::IBoxAlgorithm >
        {
        public:
            virtual void release(void) { delete this; }
 
            virtual OpenViBE::boolean initialize(void);
            virtual OpenViBE::boolean uninitialize(void);
 
            virtual OpenViBE::boolean processInput(OpenViBE::uint32 ui32InputIndex);
 
            virtual OpenViBE::boolean process(void);
 
            _IsDerivedFromClass_Final_(OpenViBEToolkit::TBoxAlgorithm < OpenViBE::Plugins::IBoxAlgorithm >, OVP_ClassId_BoxAlgorithm_SignalMaximum);
 
        protected:
            OpenViBEToolkit::TSignalDecoder < CBoxAlgorithmSignalMaximum > m_oSignalDecoder;
 
            OpenViBE::Kernel::IAlgorithmProxy* m_pMatrixMaximumAlgorithm;
            OpenViBE::Kernel::TParameterHandler < OpenViBE::IMatrix* > ip_pMatrixMaximumAlgorithm_Matrix;
            OpenViBE::Kernel::TParameterHandler < OpenViBE::IMatrix* > op_pMatrixMaximumAlgorithm_Matrix;
 
            OpenViBEToolkit::TSignalEncoder < CBoxAlgorithmSignalMaximum > m_oSignalEncoder;
        };
 
        class CBoxAlgorithmSignalMaximumDesc : virtual public OpenViBE::Plugins::IBoxAlgorithmDesc
        {
        public:
 
            virtual void release(void) { }
 
            virtual OpenViBE::CString getName(void) const                { return OpenViBE::CString("Signal Maximum"); }
            virtual OpenViBE::CString getAuthorName(void) const          { return OpenViBE::CString("Laurent Bonnet"); }
            virtual OpenViBE::CString getAuthorCompanyName(void) const   { return OpenViBE::CString("INRIA"); }
            virtual OpenViBE::CString getShortDescription(void) const    { return OpenViBE::CString("Max channel !"); }
            virtual OpenViBE::CString getDetailedDescription(void) const { return OpenViBE::CString("Output a one channel signal corresponding to the maximum of each input channel for each sample index."); }
            virtual OpenViBE::CString getCategory(void) const            { return OpenViBE::CString("Signal processing/Filtering"); }
            virtual OpenViBE::CString getVersion(void) const             { return OpenViBE::CString("1.0"); }
            virtual OpenViBE::CString getStockItemName(void) const       { return OpenViBE::CString("gtk-add"); }
 
            virtual OpenViBE::CIdentifier getCreatedClass(void) const    { return OVP_ClassId_BoxAlgorithm_SignalMaximum; }
            virtual OpenViBE::Plugins::IPluginObject* create(void)       { return new OpenViBEPlugins::SignalProcessing::CBoxAlgorithmSignalMaximum; }
 
            virtual OpenViBE::boolean getBoxPrototype(
                OpenViBE::Kernel::IBoxProto& rBoxAlgorithmPrototype) const
            {
                rBoxAlgorithmPrototype.addInput("Signal",OV_TypeId_Signal);
 
                rBoxAlgorithmPrototype.addOutput("Processed Signal",OV_TypeId_Signal);
 
                rBoxAlgorithmPrototype.addFlag(OpenViBE::Kernel::BoxFlag_IsUnstable);
 
                return true;
            }
            _IsDerivedFromClass_Final_(OpenViBE::Plugins::IBoxAlgorithmDesc, OVP_ClassId_BoxAlgorithm_SignalMaximumDesc);
        };
    };
};
 
#endif // __OpenViBEPlugins_BoxAlgorithm_SignalMaximum_H__