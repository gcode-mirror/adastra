%eeglab;

%source_data = 'D:\Work\DATA\good_data\';
source_data = 'R:\vibs\Anton\results_eye_eeg\'
global EEG;

sync_result = zeros(11,1)';%[-5 0 +5]
delta_result = zeros(21,1)';%[-10 0 10]

for k=1:87

  try  
    filename_quality = ['k' int2str(k) '_hist_quality_sync.mat'];
    filename_delta = ['k' int2str(k) '_hist_delta.mat'];

    %sync quality
    q = load([source_data filename_quality]);
    sync_result = sync_result + q.count;
    
    %delta
    d=load([source_data filename_delta]);
    delta_result = delta_result + d.count;
        
    catch err 
       disp(['Error for:' filename_quality]);
       disp(err.identifier); 
       %rethrow(err);
    end
end;

disp('Mean synchro:');
([-5:5] * sync_result')/sum(sync_result)
disp('std synchro:');
gstd([[-5:5];sync_result]',0)

disp('Blink/Derivative mean:');
([-10:10] * delta_result')/sum(delta_result)
disp('Blink/Derivative std:');
gstd([[-10:10];delta_result]',0)


figure;
bar(-5:+5,sync_result);
figure;
bar(-10:+10,delta_result);



%% ==============================================================


