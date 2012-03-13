
For project: OpenViBE-plugins-signal-processing-dynamic 

1) Put:

ovpCBoxAlgorithmSignalFlattener.cpp
ovpCBoxAlgorithmSignalFlattener.h
UnmanagedSimpleAverageFilter.h

in openvibe-plugins/signal-processing/trunc/src

2) Add the 3 files to to project OpenViBE-plugins-signal-processing-dynamic in VS or to the build

3) Add to ovp_main.cpp in openvibe-plugins/signal-processing/trunc/src

#include "ovpCBoxAlgorithmSignalFlattener.h"

OVP_Declare_New(OpenViBEPlugins::SignalProcessing::CBoxAlgorithmSignalFlattenerDesc)


4) Set "Designer_ShowUnstable = true" in OpenVibe_src\dist\share

5) Set output folder of OpenViBE-plugins-signal-processing-dynamic to "..\..\..\dist\bin"

6) In OpenViBE-plugins-signal-processing-dynamic you need to set the path to the lib file UnmanagedSimpleAverageFilter.lib that
you are generating on order to be used by OpenVibe

7) You also need to add to Linker -> Input -> Additional Dependencies "UnmanagedSimpleAverageFilter.lib"

8) Do not forget to copy the resulting dll from bin\debig\x86\UnmanagedSimpleAverageFilter.dll to OpenVibes's bin folder (at each compile).

9) You might need .net framework 2.0 installed (if not already) the way it is configured now




