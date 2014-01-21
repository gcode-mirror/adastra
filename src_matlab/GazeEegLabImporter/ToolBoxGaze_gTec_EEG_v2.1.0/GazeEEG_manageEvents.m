function GazeEEG_manageEvents( )

% New version of GazeEEG_manageEvents
%
% Event management is now jointly done on Brainamp and Eyelink

global Eyelink
global Brainamp

% Get current maximum EventTypes integer code (Eyelink)
EventType = fieldnames(Eyelink.Events.EventTypes);
maxET = 0;
for k = 1:length(EventType)
    maxET = max( maxET, Eyelink.Events.EventTypes.(EventType{k}) );
end

% Get current maximum EventTypes integer code (Eyelink+Brainamp)
EventType = fieldnames(Brainamp.Events.EventTypes);
for k = 1:length(EventType)
    maxET = max( maxET, Brainamp.Events.EventTypes.(EventType{k}) );
end

TriggerVals = unique( [Eyelink.Events.Triggers.value Brainamp.Events.Triggers.value]);
for k = 1:length(TriggerVals)
    % add Event triggerXX to the EventTypes
    Eyelink.Events.EventTypes.(['trigger' num2str(TriggerVals(k),'%i')]) = maxET+k;
    Brainamp.Events.EventTypes.(['trigger' num2str(TriggerVals(k),'%i')]) = maxET+k;
    
    % set Triggers.type to the EventType identifier of the corresponding trigger
    Eyelink.Events.Triggers.type( Eyelink.Events.Triggers.value==TriggerVals(k)) = maxET+k;
    Brainamp.Events.Triggers.type( Brainamp.Events.Triggers.value==TriggerVals(k)) = maxET+k;
end

% Add the keep field to the Events, this field can be modified by functions
% to indicate whether events are useful for further analysis (true) or
% should be discarded.
for fn = fieldnames(Eyelink.Events)'
    if (~strcmpi(char(fn), 'EventTypes'))
        Eyelink.Events.(char(fn)).keep = true(1,length(Eyelink.Events.(char(fn)).time));
        Eyelink.Events.(char(fn)).valid = true(1,length(Eyelink.Events.(char(fn)).time));
    end
end

for fn = fieldnames(Brainamp.Events)'
    if (~strcmpi(char(fn), 'EventTypes'))
        Brainamp.Events.(char(fn)).keep = true(1,length(Brainamp.Events.(char(fn)).time));
        Brainamp.Events.(char(fn)).valid = true(1,length(Brainamp.Events.(char(fn)).time));
    end
end