function GazeEEG_buildEpoch(varargin)

% function  GazeEEG_buildEpoch(varargin)
%
% This function manages the events present in the Eyelink and attributes
% these to the Brainamp data. Data will be epoched with respect to the
% chosen event.
%
% Parameter             Value
% ---------             -----
% SyncEvents            cell containing the events used to sync the epochs
%
% TimeInterval          time interval in milliseconds w.r.t. the sync event
%                           a two element array default:[-500 1500]
%
% DispEvents            cell containing the events to display in EEGLab,
%                           by default this set contains the SyncEvents
%
% Name                  specify the name of the EEG set that will be
%                           created [Eyelink.AscHeader.fileName]
%
% Replace               boolean to specify whether the new EEG set should
%                           replace the current set (true) or should be
%                           added to the already available sets [false]
% 
%
% Example:
% --------
%
% >> GazeEEG_buildEpoch(...
%           'SyncEvents',   {'flowFixationLeft','flowFixationRight'}, ...
%           'TimeInterval', [-500 1500], ...
%           'DispEvents',   {'flowSaccadeLeft','flowSaccadeRight'}, ...
%           'Name',         'Fixations', ...
%           'Replace',      true );
% 
% This will center the epochs around the events of type flowFixationLeft &
% flowFixationRight with a window starting 500ms before the Fixation and
% ending 1500ms after each Fixation. The windows most probably overlap.

% History:
% --------
% *** 2011-11-?? R. Phlypo @ GIPSA-Lab
% Birth of file and many, many alterations since
% *** 2011-11-18 R. Phlypo @ GIPSA-Lab
% parameter value inputs for compatibility and user friendliness
% *** 2012-02-22 R. Phlypo@ GIPSA-Lab
% added the parameters Name & Replace, corrected for scaling in Brainamp
% *** 2012-05-16 H. Queste@ GIPSA-Lab & A. Gu�rin@ GIPSA-Lab
% Compatibility with GTec file

%%%%%%%%%%%%%%%%%%
% Initialisation %
%%%%%%%%%%%%%%%%%%

% Default values for some parameters
TimeInterval = [-500 1500];
EvTypeSync = {};
EvTypeDisp = EvTypeSync;
userFeedback = true;
HalfUserFeedback = false;
SetNameEEG = false;
OverWrite = false;
filterData = false;

% Get user defined values for these parameters
for k = 1:2:nargin
    if strcmpi(varargin{k},'SyncEvents')
        EvTypeSync = varargin{k+1};
        if HalfUserFeedback
            userFeedback = false;
        else
            HalfUserFeedback = true;
        end
    elseif strcmpi(varargin{k},'TimeInterval')
        TimeInterval = varargin{k+1};
    elseif strcmpi(varargin{k},'DispEvents')
        EvTypeDisp = varargin{k+1};
        if HalfUserFeedback
            userFeedback = false;
        else
            HalfUserFeedback = true;
        end
    elseif strcmpi( varargin{k}, 'Name')
        SetNameEEG = varargin{k+1};
    elseif strcmpi( varargin{k}, 'Replace')
        OverWrite = logical( varargin{k+1});
    elseif strcmpi( varargin{k}, 'FilterData')
        filterData = logical(varargin{k+1});
    else
        error(sprintf('''%s'' is not a valid option, please verify the spelling.',varargin{k}))
    end
end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Set global variables 
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
global Eyelink
global Brainamp
global EEG
global ALLEEG
global CURRENTSET

% Create new EEG variable set and attribute the sampling rate
try
    EEG     = eeg_emptyset;
end
EEG.srate   = double(Brainamp.Data.Params.samplingRate);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% If no events to process, ask user feedback
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
EventTypes = fieldnames(Eyelink.Events.EventTypes); % cell of strings

if isempty(EvTypeSync)
    [ListSel,OkButton] = listdlg('ListString',EventTypes,'name','Sync Events');
    if OkButton && ~isempty(ListSel)
        EvTypeSync = EventTypes(ListSel)';
    else
        error('No events to build epochs upon.')    
    end
else
    ListSel = [];
    for ix = 1:length(EventTypes)
        if any(strcmpi(EventTypes{ix},EvTypeSync))
            ListSel = [ListSel; ix];
        end
    end
end

if isempty(EvTypeDisp)
    [ListSel,OkButton] = listdlg('ListString',EventTypes,'name','Display Events','InitialValue',ListSel);
    if OkButton && ~isempty(ListSel)
        EvTypeDisp = EventTypes(ListSel)';
    else
        EvTypeDisp = EvTypeSync; 
    end
end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Prepare user feedback
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
StrEventsToSync = '''SyncEvents'', {';
for iter = 1:length(EvTypeSync)
    StrEventsToSync = [StrEventsToSync ' ''' EvTypeSync{iter} ''','];
end
StrEventsToSync = sprintf('%s\b}',StrEventsToSync); 

StrEventsToDisp = '''DispEvents'', {';
for iter = 1:length(EvTypeDisp)
    StrEventsToDisp = [StrEventsToDisp ' ''' EvTypeDisp{iter} ''','];
end
StrEventsToDisp = sprintf('%s\b}',StrEventsToDisp);

output = sprintf(['GazeEEG_buildEpoch(...\n\t%s, ...\n\t' ...
    '''TimeInterval'', [%i %i], ...\n\t' ...
    '%s);\n'], StrEventsToSync,TimeInterval(1),TimeInterval(2),StrEventsToDisp);

% Provide user feedback on the suggested command line use
StrSize = 100;
if userFeedback
    fprintf('\n\n%s\n Suggested usage (Copy & Paste into command line or script):\n%s\n\n',repmat('-',1,StrSize),repmat('-',1,StrSize));
    fprintf('%s\n',output);
    fprintf('%s\n\n',repmat('-',1,StrSize));
end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% prepare the Event Indexing Table
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
fprintf('Prepare Event Table ...\n')
% Return a set of integers corresponding to the events of EvTypeSync as they
% occur in the event table Eyelink.Events.EventTypes
EvTypeSyncInt = [];
EvTypeDispInt = [];
EvType_LUT = zeros(length(EventTypes),1);
for ix = 1:length(EventTypes)
    % Events to sync
    if any(strcmpi(EventTypes{ix},EvTypeSync)) 
        EvTypeSyncInt = [EvTypeSyncInt Eyelink.Events.EventTypes.(EventTypes{ix})];
    end
    % Events to Display
    if any(strcmpi(EventTypes{ix},EvTypeDisp))
        EvTypeDispInt = [EvTypeDispInt Eyelink.Events.EventTypes.(EventTypes{ix})];
    end

    % The look up table
    % -----------------
    % EvType_LUT(ix) is the numerical representation used for Events of
    % type EventTypes{ix}
    % To find the phrasal description of the event type associated with the
    % numerical representation 'IndexOfEventType' use
    %       EventTypes{ find( EvType_LUT == IndexOfEventType)}
    EvType_LUT(ix) = Eyelink.Events.EventTypes.(EventTypes{ix});
end

% the number of epochs will be determined by the number of Events used to
% Sync, a first approximation of the number of events is given below
EvTypeDispInt = union(EvTypeSyncInt, EvTypeDispInt);
NbEvents = length(EvTypeSyncInt);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Process Events and Extract Events about which to center epochs
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
fprintf('Parsing Events ...\n')
% Go search for Sync Events that lie within the trigger intervals:
% trigger intervals are in the basis structures given by Struct.Trials
% where Struct may be either of Brainamp or Eyelink

% if no Trials field can be found in the Eyelink or Brainamp structure the
% user most probably hasn't ru, the GazeEEG_matchTriggers command
if ~isfield(Eyelink,'Trials') || ~isfield(Brainamp,'Trials')
    error('Run <a href="matlab: doc GazeEEG_matchTriggers">GazeEEG_matchTriggers</a> first to set the Trials field in the data');
end

% run over all event fieldnames (excluding 'EventTypes' which is just a
% tabulation of the event types) and concatenate type and time
% *** time is given in samples ! ***
% *** only start times are used at this moment ***
EyelinkEvents = setdiff( fieldnames(Eyelink.Events), 'EventTypes'); % cell with the fieldnames
AllEvents = [];
cnt = 0;
for k = 1:length(EyelinkEvents) % Get a super events vector
    for Iter = 1:length(Eyelink.Events.(EyelinkEvents{k}).type)
        cnt = cnt+1;
        AllEvents(cnt).latency = Eyelink.Events.(EyelinkEvents{k}).time(1,Iter);
        AllEvents(cnt).type = Eyelink.Events.(EyelinkEvents{k}).type(Iter);
        AllEvents(cnt).isValid2Sync = Eyelink.Events.(EyelinkEvents{k}).keep(Iter);
        AllEvents(cnt).isValid2Disp = Eyelink.Events.(EyelinkEvents{k}).valid(Iter);
    end
end

% sort the latencies in ascending order for EEGlab compatibility
[nouse,IxOrder] = sort([AllEvents.latency],'ascend');
AllEvents = AllEvents(IxOrder);

% % % Find those events that correspond to the user's request (EvTypeSyncInt)
% % % valid event times are
% % %    1. (EvTime >= start of trial) AND (EvTime <= end of trial)
% % %       trial satisfies condition (i.e. there exists an Event of type
% % %    2. EvTypeCond in the current trial)
% % % trial indices are given by GazeEEG_matchTriggers()

% Valid events are validated by the field isValid (originally called keep in the data structures)

% 1. Syncing Events
%    --------------
% get all events used to sync
% logical Indexing is faster than numerical indexing
EvIx = any(ones(NbEvents,1)*double([AllEvents.type])==double(EvTypeSyncInt)'*ones(1,length(AllEvents)),1);
EvTime = [AllEvents(EvIx).latency];

if isempty( EvTime), error('GazeEEG:incompatibleEvent','No events of type ''%s'' found, no syncing possible -- Exiting function\n', EventTypes{ find( EvType_LUT == EvTypeSyncInt)}); end

% First condition: Indices of the SyncEvent falls within a trial
% ValidEvIx = any( ones(length(Eyelink.Trials),1)*double(EvTime) >= double(Eyelink.Trials(:,1))*ones(1,length(EvTime)) & ...
%     ones(length(Eyelink.Trials),1)*double(EvTime) <= double(Eyelink.Trials(:,2))*ones(1,length(EvTime)), 1);

ValidEvIx = EvIx & [AllEvents.isValid2Sync];
ValidEvTime = [AllEvents(ValidEvIx).latency];
NbValidEv = length( ValidEvTime);
fprintf('%i valid events detected.\n', NbValidEv);

% Get the corresponding intervals: 
%   [ValidEvTime(k) + TimeInterval(1), ValidEvTime(k) + TimeInterval(2)]
% In matrix operations this gives a (2 x NbValidEv) array with entries
% (i,j) : ValidEvTime(j) + TimeInterval(i)
SampleInterval = (sign(TimeInterval).*ceil(abs(double(TimeInterval)/1000*EEG.srate)))';
EpochIxEyelink = ones(2,1)*double(ValidEvTime) + SampleInterval(:)*ones(1,NbValidEv);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Epoch Data
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
fprintf('Epoching Data ...\n')
% find the trigger in the trigger interval that is closest to the start
% time of each of the events to epoch about, this will be the reference to
% align with the Brainamp Data
%
% schematically:
%
%               (TriggerX)  (EpochStart)        (EpochEnd)   (TriggerY)
% Brainamp ----------||----------|-------------------|-----------||-----
%
%          (TriggerX)  (EpochStart)        (EpochEnd)   (TriggerY)
% Eyelink  -----||----------|-------------------|-----------||----------
%
% Because of the correspondences of Triggers on both modalities as
% obtained with GazeEEG_matchTriggers(), we may get relative sample indices
% with respect to the trigger closest to EpochStart (in the above scheme
% this is 'TriggerX')
%
% latencies in EEG.epoch(*).eventlatency are in milliseconds with respect
% to the first sample of the unfolded array seen as continuous data
% [see EEGlab documentation]

% get the indices and time of the events to display under EEGlab
EvDispIx = any(ones(length(EvTypeDispInt),1)*double([AllEvents.type])==double(EvTypeDispInt)'*ones(1,length(AllEvents)),1);
EvDispIx = find(EvDispIx & [AllEvents.isValid2Disp]);
EvDispTime = [AllEvents(EvDispIx).latency];

% initialise some values for the EEG parameters
EEG.nbchan  = Brainamp.Data.Params.nbOfChannels + Eyelink.Data.Params.nbOfChannels;
EEG.data    = zeros(EEG.nbchan, diff(EpochIxEyelink(:,1))+1, NbValidEv);
EEG.trials  = NbValidEv;
EEG.pnts    = diff(EpochIxEyelink(:,1))+1;

% initialise parameters
EpochIxBrainamp = zeros(2,NbValidEv);
NbSamplesBrainamp = Brainamp.Data.Params.nbOfSamples;
NbChB = Brainamp.Data.Params.nbOfChannels;
NbSamplesEyelink = Eyelink.Data.Params.nbOfSamples;
NbChE = Eyelink.Data.Params.nbOfChannels;
ValidEyelinkTriggerIx = find(Eyelink.Events.Triggers.valid);
ValidBrainampTriggerIx = find(Brainamp.Events.Triggers.valid);

% set local counters
cntEvents = 0;
cntEpochs = 0;

% open files to read
readEyelinkFromFile = false;
readBrainampFromFile = false;
if ischar(Eyelink.Data.raw)
    fid_Eyelink = fopen(Eyelink.Data.raw);
    readEyelinkFromFile = true;
end
if ischar(Brainamp.Data.raw)
    fid_Brainamp = fopen(Brainamp.Data.raw);
    readBrainampFromFile = true;
end

% Loop through the epochs
for k = 1:NbValidEv
%     [nouse,ClosestTriggerIx] = min(abs(double(ValidEvTime(k)) -
%     double(Eyelink.Events.Triggers.time(ValidEyelinkTriggerIx))));
    % Start Epoch on Brainamp = (start Epoch on Eyelink) +
    %   (closestTrigger_Brainamp - closestTrigger_Eyelink)
%     EpochIxBrainamp(:,k) = double(ValidEvTime(k)) + double(SampleInterval(:)) + double(Brainamp.Events.Triggers.time(ValidBrainampTriggerIx(ClosestTriggerIx)))-double(Eyelink.Events.Triggers.time(ValidEyelinkTriggerIx(ClosestTriggerIx)));
    
    [nouse,ClosestTriggerIx] = min(abs(double(ValidEvTime(k)) - double(Eyelink.Trials.time(:))));
    EpochIxBrainamp(:,k) = double(ValidEvTime(k)) + double(SampleInterval(:)) + double(Brainamp.Trials.time(ClosestTriggerIx))-double(Eyelink.Trials.time(ClosestTriggerIx));
    % End Epoch on Brainamp = start Epoch on Brainamp + Epoch Duration
%     EpochIxBrainamp(2,k) = EpochIxBrainamp(1,k) + diff(EpochIxEyelink(:,k));
   
    % EpochedData contains concatenated Brainamp and Eyelink Data
    if EpochIxBrainamp(2 ,k) <= NbSamplesBrainamp && EpochIxEyelink(2,k) <= NbSamplesEyelink
        cntEpochs = cntEpochs+1;
        
        % This is the straightforward raw data
%         EEG.data(:,:,cntEpochs) = [Brainamp.Data.raw(:,EpochIxBrainamp(1,k):EpochIxBrainamp(2,k)); ...
%             ];
        
%         % This should also be possible using file reading 'on demand'  
%         fileStartPosStatusE  = fseek(fid_Eyelink, 2*NbChE*(EpochIxEyelink(1,k)-1), 'bof');
%         fileStartPosStatusB = fseek(fid_Brainamp, 2*NbChE*(EpochIxBrainamp(1,k)-1), 'bof');
        if readEyelinkFromFile,
            fileStartPosStatusE  = fseek(fid_Eyelink, 2*double(NbChE)*(EpochIxEyelink(1,k)-1), 'bof');
            tmpEyelinkData = double(fread(fid_Eyelink,[double(NbChE) EEG.pnts],'*int16'));
            tmpEyelinkData(tmpEyelinkData==Eyelink.Data.Params.valNaN) = NaN;
        else
            tmpEyelinkData = Eyelink.Data.raw(:,EpochIxEyelink(1,k):EpochIxEyelink(2,k));
        end
        

        % Incorporate Filter design with Brainamp.Data.Params.FiltStruct.[b a]
        % Take 3 times the window length; filter and truncate to original
        % window length => reduce window border effects
        if filterData && isfield(Brainamp.Data.Params,'FiltStruct')
            StartPos    = EpochIxBrainamp(1,k) - ( EpochIxBrainamp(2,k) - EpochIxBrainamp(1,k) +1);
            EndPos      = EpochIxBrainamp(2,k) + ( EpochIxBrainamp(2,k) - EpochIxBrainamp(1,k) +1);
            if readBrainampFromFile
                switch Brainamp.Data.Params.binaryFormat 
                    case 0 % int16
                        NbOctet = 2;
                    case 1 % float32
                        NbOctet = 4; 
                    case 2 % float64
                        NbOctet = 8;
                end
                fileStartPosStatusB = fseek(fid_Brainamp, NbOctet*double(NbChB)*(StartPos-1), 'bof');
                switch Brainamp.Data.Params.binaryFormat
                    case 0
                        tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*double(fread(fid_Brainamp,[double(NbChB) 3*EEG.pnts],'*int16'));
                    case 1
                        tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*double(fread(fid_Brainamp,[double(NbChB) 3*EEG.pnts],'*float32'));
                    case 2
                        tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*double(fread(fid_Brainamp,[double(NbChB) 3*EEG.pnts],'*float64'));
                end
            else
                tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*(Brainamp.Data.raw(:,StartPos:EndPos));
            end
            tmpBrainampData = filtfilt(Brainamp.Data.Params.FiltStruct.b,Brainamp.Data.Params.FiltStruct.a,tmpBrainampData.').';
            tmpBrainampData = tmpBrainampData(:,EEG.pnts+1:2*EEG.pnts);
        elseif filterData && ~isfield(Brainamp.Data.Params,'FiltStruct')
            error('GazeEEG:prerequisites','Run <a href="matlab: doc GazeEEG_createDataFilter">GazeEEG_createDataFilter</a> first to set filter coefficients');
        else
            if readBrainampFromFile
                switch Brainamp.Data.Params.binaryFormat 
                    case 0 % int16
                        NbOctet = 2;
                    case 1 % float32
                        NbOctet = 4; 
                    case 2 % float64
                        NbOctet = 8;
                end
                fileStartPosStatusB = fseek(fid_Brainamp, NbOctet*double(NbChB)*(EpochIxBrainamp(1,k)-1), 'bof');
                switch Brainamp.Data.Params.binaryFormat
                    case 0
                        tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*double(fread(fid_Brainamp,[double(NbChB) EEG.pnts],'*int16'));
                    case 1
                        tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*double(fread(fid_Brainamp,[double(NbChB) EEG.pnts],'*float32'));
                    case 2
                        tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*double(fread(fid_Brainamp,[double(NbChB) EEG.pnts],'*float64'));
                end
            else
                tmpBrainampData = diag(Brainamp.Data.Params.channelGains)*(Brainamp.Data.raw(:,EpochIxBrainamp(1,k):EpochIxBrainamp(2,k)));
            end
        end
        
        EEG.data(:,:,cntEpochs) = [...
            tmpBrainampData; ...
            tmpEyelinkData ...
            ];
        
        % Import Triggers and Events from Eyelink
        % Only retain those events and triggers that fall within the epochs
        EpochEvIx = find( ( EvDispTime >= EpochIxEyelink(1,k)) & ( EvDispTime <= EpochIxEyelink(2,k)));
        % latencies are given in samples w.r.t. the first sample in the
        % epoch.
        for EpochEv = 1:length(EpochEvIx)
            EEG.event(cntEvents+EpochEv).latency = (cntEpochs-1)*EEG.pnts + double(AllEvents(EvDispIx(EpochEvIx(EpochEv))).latency) - double(EpochIxEyelink(1,k))+1;
            EEG.event(cntEvents+EpochEv).type = EventTypes{find(EvType_LUT==AllEvents(EvDispIx(EpochEvIx(EpochEv))).type)};
            EEG.event(cntEvents+EpochEv).epoch = cntEpochs;
        end
        cntEvents = cntEvents + length(EpochEvIx);
    else
        EEG.data(:,:,end) = [];
    end
end

if readEyelinkFromFile, fclose(fid_Eyelink); end
if readBrainampFromFile, fclose(fid_Brainamp); end

if isempty(EEG.data)
    fprintf('No valid data epochs found! EEGlab data structure may contain non valid data.')
    return
else
    fprintf('Retained %i/%i trials\n',size(EEG.data,3),NbValidEv);
    EEG.trials = size(EEG.data,3);
end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Get channel names and types
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
fprintf('Getting Channel Labels ...\n')
% Attribute channel names & types (EEG/Gaze) as for the EEGlab structure
for ChanIx = 1:EEG.nbchan
    if ChanIx <= Brainamp.Data.Params.nbOfChannels
        EEG.chanlocs(ChanIx).labels  = deblank(Brainamp.Data.Params.channelNames(ChanIx,:));
        EEG.chanlocs(ChanIx).type    = 'EEG'; 
    else
        EEG.chanlocs(ChanIx).labels  = deblank(Eyelink.Data.Params.channelNames(ChanIx-Brainamp.Data.Params.nbOfChannels,:));
        EEG.chanlocs(ChanIx).type    = 'Gaze';
    end
end

% Set EOG type and ref type
IxEOG = GazeEEG_getChanList({'EOGV','EOGH'},[],'Modality','EEG')';
for ix = IxEOG, EEG.chanlocs(ix).type = 'EOG'; end
ChanL = GazeEEG_getChanList({'Mastoïde 1', 'Mastoïde 2', 'Réference'},[],'Modality','EEG');
if ~isempty(ChanL)
    for i = 1:length(ChanL)
        EEG.chanlocs(ChanL(i)).type = 'ref';
    end
end


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Attribute EEGlab data fields
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% Relative-to-stimulus time vector
Tvec = (sign(TimeInterval).*ceil(abs(double(TimeInterval)/1000*EEG.srate)));
Tvec = (Tvec(1):Tvec(2))/EEG.srate;%*1000;
fprintf('Attributing EEG properties ...\n')
if ischar( SetNameEEG)
    EEG.setname = SetNameEEG;
else
    EEG.setname = Eyelink.AscHeader.fileName;
end
EEG.xmin = Tvec(1);
EEG.xmax = (size(EEG.data,2)-1)/EEG.srate + EEG.xmin;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Finish Up with some EEGlab scripting
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

fprintf('EEGlab consistency check ...\n')
try % see whether EEGlab has been invoked
    EEG = eeg_checkset(EEG, 'makeur');
    EEG = eeg_checkset(EEG, 'eventconsistency');
    if OverWrite
        [ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG, CURRENTSET);
    else
        [ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG);%, CURRENTSET);
    end
    evalin('base','eeglab redraw;');
catch
    fprintf('No active EEGLab session found.\n')
end
fprintf('\t--- Done ---\n')