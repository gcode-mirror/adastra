 function GazeEEG_ConstrRefLogique()
% function GazeEEG_ConstrRefLogique()

global EEG
global ALLEEG
global CURRENTSET


% Construction de la référence, en recherchant d'abord les valeurs NaN
valNaN = NaN ;

if isempty(find( strcmpi( {EEG.chanlocs.type},'Logic'))) %Creation d'une reference logique si elle n'existe pas déjà

% Trouvons les canaux "Gaze"
ixGaze = find( strcmpi( {EEG.chanlocs.type}, 'Gaze'));

% Trouvons les indices aux valeurs NaN sur les canaux "Gaze"
if isnan( valNaN)
	ixNaN = isnan( sum( EEG.data( ixGaze, :)));
else
	ixNaN = find( any( EEG.data( ixGaze, :)==valNaN, 1));
end


SignalNaN         = zeros( 1, EEG.pnts*EEG.trials);
SignalBlink       = zeros( 1, EEG.pnts*EEG.trials);
SetToOne          = zeros( 1, EEG.pnts*EEG.trials);
SignalNaN(ixNaN)  = 1;

% n1 = figure; 
% plot(SignalNaN, 'b-')
% ylabel('SignalNaN')
% xlabel('iX')
% grid on

% On garde seulement les fronts descendants de ce signal
SetToZero = max([0 SignalNaN(1:end-1)] - SignalNaN,0); 

if sum(SetToZero) ~=0 % Il y a des NaN dans le signal Gaze
    
%     figure(n1)
%     subplot(4,1,1)
%     plot(SignalNaN, 'b-')
%     ylabel('SignalNaN')
%     xlabel('iX')
%     grid on
% 
%     subplot(4,1,2)
%     grid on
%     hold on
%     ylabel('SetToZero')
%     xlabel('iX')
%     plot(SetToZero, 'r-')

% Ensuite, cherchons les indices des evennements Blink qui seront les Mises à Un du signal logique  
IxEventBlink    = strcmpi({EEG.event.type}, 'flowBlinkLeft' ) | strcmpi({EEG.event.type}, 'flowBlinkRight');
SetToOne( horzcat(EEG.event(IxEventBlink).latency)) = 1;  %Sert uniquement pour la visualisation

iXSetToOne    = horzcat(EEG.event(IxEventBlink).latency);    %Indices de mise à Un du signal logique
iXSetToZero   = find(SetToZero ==1);   %Indices de mise à Zéro du signal logique

        if ~isempty(iXSetToZero ) %Il y a des blinks
            
iXLastSetZero = max(iXSetToZero); %Dernière action
iXFirstSetOne = min(iXSetToOne);  %Première action

iXSetToOne  = iXSetToOne(iXSetToOne < iXLastSetZero);     %La première action est une mise à Un
iXSetToZero = iXSetToZero(iXSetToZero > iXFirstSetOne);   %La dernière action est une mise à Zéro

TabSetToOneZero = [iXSetToOne iXSetToZero ; ones(1, length(iXSetToOne)) zeros(1, length(iXSetToZero))];  %Tableau des actions

[val, pos]      = sort([iXSetToOne iXSetToZero]); 
TabSetOneZero   = TabSetToOneZero(:, pos);

%Etablissement progressive du signal SignalBlink
for k = 1 : size(TabSetOneZero,2)
    SignalBlink(TabSetOneZero(1,k) : end) = TabSetOneZero(2,k);
end

% subplot(4,1,3)
% grid on
% hold on
% plot(SetToOne, 'g-')
% ylabel('SetToOne')
% xlabel('iX')
% 
% subplot(4,1,4)
% grid on
% hold on
% plot(SignalBlink, 'k-')
% ylabel('SignalBlink')
% xlabel('iX')

        end
end

% Finalement, sectionner suivant les essais et concatenner dans le tableau data
SignalBlink = reshape( SignalBlink, 1, EEG.pnts, EEG.trials);
EEG.data    = cat( 1, EEG.data, SignalBlink);

% Actualisation des paramètres
EEG.nbchan          = size(EEG.data, 1);
ixLogic             = EEG.nbchan;

EEG.chanlocs( ixLogic).type     = 'Logic';
EEG.chanlocs( ixLogic).labels   = 'LogicRef';

% Pour finir : Mettre à zéro les anciens NaN
if isnan( valNaN)
	for p = 1:length(ixGaze)
		EEG.data(ixGaze(p),isnan(EEG.data(ixGaze(p),:))) = 0;
	end
else
	for p = 1:length(ixGaze)
		EEG.data(ixGaze(p),EEG.data(ixGaze(p),:)==valNaN) = 0;
	end
end

% mettre à jour la structure dans la mémoire d'EEGlab
[ALLEEG EEG CURRENTSET] = eeg_store(ALLEEG, EEG, CURRENTSET);

end

