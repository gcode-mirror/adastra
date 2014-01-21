function GazeEEG_EventTable()

EventList = setdiff( fieldnames(Eyelink.Events), 'EventTypes');

EventType = 'flowFixationLeft';
EventTypeInt = Eyelink.Events.EventTypes.(EventType);

IsValidStr = @(x) (repmat('not',1,double(~x)));

% for ixTr = find(Eyelink.Trials.keep)
%     fprintf('Trial %i: %s valid\n', ixTr, IsValidStr(Eyelink.Trials.keep(ixTr)));
%     for ixEv = 1:length( EventList)
%         if isfield( Eyelink.Events, EventList{ixEv})
%             IsInTrial   = Eyelink.Events.(EventList{ixEv}).time(1,:) >= Eyelink.Trials.time(1,ixTr) & ...
%                 Eyelink.Events.(EventList{ixEv}).time(1,:) <= Eyelink.Trials.time(2,ixTr);
%             IsOfType    = (Eyelink.Events.(EventList{ixEv}).type == EventTypeInt);
%             IsValid     = Eyelink.Events.(EventList{ixEv}).keep;
%             fprintf('\tEvents %s contains %i Events (%i valid) of type %s\n',EventList{ixEv},length(find( IsInTrial & IsOfType)),length(find( IsInTrial & IsOfType & IsValid)),EventType);
%         end
%     end
%     pause   
% end

for ixTr = find(Eyelink.Trials.keep)
    fprintf('Trial %i: %s valid\n', ixTr, IsValidStr(Eyelink.Trials.keep(ixTr)));
    for ixEv = 1:length( EventList)
        if isfield( Eyelink.Events, EventList{ixEv})
            IsInTrial   = Eyelink.Events.(EventList{ixEv}).time(1,:) >= Eyelink.Trials.time(1,ixTr) & ...
                Eyelink.Events.(EventList{ixEv}).time(1,:) <= Eyelink.Trials.time(2,ixTr);
            IsOfType    = (Eyelink.Events.(EventList{ixEv}).type == EventTypeInt);
            IsValid     = Eyelink.Events.(EventList{ixEv}).keep;
            fprintf('\tEvents %s contains %i Events (%i valid) of type %s\n',EventList{ixEv},length(find( IsInTrial & IsOfType)),length(find( IsInTrial & IsOfType & IsValid)),EventType);
        end
    end
    pause   
end
