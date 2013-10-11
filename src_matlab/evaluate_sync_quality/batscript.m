eeglab;
global gipsapath;
global subject_name;

%gipsapath = 'D:\Work\DATA\Aline-Gelu\s1\';
gipsapath = 'D:\Work\DATA\Aline-Gelu\';
before_subject = 's';
after_subject = '';

% gipsapath = 'D:\Work\DATA\Anton\Emanuelle\s01\dyn\'
% before_subject = 's0';
% after_subject = '_dyn';

for i=1:1
    
    if i==12 || i==15 
        continue; 
    end;
    
%      if i==1 || i==3 %done it won't process
%         continue; 
%     end;
    
    subject_name = strcat(before_subject,int2str(i)); 
    fname = strcat(subject_name,after_subject); 
  
    asc_filename = strcat(fname, '.asc')
    eeg_hdr_filename =  strcat(fname, '.vhdr')
    
    sync_start_event = -1;
    sync_end_event = -1;
    File_Info = importdata([gipsapath 'synchro_' subject_name '.asc.info.txt']);
    sync_start_event = File_Info.data(5)
    sync_end_event = File_Info.data(6)
    nbsegements = File_Info.data(7)

    if nbsegements >1 
        disp('Skipping subject')
        continue;
    end;
    
    eye_eeg_batch(asc_filename, eeg_hdr_filename, sync_start_event, sync_end_event)
    
    result = load([gipsapath subject_name '_hist_quality_sync.mat']);

    %chart
%     figure; hold on; box on;
%     title('Quality of synchronization','fontweight','bold')
%     bar(result.bin,result.count,'k')
%     set(gca,'xTick',-result.RADIUS:1:result.RADIUS);
%     xlim([-result.RADIUS-0.5 result.RADIUS+0.5])
%     xlabel('Offset between shared events (samples)')
%     ylabel('Number of events')
end



% gipsapath = 'D:\Work\DATA\Anton\Emanuelle\s01\dyn\'
% asc_filename = 's01_dyn.asc'
% eeg_hdr_filename = 's01_dyn.vhdr'
% eye_eeg_batch(asc_filename,eeg_hdr_filename,10,10)
% 
% gipsapath = 'D:\Work\DATA\Anton\Helene\'
% asc_filename = 'pil6_cat.asc'
% eeg_hdr_filename = 'pil6_cat.vhdr'
% eye_eeg_batch(asc_filename,eeg_hdr_filename,10,10)


%% ==============================================================


