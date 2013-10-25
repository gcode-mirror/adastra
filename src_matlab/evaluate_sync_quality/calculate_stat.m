%eeglab;

source_data = 'D:\Work\DATA\good_data\';
global EEG;

for k=1:87

  try  
    filename_set = ['k' int2str(k) '_channels_fp1_derivative_blink.set'];

    EEG = pop_loadset('filename',filename_set,'filepath',source_data);
    EEG = eeg_checkset( EEG );
    %%Load data from set
    disp(['Loaded set: k' int2str(k)]);
    
    %Array of first 100 blinks to be processed in sample positions
        leftEyeBlink = 0;
        deltas = [];
        interval = 10;
        
        indSig = -1;
        indBlink = -1;
        indDer = -1;
        
        for t =1:3
            if strcmp(EEG.chanlocs(1,t).labels , 'FP1');
                indSig = t; 
            end;
             if strcmp(EEG.chanlocs(1,t).labels , 'BLINK');
                indBlink = t; 
            end;
             if strcmp(EEG.chanlocs(1,t).labels , 'Derivative_FP1');
                indDer = t; 
            end;
        end;

        for i = 2:(EEG.pnts-1)
            if EEG.data(indBlink,i-1) == 0 && EEG.data(indBlink,i) > 0 
                
                leftEyeBlink = leftEyeBlink +1;
                
                if (i > 10) %to avoid index error

                    [derivMax,maxi] = max(EEG.data(indDer, i-interval : i+interval));

                    t1 = i;
                    t2 = i - interval + maxi(1);
                    delta = t1 - t2;

                    deltas = [deltas delta]; % no more than 100 blinks
                end
            end;
            
            if leftEyeBlink == 100 break; end;
        end;

        disp('leftEyeBlink:');
        leftEyeBlink
        disp('Saving histogram:');
        [count bin] = hist(deltas,-10:10);
        
        save([source_data 'k' int2str(k) '_hist_delta.mat'],'count','bin');

        %deltas
        %plot(hist(deltas));
        
    catch err 
       disp(['Error for:' filename_set]);
       disp(err.identifier); 
       %rethrow(err);
    end
end;



%% ==============================================================


