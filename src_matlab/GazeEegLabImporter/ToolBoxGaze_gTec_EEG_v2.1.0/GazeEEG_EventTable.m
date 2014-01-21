function GazeEEG_EventTable( varargin)

% function GazeEEG_EventTable()
% function GazeEEG_EventTable( {'Event1', 'Event2', ... , 'EventN'})
%
% Constructs a table of the Events {'Event1', 'Event2', ... , 'EventN'} or
% of all Events if no input argument is specified
%
% The table contains
% Event *EventType* [*Integer Code*] ....... : *total number of* occurrences ( *validated number of occurrences found* in *N* trials)

global Eyelink
global Brainamp

GazeEEG_debugMode = false;

points = @(x) repmat('.',1,x);


if nargin < 1
    EventType = fieldnames( Eyelink.Events.EventTypes);
else
    EventType = {varargin{1}};
end

if iscell(EventType)
    for k = 1:length(EventType)
        Temp = Eyelink.Events.EventTypes.(EventType{k});
        EventTypeInt(k) = Temp;
    end
else
    error('incompatible, cell required')
end

EventList = setdiff( fieldnames(Eyelink.Events), 'EventTypes');
LSmax = 0;

for k = 1:length( EventTypeInt)
    AllEvents(k).IsOfType   = [];
    AllEvents(k).IsValid    = [];
    AllEvents(k).IsInTrial  = [];
    AllEvents(k).InTrial    = false(1,length(Eyelink.Trials.keep));
    EvTypeS = EventType{k};
    
    for ixEv = 1:length( EventList)
        EventsOfCurrentType     = (Eyelink.Events.(EventList{ixEv}).type == EventTypeInt(k) );
        AllEvents(k).IsOfType   = [AllEvents(k).IsOfType ...
            EventsOfCurrentType];
        
        if GazeEEG_debugMode
            fprintf('For Events in %s : found %i of type %s\n',EventList{ixEv}, length(find(Eyelink.Events.(EventList{ixEv}).type == EventTypeInt(k) )) ,EvTypeS)
        end
        AllEvents(k).IsValid     = [AllEvents(k).IsValid ...
            Eyelink.Events.(EventList{ixEv}).keep];
        IsInTrial   = false(1,length(Eyelink.Events.(EventList{ixEv}).keep));
        
        for ixTr = 1:length(Eyelink.Trials.keep)
            IsInCurrentTrial = (Eyelink.Events.(EventList{ixEv}).time(1,:) >= Eyelink.Trials.time(1,ixTr) & ...
                Eyelink.Events.(EventList{ixEv}).time(1,:) <= Eyelink.Trials.time(2,ixTr));
            AllEvents(k).InTrial(ixTr) = AllEvents(k).InTrial(ixTr) || any(IsInCurrentTrial(EventsOfCurrentType));
            IsInTrial   = IsInTrial | IsInCurrentTrial;
                
        end
        AllEvents(k).IsInTrial     = [AllEvents(k).IsInTrial IsInTrial];
    end
    %         if isfield( Eyelink.Events, EventList{ixEv})
    %             IsInTrial   = false
    %             IsInTrial   = Eyelink.Events.(EventList{ixEv}).time(1,:) >= Eyelink.Trials.time(1,ixTr) & ...
    %                 Eyelink.Events.(EventList{ixEv}).time(1,:) <= Eyelink.Trials.time(2,ixTr);
    %             IsOfType    = (Eyelink.Events.(EventList{ixEv}).type == EventTypeInt);
    %             IsValid     = Eyelink.Events.(EventList{ixEv}).keep;
    %             fprintf('\tEvents %s contains %i Events (%i valid) of type %s\n',EventList{ixEv},length(find( IsInTrial & IsOfType)),length(find( IsInTrial & IsOfType & IsValid)),EventType);
    %         end
    
    LSmax = max(LSmax, length(EvTypeS));
end

fprintf('\n\tData contains %i Trials, of which %i have been validated\n\n',length(Eyelink.Trials.keep),length(find(Eyelink.Trials.keep)))

for k = 1:length( EventTypeInt)
    EvTypeS = EventType{k};
    LS = length( EvTypeS);
    fprintf('Event %s [%i] %s : %i occurences (%i in %i trials)\n', ...
        EvTypeS, EventTypeInt(k), points(LSmax-LS), length( find( AllEvents(k).IsOfType)),...
        length( find(AllEvents(k).IsOfType & AllEvents(k).IsInTrial)), length(find(AllEvents(k).InTrial)) );
end