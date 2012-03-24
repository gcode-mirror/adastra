I have successfully compiled VRPN itself and the Java wrapper. One needs a bunch of tools for each step.

Everything you need is here: https://adastra.googlecode.com/svn/trunk/src_java

Our goal will be to read 11 channel EEG signal, process it in OpenVibe and forward it to Java.

OpenVibe:
Start the designer and start the provided OpenVibe scenario in the source above.

Eclipse:
Open the Eclipse project file ".project" in test.
Go to "configure build path". Remove everything that is wrong.
Add the provided by me vrpn.jar. Then you need to expand vrpn.jar in libraries tab and set "native library location" to the provided by me java_vrpn.dll.

Then everything will compile. Your main class is "AnalogTest". This class is set to connect to "openvibe-vrpn@localhost" and it will print channel 10 (last channel of 11 channel EEG signal).

First start OpenVibe and then execute your Java application.