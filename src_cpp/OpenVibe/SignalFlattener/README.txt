1) Put in openvibe-plugins/signal-processing/trunc/src

2) Add the two files to to project OpenViBE-plugins-signal-processing-dynamic

3) Add to ovp_main.cpp in openvibe-plugins/signal-processing/trunc/src

#include "ovpCBoxAlgorithmSignalFlattener.h"

OVP_Declare_New(OpenViBEPlugins::SignalProcessing::CBoxAlgorithmSignalFlattenerDesc)


4) Set "Designer_ShowUnstable = true" in OpenVibe_src\dist\share

5) Set output folder of OpenViBE-plugins-signal-processing-dynamic to "..\..\..\dist\bin"



Box shouls appear in "Signal processing/Filtering"

******************************************************
* Thanks for using the OpenViBE Skeleton Generator ! *
******************************************************


File generation completed
[Wed Feb 15 08:19:08 2012]
-------------------------

The generator produced the following files:

The Box class:
- ovpCBoxAlgorithmSignalFlattener.h
- ovpCBoxAlgorithmSignalFlattener.cpp
Please put these files in your local repository, in the project of your choice (e.g. Signal Processing or Classification)
     [openvibe-repository]/trunk/openvibe-plugins/[plugin-project]/trunc/src/[my-box-folder]
 
You may have to change the file ovpCBoxAlgorithmSignalFlattener.h to make it find the included file ovp_defines.h, whose path is related to the project used.
 
Don't forget to declare your box in ovp_main.cpp, in order to make it available in the Designer.
Look in one of the ovp_main.cpp file, you will find examples of such declarations (#include the header, then OVP_Declare_New macro).

If your box doesn't appear in the designer, maybe it's because you cannot see Unstable Boxes.
Try to set your configuration file (openvibe.conf on windows / .openviberc on Linux) with the following token :
>>>>>
Designer_ShowUnstable = true
>>>>>

For more information about implementing algorithms and boxes to fill your skeleton, please read the official tutorial:
http://openvibe.inria.fr/tutorial-1-implementing-a-signal-processing-box/

Feel free to propose your contribution on the forum ! 
http://openvibe.inria.fr/forum/

Enjoy OpenViBE !

- The development team -
