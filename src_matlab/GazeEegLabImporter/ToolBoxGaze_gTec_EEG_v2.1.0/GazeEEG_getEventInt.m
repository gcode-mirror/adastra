function EventInt = GazeEEG_getEventInt( Events)

global Eyelink;

if ischar( Events)
    EventInt = double( Eyelink.Events.EventTypes.(Events));
elseif iscell( Events)
    EventInt = zeros(length(Events),1);
    for ix = 1:length(Events)
        % Events to rank
        EventInt(ix) = double( Eyelink.Events.EventTypes.(Events{ix}));
    end
end

