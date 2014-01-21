function GazeEEG_filterNaNTrials(varargin)

global Eyelink

% default values
feedback = false;
nanMax = 2;

% input args
for k = 1:2:length(varargin)
    if strcmpi(varargin{k},'feedback')
        feedback = logical( varargin{k+1});
    elseif strcmpi(varargin{k},'nanMaxRatio')
        nanMax = varargin{k+1};
    else
        error(sprintf('''%s'' is not a valid option, please verify the spelling.',varargin{k}))
    end
end

% --------- MODIFY TRIAL LIST ACCORDING TO NAN RATIO IN TRIAL ----------

% get integer values for the channels corresponding to the names 'eyeAb'
idLx = GazeEEG_fuseChannelLists('Modality','Eyelink','eyeLx');
idLy = GazeEEG_fuseChannelLists('Modality','Eyelink','eyeLy');
idRx = GazeEEG_fuseChannelLists('Modality','Eyelink','eyeRx');
idRy = GazeEEG_fuseChannelLists('Modality','Eyelink','eyeRy');

temp = zeros(size(Eyelink.Trials.keep));
for currentTrial = find( Eyelink.Trials.keep)
    % take the raw data in the trial time window
    if ~isempty(idLx),
        Lx = Eyelink.Data.raw( idLx, Eyelink.Trials.time(1,currentTrial):Eyelink.Trials.time(2,currentTrial) );
    else
        Lx = 0;
    end
    if ~isempty(idLy),
        Ly = Eyelink.Data.raw( idLy, Eyelink.Trials.time(1,currentTrial):Eyelink.Trials.time(2,currentTrial) );
    else
        Ly = 0;
    end
    if ~isempty(idRx),
        Rx = Eyelink.Data.raw( idRx, Eyelink.Trials.time(1,currentTrial):Eyelink.Trials.time(2,currentTrial) );
    else
        Rx = 0;
    end
    if ~isempty(idRy),
        Ry = Eyelink.Data.raw( idRy, Eyelink.Trials.time(1,currentTrial):Eyelink.Trials.time(2,currentTrial) );
    else
        Ry = 0;
    end
    
    % compute the ratio of NaN in this window
    temp(currentTrial) = sum( isnan( Lx + Ly + Rx + Ry ) ) / ...
        ( 4 * Eyelink.Trials.time(2,currentTrial) - Eyelink.Trials.time(1,currentTrial) );
    
end
% remove trials according to their NaN ratio
Eyelink.Trials.keep( temp > nanMax) = false;

GazeEEG_filterValidTrials();

if feedback
    ix = find( temp > nanMax);
    if length(ix)>=1
        for k = 1:length( ix)
            fprintf( 'Trial %i contained a ratio of %.2f NaNs and has been invalidated\n', ix(k), temp(ix(k)));
        end
    else
        fprintf( 'No Trials found with a ratio of NaNs exceeding %.2f\n', nanMax);
    end
end