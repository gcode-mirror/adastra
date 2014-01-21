function ChanList = GazeEEG_getChanList(ChanList,RefChanList,varargin)

% function ChannelList = GazeEEG_getChanList(ChannelList[,RefChanList,varargin])
%
% function that permits to get a conform numerical list of a given
% ChannelList which takes one of the following types
%   1. vector with numerical entries
%   2. array with labels as lines (possibly completed with blank spaces)
%   3. cell array (preferably column shaped)
%
% returns a single numerical list (type 1)
%
% Optionally a reference Channel List can be given e.g. to search in an
% already reduced set of labels. The reference Channel List should be given
% as either one of the three types mentioned earlier

% History:
% --------
% *** 2011-06-30 R. Phlypo @ GIPSA Lab: Created function
% *** 2011-07-01 R. Phlypo @ GIPSA Lab: Added a reference Channel List

global EEG
global Eyelink
global Brainamp

Modality = 'EEG';

if strcmpi(varargin{1},'Modality') && ~strcmpi(varargin{2},'EEG')
    Modality = varargin{2};
    NbChannels = eval([Modality '.Data.Params.nbOfChannels']);
else % user did not specify a modality
    NbChannels  = EEG.nbchan;
end

if nargin < 2 || isempty(RefChanList)  
    RefChanList = 1:NbChannels; 
else 
    RefChanList = GazeEEG_getChanList(RefChanList,[],'Modality',Modality);
    NbChannels  = length(RefChanList);
end

% Some inline function definitions
isVector = @(x) numel(x)==length(x);
isColumnVector = @(x) isVector(x) && size(x,1) == length(x);

% Create the ChanList as a vector with numerical indices
InValidEntryIx = [];
if isstr(ChanList)
    ChanListNames = ChanList;
    ChanList = cell(size(ChanListNames,1),1);
    for k = 1:size(ChanListNames,1)
        ChanList{k} = deblank(ChanListNames(k,:));
    end
    ChanList = GazeEEG_getChanList(ChanList,[],'Modality',Modality);
elseif iscell(ChanList)
    % free ChanList to contain the channel reference numbers
    % copy its content in ChanListNames
    ChanListNames = ChanList;
    if strcmpi(Modality,'EEG')
        ChanLabels = deblank({EEG.chanlocs(RefChanList).labels}');
    else
        Tmp = eval([Modality '.Data.Params.channelNames']);
        ChanLabels = deblank(mat2cell(Tmp,ones(eval([Modality '.Data.Params.nbOfChannels']),1),size(Tmp,2)));
    end
    ChanList = zeros(length(ChanListNames),1);
    for ChEntry = 1:length(ChanListNames)
        ix = find(strcmpi(ChanListNames{ChEntry},ChanLabels));
        if ~isempty(ix)
            ChanList(ChEntry) = ix;
        else
            warning([ChanListNames{ChEntry} ' does not exist in your recording, skipping this entry and continuing ...']);
            InValidEntryIx = [InValidEntryIx ChEntry];
        end
    end
elseif isVector(ChanList) && ~isColumnVector(ChanList)
    ChanList = ChanList'; 
end 
if ~isempty(InValidEntryIx)
    ChanList(InValidEntryIx) = [];
end