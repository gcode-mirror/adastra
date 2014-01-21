function GazeEEG_filterFixationsSaccades(varargin)
% initialisations
global Eyelink;
% default values
feedback            = 0;    % 0: no feedback, 1: command line feedback, 2: graphical feedback
% fromTrigger         = '';
minFixationDuration = 0;
minSaccadeDuration  = 0;    % milliseconds
maxSaccadeDuration  = 500;  % milliseconds
minSaccadeSize      = 0;    % degrees
maxSaccadeSize      = 180;  % degrees
screenSizeMM_x      = double(Eyelink.AscHeader.screenDimXmm);
screenSizeMM_y      = double(Eyelink.AscHeader.screenDimYmm);
screenSizePx_x      = double(Eyelink.AscHeader.screenWidth);
screenSizePx_y      = double(Eyelink.AscHeader.screenHeight);
eyeCenterMM_x       = double(Eyelink.AscHeader.screenDimXmm)/2;
eyeCenterMM_y       = double(Eyelink.AscHeader.screenDimYmm)/2;
distEyeScreenMM     = double(Eyelink.AscHeader.screenDistance);


% if ~isfield( Eyelink, 'trialsList')
%     Eyelink.trialsList          = [1:length(Eyelink.Trials)];
% end

for k = 1:2:length(varargin)
    if strcmpi(varargin{k},'feedback')
        feedback = varargin{k+1};
    % obsolete fromTrigger --> see mathTriggers
    elseif strcmpi(varargin{k},'fromTrigger')
        warning('ToolboxGazeEEG:ObsoleteFeature',...
            ['This feature is obsolete and will be suppressed in a future version. ' ...
            '\nRun <a href="matlab: doc GazeEEG_matchTriggers">GazeEEG_matchTriggers</a> first.']);
%         fromTrigger = varargin{k+1};
    elseif strcmpi(varargin{k},'minFixationDuration')
        minFixationDuration = varargin{k+1};
    elseif strcmpi(varargin{k},'minSaccadeDuration')
        minSaccadeDuration = varargin{k+1};
    elseif strcmpi(varargin{k},'maxSaccadeDuration')
        maxSaccadeDuration = varargin{k+1};
    elseif strcmpi(varargin{k},'minSaccadeSize')
        minSaccadeSize = varargin{k+1};
    elseif strcmpi(varargin{k},'maxSaccadeSize')
        maxSaccadeSize = varargin{k+1};
    elseif strcmpi(varargin{k},'screenSizeMM_x')
        screenSizeMM_x = varargin{k+1};
    elseif strcmpi(varargin{k},'screenSizeMM_y')
        screenSizeMM_y = varargin{k+1};
    elseif strcmpi(varargin{k},'screenSizePx_x')
        screenSizePx_x = varargin{k+1};
    elseif strcmpi(varargin{k},'screenSizePx_y')
        screenSizePx_y = varargin{k+1};
    elseif strcmpi(varargin{k},'eyeCenterMM_x')
        eyeCenterMM_x = varargin{k+1};
    elseif strcmpi(varargin{k},'eyeCenterMM_y')
        eyeCenterMM_y = varargin{k+1};
    elseif strcmpi(varargin{k},'distEyeScreenMM')
        distEyeScreenMM = varargin{k+1};
    elseif strcmpi(varargin{k},'trialStart')
        warning('ToolboxGazeEEG:ObsoleteFeature',...
            ['This feature is obsolete and will be suppressed in a future version. ' ...
            '\nRun <a href="matlab: doc GazeEEG_matchTriggers">GazeEEG_matchTriggers</a> first.']);
%         trialStart = varargin{k+1};
    elseif strcmpi(varargin{k},'trialEnd')
        warning('ToolboxGazeEEG:ObsoleteFeature',...
            ['This feature is obsolete and will be suppressed in a future version. ' ...
            '\nRun <a href="matlab: doc GazeEEG_matchTriggers">GazeEEG_matchTriggers</a> first.']);
%         trialEnd = varargin{k+1};
    elseif strcmpi(varargin{k},'trialsList')
        warning('ToolboxGazeEEG:ObsoleteFeature',...
            ['This feature is obsolete and will be suppressed in a future version. ' ...
            '\nRun <a href="matlab: doc GazeEEG_matchTriggers">GazeEEG_matchTriggers</a> first.']);
%         warning('ToolboxGazeEEG:ObsoleteFeature',...
%             ['This feature will be suppressed in a future version. Instead the Eyelink structure has been updated to include a .trialsList field.']);
%         Eyelink.trialsList = varargin{k+1};
    else
        error(sprintf('''%s'' is not a valid option, please verify the spelling.',varargin{k}))
    end
end

% conversions
px2mm_x = screenSizeMM_x/screenSizePx_x;
px2mm_y = screenSizeMM_y/screenSizePx_y;
mm2Px_x = 1/px2mm_x;
mm2Px_y = 1/px2mm_y;
eyeCenterPx_x = mm2Px_x*eyeCenterMM_x;
eyeCenterPx_y = mm2Px_y*eyeCenterMM_y;


eyes = {'left eye', 'right eye'};
xEyes = {'eyeLx' 'eyeRx'};
yEyes = {'eyeLy' 'eyeRy'};
FixationSaccades = {'FixationSaccadesLeft' 'FixationSaccadesRight'};
flowSaccades = {'flowSaccadeLeft' 'flowSaccadeRight'};
flowFixations = {'flowFixationLeft' 'flowFixationRight'};

output = '\nSaccades Filtering\n----------------\n';

% --------------- COMPUTE 'FROM TRIGGER' TIME -----------------
% fromTriggerTime = -1;
% 
% 
% 
% if ( ~strcmp(fromTrigger,'') )
%     idxFromTrigger = find( Eyelink.Events.Triggers.type == Eyelink.Events.EventTypes.(['trigger' num2str(fromTrigger)]) );    
%     if (length(idxFromTrigger) >= 1)
%         if (length(idxFromTrigger) > 1)
%             warning('several fromTrigger exist, taking the first one');
%         end
%         fromTriggerTime = Eyelink.Events.Triggers.time(1,idxFromTrigger(1));
%     end
% end

% for each eye
for eye = 1:2   
    output = [output eyes{eye} ':\n'];
    
    % Check for fieldname FixationSaccadesLeft/-Right
    if isfield(Eyelink.Events,FixationSaccades{eye})
        keptEventsNb = sum( Eyelink.Events.(FixationSaccades{eye}).keep );
    else
        keptEventsNb = 0;
    end
    output = [output num2str(keptEventsNb,'%i') '\t\t(nb of events before filtering)\n'];

    if (keptEventsNb == 0)
        output = [output 'WARNING - no saccade found on ' eyes{eye} '\n'];
        break;
    end

    % ----------- FILTER DUPLICATED EVENTS ------------
    % these events are caused by drift correction

    % Build eventType(k) - eventType(k-1)
    % CORRECTION: Build eventType(k+1) - eventType(k)
    duplicatedEvents = diff(double(Eyelink.Events.(FixationSaccades{eye}).type));
    
    % Find zeros indexes in this array (duplicated events)
    % If the difference at ix is zero, the duplicated event is ix+1
    % ix     : 1   2   3   4   5
    % diffix :   1   2   3   4
    % Difference between 3 and 2 (diffix = 2) is zero 
    % => 3 is a duplicate of 2 
    % => eliminate event at ix = 3
    idxEventsToRemove = find([duplicatedEvents 1] == 0 & Eyelink.Events.(FixationSaccades{eye}).keep)+1;

    if (~isempty(idxEventsToRemove)) 
        % duplicated events should neither be used for synchronisation, nor
        % to display
        Eyelink.Events.(FixationSaccades{eye}).keep(  idxEventsToRemove) = false;     
        Eyelink.Events.(FixationSaccades{eye}).valid( idxEventsToRemove) = false;    
        Eyelink.Events.(FixationSaccades{eye}).time(2,idxEventsToRemove-1) = ...
            Eyelink.Events.(FixationSaccades{eye}).time(2,idxEventsToRemove);
 % debug : show duplicates
%         X = zeros(4, length(Eyelink.Data.raw));
 %        X( 1,Eyelink.Events.(FixationSaccades{eye}).time(1,idxEventsToRemove) ) = Eyelink.Events.(FixationSaccades{eye}).type(idxEventsToRemove);
  %       X( 2,Eyelink.Events.(FixationSaccades{eye}).time(1,idxEventsToRemove+1) ) = Eyelink.Events.(FixationSaccades{eye}).type(idxEventsToRemove+1);
   %      X( 3,Eyelink.Trials(:,1)) = 3;
    %     X( 4,Eyelink.Trials(:,2)) = 2;
%         figure;plot(X','-+'); legend({'1st','2nd','Start Trial','End Trial'})
    end

    output = [output num2str(-length(idxEventsToRemove),'%i') '\t\t(nb of duplicated events)\n'];

%     % -------- CHECK IF SACCADE END TIME + 1 == FIXATION START TIME --------
%     logicKept = Eyelink.Events.(FixationSaccades{eye}).keep;
%     sacEndTime = Eyelink.Events.(FixationSaccades{eye}).time(2, logicKept & Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowSaccades{eye}));
%     fixStartTime = Eyelink.Events.(FixationSaccades{eye}).time(1, logicKept & Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowFixations{eye}));
%     fixStartTime = fixStartTime(2:end); % first fixation has no saccade before
%     diffFixEndSacStart = fixStartTime - sacEndTime - 1;
%     if( sum(diffFixEndSacStart) > 0)
%         % get channel indexes for current eye
%         xSignalIdx = GazeEEG_fuseChannelLists('Modality','Eyelink',{xEyes{eye}});
%         ySignalIdx = GazeEEG_fuseChannelLists('Modality','Eyelink',{yEyes{eye}});
%         
%         % get positions
%         sacEndX = Eyelink.Data.raw( xSignalIdx, sacEndTime );
%         sacEndY = Eyelink.Data.raw( ySignalIdx, sacEndTime );
%         fixStartX = Eyelink.Data.raw( xSignalIdx, fixStartTime );
%         fixStartY = Eyelink.Data.raw( ySignalIdx, fixStartTime );
%         gapLength = sqrt( (sacEndX - fixStartX).^2 + (sacEndY - fixStartY).^2 );
%         figure;
%         hold on;
%         plot(sacEndTime, gapLength, 'b+');
%         %plot(fixStartTime, 'r+');
%         trialStarts = zeros(1, length(Eyelink.Trials));
%         plot(Eyelink.Trials(:,1), trialStarts, 'g.');
%         plot(Eyelink.Trials(:,2), trialStarts, 'r.');
%         hold off;
%     end
%         
%     % -------- CHECK IF FIXATION END TIME + 1 == SACCADE START TIME --------
%     fixEndTime = Eyelink.Events.(FixationSaccades{eye}).time(2, logicKept & Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowFixations{eye}));
%     sacStartTime = Eyelink.Events.(FixationSaccades{eye}).time(1, logicKept & Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowSaccades{eye}));
%     sacStartTime = sacStartTime(1:end);
%     fixEndTime = fixEndTime(1:end-1);
%     diffSacEndFixStart = sacStartTime - fixEndTime - 1;
%     if( sum(diffSacEndFixStart) > 0)
%         % get channel indexes for current eye
%         xSignalIdx = GazeEEG_fuseChannelLists('Modality','Eyelink',{xEyes{eye}});
%         ySignalIdx = GazeEEG_fuseChannelLists('Modality','Eyelink',{yEyes{eye}});
%         
%         % get positions
%         sacStartX = Eyelink.Data.raw( xSignalIdx, sacStartTime );
%         sacStartY = Eyelink.Data.raw( ySignalIdx, sacStartTime );
%         fixEndX = Eyelink.Data.raw( xSignalIdx, fixEndTime );
%         fixEndY = Eyelink.Data.raw( ySignalIdx, fixEndTime );
%         gapLength = sqrt( (sacStartX - fixEndX).^2 + (sacStartY - fixEndY).^2 );
%         figure;
%         hold on;
%         plot(sacStartTime, gapLength, 'b+');
%         trialStarts = zeros(1, length(Eyelink.Trials));
%         plot(Eyelink.Trials(:,1), trialStarts, 'g.');
%         plot(Eyelink.Trials(:,2), trialStarts, 'r.');
%         hold off;
%     end
%     
    % ------------- REMOVE FIXATIONS AND SACCADES OUTSIDE OF TRIALS -----------------
    % THIS IS NOW DONE BY GazeEEG_matchTriggers
  
    keptEventsNb = sum( Eyelink.Events.(FixationSaccades{eye}).keep );
    % for given trials
    
    if ~isfield( Eyelink, 'Trials')
        error('GazeEEG:incompatibleStructure', 'Run <a href=''matlab: doc GazeEEG_matchTriggers''>GazeEEG_matchTriggers</a> first');
    end
    
    for currentTrial = find( Eyelink.Trials.keep)
       
        % FILTER EVENT TILL WE FOUND ONE IN THE CURRENT TRIAL
        % Finds Events whose start time index 
        % - exceeds the start time of the current trial
        % - preceeds the end time of the current trial
        TrialEventList = find( ...
            Eyelink.Events.(FixationSaccades{eye}).time(1,:) >= Eyelink.Trials.time(1,currentTrial) & ...
            Eyelink.Events.(FixationSaccades{eye}).time(1,:) <= Eyelink.Trials.time(2,currentTrial) & ...
            Eyelink.Events.(FixationSaccades{eye}).keep);
        
        % KEEP EVENTS IN THE CURRENT TRIAL
        % (starting and ending with fixations)
        
        if ~isempty( TrialEventList)
            % if first event is not a fixation
            % re-activate the previous event
            % put the start time of the previous event to the trial start time
            currentEvent = TrialEventList(1);
            if (Eyelink.Events.(FixationSaccades{eye}).type(currentEvent) ~=  Eyelink.Events.EventTypes.(flowFixations{eye}))
                Eyelink.Events.(FixationSaccades{eye}).keep(currentEvent-1) = true;
                Eyelink.Events.(FixationSaccades{eye}).time(1, currentEvent-1) = Eyelink.Trials.time(1,currentTrial);
            end
            
            % if last event is not a fixation
            % des-activate the last event
            % put the end time of the previous event to the trial end time
            currentEvent = TrialEventList(end);
            if(Eyelink.Events.(FixationSaccades{eye}).type(currentEvent) ~= Eyelink.Events.EventTypes.(flowFixations{eye}))
                Eyelink.Events.(FixationSaccades{eye}).keep(currentEvent) = false;
                Eyelink.Events.(FixationSaccades{eye}).time(2,currentEvent-1) = Eyelink.Trials.time(2,currentTrial);
            end
        end
    end     
    
    % show the number of filtered events
    filteredEvent = keptEventsNb - sum(Eyelink.Events.(FixationSaccades{eye}).keep);
    output = [output num2str(-filteredEvent, '%i') '\t\t(nb of events filtered on trial condition)\n'];
    
    % check for remaining events
    if (~any(Eyelink.Events.(FixationSaccades{eye}).keep))
        output = [output 'WARNING - no more saccades after trial filtering\n'];
        break;
    end
    

    % ------------- FILTER SACCADES ON THEIR DURATION ----------------
    idxKeptSacc = ...
        find(Eyelink.Events.(FixationSaccades{eye}).keep & ...
        Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowSaccades{eye}) );
    
    % compute saccades durations
    SaccDur = Eyelink.Events.(FixationSaccades{eye}).time(2, idxKeptSacc)...
        - Eyelink.Events.(FixationSaccades{eye}).time(1, idxKeptSacc);
    
    % filter saccades with a duration outside given values
    logicSacc2Remove = SaccDur<minSaccadeDuration | SaccDur>maxSaccadeDuration;
    
    idxSacc2Remove = idxKeptSacc(logicSacc2Remove);
    
    Eyelink.Events.(FixationSaccades{eye}).keep(idxSacc2Remove) = false;
    Eyelink.Events.(FixationSaccades{eye}).valid(idxSacc2Remove) = false;
    % filter following fixations
    Eyelink.Events.(FixationSaccades{eye}).keep(idxSacc2Remove+1) = false;
    Eyelink.Events.(FixationSaccades{eye}).valid(idxSacc2Remove+1) = false;
    % put previous fixation end to following fixation end
    Eyelink.Events.(FixationSaccades{eye}).time(2, idxSacc2Remove-1) = Eyelink.Events.(FixationSaccades{eye}).time(2, idxSacc2Remove+1);

    output = [output num2str(-2*length(idxSacc2Remove),'%i') '\t\t(nb of saccades filtered on duration condition)\n'];
    
    if (~any(Eyelink.Events.(FixationSaccades{eye}).keep))
        output = [output 'WARNING - no more saccades after duration filtering\n'];
        break;
    end
    
    % ------------- FILTER SACCADES ON THEIR SIZE ----------------
    idxKeptSacc = ...
        find(Eyelink.Events.(FixationSaccades{eye}).keep & ...
        Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowSaccades{eye}) );
    
    % compute saccade size
    %    get channel indexes for current eye
    xSignalIdx = GazeEEG_fuseChannelLists('Modality','Eyelink',{xEyes{eye}});
    ySignalIdx = GazeEEG_fuseChannelLists('Modality','Eyelink',{yEyes{eye}});
    
    % get saccade start and end
    saccadeStartX = Eyelink.Data.raw(xSignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(1, idxKeptSacc)) - eyeCenterPx_x;
    saccadeStartY = Eyelink.Data.raw(ySignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(1, idxKeptSacc)) - eyeCenterPx_y;
    saccadeEndX = Eyelink.Data.raw(xSignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(2, idxKeptSacc)) - eyeCenterPx_x;
    saccadeEndY = Eyelink.Data.raw(ySignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(2, idxKeptSacc)) - eyeCenterPx_y;
    
    % saccade size in degrees
    %    for a saccade from (x1,y1) to (x2,y2), with eye screen distance 'f',
    %    360/2pi * acos ( sqrt(f^2+x1x2+y1y2) / ( sqrt(f^2+x1^2+y1^2) + sqrt(f^2+x2^2+y2^2) )
    step1 = (distEyeScreenMM^2 + px2mm_x^2*saccadeStartX.*saccadeEndX + px2mm_y^2*saccadeStartY.*saccadeEndY);
    step2 = (distEyeScreenMM^2 + px2mm_x^2*saccadeStartX.^2 + px2mm_y^2*saccadeStartY.^2);
    step3 = (distEyeScreenMM^2 + px2mm_x^2*saccadeEndX.^2 + px2mm_y^2*saccadeEndY.^2);
    
    saccadeSizeDeg = 180/pi*acos( step1 ./ sqrt(  step2 .* step3 ) );
    
    % filter saccades with a size outside given values
    logicSaccades2Remove = saccadeSizeDeg<minSaccadeSize | saccadeSizeDeg>maxSaccadeSize;
    idxSacc2Remove = idxKeptSacc(logicSaccades2Remove);

    Eyelink.Events.(FixationSaccades{eye}).keep(idxSacc2Remove) = false;
    Eyelink.Events.(FixationSaccades{eye}).valid(idxSacc2Remove) = false;

    % filter following fixations
    Eyelink.Events.(FixationSaccades{eye}).keep(idxSacc2Remove+1) = false;
    Eyelink.Events.(FixationSaccades{eye}).valid(idxSacc2Remove+1) = false;
    % put previous fixation end to following fixation end
    Eyelink.Events.(FixationSaccades{eye}).time(2, idxSacc2Remove-1) = Eyelink.Events.(FixationSaccades{eye}).time(2, idxSacc2Remove+1);

    output = [output num2str(-2*length(idxSacc2Remove),'%i') '\t\t(nb of saccades filtered on size condition)\n'];
    
    if (~any(Eyelink.Events.(FixationSaccades{eye}).keep))
        output = [output 'WARNING - no more saccades after size filtering\n'];
        break;
    end

    
    % ------------- FILTER FIXATIONS ON THEIR DURATION ----------------
    idxKeptFix = ...
        find(Eyelink.Events.(FixationSaccades{eye}).keep & ...
        Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowFixations{eye}) );
    
    % compute fixations durations
    FixDur = Eyelink.Events.(FixationSaccades{eye}).time(2, idxKeptFix)...
        - Eyelink.Events.(FixationSaccades{eye}).time(1, idxKeptFix);
    
    % filter short fixations
    logicFix2Remove = FixDur<minFixationDuration;
    idxFix2Remove = idxKeptFix(logicFix2Remove);
    
    Eyelink.Events.(FixationSaccades{eye}).keep(idxFix2Remove) = false;
    Eyelink.Events.(FixationSaccades{eye}).valid(idxFix2Remove) = false;
    % filter following saccade
    Eyelink.Events.(FixationSaccades{eye}).keep(idxFix2Remove+1) = false;
    Eyelink.Events.(FixationSaccades{eye}).valid(idxFix2Remove+1) = false;
    % put previous saccade end time to following saccade end time
    Eyelink.Events.(FixationSaccades{eye}).time(2, idxFix2Remove-1) = Eyelink.Events.(FixationSaccades{eye}).time(2, idxFix2Remove+1);

    output = [output num2str(-2*length(idxFix2Remove),'%i') '\t\t(nb of fixations filtered on duration condition)\n'];

    if (~any(Eyelink.Events.(FixationSaccades{eye}).keep))
        output = [output 'WARNING - no more fixations after duration filtering\n'];
        break;
    end
    
    
    

    % ------------------- GRAPHIC OUTPUT --------------------------
    if (feedback>1)
        % show kept saccades and fixations
        figure('Name', [eyes{eye} ' - events'], 'NumberTitle', 'off', 'WindowStyle', 'docked');
        hold on;
        trialStarts = find(Eyelink.Trials.keep);
        hg = plot(Eyelink.Trials.time(1,trialStarts), trialStarts, 'g.');
        hr = plot(Eyelink.Trials.time(2,trialStarts), trialStarts, 'r.');
        for fn = setdiff(fieldnames(Eyelink.Events)', 'EventTypes')
            logicKept = logical( Eyelink.Events.( FixationSaccades{eye}).keep);
            hb = plot(Eyelink.Events.(FixationSaccades{eye}).time(1,logicKept), Eyelink.Events.(FixationSaccades{eye}).type(logicKept), 'b.');
        end
        hold off;
        xlabel('time');
        ylabel('event number for events, trial ranks for trial starts and ends');
        legend([hb, hg, hr], {'events', 'trial starts', 'trial ends'});
        
        % show fixations durations profile
        figure('Name', [eyes{eye} ' - fixations durations'], 'NumberTitle', 'off', 'WindowStyle', 'docked');
%        hist(double(FixDur), 100);
        hist(double(FixDur(FixDur>=minFixationDuration)), 100);
             
        
        % show saccades size profile
        logicSaccades2Keep = saccadeSizeDeg>=minSaccadeSize & saccadeSizeDeg<=maxSaccadeSize;
        figure('Name', [eyes{eye} ' - saccades amplitude'], 'NumberTitle', 'off', 'WindowStyle', 'docked');
        hist(abs(saccadeSizeDeg(logicSaccades2Keep)),100);
        xlabel('amplitude (degrees)');
        ylabel('nb of saccades');
        
        % show saccades orientations
        idxKeptSacc = find(Eyelink.Events.(FixationSaccades{eye}).keep & ...
            Eyelink.Events.(FixationSaccades{eye}).type == Eyelink.Events.EventTypes.(flowSaccades{eye}) );
        
        saccadeStartX = Eyelink.Data.raw(xSignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(1, idxKeptSacc)) - eyeCenterPx_x;
        saccadeStartY = Eyelink.Data.raw(ySignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(1, idxKeptSacc)) - eyeCenterPx_y;
        saccadeEndX = Eyelink.Data.raw(xSignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(2, idxKeptSacc)) - eyeCenterPx_x;
        saccadeEndY = Eyelink.Data.raw(ySignalIdx, Eyelink.Events.(FixationSaccades{eye}).time(2, idxKeptSacc)) - eyeCenterPx_y;
        
        saccadeX = saccadeEndX - saccadeStartX;
        saccadeY = saccadeEndY - saccadeStartY;
        saccades = saccadeX - sqrt(-1)*saccadeY;
        
        figure('Name', [eyes{eye} ' - saccades impact (bring back to zero)'], 'NumberTitle', 'off', 'WindowStyle', 'docked');
        plot(saccadeX, -saccadeY, '.');
        xlabel('x coord of the saccade (pixels)');
        ylabel('y coord of the saccade (pixels)');
        figure('Name', [eyes{eye} ' - saccades direction'], 'NumberTitle', 'off', 'WindowStyle', 'docked');
        rose(angle(saccades),100);
        xlabel('direction (degrees)');
        ylabel('number');
        axis equal;
        
        % show saccades and fixation mean times per rank in the trial
        idxKept = find(Eyelink.Events.(FixationSaccades{eye}).keep);
        currentEvent = 1;
        nbOfKeptEvents = 6;
        evtTimesByRank = zeros(length(find( Eyelink.Trials.keep)), nbOfKeptEvents);
        % for given trials

        for currentTrial = find( Eyelink.Trials.keep)
            keptEventRankInTheTrial = 1;
            while (currentEvent <= length(idxKept) && ...                                                                         % we still have events
                    Eyelink.Events.(FixationSaccades{eye}).time(1,idxKept(currentEvent)) >= Eyelink.Trials.time(1,currentTrial) && ...
                    Eyelink.Events.(FixationSaccades{eye}).time(1,idxKept(currentEvent)) <= Eyelink.Trials.time(2,currentTrial))       % event is in the trial
                % record event time for further mean computation
                if ( keptEventRankInTheTrial <= nbOfKeptEvents )
                    evtTimesByRank(currentTrial, keptEventRankInTheTrial) = Eyelink.Events.(FixationSaccades{eye}).time(1,idxKept(currentEvent)) - Eyelink.Trials.time(1,currentTrial);
                end
                keptEventRankInTheTrial = keptEventRankInTheTrial + 1;
                currentEvent = currentEvent + 1;
            end
        end
        figure('Name', [eyes{eye} ' - six first events'], 'NumberTitle', 'off', 'WindowStyle', 'docked');
        boxplot(evtTimesByRank);
        %         xlabel('fix1   sacc1   fix2   sacc2   fix3   sacc3');
        set(findobj(gca, 'Type', 'text','String','0'),'String','');
        set(findobj(gca, 'Type', 'text','String','1'),'String','');
        set(findobj(gca, 'Type', 'text','String','2'),'String','');
        set(findobj(gca, 'Type', 'text','String','3'),'String','');
        set(findobj(gca, 'Type', 'text','String','4'),'String','');
        set(findobj(gca, 'Type', 'text','String','5'),'String','');
        set(gca,'xtick',1:6,'xticklabel',{'fix1';'sacc1';'fix2';'sacc2';'fix3';'sacc3'})
        ylabel('Time elapsed from trial start (mean over all trials)');
    end
    
    % show stats on filtering
    keptEventsNb = sum( Eyelink.Events.(FixationSaccades{eye}).keep );
    output = [output num2str(keptEventsNb,'%i') '\t\t(nb of kept events)\n\n'];
    
end
if (feedback>0)
    fprintf(output);
end

end