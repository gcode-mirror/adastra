%eeglab;

source_data = 'D:\Work\DATA\good_data\';
global EEG;

for k=1:1

  try  
    filename_set = ['k' int2str(k) '_channels_fp1_derivative_blink.set'];

    EEG = pop_loadset('filename',filename_set,'filepath',source_data);
    EEG = eeg_checkset( EEG );
    %%Load data from set
    disp(['Loaded set: k' int2str(k)]);
    
    %Array of first 100 blinks to be processed in sample positions
        leftEyeBlink = 0;
        deltas = [];
        
        for i = 2:(EEG.pnts-1)
            if EEG.data(2,i-1) == 0 && EEG.data(2,i) > 0 
                
                leftEyeBlink = leftEyeBlink +1;
                
                if (i > 10) %to avoid index error

                    [derivMax,maxi] = max(EEG.data(3,i-10:i+10));

                    delta = i - (i + maxi(1));

                    deltas = [deltas delta];
                end
            end;
            
            if leftEyeBlink == 100 break; end;
        end;

        disp('Saving histogram:');
        save([source_data 'k' int2str(k) '_hist_delta.mat'],'deltas');

        %deltas
        %hist(deltas);
        
    catch err 
       disp(['Error for:' filename_set]);
       disp(err.identifier); 
    end
end;



%% ==============================================================


