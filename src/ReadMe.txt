Homepage: http://code.google.com/p/adastra

Description:

Adastra is a BCI (Brain Computer Interface) application. It works in combination with another BCI application called OpenVibe. Adastra can use real-time output from OpenVibe. Adastra is written in C#. There are different scenarios supported in Adastra. For example OpenVibe is used to acquire, filter the EEG signal and generate feature vectors from it. Then these feature vectors are forwarded to Adastra's machine learning (ML) algorithms. ML is used to train Adastra to detect actions such as left/right and up/down mouse cursor movement.

Several machine learning algorithms are provided:

LDA+MLP: Linear Discriminant Analysis + Multi - Layer Perceptron
LDA+SVM: Linear Discriminant Analysis + Support Vector Machines 

Installation:

1. Install .NET 4.0 framework
2. Install OpenVibe from: http://openvibe.inria.fr
3. Start adastra.exe
4. Set OpenVibe's install folder.
5. Select application scenario and click "Start".

License: http://www.gnu.org/licenses/gpl-2.0.html
All included libraries may come with licenses different than Adastra's license.