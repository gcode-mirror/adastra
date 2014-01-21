function [out] = emgFilterIntegrate(in,fs)
 
[b a] = butter(4,[20 500]/(fs/2),'bandpass');
 
in = filtfilt(b,a,in);
 
b_int = ones(0.2*fs,1)/fs;
out = filter(b_int,1,in.^2);
 
end