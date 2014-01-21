function GazeEEG_reconstructAfterJointICA(varargin)

% function GazeEEG_reconstructFromICA(varargin)
%
% A function to reconstruct your EEG signal by suppressing some of the
% components
%
% If this  function is used after a function call to <a href="matlab: doc GazeEEG_jointICA">GazeEEG_jointICA</a>
% the first components should be removed. Only those epochs for which an
% icaweights[epoch] matrix exist will be corrected. If no specific
% icaweights matrices exist, the global icaweight matrix will be used.
%
% One may also enforce to use either one of the above methods in case all
% icaweight matrices exist.
%
% The function uses either
%       EEG.icasphereXX
%       EEG.icaweightsXX
%       EEG.icaact(:,:,XX)
% where XX refers to the epochs, or, 
%       EEG.icasphere
%       EEG.icaweights
%       EEG.icaact
%
% To enforce the use of the latter (global reconstruction), use the
% parameter value pair {'unfold', true}

global EEG
global ALLEEG
global CURRENTSET

ChanList = 1:length(find( strcmpi( {EEG.chanlocs.type},'EEG')));
Unfold = false;
feedback = 0;
Replace = false;

CalledFrom = 'GazeEEG_Toolbox';
for k = 1:2:nargin
    if strcmpi(varargin{k},'CalledFrom')
		CalledFrom = char(varargin{k+1});
    elseif strcmpi(varargin{k},'feedback')
        feedback = varargin{k+1};
    elseif strcmpi(varargin{k},'Unfold')
        Unfold = logical(varargin{k+1});
    elseif strcmpi(varargin{k},'Replace')
        Replace = logical( varargin{k+1});
    end
end

% Get the channels and their corresponding entries in the total data matrix
ixExclude = find( all( isnan(reshape( EEG.data, EEG.nbchan, [])), 2));
ixValid = setdiff( 1:EEG.nbchan, ixExclude);

ixEEG   = find( strcmpi( {EEG.chanlocs.type},'EEG'));
ixEOG   = find( strcmpi( {EEG.chanlocs.type},'EOG'));
ixGaze  = find( strcmpi( {EEG.chanlocs.type},'Gaze'));
ixRef   = find( strcmpi( {EEG.chanlocs.type},'Ref'));

NbChan2Correct = min(2,length(ixGaze));
 
if ~Unfold
    foundEpochs = false;
    for ixEpoch = 1:EEG.trials
        if isfield(EEG.epoch(ixEpoch),'icaweights') && numel(EEG.epoch(ixEpoch).icaweights)>0
            fprintf('Reconstructing epoch %i\n',ixEpoch)
            % Some channels should be excluded from any further analysis since they
            % contain only NaN's (e.g., corrupted Gaze channel)
            ixExclude = find( all( isnan(reshape( EEG.data(:,:,ixEpoch), EEG.nbchan, [])), 2));     
            ixValid = setdiff( 1:EEG.nbchan, ixExclude);

            if ~isempty(ixExclude) && feedback > 1
                for k = 1:length(ixExclude)
                    warning( 'GazeEEG:InvalidChannel', ...
                        'The channel %s (channel nr %i) has not been validated and will be removed from the reference set.', ...
                        EEG.chanlocs(ixExclude(k)).labels, ixExclude(k) );
                end
            end
            
            ixEEG_EP        = intersect( ixEEG, ixValid);
            
            foundEpochs = true;
            icasphere   = EEG.epoch(ixEpoch).icasphere(ixEEG_EP,ixEEG_EP);
            icaweights  = EEG.epoch(ixEpoch).icaweights(ixEEG_EP,ixEEG_EP); 
            icawinv     = pinv(icasphere*icaweights);
            ProjMat = icasphere*icaweights(:,NbChan2Correct+1:end)*icawinv(NbChan2Correct+1:end,:);
            EEG.data(ixEEG_EP,:,ixEpoch) = ProjMat*(squeeze(EEG.data(ixEEG_EP,:,ixEpoch)) - EEG.epoch(ixEpoch).mu(ixEEG_EP)*ones(1,EEG.pnts)) + EEG.epoch(ixEpoch).mu(ixEEG)*ones(1,EEG.pnts);
        end
    end
    % If there is no reconstruction found
    if ~foundEpochs, GazeEEG_reconstructAfterJointICA('Unfold',true); end
else
    if ~isempty(EEG.icasphere)
        
        % Some channels should be excluded from any further analysis since they
        % contain only NaN's (e.g., corrupted Gaze channel)
        ixExclude = find( all( isnan(reshape( EEG.data, EEG.nbchan, [])), 2));
        ixValid = setdiff( 1:EEG.nbchan, ixExclude);
        
        if ~isempty(ixExclude) && feedback > 1
            for k = 1:length(ixExclude)
                warning( 'GazeEEG:InvalidChannel', ...
                    'The channel %s (channel nr %i) has not been validated and will be removed from the reference set.', ...
                    EEG.chanlocs(ixExclude(k)).labels, ixExclude(k) );
            end
        end
        
        ixEEG   = intersect( ixEEG, ixValid);
        
        DATA = reshape(EEG.data,EEG.nbchan,EEG.pnts*EEG.trials);
        DATA(ixEEG,:) = EEG.icasphere(ixEEG,ixEEG)*EEG.icaweights(ixEEG,ixEEG(NbChan2Correct+1:end))*DATA(ixEEG(NbChan2Correct+1:end),:);
        EEG.data = reshape(DATA,EEG.nbchan,EEG.pnts,EEG.trials);
    else
        error('No ICA transformation found, try running <a href="matlab: doc GazeEEG_jointICA>GazeEEG_jointICA"</a> first.')
    end
end
if ~strcmpi(CalledFrom,'BrainAnalyzer')
	fprintf('\nUpdating EEG structure\n');

	EEG.setname = [EEG.setname ' - ICA'];
	if Replace
		[ALLEEG EEG CURRENTSET]     = eeg_store(ALLEEG, EEG, CURRENTSET);
	else
		[ALLEEG EEG CURRENTSET]     = eeg_store(ALLEEG, EEG);
	end
	evalin('base','eeglab redraw;');
end