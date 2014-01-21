function GazeEEG_filterValidTrials()

% function GazeEEG_filterValidTrials()
%
% for all trials that are not valid, put the .keep field of their events to
% false

global Eyelink
global Brainamp

% Construct List of all "Events" fields (excluding EventTypes !)
EventList = setdiff( fieldnames( Eyelink.Events), 'EventTypes');
% The .keep field of the Event guarantees that this event can be used as a
% synchronisation point for GazeEEG_buildEpoch (if set logically true)
% => Put all events with end date posterior to trial start date and start
% date anterior to trial end date to be kept (.keep = true)

for TrialIx = find( ~Eyelink.Trials.keep)
    for ix = 1:length( EventList)
        if isfield( Eyelink.Events, EventList{ ix})
            Eyelink.Events.( EventList{ ix}).keep( ...
                Eyelink.Events.( EventList{ ix}).time(1,:) >= Eyelink.Trials.time(1,TrialIx) & ...
                Eyelink.Events.( EventList{ ix}).time(1,:) <= Eyelink.Trials.time(2,TrialIx)) = false;
        end
        if isfield( Brainamp.Events, EventList{ ix})
            Brainamp.Events.( EventList{ ix}).keep( ...
                Brainamp.Events.( EventList{ ix}).time(1,:) >= Brainamp.Trials.time(1,TrialIx) & ...
                Brainamp.Events.( EventList{ ix}).time(1,:) <= Brainamp.Trials.time(2,TrialIx)) = false;
        end
    end
%     fprintf('Events of Trial %i can no longer be used for synchronisation.\n', TrialIx);
end