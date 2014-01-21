function [x,W,Sx] = GazeEEG_prewhitenData(X)

% function [x,W] = GazeEEG_prewhitenData(X)
%
% Calucates the prewhitened data for X
% it returns the whitened data x and the whitening transform
% 
% X is of dimension m x n, then
%       with m > n, X = x*W,    or      x = X*inv(W) 
%       with m < n, X = W*x,   or      x = inv(W')*X

% History:
% --------
% *** 2011-06-30 R. Phlypo @ GIPSA Lab: Created function

transp = false;
if size(X,1) < size(X,2), transp = true; X = X'; end

[Vx,Sx,Ux] = svd(X,0);
W = Sx*Ux'/sqrt(length(Vx)-1);
x = Vx*sqrt(length(Vx)-1);

Sx = diag(Sx);

if transp, x = x'; W = W'; end