function ChanList = GazeEEG_fuseChannelLists(varargin)


% fuses different kinds of channel lists of heterogeneous types
%
% function ChannelList = GazeEEG_fuseChannelLists(arg#1, arg#2, ...)
%
% allowed types for fusion are
%   1. vector with numerical entries
%   2. text array with labels on lines (possibly completed with blank spaces)
%   3. cell arrays with text and/or numerical entries
%
% Returns a single channel list of type 1. (numerical array)
%
% Example:
% --------
% ChanList = fuseChannelLists({'Fp1';'Fp2'}, 30)
%
% This returns the channel numbers of Fp1 (usually channel 1), channel Fp2
% and 30 (usually 30 = Oz in the standard montages)

% History:
% --------
% *** 2011-06-30 R. Phlypo @ GIPSA Lab: Created function fuseChannelLists
% *** 2011-11-14 R. Phlypo @ GIPSA Lab: Adaptation to be conform with
%           GazeEEG_ callings

if strcmpi(varargin{1},'Modality')
	Modality = varargin{2};
    kstart = 3;
else
    Modality = 'EEG';
    kstart = 1;
end

% Get integer values for a given channel
ChanList = [];
for k = kstart:nargin
    if iscell(varargin{k})
        for p = 1:length(varargin{k})
            ChanList = [ChanList; GazeEEG_getChanList(cell2mat(varargin{k}(p)),[],'Modality',Modality)];
        end
    else
        ChanList = [ChanList; GazeEEG_getChanList(varargin{k},[],'Modality',Modality)];
    end
end