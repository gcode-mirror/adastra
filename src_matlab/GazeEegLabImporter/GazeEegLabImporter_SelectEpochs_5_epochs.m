function NewEpochs = GazeEegLabImporter_SelectEpochs_5_epochs(varargin)

%%init
global EEG;
MaxEpochsToProcess = -1;

for k = 1:2:nargin
    
    if strcmpi(varargin{k},'EpochEvents')
        EpochEvents = varargin{k+1};
    elseif strcmpi(varargin{k},'TimeInterval')
        TimeInterval = varargin{k+1};
    elseif strcmpi(varargin{k},'MaxEpochsToProcess') %Usually 
        MaxEpochsToProcess = varargin{k+1};
    else 
        error('Error in arguments!')
    end
end

global EegAcq;

NbChB = EegAcq.Data.Params.nbOfChannels;

%% process

%newEpochsPositions = find (EegAcq.Events.Triggers.value == EegAcq.Events.EventTypes.flowSaccadeLeft,max);
newEpochsPositions = cell2mat( arrayfun(@(x) find(EegAcq.Events.Triggers.value==x), EpochEvents, 'UniformOutput', false) );

%leave only 5 of the epochs  

newEpochsPositionsJust5 = ones(1,5);
newEpochsPositionsJust5 = newEpochsPositionsJust5 .* 10000000;

newEpochsPositionsJust5(1) = newEpochsPositions(1);
newEpochsPositionsJust5(5) = newEpochsPositions(end);

% newEpochsPositionsJust5(2) = newEpochsPositions(5);
% newEpochsPositionsJust5(3) = newEpochsPositions(6);
% newEpochsPositionsJust5(4) = newEpochsPositions(12);

endTime = EegAcq.Events.Triggers.time(:, newEpochsPositions(end));
time25percent = endTime(1) / 4;
time75percent = (endTime(1) / 4) * 3;
time50percent = endTime(1) / 2;

disp('Times for the 5 epochs:');
disp(time25percent);
disp(time75percent);
disp(time50percent);

%calculate time25percent
for k = 1:length(newEpochsPositions)
    triggerTimeValues  = EegAcq.Events.Triggers.time(:, newEpochsPositions(k));
    if abs(triggerTimeValues(1) - time25percent) < newEpochsPositionsJust5(2)
        newEpochsPositionsJust5(2) = newEpochsPositions(k);
    end;
end;

%calculate time75percent
for k = 1:length(newEpochsPositions)
    triggerTimeValues  = EegAcq.Events.Triggers.time(:, newEpochsPositions(k));
    if abs(triggerTimeValues(1) - time75percent) < newEpochsPositionsJust5(4)
        newEpochsPositionsJust5(4) = newEpochsPositions(k);
    end;
end;

%calculate time50percent
for k = 1:length(newEpochsPositions)
    triggerTimeValues  = EegAcq.Events.Triggers.time(:, newEpochsPositions(k));
    if abs(triggerTimeValues(1) - time50percent) < newEpochsPositionsJust5(3)
        newEpochsPositionsJust5(3) = newEpochsPositions(k);
    end;
end;

disp('all');
disp(newEpochsPositions);
disp('5epochs');
disp(newEpochsPositionsJust5);

%end of leave five epochs

NbNewEpochs = length(newEpochsPositionsJust5);%must be 5 NbNewEpochs

if MaxEpochsToProcess ~= -1 && MaxEpochsToProcess < NbNewEpochs
    NbNewEpochs = MaxEpochsToProcess;
end;

NewEpochs = zeros(4,NbNewEpochs);% start,end, type

%% Extract from raw file and apply filter
for k = 1:NbNewEpochs
    
    triggerTimeValues  = EegAcq.Events.Triggers.time(:, newEpochsPositionsJust5(k));% gets the two values that positions the trigger - both are usually identical
    
    % calculate the interval in ms and then
    epochStartS = double(triggerTimeValues(1) + TimeInterval(1));%in ms
    epochEndS =   double(triggerTimeValues(1) + TimeInterval(2));%in ms
    
    %append: start,end,type
    NewEpochs(:,k) = [epochStartS;epochEndS;double(EegAcq.Events.Triggers.value(newEpochsPositionsJust5(k)));double(newEpochsPositionsJust5(k));];
end;

disp(['NbNewEpochs to be constructed: ' num2str(NbNewEpochs)]);

