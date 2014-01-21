function [b,a,y] = SysIdGazeEEG(varargin)

% function [b,a,y] = SysIdGazeEEG(GazeDirection,'param',value)
% function [b,a,y] = SysIdGazeEEG('param',value)
%
% This function identifies the Time Invariant Linear System having the Gaze
% as input and the EOG as output. The default filter order is 1 (2
% coefficients for either a and b), with a(1) = 1.
%
% The filter acts on the Gaze signal as (for an order 2 filter)
%                          -1          -2
%            b(1) + b(2)*z    + b(3)*z 
% EEG(z) = ------------------------------ Gaze(z) + epsilon
%                          -1          -2
%            1    + a(2)*z    + a(3)*z 
% 
% and minimises de 2-norm of epsilon
%
% Input:
% ------
%   GazeDirection   : takes either 'H' (horizontal) or 'V' (vertical)
%
% In ('param',value) pairs one can specify the order of both b and a as
%
% 'FiltOrd' : the filter order of a and b is the same and equals the
%       integer value that is given by the user or it differs in which case
%       the parameter value is a 2-element vector [FiltOrd_b, FiltOrd_a]
% 'Mode'    : the mode in which one functions, either 'raw' (EEG based) or
%       'ICA' (components based)
% 'XData'   : user specified x-data (EEG)
% 'YData'   : user specified y-data (Gaze)
%
% Output:
% -------
%   b, a    : the filter coefficients
%   y       : the filtered signal (due to the NaN in the original signal,
%           the function 'filter.m' of Matlab does not work)

% History:
% --------
% *** 2011-07-01 R. Phlypo @ GIPSA Lab: Created function

FiltOrd.a   = 1;
FiltOrd.b   = 1;
rawData     = true;
setXData    = false;
setYData    = false;
ODF         = 1;
kstart = 1;
if strcmpi(varargin{1},'h'), L = varargin{1}; c = '1'; kstart = 2; 
elseif strcmpi(varargin{1},'v'), L = varargin{1}; c = '2'; kstart = 2;
elseif isreal(varargin{1}), c = num2str(varargin{1},'%i'); kstart = 2; end

for k = kstart:2:length(varargin)
    if strcmpi(varargin{k},'FiltOrd')
        if length(varargin{k+1}) > 1
            FiltOrd.a = varargin{k+1}(1);
            FiltOrd.b = varargin{k+1}(2); 
        else
            FiltOrd.a = varargin{k+1};
            FiltOrd.b = varargin{k+1};
        end
    elseif strcmpi(varargin{k},'Mode')
        if strcmpi(varargin{k+1},'ICA'), rawData = false; end
    elseif strcmpi(varargin{k},'XData')
        xtF = varargin{k+1};
        setXData = true;
    elseif strcmpi(varargin{k},'YData')
        ytF = varargin{k+1};
        setYData = true;    
    elseif strcmpi(varargin{k},'odf')
        ODF = varargin{k+1};
    end
end

FLength.a   = FiltOrd.a + 1;
FLength.b   = FiltOrd.b + 1;
NbUnknowns  = FiltOrd.b + FiltOrd.a +1;
NbCoeff     = ceil( ODF*NbUnknowns);

if rawData
    if ~setXData, xtF = evalin('base',['Xt(:,getChanList({''EOG' upper(L) '''},ChannelList))'';']); end
    if ~setYData, ytF = evalin('base',['Yt(:,' c ')'';']); end
else
    if ~setXData, xtF = evalin('base',['xt(:,' c ')'';']); end
    if ~setYData, ytF = evalin('base',['yt(:,' c ')'';']); end
end


% Create the EEG reference signal
TxTemp = xtF(:,NbCoeff:end);

% Create the Toeplitz matrix for Gaze
Tyt = toeplitz([ytF(1); zeros(NbCoeff-1,1)], ytF);
Tyt = Tyt(:,NbCoeff:end);

% A first estimate based on an oversampled FIR filter
ixNaN = find(~isnan(sum(Tyt)));
TCov = Tyt(:,ixNaN)*Tyt(:,ixNaN)';
u = pinv(TCov)*Tyt(:,ixNaN)*TxTemp(ixNaN)';

H = toeplitz([u(1); zeros(FiltOrd.a,1)],u)';
H1 = H(1:FiltOrd.b+1,:);
h = H(FiltOrd.b+2:NbCoeff,1);
H2 = H(FiltOrd.b+2:NbCoeff,2:FiltOrd.a+1);

ytemp   = Tyt'*u;

Tyt     = Tyt(1:FLength.a,:);

cIt = 0; % current iteration number
MaxNbIter = 1e4; % max. number of iterations allowed

% get track of the errors/convergence
% epsilon(:,1) contains the difference between the expected output and the
%       output of the filter
% epsilon(:,2) contains the difference between the filter output and the
%       EEG reference channel 
epsilon = NaN*ones(MaxNbIter,2); 

while (cIt==0 || epsilon(cIt,1)>1e-6) && cIt<MaxNbIter
    cIt = cIt + 1;
    Tytemp = toeplitz([ytemp(1); zeros(FiltOrd.a,1)], ytemp);
    
    T = [Tyt; -Tytemp(2:end,:)];
    ixNaN = find(~isnan(sum(T)));
    TCov = T(:,ixNaN)*T(:,ixNaN)';
   
    u = pinv(TCov)*T(:,ixNaN)*TxTemp(ixNaN)';
    
%     for h = ixNaN1
%         ixNaN2 = (h-2+(1:FLength.b))*FLength.b + (1:FLength.b);
%         if min(ixNaN2)>0
%         Tyt(ixNaN2) = ( TxTemp(h) + ...
%             u(2:FLength.b)'*Tytemp(1:end-1,h-1) - ...
%             u(FLength.b+1:FLength.b+FLength.a)'*Tyt(1:end-1,h-1))/u(1);
%         end
%     end
    
    epsilon(cIt,1) = sum( (ytemp(ixNaN)' - (u'*T(:,ixNaN))).^2)/length(TxTemp);
    ytemp = T'*u;
    epsilon(cIt,2) = sum( (ytemp(ixNaN)' - TxTemp(ixNaN)).^2)/length(TxTemp);
end
% cIt
if cIt == MaxNbIter, fprintf('Convergence has not been reached in %i steps. Results may not be trustworthy.\n',MaxNbIter);
else fprintf('Convergence reached in %i steps.\n', cIt); end
% Phase distortion should be allowed (no necessity to impose filter symmetry)
b = [u(1:FLength.b)'];
a = [1 u(FLength.b+1:FLength.a+FLength.b-1)'];
y = T'*u;