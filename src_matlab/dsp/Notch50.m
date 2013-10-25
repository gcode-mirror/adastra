fs = 2000;

t = [0:20000]/fs;

s = sin(2*pi*30*t)+ 3*sin(2*pi*50*t)+5*sin(2*pi*100*t)+2*sin(2*pi*150*t);

plot(s);

s_fft=fft(s);

f = [0:length(s)-1]/length(s)*fs;

figure, plot(f,abs(s_fft));

deg = 3;
Wn = [45*2/fs,55*2/fs]; %this 2 is because of the nyquist frequency?  

[B,A] = butter(deg,Wn,'stop');
filteredSignal = filter(B,A,s);

y_fft=fft(filteredSignal);
figure, plot(f,abs(y_fft))