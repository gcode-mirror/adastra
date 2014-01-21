function varargout = GazeEEG_getDiagBlocks(A,p)

%  function [A1, A2, A3, A4, ...] = GAazeEEG_getDiagBlocks(A,[d1, d2, d3, d4, ...])
%
% Gets the partitions in the partitioned matrix A, the dimension of the
% partitions are d1xd1, d2xd2, ... and the matrix is supposed block
% diagonal of dimension (d1+d2+d3+d4+...)x(d1+d2+d3+d4+...)

% History:
% --------
% *** 2011-07-01 R. Phlypo @ GIPSA Lab: Created function

% Dimensional consistency check
if length(size(A))~=2 || diff(size(A)), error('The block diagonal matrix should be square.'); end
if sum(p) > size(A,1), error('Partitioning is non consistent with the matrix dimensions');
elseif sum(p) < size(A,1), p(length(p)+1) =  size(A,1) - sum(p); end

if size(p,1)~=length(p), p = p'; end

pstart  = cumsum([0; p(1:length(p)-1)])+1;
pend    = cumsum(p);

for k = 1:length(p)
    varargout{k} = A(pstart(k):pend(k),pstart(k):pend(k));
end