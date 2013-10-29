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
        interval = 100; %around which we search for the max
        
        indSig = -1;
        indBlink = -1;
        indDer = -1;
        
        %check channels order
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
        
        blinksSkippedAtBorder = 0;
        processedEyeBlinks = 0;
        noDownEdgeFound = 0;
        blinksSkippedOnBadLength = 0;
        
        for i = 2:(EEG.pnts-1)
            if EEG.data(indBlink,i-1) == 0 && EEG.data(indBlink,i) > 0 %detect a rising edge
                
                if (i >= interval)
                    indDownEdge = find(EEG.data(indBlink,[i+1:i+400]) == 0, 1, 'first');
                    
                    leftEyeBlink = leftEyeBlink + 1;
                    
                    if (~isempty(indDownEdge))
                        
                        samplelength = indDownEdge(1); %length of the blink in samples
                        timeLength = (samplelength * 1000) / EEG.srate;
                        
                        if (timeLength >=100 && timeLength <=300) %to avoid index error

                            [derivMax,maxi] = max(EEG.data(indDer, i-interval : i+interval));

                            t1 = i; % pos begin of blink according to logical channel
                            t2 = (i - interval) + maxi(1); % pos nax of the derivative
                            
                            if (maxi(1) <= - interval || maxi(1) >= interval)
                                blinksSkippedAtBorder = blinksSkippedAtBorder + 1;
                                disp('Blink skipped because it was at the border of the interval');
                                continue;
                            end;
                            
                            delta = t1 - t2;
                            if (delta < 0) % peak is before
                                disp('minus delta detected');
                            end;
                            deltas = [deltas delta]; % no more than 100 blinks
                            processedEyeBlinks = processedEyeBlinks + 1;
                        else
                            blinksSkippedOnBadLength = blinksSkippedOnBadLength + 1;
                        end;
                    else
                        noDownEdgeFound = noDownEdgeFound + 1;
                    end;
                end;
            end;
            
            if leftEyeBlink == 100 
                break; 
            end;
        end;

        disp('All left Eye Blink:');
        disp(leftEyeBlink);
        
        disp('blinks Skipped At Border:');
        disp(blinksSkippedAtBorder);
        
        disp('Blinks skipped because of no down edge:');
        disp(noDownEdgeFound);
        
        disp('Blinks Skipped On Bad Length:');
        disp(blinksSkippedOnBadLength);
        
        disp('Blinks processed:');
        disp(processedEyeBlinks);
        
        disp('Saving histogram:');
        [count bin] = hist(deltas,[-interval:interval])
        
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


