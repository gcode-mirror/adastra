function res = GazeEegLabImporter_createDataFilter(varargin)

% function GazeEEG_FilterEEGData(arg#1,arg#2,...)
%
% The function filters the data in Brainamp.Data.raw with various types of
% filters: 
% Parameter         Value
% ---------         -----
%   'notch'         a notch filter (half amplitude band width 1Hz) with
%                       central frequency specified by the user (in Hz)
%
%   'HP'            a high pass filter (butterworth order 4) with cut off
%                       frequency specified by the user (in Hz)
%
%   'LP'            a low pass filter (butterworth order 4) with cut off
%                       frequency specified by the user (in Hz)
%
%   'feedback'      set true for visual feedback 
%
% Reference: SK Mitra: Digital Signal Processing, Mc Graw Hill 2001, 2nd
% ed., pp 433-435

% History:
% --------
% *** 2011-06-30 R. Phlypo @ GIPSA Lab: Created function
% *** 2011-11-16 R. Phlypo @ GIPSA Lab: use Brainamp as a global variable
% *** 2011-11-18 R. Phlypo @ GIPSA Lab: Adapted to Parameter-Value
% arguments and prepended GazeEEG_ to the function name
% *** 2012-12-22 R. Phlypo @ GIPSA Lab: output only filter coefficients, no
% filtering, added visual feedback option
% syncro version added by Anton

%global Brainamp

sos = [];
%Fs = Brainamp.Data.Params.samplingRate;
feedback = 0;
BandWidth = 2;

for k = 1:2:nargin
    if strcmpi(varargin{k},'notch')
        omega   = 2*pi*varargin{k+1}/Fs; % Notch frequency
        Bw      = 2*pi*BandWidth/Fs; % bandwidth
        beta    = cos(omega);
        alpha   = (1 - tan(Bw/2))/(1 + tan(Bw/2));
        
        b       = (1+alpha)/2*[1, -2*beta, 1];
        a       = [1, -beta*(1+alpha), alpha];
        
        sos             = [sos; zeros(2,6)]; % second-order structure
        sos(end-1,:)    = [b, a]; % no conversion needed, already second order construction
        sos(end,:)      = sos(end-1,:); % doubling the order of the filter (order 2x2=4)
    elseif strcmpi(varargin{k},'HP')
        [z,p,k] = butter(2,varargin{k+1}/Fs*2,'high');
        sos     = [sos; zp2sos(z,p,k)];
    elseif strcmpi(varargin{k},'LP')
        [z,p,k] = butter(2,varargin{k+1}/Fs*2,'low');
        sos     = [sos; zp2sos(z,p,k)];
    elseif strcmpi(varargin{k},'feedback')
        feedback = logical( varargin{k+1});
    elseif strcmpi(varargin{k},'EegAcq')
        EegAcq = varargin{k+1};
        Fs = EegAcq.Data.Params.samplingRate;
    end
end

% Filtering only needed if the user effectively gives parameters, otherwise
% do nothing
% % if nargin > 0
    [EegAcq.Data.Params.FiltStruct.b,EegAcq.Data.Params.FiltStruct.a] = sos2tf(sos); % from second order structure to transfer function
    
    disp('Filter parameters');
    disp(EegAcq.Data.Params.samplingRate);
    disp(EegAcq.Data.Params.FiltStruct.a);
    disp(EegAcq.Data.Params.FiltStruct.b);
%     figure,
if feedback
    figure,
    freqz(conv(EegAcq.Data.Params.FiltStruct.b,fliplr(EegAcq.Data.Params.FiltStruct.b)), ...
        conv(EegAcq.Data.Params.FiltStruct.a,fliplr(EegAcq.Data.Params.FiltStruct.a)),[],EegAcq.Data.Params.samplingRate); % check filter response
end
% %     Brainamp.Data.raw = filter(b,a,Brainamp.Data.raw')';
% % end

res = EegAcq
