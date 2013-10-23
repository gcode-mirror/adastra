function [bin, count, radius] = eye_eeg_batch(asc_filename,eeg_filename,sync_start_event,sync_end_event)

%variables
global EEG
global ALLEEG
global CURRENTSET

global gipsapath;
plotFig = 0;

disp('Converting eyetracking asc file to mat file');
full_asc_filename = asc_filename;

if exist( strcat(full_asc_filename,'.mat'), 'file' ) ~= 0
    disp('Loading previously generated asc file');
    ET = load([full_asc_filename '.mat']);    
else 
    ET = parseeyelink(full_asc_filename,strcat(full_asc_filename,'.mat'));
end
disp('Done: eyetracking asc file to mat file');

disp('Adding the new BLINK LEFT logical channel by overwriting the newly create  mat file');
ET.data = horzcat( ET.data,  zeros(size(ET.data, 1),1));

for ii=1:size(ET.eyeevent.blinks.eye, 1)
    if(ET.eyeevent.blinks.eye(ii) == 'L')
        continue;
    end
    
    off = find(ET.data(:, 1) == ET.eyeevent.blinks.data(ii, 1));
    
    ET.data(off:off+ET.eyeevent.blinks.data(ii, 3), end) = 500;
end

ET.colheader{1, end+1} = 'BLINK';
save([full_asc_filename '.mat'], '-struct', 'ET');
disp('Done: adding the new BLINK LEFT logocal channel');

disp('Importing EEG data in memeory');
EEG = pop_loadbv(gipsapath, eeg_filename);%you point to the header to read the 3 files
EEG.setname='EegData';
EEG = eeg_checkset( EEG );

disp('Synchronizing EEG and ET:');
EEG = pop_importeyetracker(EEG,strcat(full_asc_filename,'.mat'),[sync_start_event sync_end_event] ,[1:3 5:6 8] ,{'TIME' 'L_GAZE_X' 'L_GAZE_Y' 'R_GAZE_X' 'R_GAZE_Y', 'BLINK'},1,1,0,plotFig);
EEG.setname='eye-eeg-synced';
EEG = eeg_checkset( EEG );

disp('Done: synchronizing EEG and ET with "eye-eeg"');

disp('Calculating first derivative of FP1 channel');
alpha = 0.05;
d = Deriche.FirstDeriv(EEG.data(1,:), alpha);
EEG.data = vertcat( EEG.data,  d);
EEG.nbchan = EEG.nbchan + 1;
EEG.chanlocs(39).labels = 'Derivative_FP1'
disp('Done: calculating first derivative of FP1 channel');

%remove channels
EEG = pop_select(EEG, 'channel', {'1', 'Derivative_FP1', 'BLINK'});
EEG.setname = 'Blink channels';

%save data
global subject_name;
global after_subject;
result_set_filename = [gipsapath subject_name after_subject '_channels_fp1_derivative_blink.set'];
EEG = pop_saveset( EEG, 'filename', result_set_filename);

disp('EEGLAB store');
[ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG, CURRENTSET);
eeglab redraw;

%save data: derivative, logical channel


%result = load(strcat(gipsapath,'histogram.mat'));

% bin = result.bin, 
% count = result.count;
% resdius = result.RADIUS;
% 
% figure; hold on; box on;
% title('Quality of synchronization','fontweight','bold')
% bar(result.bin,result.count,'k')
% set(gca,'xTick',-result.RADIUS:1:result.RADIUS);
% xlim([-result.RADIUS-0.5 result.RADIUS+0.5])
% xlabel('Offset between shared events (samples)')
% ylabel('Number of events')