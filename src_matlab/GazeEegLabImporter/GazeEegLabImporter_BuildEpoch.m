function GazeEegLabImporter_BuildEpoch(varargin)

%% Init
global EEG
global ALLEEG
global CURRENTSET

global EegAcq;
global ChannelNames; %added by Anton

filterData = false;
MaxEpochsToProcess = -1;
NbNonEEGChan = 5; %number of Non EEG channels such as 'EOGV','EOGH'

for k = 1:2:nargin
    
    if strcmpi(varargin{k},'Epochs')
        NewEpochs = varargin{k+1};
    elseif strcmpi(varargin{k},'FileNameRawData')
        filename_egg_raw = varargin{k+1};
    elseif strcmpi(varargin{k},'FilterData')
        filterData = varargin{k+1};
    elseif strcmpi(varargin{k},'MaxEpochsToProcess')
         MaxEpochsToProcess = varargin{k+1};
    elseif strcmpi(varargin{k},'TimeInterval')
         TimeInterval = varargin{k+1};
    elseif strcmpi(varargin{k},'DispEvents')
        DispEvents = varargin{k+1};
    elseif strcmpi(varargin{k},'NbNonEEGChan')
        NbNonEEGChan = varargin{k+1};
    else 
        error('Error in arguments!')
    end
end

switch EegAcq.Data.Params.binaryFormat 
    case 0 % int16
        NbOctet = 2;
    case 1 % float32
        NbOctet = 4; 
    case 2 % float64
        NbOctet = 8;
end

NbNewEpochs = size(NewEpochs,2); %set the number of epochs that were previously selected
NbChB = EegAcq.Data.Params.nbOfChannels;

%% Construct event map
values = fieldnames(EegAcq.Events.EventTypes);
keys = {};
for i = 1:length(values)
     keys{end+1} = int2str(EegAcq.Events.EventTypes.(values{i}));
end;
eventsMap = containers.Map(keys, values);

%% Process

fid_Synchro = fopen(filename_egg_raw,'r');
EEG.setname = filename_egg_raw;

if MaxEpochsToProcess ~= -1 && MaxEpochsToProcess < NbNewEpochs
    NbNewEpochs = MaxEpochsToProcess;
end;
   
eventIndex = 0;

for k = 1:NbNewEpochs
    
    time1 = NewEpochs(1,k); %in ms
    time2 = NewEpochs(2,k); %in ms
    
    %first convert to seconds (/1000), then to points
    %(*EegAcq.Data.Params.samplingRate)
    %extraction (*NbChB)
    positionStart = (double(time1) / double(1000)) * double(EegAcq.Data.Params.samplingRate);
    positionEnd = (double(time2) / double(1000)) * double(EegAcq.Data.Params.samplingRate);
    
    eventDuration = int32(positionEnd - positionStart) %this is a subtraction of two positions in samples

    EEG.pnts = eventDuration; %to convert from file position to samples 

    if isempty(EEG.data) %performed just once 
        EEG.data = zeros(EEG.nbchan, EEG.pnts, NbNewEpochs);
        EEG.trials = NbNewEpochs;
    end;
    
    if filterData && isfield(EegAcq.Data.Params,'FiltStruct') 
        positionStart = positionStart - eventDuration ; % anton: we position one chunk before because instead of 1 we read 3 chunks for the filtering
        times = 3; 
    else
        times = 1;
    end
    
    %file positioning - one data point is all channels multiplied by the size of the value 
    fileStartPosStatusB = fseek(fid_Synchro, NbOctet * double(NbChB) * positionStart, 'bof'); %offset is in bytes
    
    %Performs actual reading from file
    %One unit of EEG.pnts contains 1 sample with all the channels
    %so when we read EEG.pnts points we actually read a matrix because every point has channels 
    switch EegAcq.Data.Params.binaryFormat
        case 0
            tmpData = diag(EegAcq.Data.Params.channelGains) * double(fread(fid_Synchro,[double(NbChB) (times * EEG.pnts)],'*int16'));
        case 1
            tmpData = diag(EegAcq.Data.Params.channelGains) * double(fread(fid_Synchro,[double(NbChB) (times * EEG.pnts)],'*float32'));
        case 2
            tmpData = diag(EegAcq.Data.Params.channelGains) * double(fread(fid_Synchro,[double(NbChB) (times * EEG.pnts)],'*float64'));
    end
    
    eegData = tmpData(1:(NbChB-NbNonEEGChan),:); %anton: separate EEG data
    
    if filterData && isfield(EegAcq.Data.Params,'FiltStruct')
         disp ('Applying filter');
         
         eyetrackerData = tmpData(((NbChB-NbNonEEGChan)+1):NbChB,(EEG.pnts+1):2*EEG.pnts); %anton: we reduce the eye-tracking data to 1 chunk - the center one
          
         size(eegData)
         %filtfilt applied on the first dimenion
         eegData = filtfilt(EegAcq.Data.Params.FiltStruct.b,EegAcq.Data.Params.FiltStruct.a,eegData.').';%first transpose (element wise not really needed) is because filtfilt must be aplied on the time dimension (which is the second one in our case before the transpose and first after), then we transpose back to continue
         eegData = eegData(:,(EEG.pnts+1):2*EEG.pnts); % get the middle chunk 
         size(eegData)
         size(eyetrackerData)
    else %no filtering
         eyetrackerData = tmpData(((NbChB-NbNonEEGChan)+1):NbChB,1:EEG.pnts); %anton: we just separate the eyetracker channels
    end;
    
    %save data
    tmpData = [eegData;eyetrackerData]; %anton: both have the same length: EEG.pnts
    size(tmpData)
    EEG.data(:,:,k) = tmpData; %actual data storage
    
    %% Add Display events
    epochEvent = NewEpochs(3,k); %the event that generated the epoch using the specified TimeInterval
    epochEventPos = NewEpochs(4,k); % the position in the EegAcq.Events.Triggers, used to access events that near this event 
   
    if isempty(DispEvents) == false
        
        %Check range [-eventRange eventRange+] for events that are in the
        %same epoch including the one that generated the epoch
        eventRange = 30;%this value must be big enough to cover all the events in the epoch 
        
        if (epochEventPos-eventRange) < 1 
            EventsBefore=1;
        else
            EventsBefore = epochEventPos-eventRange;
        end
        EventsAfter = (epochEventPos+eventRange);
        
        for pos = EventsBefore:1:EventsAfter
        
            if (EegAcq.Events.Triggers.time(1:1,pos)>=time1) && (EegAcq.Events.Triggers.time(1:1,pos)<=time2)

               type = EegAcq.Events.Triggers.value(pos);

               if type<1000 %we are only interested in the ones above 1000
                   continue; 
               end;
               
               addEvent = false;
               for r = 1:length(DispEvents)
                   if type == DispEvents(r) || type == epochEvent
                      addEvent = true;
                   end;
               end;
               
               if (addEvent == false) %skip if the event is neither Display one or the one that generated this epoch
                   continue; 
               end;
               
               timeRelativeToEpochStart = EegAcq.Events.Triggers.time(1:1,pos) - time1;

               eventPntsRelativeToEpochStart = (double(timeRelativeToEpochStart) / double(1000)) * double(EEG.srate);

               %Time format used in EEG lab is actually in points.
               %Time for the events looks continous because its min and max values are
               %[0 EEG.pnts * epochNumber]. Time of the event is relative to the
               %epoch, but increased with the time of the previous epochs before the
               %current. This gives an increasing value for each event. 
               latency = (k-1) * EEG.pnts + eventPntsRelativeToEpochStart; % the correct position of the event, so that later is visualzed correctly relative to its epoch

               eventIndex = eventIndex + 1;
               EEG.event(eventIndex).latency = latency;
               EEG.event(eventIndex).type = eventsMap(int2str(type));
               EEG.event(eventIndex).epoch = k;
            end;
        end;
    end;
    
    %disp('================');

end;

fclose(fid_Synchro);

%% quick verification
if isempty(EEG.data)
    disp('Warning! No valid data epochs found! EEGlab data structure may contain non valid data.');
    return;
end;

% Get channel names and types
fprintf('Getting Channel Labels ...\n')
% Set channel names & types(EEG/Gaze) in the EEGlab structure
% Anton: types are later used by our ICA implementation
for ChanIx = 1:EEG.nbchan
    chname = deblank(EegAcq.Data.Params.channelNames(ChanIx,:));

    EEG.chanlocs(ChanIx).labels  = chname;
    NbEEGChannels = EegAcq.Data.Params.nbOfChannels - 5; % 5 for eyelink data 
    
    if ChanIx <= (NbEEGChannels)
        
        %we apply names that were additionally configured
        EEG.chanlocs(ChanIx).labels  = deblank(ChannelNames(ChanIx,:));
        EEG.chanlocs(ChanIx).type    = 'EEG'; 
        disp(['Channel:' int2str(ChanIx) ' ' deblank(ChannelNames(ChanIx,:))]);
        
    else
        %here we use directly the information provided from the synchro
        %file 
        EEG.chanlocs(ChanIx).labels  = deblank(EegAcq.Data.Params.channelNames(ChanIx,:));
        EEG.chanlocs(ChanIx).type    = 'Gaze';
        disp(['Channel: ' int2str(ChanIx) ' ' deblank(EegAcq.Data.Params.channelNames(ChanIx,:)) ]);
    end
end

%Set EOG type and ref type
%Anton: Important because our ICA implementation uses this information 
IxEOG = GazeEEG_getChanList({'EOGV','EOGH'},[],'Modality','EEG')';
for ix = IxEOG, EEG.chanlocs(ix).type = 'EOG'; end
% Anton: searches for reference, but there is no reference actually
%ChanL = GazeEEG_getChanList({'Mastoïde 1', 'Mastoïde 2',
%'Réference'},[],'Modality','EEG'); % original line
ChanL = GazeEEG_getChanList({},[],'Modality','EEG');
if ~isempty(ChanL)
    for i = 1:length(ChanL)
        EEG.chanlocs(ChanL(i)).type = 'ref';
    end
end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Attribute EEGlab data fields
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

Tvec = (sign(TimeInterval).*ceil(abs(double(TimeInterval)/1000*EEG.srate)));
Tvec = (Tvec(1):Tvec(2))/EEG.srate;%*1000;
fprintf('Attributing EEG properties ...\n')
EEG.xmin = Tvec(1);
EEG.xmax = (size(EEG.data,2)-1)/EEG.srate + EEG.xmin;

% if ischar( SetNameEEG)
%     EEG.setname = SetNameEEG;
% else
%     EEG.setname = Eyelink.AscHeader.fileName;
% end

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Finish Up with some EEGlab scripting
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

fprintf('EEGlab consistency check ...\n')
%try % see whether EEGlab has been invoked
    EEG = eeg_checkset(EEG, 'makeur');
    EEG = eeg_checkset(EEG, 'eventconsistency');
%     if OverWrite
%         [ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG, CURRENTSET);
%     else
        [ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG, CURRENTSET);
    %end
    evalin('base','eeglab redraw;');
%catch
 %   fprintf('No active EEGLab session found.\n')
%end
fprintf('\t--- Done ---\n')