function [r] = analyticroots(a)

% function [r] = analyticroots(a)
%
% a function to calculate the roots of a fourth order equation analytically
% with Ferrari's method
%
% The equation to be rooted is given as:
%       a(1)*x^4 + a(2)*x^3 + a(3)*x^2 + a(4)*x + a(5) = 0
%
% Inputs
% ------
%   a   : the coefficients in decreasing order of powers of x
%
% Outputs
% -------
%   r   : the four roots of the equation
%
% History
% -------
% *** Created 2007-12-03 by R. Phlypo

if size(a,2)>size(a,1)
    a = a';
end
if length(a) < 5 
    a = [zeros(5-length(a),1); a];
elseif length(a) > 5
    E = eye(length(a));
    E = E(2:end,:);
    E = [E; -p(end:-1:2)/p(1)];
    r = eig(E);
end

% The first order equation
if ~abs(a(1)) && ~abs(a(2)) && ~abs(a(3)) && abs(a(4))
    r = -a(5)/a(4);
% The quadratic equation
elseif ~abs(a(1)) && ~abs(a(2)) && abs(a(3))
    r = -a(4)/a(3)*[.5; .5] + [.5; -.5]*sqrt(a(4)^2-4*a(3)*a(5))/a(3);
% The degenerate case of a cubic equation
elseif ~abs(a(1)) && abs(a(2))
    a = a/a(2);
    P = a(4) - a(3)^2/3;
    Q = a(5) + 2*a(3)^3/27 - 9*a(3)*a(4)/27;
    U = (-Q/2 + sqrt(Q^2/4 + P^3/27))^(1/3);
    U = [1; -.5+sqrt(-3)/2; -.5-sqrt(-3)/2]*U;
    r = -P/3./U + U - a(3)/3;
% The degenerate case of a biquadratic equation
elseif ~abs(a(2)) && ~abs(a(4))
    r = [1; 1; -1; -1].*sqrt(.5*([-1; -1; -1; -1]*a(3)+[1; -1; 1; -1]*sqrt(a(3)^2-4*a(1)*a(5)))...
        /a(1));
% The method of Ferrari 
elseif abs(a(1))
    a = a/a(1);
    % coefficients of a depressed quartic in u = x + a(2)/4*a(1)
    alfa = -3*a(2)^2/8 + a(3);
    beta = a(2)^3/8 - a(2)*a(3)/2 + a(4);
    gamma = -3*a(2)^4/256 + a(3)*a(2)^2/16 - a(2)*a(4)/4 + a(5);
    P = -alfa^2/12-gamma;
    Q = -alfa^3/108 + alfa*gamma/3-beta^2/8;
%     Q = alfa^3*91/108 + alfa*gamma/3-beta^2/8;%???
    U = (Q/2+sqrt(Q^2/4+P^3/27))^(1/3);

    if abs(U)
        y = -5*alfa/6 - U + P/3./U;
    else
        y = -5*alfa/6;
    end
    W = sqrt(alfa+2*y);
    r = -ones(4,1)*.25*a(2)/a(1) + [.5; .5; -.5; -.5]*W +...
        [.5; -.5; .5; -.5].*sqrt(-3*alfa - 2*y + [-2; -2; 2; 2]*beta/W);
else
    fprintf('No solution available\n');
end