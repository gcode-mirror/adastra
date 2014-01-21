function GazeEEG_jointICA(varargin)

% calculates the jointICA on the Gaze and EEG signals
%
% function GazeEEG_jointICA()
%
% the function sets the following EEG fields
%   EEG.icasphere   : the sphering matrix
%   EEG.icaweights  : the weights
%   EEG.icawinv     : the inverse of the weights matrix
%   EEG.icaact      : the activation functions of the components
%
% We have
%   [EEG.data(t); 1]    = EEG.icasphere * EEG.icaweights * [EEG.icaact(t); 1]
% and also 
%   [EEG.icaact(t); 1]  = EEG.icawinv * [EEG.data(t); 1]
%
%
% This function acts as a wrapper for <a href="matlab: doc GazeEEG_referencedICA.m">GazeEEG_referencedICA.m</a>
%
% Inputs:
% -------
%   inputs come in ('param',value) pairs, entries delimited by [..] refer
%   to the default values
%
%       'StatOrder'     : the order of the statistics used to measure
%                       (in)dependence [3]
%
%       'Epochs'        : the list of trials for which a joint ICA needs to
%                       be calculated [1:EEG.trials]
%
%       'ChanList'      : the ICA calculations will be limited to the
%                       EEG channel list given in its argument [1:#EEGChannels],
%                       channel lists can be constructed with the function 
%                       <a href="matlab: doc GazeEEG_fuseChannelLists.m">GazeEEG_fuseChannelLists.m</a>
%
%       'Unfold'        : one ICA for all data is calculated when this
%                       option is set to true, otherwise each epoch is
%                       treated independently [false]
%
%       'VisualFB'      : get visual feedback about the components [false]
%                       
% 		'CalledFrom'	: set this option to 'BrainAnalyzer' if data comes from BrainAnalyzer
%
% 		'EyeData' 		: may be 'Gaze', 'EOG', 'Logic', 'GazeEOG', 'GazeLogic', 'EOGLogic' or 'all'

% History
% -------
% *** 2011-11-15 R. Phlypo
% Birth of file

global EEG
global ALLEEG
global CURRENTSET

% Default values
ord = 3;
epochList = 1:EEG.trials;
ChanList = 1:length(find( strcmpi( {EEG.chanlocs.type},'EEG')));
Unfold = false;
VisualFB = false;
CalledFrom = 'GazeEEG_Toolbox';
EyeData = 'Gaze';

for k = 1:2:nargin
    if strcmpi(varargin{k},'StatOrd')
        ord = varargin{k+1};
    elseif strcmpi(varargin{k},'Epochs')
        epochList = varargin{k+1};
    elseif strcmpi(varargin{k},'ChanList')
        ChanList = GazeEEG_fuseChannelLists(varargin{k+1});
    elseif strcmpi(varargin{k},'Unfold')
        Unfold = logical(varargin{k+1});
    elseif strcmpi(varargin{k},'VisualFB')
        VisualFB = logical(varargin{k+1});
	elseif strcmpi(varargin{k},'CalledFrom')
		CalledFrom = char(varargin{k+1});
	elseif strcmpi(varargin{k},'EyeData')
		EyeData = char(varargin{k+1});
    end
end
ixEpoch = epochList;

% Find appropriate partitioning of the data in the EEG structure in 
%   |- EEG
%   |- EOG
%   |- Gaze
% This is done using the type field of the chanlocs


ixEEG   = find( strcmpi( {EEG.chanlocs.type},'EEG'));
ixRef   = find( strcmpi( {EEG.chanlocs.type},'Ref'));
switch EyeData
	case 'Gaze'
		ixGaze  = find( strcmpi( {EEG.chanlocs.type},'Gaze'));
		ixEOG   = find( strcmpi( {EEG.chanlocs.type},'EOG') | strcmpi( {EEG.chanlocs.type},'Logic'));
	case 'EOG'
		ixGaze 	= find( strcmpi( {EEG.chanlocs.type},'EOG'));
		ixEOG  	= find( strcmpi( {EEG.chanlocs.type},'Gaze') | strcmpi( {EEG.chanlocs.type},'Logic'));
	case 'Logic'
		ixGaze	= find( strcmpi( {EEG.chanlocs.type},'Logic'));
		ixEOG 	= find( strcmpi( {EEG.chanlocs.type},'Gaze') | strcmpi( {EEG.chanlocs.type},'Gaze'));
	case 'GazeEOG'
		ixGaze	= find( strcmpi( {EEG.chanlocs.type},'EOG') | strcmpi( {EEG.chanlocs.type},'Gaze'));
		ixEOG  	= find( strcmpi( {EEG.chanlocs.type},'Logic'));
	case 'GazeLogic'
		ixGaze	= find( strcmpi( {EEG.chanlocs.type},'Gaze') | strcmpi( {EEG.chanlocs.type},'Logic'));
		ixEOG  	= find( strcmpi( {EEG.chanlocs.type},'EOG'));
	case 'EOGLogic'
		ixGaze	= find( strcmpi( {EEG.chanlocs.type},'EOG') | strcmpi( {EEG.chanlocs.type},'Logic'));
		ixEOG  	= find( strcmpi( {EEG.chanlocs.type},'Gaze'));
	case 'all'
		ixGaze	= find( strcmpi( {EEG.chanlocs.type},'Gaze') | strcmpi( {EEG.chanlocs.type},'Logic') | strcmpi( {EEG.chanlocs.type},'EOG'));
		ixEOG  	= [];
	otherwise
		fprintf('Invalid Option: %s\n',EyeData);
end
		
			

% Are there channels that are asked to be in the EEG reference set that are
% not belonging to the group of EEG channels ?
ixNotEEG = setdiff(GazeEEG_fuseChannelLists(ChanList),ixEEG);
for k = 1:length(ixNotEEG)
    warning( 'GazeEEG:InvalidChannelEEG', ...
        '%s is not an EEG channel, skipping entry and continuing ...', ...
        EEG.chanlocs(ixNotEEG(k)).labels);
end
ixEEG1  = intersect(ixEEG,GazeEEG_fuseChannelLists(ChanList));
ixEEG2  = setdiff(ixEEG,ixEEG1);

% initialise the ICA component activation patterns 
EEG.icaact = EEG.data;

if ~Unfold % Each epoch in epochList has its own transformation   
    for ixEpoch = epochList
        
        fprintf('Processing epoch %i\n',ixEpoch);
        
        % Some channels should be excluded from any further analysis since they
        % contain only NaN's (e.g., corrupted Gaze channel)
        ixExclude = find( all( isnan(reshape( EEG.data(:,:,ixEpoch), EEG.nbchan, [])), 2));     
        ixValid = setdiff( 1:EEG.nbchan, ixExclude);
        
        if ~isempty(ixExclude)
            for k = 1:length(ixExclude)
                warning( 'GazeEEG:InvalidChannel', ...
                    'The channel %s (channel nr %i) has not been validated and will be removed from the reference set.', ...
                    EEG.chanlocs(ixExclude(k)).labels, ixExclude(k) );
            end
        end
        
        ixEEG_EP        = intersect( ixEEG, ixValid);
        ixEEG1_EP       = intersect( ixEEG1, ixValid);
        ixEEG2_EP       = intersect( ixEEG2, ixValid);
        ixGaze_EP       = intersect( ixGaze, ixValid);
        ixEOG_EP        = intersect( ixEOG, ixValid);
        ixRef_EP        = intersect( ixRef, ixValid);
        
        GazeSignal      = squeeze(EEG.data(ixGaze_EP,:,ixEpoch));
        isNotNaNGaze    = find(~isnan(sum(GazeSignal)));
        GazeSignal      = GazeSignal(:,isNotNaNGaze);
        EEGSignal1      = squeeze(EEG.data(ixEEG1_EP,isNotNaNGaze,ixEpoch));
        EEGSignal2      = squeeze(EEG.data(ixEEG2_EP,isNotNaNGaze,ixEpoch));
        EOGSignal       = squeeze(EEG.data(ixEOG_EP,isNotNaNGaze,ixEpoch));
        RefSignal       = squeeze(EEG.data(ixRef_EP,isNotNaNGaze,ixEpoch));
        L               = length(isNotNaNGaze);

        muGlobal        = zeros(EEG.nbchan,1);
        
        % data dimension reduction ?
        % baseline correction or mean subtraction ?
        muEEG1              = mean(EEGSignal1,2);
        muEEG2              = mean(EEGSignal2,2);
        muGaze              = mean(GazeSignal,2);
        muEOG               = mean(EOGSignal,2);
        muRef               = mean(RefSignal,2);
        
        muGlobal(ixEEG1_EP)    = muEEG1;
        muGlobal(ixEEG2_EP)    = muEEG2;
        muGlobal(ixEOG_EP)     = muEOG;
        muGlobal(ixRef_EP)     = muRef;
        muGlobal(ixGaze_EP)    = muGaze;
        
        % The subtraction of the mean is represented by an extra column in
        % EEG.icasphere
        EEG.epoch(ixEpoch).mu           = zeros(EEG.nbchan,1);
        EEG.epoch(ixEpoch).mu(ixEEG1_EP)= muEEG1;
        EEG.epoch(ixEpoch).mu(ixEEG2_EP)= muEEG2;
        EEG.epoch(ixEpoch).mu(ixGaze_EP)= muGaze;
        EEG.epoch(ixEpoch).mu(ixEOG_EP) = muEOG;
        EEG.epoch(ixEpoch).mu(ixRef_EP)	= muRef;
        
        [whiteEEG1,Qb1,Sb1] = GazeEEG_prewhitenData(EEGSignal1  - muEEG1*ones(1,L));
        [whiteEEG2,Qb2,Sb2] = GazeEEG_prewhitenData(EEGSignal2  - muEEG2*ones(1,L));
        [whiteGaze,Qe,Se]   = GazeEEG_prewhitenData(GazeSignal  - muGaze*ones(1,L));
        [whiteEOG,Qo,So]    = GazeEEG_prewhitenData(EOGSignal   - muEOG*ones(1,L));
        [whiteRef,Qr,Sr]    = GazeEEG_prewhitenData(RefSignal   - muRef*ones(1,L));
        
%         figure, plot( Sb1);
       
        % CCA-like :
        % EEGsignal1 - muEEG1 = Qb1 * whiteEEG1 = Qb1 * V * whiteEEG1a;
        [V,S,U] = svd(whiteEEG1*whiteGaze'/(length(whiteEEG1)-1));
        if VisualFB
            figure, subplot()
            plot( diag( S))
            title('SVD Spectrum')
            xlabel('Component Number')
            ylabel('Absolute Amplitude')
        end
        
        whiteEEG1a = V(:,1:length(ixGaze_EP))'*whiteEEG1;
        whiteEEG1b = V(:,length(ixGaze_EP)+1:end)'*whiteEEG1;
        
        whiteGaze = U'*whiteGaze;
   
        % Sphering of the data (prewhitening)
        % Should this include dimension reduction ?
        EEG.epoch(ixEpoch).icasphere  = eye(EEG.nbchan);
        EEG.epoch(ixEpoch).icasphere(ixEEG1_EP,ixEEG1_EP)   = Qb1*V;
        EEG.epoch(ixEpoch).icasphere(ixEEG2_EP,ixEEG2_EP)	= Qb2;
        EEG.epoch(ixEpoch).icasphere(ixGaze_EP,ixGaze_EP)	= Qe*U;
        EEG.epoch(ixEpoch).icasphere(ixEOG_EP,ixEOG_EP)     = Qo;
        EEG.epoch(ixEpoch).icasphere(ixRef_EP,ixRef_EP)     = Qr;
        EEG.icasphere = EEG.epoch(ixEpoch).icasphere;

        % run the actual Joint ICA algorithm
        [y, Q]  = GazeEEG_referencedICA(whiteEEG1a, whiteGaze, min(size(whiteGaze,1),3), ord);
        
        % recuperate the blocks corresponding to the data transformations
        [A1,A2] = GazeEEG_getDiagBlocks(Q,[length(ixGaze_EP) length(ixGaze_EP)]);

%         figure
%         for k = 1:length(ixGaze)
%             subplot(length(ixGaze),2,2*k-1)
%             plot( whiteEEG1a(k,:),'k') 
%             hold all
%             plot( whiteGaze(k,:),'b')
%             plot( y(k,:), 'r');
%             plot( y(length(ixGaze)+k,:), 'g')
%             
%             subplot(length(ixGaze),2,2*k)
%             plot( squeeze(EEG.data(ixGaze(k),:,ixEpoch)))
%         end
        
        
        % Attribute the icaweights, the extra column contains zeroes only
        EEG.epoch(ixEpoch).icaweights   = eye(EEG.nbchan);
        EEG.epoch(ixEpoch).icaweights(ixEEG1(1:length(ixGaze_EP)),ixEEG1(1:length(ixGaze_EP)))   = A1;
        EEG.epoch(ixEpoch).icaweights(ixGaze_EP,ixGaze_EP)   = A2;
        EEG.icaweights = EEG.epoch(ixEpoch).icaweights;

        % The inverse matrix, excluding the mean correction
        EEG.epoch(ixEpoch).icawinv  = EEG.epoch(ixEpoch).icaweights'*pinv(EEG.epoch(ixEpoch).icasphere);
        EEG.icawinv                 = EEG.epoch(ixEpoch).icawinv;

        % The independent components' activation patterns 
        EEG.icaact(ixValid,:,ixEpoch)= EEG.icawinv(ixValid,ixValid)*( squeeze(EEG.data(ixValid,:,ixEpoch)) - EEG.epoch(ixEpoch).mu(ixValid)*ones(1,EEG.pnts) );

        % copy to EEGLAB memory
         if ~strcmpi(CalledFrom,'BrainAnalyzer')
			[ALLEEG EEG]            = eeg_store(ALLEEG, EEG, CURRENTSET); % copy to EEGLAB memory
		end
    end
else % calculate ICA on the epochs in "epochList", apply transformation to all epochs
    
    fprintf('Processing global data\n');
    
	% a channel containing only NaN should be excluded
    ixExclude = find( all( isnan(reshape( EEG.data(:,:,:), EEG.nbchan, [])), 2)); 
	% valid channels are all channels that have not been excluded
    ixValid = setdiff( 1:EEG.nbchan, ixExclude);

    if ~isempty(ixExclude)
        for k = 1:length(ixExclude)
            warning( 'GazeEEG:InvalidChannel', ...
                ['The channel %s (channel nr %i) has not been validated and will be removed from the reference set.\n' ...
                'The channel probably contains only NaN'], ...
                EEG.chanlocs(ixExclude(k)).labels, ixExclude(k) );
        end
    end
    
    ixEEG   = intersect( ixEEG, ixValid);
    ixEEG1  = intersect( ixEEG1, ixValid);
    ixEEG2  = intersect( ixEEG2, ixValid);
    ixGaze  = intersect( ixGaze, ixValid);
    ixEOG   = intersect( ixEOG, ixValid);
    ixRef   = intersect( ixRef, ixValid);
    
    mu = zeros(EEG.nbchan,EEG.trials);
    % baseline correction or mean subtraction ?
    for ixEpoch = 1:EEG.trials
        isNotNaNGaze = ~any(isnan(squeeze(EEG.data(:,:,ixEpoch))));
        mu(:,ixEpoch) = squeeze(nanmean(EEG.data(:,isNotNaNGaze,ixEpoch),2));
        % store each epoch's mean in the associated vector mu
        EEG.epoch(ixEpoch).mu(ixEEG1)   = mu(ixEEG1,ixEpoch);
        EEG.epoch(ixEpoch).mu(ixEEG2)   = mu(ixEEG2,ixEpoch);
        EEG.epoch(ixEpoch).mu(ixGaze)   = mu(ixGaze,ixEpoch);
        EEG.epoch(ixEpoch).mu(ixEOG)    = mu(ixEOG,ixEpoch);
        EEG.epoch(ixEpoch).mu(ixRef)    = mu(ixRef,ixEpoch);
    end
    % mean electrodes * epochs
    muGlobal        = kron(mu(:, epochList),ones(1,EEG.pnts));
    mu              = kron(mu, ones(1,EEG.pnts));
    
    GazeSignal      = reshape(EEG.data(ixGaze,:,epochList),length(ixGaze),EEG.pnts*length(epochList));
    isNotNaNGaze    = find(~any(isnan(GazeSignal),1));
    GazeSignal      = GazeSignal(:,isNotNaNGaze);
    EEGSignal1      = reshape(EEG.data(ixEEG1,:,epochList),length(ixEEG1),EEG.pnts*length(epochList));
    EEGSignal1      = EEGSignal1(:,isNotNaNGaze);
    EEGSignal2      = reshape(EEG.data(ixEEG2,:,epochList),length(ixEEG2),EEG.pnts*length(epochList));
    EEGSignal2      = EEGSignal2(:,isNotNaNGaze);
    EOGSignal       = reshape(EEG.data(ixEOG,:,epochList),length(ixEOG),EEG.pnts*length(epochList));
    EOGSignal       = EOGSignal(:,isNotNaNGaze);
    RefSignal       = reshape(EEG.data(ixRef,:,epochList),length(ixRef),EEG.pnts*length(epochList));
    RefSignal       = RefSignal(:,isNotNaNGaze);
    
    L               = length(isNotNaNGaze);
    
    muGlobal        = muGlobal(:,isNotNaNGaze);
    
    % data dimension reduction ?
    muEEG1          = muGlobal(ixEEG1,:);
    muEEG2          = muGlobal(ixEEG2,:);
    muEOG           = muGlobal(ixEOG,:);
    muGaze          = muGlobal(ixGaze,:);
    muRef           = muGlobal(ixRef,:);
    
    [whiteEEG1,Qb1,Sb1] = GazeEEG_prewhitenData(EEGSignal1 - muEEG1);
    [whiteEEG2,Qb2,Sb2] = GazeEEG_prewhitenData(EEGSignal2 - muEEG2);
    [whiteGaze,Qe,Se]   = GazeEEG_prewhitenData(GazeSignal - muGaze);
    [whiteEOG,Qo,So]    = GazeEEG_prewhitenData(EOGSignal - muEOG);
    [whiteRef,Qr,Sr]    = GazeEEG_prewhitenData(RefSignal - muRef);
    
    EEG.icasphere                   = eye(EEG.nbchan);
    EEG.icasphere(ixEEG1,ixEEG1)    = Qb1;
    EEG.icasphere(ixEEG2,ixEEG2)    = Qb2;
    EEG.icasphere(ixGaze,ixGaze)    = Qe;
    EEG.icasphere(ixEOG,ixEOG)      = Qo;
    EEG.icasphere(ixRef,ixRef)      = Qr;
    
    
    [y, Q]  = GazeEEG_referencedICA(whiteEEG1, whiteGaze, min(size(whiteGaze,1),3), ord);

    [A1,A2] = GazeEEG_getDiagBlocks(Q,[length(ixEEG1) length(ixGaze)]);
    
    EEG.icaweights  = eye(EEG.nbchan);
    EEG.icaweights(ixEEG1,ixEEG1)    = A1; % Store existing ICA weights matrix
    EEG.icaweights(ixGaze,ixGaze)    = A2;
    
    % The inverse matrix, (including the mean correction ?)
    EEG.icawinv     = EEG.icaweights'*pinv(EEG.icasphere);
    
    % The independent components' activation patterns 
    EEG.icaact              = reshape(EEG.data,EEG.nbchan,EEG.pnts*EEG.trials);
    EEG.icaact(ixValid,:)   = EEG.icawinv(ixValid,ixValid)*(EEG.icaact(ixValid,:) - mu(ixValid,:));
    EEG.icaact              = reshape(EEG.icaact,EEG.nbchan,EEG.pnts,EEG.trials);
    
    %     EEG.data(:,:,epoch)     =
	if ~strcmpi(CalledFrom,'BrainAnalyzer')
		[ALLEEG EEG]            = eeg_store(ALLEEG, EEG, CURRENTSET); % copy to EEGLAB memory
	end
end