function NewEpochs = GazeEegLabImporter_SelectEpochs(varargin)

%%init
global EEG;
MaxEpochsToProcess = -1;
StartFromTrigger = -1;

for k = 1:2:nargin
    
    if strcmpi(varargin{k},'EpochEvents')
        EpochEvents = varargin{k+1};
    elseif strcmpi(varargin{k},'TimeInterval')
        TimeInterval = varargin{k+1};
    elseif strcmpi(varargin{k},'MaxEpochsToProcess') %Usually 
        MaxEpochsToProcess = varargin{k+1};
    elseif strcmpi(varargin{k},'StartFromTrigger') 
        StartFromTrigger = varargin{k+1};
    else 
        error('Error in arguments!')
    end
end

global EegAcq;

NbChB = EegAcq.Data.Params.nbOfChannels;

%% process

%find actual data start 
disp('startFromTriggerPosition');
startFromTriggerPosition = cell2mat( arrayfun(@(x) find(EegAcq.Events.Triggers.value==x), StartFromTrigger, 'UniformOutput', false) )

%newEpochsPositions = find (EegAcq.Events.Triggers.value == EegAcq.Events.EventTypes.flowSaccadeLeft,max);

%finds requested triggers
newEpochsPositions = cell2mat( arrayfun(@(x) find(EegAcq.Events.Triggers.value==x), EpochEvents, 'UniformOutput', false));

%remove all triggers before the start trigger
if (StartFromTrigger ~= -1 && length(startFromTriggerPosition) > 0 )
   newEpochsPositions = newEpochsPositions(find ( newEpochsPositions > startFromTriggerPosition));
end;

NbNewEpochs = length(newEpochsPositions);

if MaxEpochsToProcess ~= -1 && MaxEpochsToProcess < NbNewEpochs
    NbNewEpochs = MaxEpochsToProcess;
end;

NewEpochs = zeros(4,NbNewEpochs);% start,end, type

%% Extract from raw file and apply filter
for k = 1:NbNewEpochs
    
    triggerTimeValues  = EegAcq.Events.Triggers.time(:, newEpochsPositions(k));% gets the two values that positions the trigger - both are usually identical
    
    % calculate the interval in ms and then
    epochStartS = double(triggerTimeValues(1) + TimeInterval(1));%in ms
    epochEndS =   double(triggerTimeValues(1) + TimeInterval(2));%in ms
    
    %calculate indexes for file read: 
    %position = time * rate * NbChB(because 1 sample equals all the channels)
    %indexStart = epochStartS / 1000 * double(EegAcq.Data.Params.samplingRate) * NbChB;
    %indexStart / NbChB
    %indexEnd   = epochEndS / 1000 * double(EegAcq.Data.Params.samplingRate) * NbChB;
    
    %append: start,end,type
    NewEpochs(:,k) = [epochStartS;epochEndS;double(EegAcq.Events.Triggers.value(newEpochsPositions(k)));double(newEpochsPositions(k));];
end;

disp(['NbNewEpochs to be constructed: ' num2str(NbNewEpochs)]);

