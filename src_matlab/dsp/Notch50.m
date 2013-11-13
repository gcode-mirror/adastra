fs = 2000;

t = [0:20000]/fs;

s = sin(2*pi*30*t)+ 3*sin(2*pi*50*t)+5*sin(2*pi*100*t)+2*sin(2*pi*150*t);

plot(s);

s_fft=fft(s);

f = [0:length(s)-1]/length(s)*fs;

figure, plot(f,abs(s_fft));

deg = 3;
%we mitigate the frequency around 50 by setting an interval around 50 being [45 55]
%Wn is the normalized cutoff frequency - in this case from both sides because we want to remove only the 50 Hz
Wn = [45*2/fs,55*2/fs]; %this division by 2 is because of the nyquist frequency?  

%design filter
[B,A] = butter(deg,Wn,'stop');

%apply filter
filteredSignal = filter(B,A,s);

y_fft=fft(filteredSignal);
figure, plot(f,abs(y_fft))