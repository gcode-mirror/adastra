function [NotListed, NotInRecording] = GazeEEG_setElectrodeLocations()

% [NotListed, NotInRecording] = GazeEEG_setElectrodeLocations()
%
% This function partially constructs the electrode locations for a given
% set under the hypothesis that they can be found in the file
% Standard-10-20-Cap81.ced distributed with EEGlab

global EEGLabPath
global EEG
global ALLEEG
global CURRENTSET

if isempty( EEGLabPath)
    error('GazeEEG:EEGLabPath','EEGLab path ')
end

fid = fopen([EEGLabPath '\sample_locs\Standard-10-20-Cap81.ced']);
tempS = fgetl(fid);
fclose( fid);
eval(['[' tempS '] = textread(''' EEGLabPath '\sample_locs\Standard-10-20-Cap81.ced'',''%u %s %d %f %f %f %f %f %f %u'',''headerlines'',1);'])

Listed = [];
InFile = [];
for k = 1:EEG.nbchan
    ixInFile = find(strcmpi(labels, EEG.chanlocs(k).labels));
    if ~isempty(ixInFile)
        Listed = [Listed k];
        InFile = [InFile ixInFile];
        EEG.chanlocs(k).theta       = theta(ixInFile);
        EEG.chanlocs(k).X           = X(ixInFile);
        EEG.chanlocs(k).Y           = Y(ixInFile);
        EEG.chanlocs(k).Z           = Z(ixInFile);
        EEG.chanlocs(k).radius      = radius(ixInFile);
        EEG.chanlocs(k).sph_phi     = sph_phi(ixInFile);
        EEG.chanlocs(k).sph_radius  = sph_radius(ixInFile);
        EEG.chanlocs(k).sph_theta   = sph_theta(ixInFile);
    end
end

NotInRecording = setdiff(1:length(labels),InFile);
NotInRecording = {labels{NotInRecording}};

NotListed = setdiff(1:EEG.nbchan, Listed);
NotListed = {EEG.chanlocs(NotListed).labels};

[ALLEEG EEG] = eeg_store(ALLEEG, EEG, CURRENTSET);