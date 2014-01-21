function GazeEEG_setElectrodeType(varargin)
% % par défaut on met tous les canaux du type EEG (car une minorité seulement ne le sont pas)
% for k = 1:size(EEG.data,1)
% 	EEG.chanlocs( k).type = 'EEG';
% end

global EEG
global ALLEEG
global CURRENTSET


% Default values
% for k = 1:size(EEG.data,1)
% 	EEG.chanlocs( k).type = 'EEG';
% end

ixEOG   = []; 
ixGaze  = []; 
ixRef   = [];
ixEEG   = [];
ixLogic = [];

for k = 1:2:nargin
    if strcmpi(varargin{k},'EEG')
        ixEEG = varargin{k+1};
    elseif strcmpi(varargin{k},'EOG')
        ixEOG = varargin{k+1};
    elseif strcmpi(varargin{k},'Gaze')
        ixGaze = varargin{k+1};
    elseif strcmpi(varargin{k},'Ref')
        ixRef = varargin{k+1};
    elseif strcmpi(varargin{k},'Logic')
        ixLogic = varargin{k+1};
    end
end

if ~isempty(ixEEG)
for k = ixEEG
	EEG.chanlocs( k).type = 'EEG';
end
end

if ~isempty(ixEOG)
for k = ixEOG
	EEG.chanlocs( k).type = 'EOG';
end
end

if ~isempty(ixGaze)
for k = ixGaze
	EEG.chanlocs( k).type = 'Gaze';
end
end

if ~isempty(ixRef)
for k = ixRef
	EEG.chanlocs( k).type = 'Ref';
end
end

if ~isempty(ixLogic)
for k = ixLogic
	EEG.chanlocs( k).type = 'Logic';
end
end

