function LinParam = GazeEEG_matchTriggers(varargin)

% This function aligns the requested triggers of two datasets
%
% function GazeEEG_matchTriggers(varargin)
%
% General Info
% ------------
% This file searches for corresponding triggers in the Gaze and EEG based
% on the assumption that the test phase and learning phase triggers are
% informative for the linear regressor
%
% By making it a function we reduce the variable overload in the 'base'
% workspace. Indices of the different trials can be found in the basis
% workspace of matlab in the structure "Trials"
%
% the structure "Trials" has entries
% ixBTStart     : the start of the valid trials on Brainamp
% ixBTEnd       : the end of the valid trials on Brainamp
% ixETStart     : the start of the valid trials on Eyelink
% ixETEnd       : the end of the valid trials on Eyelink
%
% This info can also be found in the fields Brainamp.Trials and
% Eyelink.Trials which are added to the respective datastructures assumed
% available in your workspace
%
% Two working modes are available
% -------------------------------
% 'StartStop'   : triggers define the start and end of an epoch
%                   the optional arguments are
%                   * 'TStart'  : trigger used to delimit start of epochs 
%                   * 'TEnd'    : trigger used to delimit end of epochs
% 'SingleSync'  : triggers define the time instant to sync about with a
%               window going from t(TSync)-tmin to t(TSync)+tplus
%                   the optional arguments are
%                   * 'TSync'   : trigger to use as a reference in the
%                               epochs
%                   * 'tmin'    : time before trigger (in ms)
%                   * 'tplus'   : time after trigger (in ms)
%
% In the second 
%
% Visual Feedback & Default Arguments
% -----------------------------------
% The function argument VFB is used for visual feedback of the regression
% errors and takes a logical (0/none) or [1/text] or (2/text+fig). 
% Command line feedback will not be suppressed if user interaction is
% required to disambiguate trigger multiplicities. The working mode
% defaults to 'StartStop' with the following triggers:
%   Test    = 120
%   Learn   = 130
%   Start   = 10
%   End     = 16
% and the feedback defaults to text [1]
%
% Example of function use
% -----------------------
% EpochSyncGazeEEG('feedback',1,'Test',130,'Learn',120,'TStart',30,'TEnd',90);
%
%   'feedback',1    --> textual feedback
%   'Test',130      --> trigger for start test phase is 130
%   'Learn',120     --> trigger for start learn phase is 120
%   'TStart',30     --> the trigger that marks the start of an epoch
%   'TEnd',90       --> the trigger that marks the end of an epoch
%   no mode specified => defaults to 'StartStop'
%
% EpochSyncGazeEEG('feedback',0,'Mode','Sync','Test',130,'Learn',120,'TSync',30,'tmin',500,'tplus',1500); 
%
%   'feedback',0    --> no visual feedback
%   'Mode','Sync'   --> run in Sync mode, i.e. window about a trigger
%   'Test',130      --> trigger for start test phase is 130
%   'Learn',120     --> trigger for start learn phase is 120
%   'TSync',30      --> the trigger that marks the start of an epoch
%   'tmin',500      --> epoch starts 500ms before time of sync trigger
%   'tplus',1500    --> epoch ends 1500ms after time of sync trigger

% History:
% --------
% *** 2011-06-30 R. Phlypo @ GIPSA Lab: Created function

global Brainamp;
global Eyelink;

% defaultvalues
VFB = 0;
SyncMode = 'StartStop';

%process arguments
for k = 1:2:length(varargin)
    
    if strcmpi(varargin{k},'feedback')
        VFB = varargin{k+1};
    elseif strcmpi(varargin{k},'Test')
        trTest = varargin{k+1};
        if ischar( trTest)
            trTest = GazeEEG_getEventInt( trTest);
                   %disp(['trTest:' trTest]);
        end
 
    elseif strcmpi(varargin{k},'Learn')
        trLearn = varargin{k+1};
        if ischar( trLearn)
            trLearn = GazeEEG_getEventInt( trLearn);
        end
    elseif strcmpi(varargin{k},'TStart')
        trTStart = varargin{k+1};
        if (ischar( trTStart) || iscell( trTStart))
            trTStart = GazeEEG_getEventInt( trTStart);
            %disp(['trTStart:' trTStart]);
        end
    elseif strcmpi(varargin{k},'TEnd')
        trTEnd = varargin{k+1};
        if (ischar( trTEnd) || iscell( trTEnd))
            trTEnd = GazeEEG_getEventInt( trTEnd);
            %disp(['trTEnd:' trTEnd]);
        end
        
    elseif strcmpi(varargin{k},'Mode')
        SyncMode = varargin{k+1};
    elseif strcmpi(varargin{k},'TSync')
        trTSync = varargin{k+1};
        if (ischar( trTSync) || iscell( trTSync))
            trTSync = GazeEEG_getEventInt( trTSync);
        end
    elseif strcmpi(varargin{k},'tmin')
        TMin = varargin{k+1};
    elseif strcmpi(varargin{k},'tplus')
        TPlus = varargin{k+1};
    else
        error('GazeEEG:unsupportedInput','''%s'' is not a valid option, please verify the spelling.',varargin{k})
    end
end

% This are inline functions for later use only
isPosInteger = @(x) (length(x)==1)*(mod(x,1)==0)*(x>0);
MultiIntersect = @(x,y) find( any( ones(numel(y),1)*x(:)' == y(:)*ones(1,numel(x)),1 ));

% Find 'test' and 'learn' triggers index
% Begin Test
ixBT = double( Brainamp.Events.Triggers.time( 1, MultiIntersect( Brainamp.Events.Triggers.type.', trTest)));
ixET = double( Eyelink.Events.Triggers.time( 1, MultiIntersect( Eyelink.Events.Triggers.type.', trTest)));
%Begin Learn
ixBL = double( Brainamp.Events.Triggers.time( 1, MultiIntersect( Brainamp.Events.Triggers.type.', trLearn)));
ixEL = double( Eyelink.Events.Triggers.time( 1, MultiIntersect( Eyelink.Events.Triggers.type.', trLearn)));

if strcmpi(SyncMode,'StartStop')
    disp('Mode is StartStop');
    % Build vectors of indexes for each trigger
    ixB_Start = []; %Anton: B for BrainAmp Start
    ixE_Start = []; %E for eylink
    ixB_End = []; %B for BrainAmp end
    ixE_End= [];
    errB = true;
    errE = true;
    
    %Anton: process only trTStart
    %Anton: process both BrainAmp and Eyelink
    for k = 1:length( trTStart)
        tmp = double( Brainamp.Events.Triggers.time( 1, ...
            MultiIntersect( Brainamp.Events.Triggers.type.', trTStart(k))));
        
        if isempty(tmp), fprintf('No trigger of type TStart (%i) found in Brainamp\n',trTStart(k));
        else ixB_Start = [ixB_Start tmp]; errB = false; end %anton: adds all start postions
        
        tmp = double( Eyelink.Events.Triggers.time( 1, ...
            MultiIntersect( Eyelink.Events.Triggers.type.', trTStart(k) )));
        
        if isempty(tmp), fprintf('No trigger of type TStart (%i) found in Eyelink\n',trTStart(k)); 
        else ixE_Start = [ixE_Start tmp]; errE = false; end
    end
    
    %Anton: process only trTEnd
    %Anton: process both BrainAmp and Eyelink
    for k = 1:length( trTEnd)
        tmp   = double( Brainamp.Events.Triggers.time( 1, ...
            MultiIntersect( Brainamp.Events.Triggers.type.', trTEnd(k) )));
        if isempty(tmp), fprintf('No trigger of type TEnd (%i) found in Brainamp\n',trTEnd(k));
        else ixB_End = [ixB_End tmp]; errB = false; end

        tmp   = double( Eyelink.Events.Triggers.time( 1, ...
            MultiIntersect( Eyelink.Events.Triggers.type.', trTEnd(k) )));
        if isempty(tmp), fprintf('No trigger of type TEnd (%i) found in Eyelink\n',trTEnd(k)); 
        else ixE_End = [ixE_End tmp]; errE = false; end %anton: adds all end postions
    end
    
    ixB_Start = sort( ixB_Start, 'ascend'); %we have all the start from BrainAmp
    ixB_Start % print to see
    ixE_Start = sort( ixE_Start, 'ascend'); 
    ixB_End = sort( ixB_End, 'ascend'); %we have all the end from BrainAmp
    ixE_End = sort( ixE_End, 'ascend');
    
    if errB || errE, error('GazeEEG:matchTriggers',...
            'At least one of the trigger types is missing in either one of the modalities.'); end
    
elseif strcmpi(SyncMode,'Sync')
    for k = 1:length( trTSync)
        tmp = double( Brainamp.Events.Triggers.time( 1, ...
            MultiIntersect( Brainamp.Events.Triggers.type.', trTSync(k))));
        ixB_Sync = [ixB_Sync tmp];
        
        tmp = double( Eyelink.Events.Triggers.time( 1, ...
            MultiIntersect( Eyelink.Events.Triggers.type.', trTSync(k))));
        ixE_Sync = [ixE_Sync tmp];
    end
    ixB_Sync = sort( ixB_Sync, 'ascend');
    ixE_Sync = sort( ixE_Sync, 'ascend');
    ixB_Start   = round(ixB_Sync - TMin*Brainamp.Data.Params.samplingRate/1000);
    ixB_End     = round(ixB_Sync + TPlus*Brainamp.Data.Params.samplingRate/1000);
    ixE_Start	= round(ixE_Sync - TMin*Eyelink.Data.Params.samplingRate/1000);
    ixE_End     = round(ixE_Sync + TPlus*Eyelink.Data.Params.samplingRate/1000);
else
    error('GazeEEG:unsupportedOption','Unknown Sync mode')
end

% Ask for user feedback if triggers are absent or doubled
if VFB >0
    fprintf('\nThe following values have been detected in the files:\n')
    fprintf('[B: Brainamp, E: Eyelink, T: start testing phase, L: start learning phase]\n')
    fprintf('\nBT (%i):\t',trTest), fprintf('%i\t\t',ixBT)
    fprintf('\nET (%i):\t',trTest), fprintf('%i\t\t',ixET)
    fprintf('\nBL (%i):\t',trLearn), fprintf('%i\t\t',ixBL)
    fprintf('\nEL (%i):\t',trLearn), fprintf('%i\t\t',ixEL)
    try fprintf('\nEL - ET  and BL - BT :\t'), fprintf('%i\t %i\t', ixEL-ixET, ixBL-ixBT); end
    fprintf('\n')
end

helpPhrase1 = 'HELP: The choice of the indices should be such that the equality ET - BT = EL - BL approximately holds.';
helpPhrase2 = 'The index is the column in which the value appears in the above summary.';

if length(ixBL)>1 || length(ixEL)>1 || length(ixBT)>1 || length(ixET)>1
        if length(ixBL)>1,
            ix  = -1;
            while ix < 0
                ix = input('Which entry of BL should be retained (index)? [Type ''help'' for support] ');
                if strcmpi(ix,'help')
                    ix = -1;
                    fprintf('\n\n%s\n%s\n\n',helpPhrase1,helpPhrase2)
                elseif isPosInteger(ix) && ix <= length(ixBL),
                    ixBL = ixBL(ix);
                else
                    ix = -1;
                    fprintf('\n \t*** The value should be a valid index! ***\n\n')
                end
            end
        end
        if length(ixEL)>1, 
            ix  = -1;
            while ix < 0
                ix = input('Which entry of EL should be retained (index)? [Type ''help'' for support] ');
                if strcmpi(ix,'help')
                    ix = -1;
                    fprintf('\n\n%s\n%s\n\n',helpPhrase1,helpPhrase2)
                elseif isPosInteger(ix) && ix <= length(ixEL)
                    ixEL = ixEL(ix); 
                else
                    ix = -1; 
                    fprintf('\n \t*** The value should be a valid index! ***\n\n')
                end
            end
        end
        if length(ixBT)>1, 
            ix  = -1;
            while ix < 0
                ix = input('Which entry of BT should be retained (index)? [Type ''help'' for support] ');
                if strcmpi(ix,'help')
                    ix = -1;
                    fprintf('\n\n%s\n%s\n\n',helpPhrase1,helpPhrase2)
                elseif isPosInteger(ix) && ix <= length(ixBT), 
                    ixBT = ixBT(ix);
                else
                    ix = -1; 
                    fprintf('\n \t*** The value should be a valid index! ***\n\n')
                end
            end
        end
        if length(ixET)>1, 
            ix  = -1;
            while ix < 0
                ix = input('Which entry of ET should be retained (index)? [Type ''help'' for support] ');
                if strcmpi(ix,'help')
                    ix = -1;
                    fprintf('\n\n%s\n%s\n\n',helpPhrase1,helpPhrase2)
                elseif isPosInteger(ix) && ix <= length(ixET),
                    ixET = ixET(ix);
                else
                    ix = -1; 
                    fprintf('\n \t*** The value should be a valid index! ***\n\n')
                end
            end
        end
end
        
% ------------------ linear Regression approximation (drift)
% t_Brainamp = a*t_Eyelink + b
% Default : a = 1
if length(ixBL)==1 && length(ixEL)==1 && length(ixBT)==1 && length(ixET)==1
    LinParam.a = (ixBL - ixBT)/(ixEL - ixET);
    LinParam.b = (ixBL+ixBT)/2 - LinParam.a*(ixEL+ixET)/2;
else
    LinParam.a = 1;
    if length(ixBT)==1 && length(ixET)==1
        LinParam.b = ixBT - LinParam.a*ixET;
    elseif length(ixBL)==1 && length(ixEL)==1
        LinParam.b = ixBL - LinParam.a*ixEL;
    end
end
   

% Matrix entries: Eyelink (-> Brainamp Regressor) x Brainamp
%    matrix of distances between brainamp and rematched eyelink start times
DM = ((LinParam.a*ixE_Start + LinParam.b)'*ones(1,length(ixB_Start)) - ones(length(ixE_Start),1)*ixB_Start)';
% Connect those that are inferior in number to the others
transp = false;
if size(DM,2)>size(DM,1), DM = DM';
    transp = true;
end

% Find the minimum distances between the regressor and the observed values
% return values :
%    column minimum value
%    column minimum index (in the column)
[AbsOffset, ixOffset] = min(abs(DM));

% Visual Feedback
%   Offset contains the minimum distances between starts
%   more precisely, Offset(1) is the smallest distance between the first brainamp start and
%   an eyelink trial start, hopefully the first eyelink trial start :)
Offset = DM( sub2ind( size(DM), ixOffset, 1:size(DM,2) ) );
if VFB == 2
    figure, 
    plot( Offset), 
    ylabel('Disagreement Linear Model (in samples)');
    s = 'Correct Trial Index ';
    if transp, s = [s 'Eyelink'];
    else s = [s 'Brainamp']; end
    xlabel(s)
end


if ~transp
    ixBrainamp = ixB_Start(ixOffset); % Get indexes of brainamp starts for which we found an eyelink start
    ixEyelink  = ixE_Start(1:size(DM,2)); % Get all the eyelink starts indexes
else
    ixBrainamp = ixB_Start(1:size(DM,2)); % Get all the brainamp starts indexes
    ixEyelink  = ixE_Start(ixOffset); % Get indexes of eyelink starts for which we found a brainamp start
end


% Find end indexes corresponding to each start
%   assumes that 'start' and 'end' are well ordered (no overlap of trials)
ixBTStart = ixBrainamp;
temp = ixB_End'*ones(1,length(ixBTStart)) - ones(length(ixB_End),1)*ixBTStart;
temp(temp<=0) = NaN;
[temp,ixtemp] = min(temp);
ixBTEnd = ixB_End(ixtemp);

ixETStart  = ixEyelink;
temp = ixE_End'*ones(1,length(ixETStart)) - ones(length(ixE_End),1)*ixETStart;
temp(temp<=0) = NaN;
[temp,ixtemp] = min(temp);
ixETEnd = ixE_End(ixtemp);

% 5 sample difference tolerance in the length of a trial, otherwise the
% trial is rejected from any further analysis
ix = find(abs(diff([ixBTEnd' - ixBTStart' ...
    Brainamp.Data.Params.samplingRate/Eyelink.Data.Params.samplingRate*(ixETEnd' - ixETStart')],[],2))<=5);
% ixBrainamp = ixBrainamp(ix);
ixBTStart = ixBTStart(ix);
ixBTEnd = ixBTEnd(ix);
% ixEyelink = ixEyelink(ix);
ixETStart  = ixETStart(ix);
ixETEnd = ixETEnd(ix);

% This will be suppressed in future versions to group these variables under
% the structures Brainamp and Eyelink

%Anton: here the result is saved 
Brainamp.Trials.time = [ixBTStart; ixBTEnd];
Eyelink.Trials.time = [ixETStart; ixETEnd];
Brainamp.Trials.keep = true(1,size( Brainamp.Trials.time, 2));
Eyelink.Trials.keep = true(1,size( Eyelink.Trials.time, 2));

NbTrials = length(ix);
Brainamp.Data.Params.nbTrials = NbTrials;
Eyelink.Data.Params.nbTrials = NbTrials;

%Print result
if VFB > 0
    s = sprintf('A total of %i valid trials have been detected in the currently selected files', NbTrials);
    L = length(s);
    fprintf(['\n' repmat('%c',1,L+6) '\n'],repmat('*',1,L+6))
    fprintf(['*  ' s '  *\n']);
%     fprintf(['\n' repmat('%c',1,L+6) '\n'],repmat('*',1,L+6))
end

%% Validation of triggers
% Only Events during the learning phase are of interest to the experimenter
% Identify Events occurring within trials AND during learning phase

% First Trial is the first trial with starting date posterior to the start
% of the learning phase
ixFirstTrial = find( Eyelink.Trials.time(1,:) >= max(ixEL,ixET), 1, 'first'); 
ixFirstTrial = max( ixFirstTrial, find( Brainamp.Trials.time(1,:) >= max(ixBL,ixBT), 1, 'first'));

Eyelink.Trials.keep(1:ixFirstTrial-1) = false;
Brainamp.Trials.keep(1:ixFirstTrial-1) = false;

% Construct List of all "Events" fields (excluding EventTypes !)
EventList = setdiff( fieldnames( Eyelink.Events), 'EventTypes');
% The .keep field of the Event guarantees that this event can be used as a
% synchronisation point for GazeEEG_buildEpoch (if set logically true)
% => Put all events with end date posterior to trial start date and start
% date anterior to trial end date to be kept (.keep = true)
for ix = 1:length( EventList)
    if isfield( Eyelink.Events, EventList{ ix})
        Eyelink.Events.( EventList{ ix}).keep   = false(1, length( Eyelink.Events.( EventList{ ix}).time));
        Eyelink.Events.( EventList{ ix}).valid   = true(1, length( Eyelink.Events.( EventList{ ix}).time));
    end
    if isfield( Brainamp.Events, EventList{ ix})
        Brainamp.Events.( EventList{ ix}).keep  = false(1, length( Brainamp.Events.( EventList{ ix}).time));
        Brainamp.Events.( EventList{ ix}).valid  = true(1, length( Brainamp.Events.( EventList{ ix}).time));
    end
    for TrialIx = find( Eyelink.Trials.keep)
        if isfield( Eyelink.Events, EventList{ ix})
            Eyelink.Events.( EventList{ ix}).keep( ...
                Eyelink.Events.( EventList{ ix}).time(1,:) >= Eyelink.Trials.time(1,TrialIx) & ...
                Eyelink.Events.( EventList{ ix}).time(1,:) <= Eyelink.Trials.time(2,TrialIx)) = true;
        end
        if isfield( Brainamp.Events, EventList{ ix})
            Brainamp.Events.( EventList{ ix}).keep( ...
                Brainamp.Events.( EventList{ ix}).time(1,:) >= Brainamp.Trials.time(1,TrialIx) & ...
                Brainamp.Events.( EventList{ ix}).time(1,:) <= Brainamp.Trials.time(2,TrialIx)) = true;
        end
    end
end

if VFB > 0
    s = sprintf('%i trials have been detected as testing trials', NbTrials - ixFirstTrial + 1);
    L2 = length(s);
    HalfL = (L - L2)/2;
%     fprintf(['\n' repmat('%c',1,L+6) '\n'],repmat('*',1,L+6))
    fprintf(['*  ' repmat(' ',1,floor(HalfL))  s repmat(' ',1,ceil(HalfL)) '  *\n']);
    fprintf([repmat('%c',1,L+6) '\n'],repmat('*',1,L+6))
end

%% Experimental section
TriggerTypes = unique(sort([Eyelink.Events.Triggers.type Brainamp.Events.Triggers.type]));

if length( unique( Brainamp.Events.Triggers.type)) == 1
    warning('GazeEEG:noEventManagement','Full visual feedback is only possible when <a href=''matlab: doc GazeEEG_manageEvents''>GazeEEG_manageEvents()</a> has been run first.');
    return
end

% If no match between triggers on Eyelink and Brainamp, they should neither
% be used for synchronisation nor for display purposes
for TT = TriggerTypes
    % Find indices of trigger type TT
    IxE = find( Eyelink.Events.Triggers.type==TT & Eyelink.Events.Triggers.valid);
    IxB = find( Brainamp.Events.Triggers.type==TT & Brainamp.Events.Triggers.valid);
    % get corresponding sampletimes
    tEtmp = double( Eyelink.Events.Triggers.time(1,IxE))';
    tBtmp = double( Brainamp.Events.Triggers.time(1,IxB))';
    
    LIxE = length( IxE);
    LIxB = length(IxB);
    
    if LIxB > 0
        % Look for invalid triggers in Eyelink (present in Eyelink, not in Brainamp)
        [minabs,ix] = min(abs( tBtmp*ones(1,length(tEtmp)) - LinParam.b - LinParam.a*ones(length(tBtmp),1)*tEtmp'));
        % more than a hundred samples difference w.r.t. model means there is no
        % matching trigger found! (bound may be taken large, since rarely twice
        % the same trigger is observed in one second of data)
        InValidMatchIx = find( minabs > 100);
%         Eyelink.Events.Triggers.keep( IxE( InValidMatchIx)) = false;
        Eyelink.Events.Triggers.valid( IxE( InValidMatchIx)) = false;
    else
        InValidMatchIx = IxE;
%         Eyelink.Events.Triggers.keep(IxE) = false;
        Eyelink.Events.Triggers.valid(IxE) = false;
    end
    
    LCorrectIxE = LIxE - length(InValidMatchIx);
    
    if LIxE > 0
        % Look for invalid triggers in Eyelink (present in Brainamp, not in Eyelink)
        [minabs,ix] = min( abs(tBtmp*ones(1,length(tEtmp)) - LinParam.b - LinParam.a*ones(length(tBtmp),1)*tEtmp'),[],2);
        % more than a hundred samples difference w.r.t. model means there is no
        % matching trigger found!
        InValidMatchIx = find( minabs > 100);
%         Brainamp.Events.Triggers.keep( IxB( InValidMatchIx)) = false;
        Brainamp.Events.Triggers.valid( IxB( InValidMatchIx)) = false;
    else
        InValidMatchIx = IxB;
%         Brainamp.Events.Triggers.keep(IxB) = false;
        Brainamp.Events.Triggers.valid(IxB) = false;
    end
    LCorrectIxB = LIxB - length(InValidMatchIx);
    
    EvTypes = fieldnames( Eyelink.Events.EventTypes);
    for ix = 1:length(EvTypes)
        if Eyelink.Events.EventTypes.( EvTypes{ ix}) == TT
            TriggerName = EvTypes{ ix};
        end
    end
    
    if VFB > 0
        fprintf('%s\t: Eyelink %i/%i,\tBrainamp %i/%i\n',TriggerName,LCorrectIxE,LIxE,LCorrectIxB,LIxB);
    end
end

if VFB > 0
    fprintf('%i/%i triggers have been validated for Brainamp.\n', length(find(Brainamp.Events.Triggers.keep)), length(Brainamp.Events.Triggers.keep));
    fprintf('%i/%i triggers have been validated for Eyelink.\n', length(find(Eyelink.Events.Triggers.keep)), length(Eyelink.Events.Triggers.keep));
end

