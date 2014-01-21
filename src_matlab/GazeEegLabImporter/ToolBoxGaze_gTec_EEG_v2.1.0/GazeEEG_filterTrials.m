function varargout = GazeEEG_filterTrials(varargin)

% function GazeEEG_filterTrials(varargin)
%
% New function call does not have output argument
%       GazeEEG_filterTrials(varargin)
% Obsolete function call (still valid in current version, will be suppressed in future versions):
%   	trialsList = GazeEEG_filterTrials(varargin)
%
% filters trials corresponding to some displayable type
% returns a field in the structure Eyelink corresponding to the retained
%   trials

% initialisations
global Eyelink;
global Brainamp;
% default values
feedback            = 0;    % 0: no feedback, 1: command line feedback, 2: graphical feedback
displayable         = '';

fprintf('\nFilter Trials\n-------------\n');

for k = 1:2:length(varargin)
    if strcmpi(varargin{k},'feedback')
        feedback = varargin{k+1};
    elseif strcmpi(varargin{k},'displayable')
        displayable = varargin{k+1}
    else
        error(sprintf('''%s'' is not a valid option, please verify the spelling.',varargin{k}))
    end
end

trialsList = [];
strIndices = [];
for k = 1:length(Eyelink.Events.Displayables.name)
    for j = 1:length(displayable)
        strIndice = strfind(Eyelink.Events.Displayables.name(k,:), displayable{j});
        if ~isempty(strIndice) % found match
            %strIndices = [strIndices k];
            trialIndice = find(Eyelink.Trials.time(1,:)-10 <= Eyelink.Events.Displayables.time(1, k) & ...
                Eyelink.Trials.time(2,:)+10 >= Eyelink.Events.Displayables.time(1, k) & ...
                Eyelink.Trials.keep );
            if ~isempty( trialIndice)
                trialsList = [trialsList trialIndice];
                strIndices = [strIndices k];
                if ( feedback > 0 )
                    fprintf('trial %i : %s \n', trialIndice, Eyelink.Events.Displayables.name(k,:));
                end  
            end
        end
    end
end

trialsList = unique(trialsList);
Eyelink.Trials.keep( setdiff( find( Eyelink.Trials.keep), trialsList)) = false;
Brainamp.Trials.keep( setdiff( find( Eyelink.Trials.keep), trialsList)) = false;

% Unvalidated trials should have all events invalid
GazeEEG_filterValidTrials();

if ( feedback > 1 )
    figure('Name', 'Displayables and trials', 'NumberTitle', 'off');
    hold on;
    trialStarts = find( Eyelink.Trials.keep);
    hg = plot(Eyelink.Trials.time(1,trialStarts), trialStarts, 'g.');
    hr = plot(Eyelink.Trials.time(2,trialStarts), trialStarts, 'r.');
    X = 200*ones(1, length(strIndices));
    hb = plot(Eyelink.Events.Displayables.time(1,strIndices), X, 'b.');
    hold off;
    xlabel('time');
    ylabel('event number or/ trial rank');
end

if length( displayable) > 1
    output = '';
    for j=1:length(displayable)-1
        output = [output '''' displayable{j} ''', '];
    end
    output = [output 'or ''' displayable{end} ''''];
else
    output = ['''' displayable{end} ''''];
end
fprintf('\n%i displayable(s) containing %s\n', length(strIndices), output);
fprintf('%i corresponding trial(s) \n\n', length(find(Eyelink.Trials.keep)));

if nargout > 0
    warning('ToolboxGazeEEG:IncompatibleOutput',...
        ['The output argument ''trialsList'' will no longer be supported from this version on.\n'... 
        'Please update your function calls and no longer input trialsList as an argument.']);
    varargout{1} = trialsList;
end