%% Set parmaters

% where EEGLAB is located, use EE or higher
EEGLabPath = 'D:\Work\eeglab11_0_5_4b'

% full path to an eeg-eyetracking file - only synchronized files are
% expected
SynchroFilename = 'D:\Dropbox\Gipsa-work\GazeEeg\Data\new_s1\synchro_s1.asc.eeg.mat' %new file from gelu with new triggers above 2000, below 1000

%events which are used for the epochs generation
EpochEventsStr = '[EegAcq.Events.EventTypes.flowBlinkRight]';

%events that can be seen with data scroll
DisplayEventsStr = '{''flowFixationLeft''}';

% epoch interval in milliseconds
TimeInterval = [-100, 500];

% start processing after trigger
StartFromTrigger = 'EegAcq.Events.EventTypes.trigger130';

FilterData = true;

% number of channels (the last ones) that are not EEG, but 'EOGV','EOGH'.
% For example 32 EEG + 5(non EEG) = 37 total channels
NbNonEEGChan = 5;

%Set channel names
GazeEegLabImporter_setChannelNamesMontageParis();

%% Execute
GazeEegLabImporter_Process(EEGLabPath, SynchroFilename, EpochEventsStr, DisplayEventsStr, TimeInterval, StartFromTrigger, FilterData, NbNonEEGChan);