function GazeEEG_rankEvent(varargin)

global Eyelink

% default values
feedback = false;
eventsToRankStr = [];
keepRankInTrial = '1';
nanMax = 2;

% input args
for k = 1:2:length(varargin)
    if strcmpi(varargin{k},'feedback')
        feedback = logical( varargin{k+1});
    elseif strcmpi(varargin{k},'EventType')
        eventsToRankStr = varargin{k+1};
    elseif strcmpi(varargin{k},'rank')
        if ~iscell( varargin{k+1}) % for numerical array input
            if ~ischar( varargin{k+1})
                RankStr = num2str( varargin{k+1}(:), '%i');
                keepRankInTrial = mat2cell( RankStr, ones(length(RankStr),1));
            else
                keepRankInTrial = {varargin{k+1}};
            end
        else % for character inputs, allows for expressions like 'end-2'
            keepRankInTrial = varargin{k+1};
        end
    else
        error(sprintf('''%s'' is not a valid option, please verify the spelling.',varargin{k}))
    end
end

% --------------- STRING TO INTEGER CODING OF EVENTS ------------
% eventsToRank will contain Integer values corresponding to the names in
% eventsToRankStr

if ~isempty( eventsToRankStr) && length( eventsToRankStr) > 1
    for ix = 1:length( eventsToRankStr)
        GazeEEG_rankEvent( 'EventType', eventsToRankStr{ix} , 'rank', keepRankInTrial);
    end
end

EventTypes = fieldnames(Eyelink.Events.EventTypes); % cell of strings
for ix = 1:length(EventTypes)
    % Events to rank
    if strcmpi(EventTypes{ix},eventsToRankStr)
        eventsToRank = Eyelink.Events.EventTypes.(EventTypes{ix});
    end
end

fprintf('\nEvent Filtering\n---------------\n');


% ------------------- FILTER EVENTS -----------------------
% parses each event field chronologically and filter them according to
% their type and to the constraints given in parameters

% for each event type
EventList = setdiff( fieldnames(Eyelink.Events), 'EventTypes');
foundEvent = false;
for EvIx = 1:length(EventList) % For all Eyelink.Events 
    for currentTrial = find( Eyelink.Trials.keep) % for all valid Eyelink.Trials
    
        % Event in Trial & Event has not been "deleted"
        
        IsInTrial   = Eyelink.Events.( EventList{EvIx}).time(1,:) >= Eyelink.Trials.time( 1, currentTrial) & ...
            Eyelink.Events.( EventList{EvIx}).time(1,:) <= Eyelink.Trials.time( 2, currentTrial);
        IsOfType    = (Eyelink.Events.( EventList{EvIx}).type == eventsToRank);
        IsValid     = Eyelink.Events.( EventList{EvIx}).keep;
        
        ixEventsOfTypeToRank = find( IsInTrial & IsOfType & IsValid);
            
        if ~isempty(ixEventsOfTypeToRank)%ismember( eventsToRank, Eyelink.Events.( EventList{EvIx}).type( TrialEventList) )
            % the event has been found in the current Event field         
                        
            ixEventsOfRank = [];
            for ix = 1:length( keepRankInTrial)
                try
                    ixEventsOfRank = [ixEventsOfRank eval(['ixEventsOfTypeToRank('  keepRankInTrial{ ix} ')'])];
                end
            end
            
            ixEventsToKeep      = ixEventsOfRank;
            ixEventsToSuppress  = setdiff( ixEventsOfTypeToRank, ixEventsOfRank);
            % The Event can no longer be used for synchronisation purposes
            % Displaying of the Event is not affected (.valid)
            Eyelink.Events.( EventList{EvIx}).keep( ixEventsToSuppress) = false; 
            Eyelink.Events.( EventList{EvIx}).keep( ixEventsToKeep)     = true; 
            if feedback
                fprintf('Trial %i: kept %i/%i Events of type ''%s''.\n', ...
                    currentTrial, length(ixEventsToKeep), length(ixEventsOfTypeToRank), eventsToRankStr{1});
            end
        end
    end
%     if foundEvent, break; end
end