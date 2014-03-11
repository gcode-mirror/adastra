function NewEpochs = GazeEegLabImporter_Process(EEGLabPath, synchro_filename, EpochEventsStr, DisplayEventsStr, TimeInterval, StartFromTriggerStr, FilterData, NbNonEEGChan)

%% Initialize Gaze
% It is needed for the ICA implementation and others
GazeEEG_init(                   ...
    'EEGLabPath',       EEGLabPath, ...
    ...    'EEGLab', false,...
    'CleanWorkSpace',   true    ... do you want the workspace to be cleaned up ?
    );



global EegAcq;
%% Init EEGlab
global EEG
global ALLEEG
global CURRENTSET

%% Read the syncho file for metadata
%synchro_filename = 'D:\Dropbox\Gipsa-work\GazeEeg\Data\synchro_s01_tag.EEG.mat'
%synchro_filename = 'D:\Dropbox\Gipsa-work\GazeEeg\Data\new_s1\synchro_s1.asc.eeg.mat' %new file from gelu with new triggers above 2000, below 1000

load(synchro_filename);
disp('[step 1] Mat file loaded');
disp('You can build epochs from the following events:');
%EegAcq.Events.EventTypes

%% Init 
NbChB = EegAcq.Data.Params.nbOfChannels;

% Create new EEG variable set and attribute the sampling rate
EEG = eeg_emptyset;
EEG.srate   = double(EegAcq.Data.Params.samplingRate);
EEG.nbchan = EegAcq.Data.Params.nbOfChannels; %Anton
EEG.data = [];
%EEG.trials  = NbNewEpochs;
%EEG.pnts = NbNewEpochs;%EEG.pnts    = EegAcq.Data.Params.nbOfSamples; %????? %  diff(EpochIxEyelink(:,1))+1;
%EEG.trials = NbNewEpochs;
%EEG.data = zeros(EEG.nbchan, , NbNewEpochs);

%% Create Temporal Filter - synchro version
disp('Construct filter:');
EegAcq = GazeEegLabImporter_createDataFilter('EegAcq',EegAcq,'Notch',50,'HP',2,'feedback',1);

%% Set raw data file 
% Mode 1 - raw data is outside the mat file
filename_egg_raw = synchro_filename(1:(length(synchro_filename))-4);
disp (filename_egg_raw);
if exist(filename_egg_raw,'file') == false 
    error('RAW EEG data file does not exist!');
end;
% TODO: check for filename existance

%% Set channel names
%GazeEegLabImporter_setChannelNamesMontageParis();

%channels EEG are (1-32 without 20)
% Electrooculography: 20:'EOGV',
% + 5 more eytracker: 'EyeRx', 'EyeRy', 'EyeLx', 'EyeLy', blink (33 - 37)

%% Select epochs
%EpochEvents = [EegAcq.Events.EventTypes.flowSaccadeLeft EegAcq.Events.EventTypes.flowSaccadeRight];

% EpochEvents = [EegAcq.Events.EventTypes.flowBlink];%events which are used for the epochs generation
% DisplayEvents = [EegAcq.Events.EventTypes.flowBlink];%events that can be seen with data scroll
%  
% TimeInterval = [-420, 420];%-140*3, 140*3
% MaxEpochs = 100;

% EpochEvents = [EegAcq.Events.EventTypes.flowBlinkRight];%events which are used for the epochs generation
% DisplayEvents = [EegAcq.Events.EventTypes.flowFixationLeft EegAcq.Events.EventTypes.flowFixationRight EegAcq.Events.EventTypes.flowBlinkLeft EegAcq.Events.EventTypes.flowBlinkRight];%events that can be seen with data scroll
% 
% TimeInterval = [-100, 500];

EpochEvents = eval(EpochEventsStr);%[EegAcq.Events.EventTypes.flowBlinkRight];%events which are used for the epochs generation
DisplayEvents = eval(DisplayEventsStr);%[EegAcq.Events.EventTypes.flowFixationLeft EegAcq.Events.EventTypes.flowFixationRight EegAcq.Events.EventTypes.flowBlinkLeft EegAcq.Events.EventTypes.flowBlinkRight];%events that can be seen with data scroll
%MaxEpochs = 100;
StartFromTrigger = eval(StartFromTriggerStr);

%NewEpochs = GazeEegLabImporter_SelectEpochs_5_epochs ... % Warning used only with 5 epochs !!!!!!!!
NewEpochs = GazeEegLabImporter_SelectEpochs ...
                          ('EpochEvents',EpochEvents, ...
                           'TimeInterval', TimeInterval, ...
                           'StartFromTrigger', StartFromTrigger); % process all triggers after "Learn" phase - trigger130

%% Read data from file, apply filter if specified, fill EEG structure
GazeEegLabImporter_BuildEpoch('FileNameRawData',filename_egg_raw, ...
                           'TimeInterval', TimeInterval, ...
                           'Epochs',NewEpochs, ...
                           'FilterData',FilterData, ...
                           'NbNonEEGChan', NbNonEEGChan ... non EEG channels such as 'EOGV','EOGH'
                           );   
                       %'MaxEpochsToProcess',MaxEpochs);

%% Processing in EEGLAB ===========================================================================================================


%% Locate the electrodes
[NotListed, NotInRecording] = GazeEEG_setElectrodeLocations();

EEG = pop_select(EEG, 'nochannel', {'EOGH'}); % anton: prevents error "integers vs doubles", EOGH channel does not exist

if (FilterData)
   EEG.setname = 'Loaded with filtering';
else
   EEG.setname = 'Loaded no filtering';
end;

%% remove noisy channels
% EEG = pop_select(EEG, 'nochannel', {'TP10', 'AF7'});


%% The joint Independent Component analysis

apply_IcaRmBase_make_chart = true;

if apply_IcaRmBase_make_chart
    
        GazeEEG_jointICA(               ...
                  ...    'chanlist',     fchanlist,     ... eeg channels to filter
                 'statorder',    3          ...
                 ...    'epochs',       [1:2]         ... suppose non stationary topography
                 ...    'unfold',       1           ... if set to 1, suppose stationary topography
                 );

        % Remove eye artifacts from EEG signal - reconstruction
        %...    GazeEEG_reconstructAfterJointICA('Replace',false);
        GazeEEG_reconstructAfterJointICA('Replace',false);% anton: false was the default - it should make a new copy ALLEEG(2)


        %% Remove baseline

        EEG = pop_rmbase(EEG, [-100 0]); 
        EEG.setname = 'RPM base, ICA';

        %% Remove eye signals - for scaling in drawings 

        EEG = pop_select(EEG, 'nochannel', {'EOGH', 'EOGV', 'EyeRx', 'EyeRy', 'EyeLx', 'EyeLy'});
        
        %% Replace in ALLEEG
        ALLEEG(2) = EEG;

        %% Save EEGLab Set
        ALLEEG(2) = pop_saveset( ALLEEG(2), 'filename',[synchro_filename '.set']);
        ALLEEG(2) = eeg_checkset( ALLEEG(2) );

        % Anton: Remove channels from the first - raw data set in order to make the number
        % of channels the same in the two datasets
        ALLEEG(1) = pop_select(ALLEEG(1), 'nochannel', {'EOGH', 'EOGV', 'EyeRx', 'EyeRy', 'EyeLx', 'EyeLy'});
        
        %% Plot the (F)ERP
        %pop_comperp( ALLEEG, 1, 1,2,'addavg','on','addstd','off','subavg','on','diffavg','on','diffstd','off','tplotopt',{'ydir' -1});
        %pop_comperp( ALLEEG, flag, datadd, datsub, 'key', 'val', ...);
        useICA = 1;
        dataadd = 1; %List of ALLEEG dataset indices to average to make an ERP grand average and optionally to compare with 'datsub' datasets.
        datasub = 2; %List of ALLEEG dataset indices to average and then subtract from the 'datadd' result to make an ERP grand mean difference.
        pop_comperp( ALLEEG, useICA , dataadd , datasub ,'addavg','on','addstd','off','subavg','on','diffavg','on','diffstd','off','tplotopt',{'ydir' -1});
        %5 common average reference is the third chart
        %pop_comperp( ALLEEG, 1, 2);%anton: 
         
        %% Filter events - create ALLEEG(3)
        EEG = pop_selectevent( EEG, 'type',DisplayEvents,'deleteevents','on','deleteepochs','off','invertepochs','off');
        EEG.setname= ['selected events'];
        [ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG);
        
        eeglab redraw;   
        
else
  %just show the epochs on FP1 and FP2      
  EEG = pop_select(EEG, 'nochannel', {'EOGH', 'EOGV', 'EyeRx', 'EyeRy', 'EyeLx', 'EyeLy','F7','F3','Fz','F4','F8','FC5','FC1','FC2','FC6','T7','C3','Cz','C4','T8','TP9','CP5','CP1','EOGV','CP6','TP10','P7','P3','Pz','P4','P8','PO9','O1','Oz','O2','PO10'});
  eeglab redraw;
  pop_eegplot( EEG, 1, 1, 1);
  
end

