function GazeEEG_init( varargin)

% GazeEEG_init - initialisation function for the GazeEEG Toolbox
%
% Parameter         Value
% ---------         -----
% EEGLabPath        absolute path of the eeglab version the user wants to
%                       be called, specifying a path sets 'EEGLab' to true
%
% CleanWorkSpace    defines whether the sorkspace should be cleared prior
%                       to calling any other function [default:false]
%
% EEGLab            if eeglab can be found on the Matlab search path, evoke
%                       it by calling eeglab from the command line
%                       [default:false]

global EEGLabPath

%% File inspection
% Check whether all functions called from this script are on the Matlab
% path, returns an error if there are files missing
GazeEEG_CheckFileList();

%% Read Params
EEGLabPath = '';
CleanWorkspace = false;
evokeEEGLab = false;
for k = 1:2:length(varargin)
    if strcmpi(varargin{k},'eeglabpath')
        EEGLabPath = varargin{k+1};
        evokeEEGLab = true;
    elseif strcmpi(varargin{k},'CleanWorkSpace')
        CleanWorkspace = varargin{k+1};
    elseif strcmpi(varargin{k},'eeglab')
        evokeEEGLab = varargin{k+1};
    else
        error(sprintf('''%s'' is not a valid option, please verify the spelling.',varargin{k}))
    end
end

%% Starting with an empty workspace and closing all figures
if CleanWorkspace
    evalin('base','clear; close all; clc;')
end
%% set global variables in the base workspace
evalin('base','global EEG; global ALLEEG; global CURRENTSET; global Brainamp; global Eyelink;');
%% Evoke EEGLab
if evokeEEGLab
    if ~isempty(EEGLabPath)
        try
            evalin('base',sprintf('cd(''%s''); eeglab; cd(''%s'');',EEGLabPath,cd));
        catch
            if evalin('base','exist(''eeglab'')')
                fprintf(['No eeglab can be found in ''%s''\n'...
                    'Trying to evoke eeglab on the Matlab search path'],EEGLabPath);
                evalin('base','eeglab');
            else
                fprintf('No eeglab found, please verify your path: ''%s''\n',EEGLabPath);
            end
        end
    elseif ~evalin('base','exist(''eeglab'')')
        fprintf(['EEGLab has not been invoked\n' ...
            '\t1. no user defined path has been given\n' ...
            '\t2. eeglab is not found on your Matlab search path\n' ...
            'Some function of the toolbox may not work properly.']);
    else
        evalin('base','eeglab');
        [EEGLabPath] = fileparts(which('eeglab'));
    end
end
