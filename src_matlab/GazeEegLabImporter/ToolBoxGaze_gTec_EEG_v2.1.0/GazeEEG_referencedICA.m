function [y, Q] = GazeEEG_referencedICA(z1, z2, NbRef, ord, max_iter, Q)

% joint ICA analysis of z1 and z2 (also referred to as canonical dependence
% analysis)
%
% function [y, Q] = GazeEEG_referencedICA(z1, z2, NbRef, ord, max_iter, Q)
%
% z1 and z2 are column matrices, the matrix Q is a block diagonal matrix
% whose blocks can be recuperated through the function <a href="doc: matlab getDiagBlocks.m">getDiagBlocks.m</a>
%
% Input:
% ------
%
%   z1      : observations of a first modality, T samples, N1 sensors
%   z2      : observations of a second modality, T samples, N2 sensors
%   NbRef   : number of common signals, NbRef <= min(N1,N2), defaults to
%               [min(N1,N2)]
%   ord     : order at which dependence is measured, actually only orders 3
%               and [4] are available, 2 should be implemented soon
%   max_iter: only at orders 3 and 4, one may put an upper limit on the
%               iterations [10*ceil(1+sqrt(N1+N2))]
%   Q       : starting point, may be useful if one checks the optimisation
%               path iteration per iteration, i.e., inserting Q^[k-1] to
%               obtain Q^[k], k being the iteration index defaults to
%               [eye(N1+N2)]
%
% Output:
% -------
% first case: [T>N1 and T>N2]
% 
%   z1 � IR(T x N1)
%   z2 � IR(T x N2)
% then
%   y*Q = [z1 z2]
%
% second case: [T>N1 and T>N2]
%
%   z1 � IR(N1 x T)
%   z2 � IR(N2 x T)
% then
%   Q*y = [z1; z2]

% History:
% --------
% *** 2011-11-15 R. Phlypo
% finally documented the function

transp = false;
if size(z1,1) < size(z1,2), transp = true; z1 = z1'; end 
if nargin < 2, z2 = []; end
if transp, z2 = z2'; end
sz = [size(z1,2) size(z2,2)];
if nargin < 3 || isempty( NbRef), NbRef = min(sz); end
if nargin < 4 || isempty( ord), ord = 4; end
if nargin < 5 || isempty( max_iter), max_iter = inf; end
if nargin < 6 || isempty( max_iter), Q = eye(sum(sz)); end
iter = 1;

max_iter = min(max_iter, 10*ceil(1+sqrt(sum(sz))) );

% F(1) = 0;

while iter == 1 || (cumt(iter-1) > 1e-2 && iter <= max_iter);
    fprintf('%i/%i\n', iter, max_iter);
    %F(iter+1) = F(iter);
    F(iter) = 0;
    cumt(iter) = 0;
    for k = 1:2%[2 1]
        kbar = setdiff([1 2], k);
        for i = 1:sz(k)-1
            for j= i+1:sz(k)
                
                if i <= min(sz(kbar),NbRef)
                    if j <= min(sz(kbar),NbRef)
                        x = eval(['[z' num2str(k,'%i') '(:,[i j]) z' num2str(kbar,'%i') '(:,[i j])]']);
                    else
                        x = eval(['[z' num2str(k,'%i') '(:,[i j])  z' num2str(kbar,'%i') '(:,i) zeros(size(z1,1),1)]']);
                    end
                    
                    if ord == 4
                        
                        xx = zeros(size(x,1),4*(4+1)/2);
                        ixarray = cumsum([0 4:-1:1]);
                        for jj = 1:4
                            xx(:,ixarray(jj)+1:ixarray(jj+1)) = (x(:,jj)*ones(1,4-jj+1)).*x(:,jj:4);
                        end
                        M4 = xx'*xx/length(x);
                        M2 = x'*x/length(x);
                        g4000 = M4(1,1)  - 3*M2(1,1)^2;
                        g0400 = M4(5,5)  - 3*M2(2,2)^2;
                        g3100 = M4(2,1)  - 3*M2(2,1)*M2(1,1);
                        g3010 = M4(3,1)  - 3*M2(3,1)*M2(1,1);
                        g3001 = M4(4,1)  - 3*M2(4,1)*M2(1,1);
                        g1300 = M4(5,2)  - 3*M2(1,2)*M2(2,2);
                        g0310 = M4(5,6)  - 3*M2(2,3)*M2(2,2);
                        g0301 = M4(5,7)  - 3*M2(2,4)*M2(2,2);
                        g1030 = M4(8,3)  - 3*M2(1,3)*M2(3,3);
                        g0130 = M4(8,6)  - 3*M2(2,3)*M2(3,3);
                        g1003 = M4(4,10) - 3*M2(1,4)*M2(4,4);
                        g0103 = M4(7,10) - 3*M2(2,4)*M2(4,4);
                        g2200 = M4(1,5)  - M2(1,1)*M2(2,2) - 2*M2(1,2)^2;
                        g2020 = M4(1,8)  - M2(1,1)*M2(3,3) - 2*M2(1,3)^2;
                        g2002 = M4(1,10) - M2(1,1)*M2(4,4) - 2*M2(1,4)^2;
                        g0220 = M4(5,8)  - M2(2,2)*M2(3,3) - 2*M2(2,3)^2;
                        g0202 = M4(5,10) - M2(2,2)*M2(4,4) - 2*M2(2,4)^2;
                        g2110 = M4(1,6)  - M2(1,1)*M2(2,3) - 2*M2(1,2)*M2(1,3);
                        g2101 = M4(1,7)  - M2(1,1)*M2(2,4) - 2*M2(1,2)*M2(1,4);
                        g1210 = M4(3,5)  - M2(2,2)*M2(1,3) - 2*M2(1,2)*M2(2,3);
                        g1201 = M4(4,5)  - M2(2,2)*M2(1,4) - 2*M2(2,1)*M2(2,4);
                        g1120 = M4(2,8)  - M2(3,3)*M2(1,2) - 2*M2(3,1)*M2(3,2);
                        g1102 = M4(2,10) - M2(4,4)*M2(1,2) - 2*M2(4,1)*M2(4,2);
                        
                        a(1) = g3100*g4000 + g0103*g1003 - g0400*g1300 - g0130*g1030 + ...
                            3*(g2101*g3001 + g1102*g2002 - g0310*g1210 - g0220*g1120); % xi^8
                        a(2) = g4000^2 + g0400^2 + g0130^2 + g1003^2 - g1030^2 - g0103^2 + ...
                            3*(g3001^2 + g0310^2 + g0220^2 + g2002^2 - g2200*g4000 - g0400*g2200 - g0220*g2020 - g0202*g2002) - ...
                            4*(g3100^2 + g1300^2) - 6*(g1201*g3001 + g0310*g2110 + g1120^2 + g1102^2) - ...
                            9*(g2101^2 + g1210^2); % xi^7
                        a(3) = 2*(g0103*g1003 - g0130*g1030) + ...
                            3*(g1300*g4000 - g0400*g3100 - g0310*g3010 + g0301*g3001 - g1102*g2002 + g0220*g1120) + ...
                            7*(-g3100*g4000 + g0400*g1300) + 9*(g0202*g1102 - g1120*g2020) + ...
                            12*(g0310*g1210 - g2101*g3001) + 18*(g2200*g3100 - g1300*g2200) + ...
                            27*(g1201*g2101 - g1210*g2110);
                        a(4) = -2*g0400*g4000 + ...
                            3*(g3001^2 + g1003^2 + g0310^2 + g0130^2 - g1030^2 - g0103^2 - g2020^2 - g0202^2 - g0220*g2020 - g0202*g2002) + ...
                            6*(g1201*g3001 + g0310*g2110 + g2002^2 - g1120^2 - g1102^2) + ...
                            9*(g2200*g4000 + g0400*g2200 + g2101^2 + g1210^2) + ...
                            12*(g3100^2 + g1300^2 - g1210*g3010 - g0301*g2101) - 18*(g1201^2 + g2110^2) - ...
                            32*g1300*g3100 - 36*g2200^2;
                        a(5) = 10*(g0400*g3100 - g4000*g1300) + ...
                            15*(g0202*g1102 + g0220*g1120 + g0301*g1201 + g0310*g1210 - g1102*g2002 - g1120*g2020 - g2101*g3001 - g2110*g3010) + ...
                            60*(g1300*g2200 - g3100*g2200);
                        a(6) = 2*g0400*g4000 + ...
                            3*(g1003^2 + g0130^2 - g3010^2 - g1030^2 - g0301^2 - g0103^2 + g0220*g2020 + g2002^2 + g0202*g2002 + g0220^2) + ...
                            6*(g1120^2 + g1102^2 - g2020^2 - g0202^2 - g1210*g3010 - g0301*g2101) + ...
                            9*(-g2200*g4000 - g0400*g2200 - g2110^2 -g1201^2) + ...
                            12*(-g3100^2 + g1201*g3001 + g0310*g2110 - g1300^3) + 18*(g1210^2 + g2101^2) + ...
                            32*g1300*g3100 + 36*g2200^2;
                        a(7) = 2*(g0130*g1030 - g0103*g1003) + ...
                            3*(g1300*g4000 - g0400*g3100 + g0310*g3010 - g0301*g3001 - g1120*g2020 + g0202*g1102) + ...
                            7*(g0400*g1300 - g3100*g4000) + 9*(g0220*g1120 - g1102*g2002) + ...
                            12*(g0301*g1201 - g2110*g3010) + 18*(g2200*g3100 - g1300*g2200) + ...
                            27*(g1210*g2110 - g1201*g2101);
                        a(8) = - g4000^2 - g0400^2 - g1030^2 + g1003^2 + g0130^2 - g0103^2 + ...
                            3*(g2200*g4000 + g0400*g2200 + g0220*g2020 + g0202*g2002 -  g0202^2 - g2020^2 - g3010^2 - g0301^2) + ...
                            4*(g3100^2 + g1300^2) + 6*(g1210*g3010 + g0301*g2101 + g1120^2 + g1102^2);
                        a(9) = g3100*g4000 - g1300*g0400 + g0130*g1030 - g0103*g1003 + ...
                            3*(g3010*g2110 - g0301*g1201 + g1120*g2020 - g0202*g1102);

    %                     while abs(a(1)) < eps, a = a(2:end); end

                        a = a/a(1);

                        t = [real(eig([[zeros(1,length(a)-2); eye(length(a)-2)] -flipud(a(2:end)')])); 0];

                        b(1) = g4000^2 + 4*g3001^2 + 6*g2002^2 + 4*g1003^2 + g0400^2 + 4*g0310^2 + 6*g0220^2 + 4*g0130^2;
                        b(2) = 8*( g0400*g1300 - g4000*g3100 + g0130*g1030 - g0103*g1003) + ...
                            24*(g0310*g1210 - g3001*g2101 + g1120*g0220 - g1102*g2002);
                        b(3) = 4*( g3001^2 + g1030^2 + g0310^2 + g0103^2) + ...
                            12*(g4000*g2200 + g0400*g2200 + g0220*g2020 + g0202*g2002 + g1003^2 + g0130^2 + g0220^2 + g2002^2) + ...
                            16*(g3100^2 + g1300^2) + 24*(g1201*g3001 + g0310*g2110 + g1120^2 + g1102^2) + 36*(g2101^2 + g1210^2);
                        b(4) = 8*(-g1300*g4000 + g0400*g3100 + g0310*g3010 - g0301*g3001) + ...
                            24*(-g2101*g3001 + g1120*g2020 + g0310*g1210 - g0202*g1102 + g0130*g1030 - g0103*g1003) + ...
                            48*(-g2200*g3100 + g1300*g2200 - g1102*g2002 + g0220*g1120) + 72*(g1210*g2110 - g1201*g2101);
                        b(5) = 4*g0400*g4000 + 6*(g2020^2 + g2002^2 + g0220^2 + g0202^2) + 12*(g1030^2 + g1003^2 + g0130^2 + g0103^2) + ...
                            24*(g1210*g3010 + g1201*g3001 + g0310*g2110 + g0301*g2101 + g0220*g2020 + g0202*g2002) + ...
                            36*(g2110^2 + g2101^2 + g1210^2 + g1201^2) + 48*(g1120^2 + g1102^2) + ...
                            64*g1300*g3100 + 72*g2200^2;
                        b(6) = 8*( g4000*g1300 - g0400*g3100 + g0310*g3010 - g0301*g3001) + ...
                            24*(g2110*g3010 - g0301*g1201 - g1102*g2002 + g0220*g1120 + g0130*g1030 - g0103*g1003) + ...
                            48*(g2200*g3100 - g1300*g2200 + g1120*g2020 - g0202*g1102) + 72*(g1210*g2110 - g1201*g2101);
                        b(7) = 4*(g3010^2 + g0301^2 + g1003^2 + g0130^2) + ...
                            12*(g2200*g4000 + g0400*g2200 + g2020^2 + g0220*g2020 + g0202*g2002 + g1030^2 + g0202^2 + g0103^2) + ...
                            16*(g3100^2 + g1300^2) + 24*(g1210*g3010 + g0301*g2101 + g1120^2 + g1102^2) + 36*(g2110^2 + g1201^2);
                        b(8) = 8*(g3100*g4000 - g0400*g1300 + g0130*g1030 - g0103*g1003) + ...
                            24*(g2110*g3010 + g1120*g2020 - g0301*g1201 - g0202*g1102);
                        b(9) = g4000^2 + 4*g3010^2 + 6*g2020^2 + 4*g1030^2 + g0400^2 + 4*g0301^2 + 6*g0202^2 + 4*g0103^2;

                        [nu,ix] = max( (b(1)*t.^8 + b(2)*t.^7 + b(3)*t.^6 + b(4)*t.^5 + b(5)*t.^4 + b(6)*t.^3 + b(7)*t.^2 + b(8)*t.^1 + b(9))./(1+t.^2).^4);
                        Fgain = nu - b(9);
                    else
                        g3000 = (x(:,1).^2)'*x(:,1)/length(x);
                        g2100 = (x(:,1).^2)'*x(:,2)/length(x);
                        g1200 = x(:,1)'*x(:,2).^2/length(x);
                        g0300 = (x(:,2).^2)'*x(:,2)/length(x);
                        g1101 = (x(:,1).*x(:,2))'*x(:,4)/length(x);
                        g1110 = (x(:,1).*x(:,2))'*x(:,3)/length(x);
                        g2010 = (x(:,1).^2)'*x(:,3)/length(x);
                        g2001 = (x(:,1).^2)'*x(:,4)/length(x);
                        g0210 = (x(:,2).^2)'*x(:,3)/length(x);
                        g0201 = (x(:,2).^2)'*x(:,4)/length(x);
                        g1020 = x(:,1)'*x(:,3).^2/length(x);
                        g1002 = x(:,1)'*x(:,4).^2/length(x);
                        g0120 = x(:,2)'*x(:,3).^2/length(x);
                        g0102 = x(:,2)'*x(:,4).^2/length(x);
                        
                        a(1) = 2*(g1101*g2001 - g1110*g0210) + g3000*g2100 - g0300*g1200 - g0120*g1020 + g1002*g0102;
                        a(2) = -4*(g1110^2 + g1101^2) - 3*(g2100^2 + g1200^2) + ...
                            2*(-g3000*g1200 - g0300*g2100 - g0210*g2010 + g2001^2 - g0201*g2001 + g0210^2) + ...
                            g3000^2 - g1020^2 + g1002^2 + g0300^2 + g0120^2 - g0102^2;
                        a(3) = 6*(g0201*g1101 - g2010*g1110) + 5*(g0300*g1200 - g3000*g2100)+...
                            4*(g0210*g1110 - g1101*g2001) + g0102*g1002^2 - g0120*g1020;
                        a(4) = 2*(-g2010^2 + g2001^2 - g1020^2 + g1002^2 + g0210^2 - g0201^2 + g0120^2 - g0102^2);
                        a(5) = 6*(g1110*g0210 - g1101*g2001) + 5*(g0300*g1200 - g3000*g2100) + ...
                            4*(g0201*g1101 - g2010*g1110) + g0120*g1020 - g1002*g1002;
                        a(6) = 4*(g1110^2 + g1101^2) + 3*(g2100^2 + g1200^2) + ...
                            2*(g3000*g1200 + g0300*g2100 - g2010^2 + g0210*g2010 + g0201*g2001 - g0201^2) - ...
                            g3000^2 - g0300^2 - g1020^2 + g1002^2 + g0120^2 - g0102^2;
                        a(7) = 2*(g1110*g2010 - g1101*g0201) + (g3000*g2100 - g0300*g1200 + g0120*g1020);
                        
                        a = a/a(1);

                        t = [real(eig([[zeros(1,length(a)-2); eye(length(a)-2)] -flipud(a(2:end)')])); 0];
                        
                        b(1) = 3*(g2001^2 + g1002^2 + g0210^2 + g0120^2) + g3000^2 + g0300^2;
                        b(2) = 12*(g1110*g0210 - g1101*g2001) + 6*(g0300*g1200 - g3000*g2100 + g0120*g1020 - g1002*g0102);
                        b(3) = 12*(g1110^2 + g1101^2) + 9*(g2100^2 + g1200^2) + ...
                            6*(g3000*g1200 + g0300*g2100 + g0210*g2010 + g0201*g2001 + g1002^2 + g0120^2) + ...
                            3*(g2001^2 + g1020^2 + g0210^2 + g0102^2);
                        b(4) = 12*(g1110*g2010 - g1101*g2001 + g0210*g1110 - g0201*g1101 + g0120*g1020 - g1002*g0102);
                        b(5) = 12*(g1110^2 + g1101^2) + 9*(g2100^2 + g1200^2) + ...
                            6*(g3000*g1200 + g0300*g2100 + g0210*g2010 + g0201*g2001 + g1020^2 + g0102^2) + ...
                            3*(g2010^2 + + g1002^2 + g0201^2 + g0120^2);
                        b(6) = 12*(g1110*g2010 - g1101*g0201) + 6*(g3000*g2100 - g0300*g1200 + g0120*g1020 - g1002*g0102);
                        b(7) = 3*(g2010^2 + g1020^2 + g0102^2 + g0201^2) + g3000^2 + g0300^2;
                        
                        [nu,ix] = max( (b(1)*t.^6 + b(2)*t.^5 + b(3)*t.^4 + b(4)*t.^3 + b(5)*t.^2 + b(6)*t + b(7))./(1+t.^2).^3);
			
%                         r = -10:.01:10;
%                         Fval = (b(1)*r.^6 + b(2)*r.^5 + b(3)*r.^4 + b(4)*r.^3 + b(5)*r.^2 + b(6)*r + b(7))./(1+r.^2).^3;
%                         figure, plot( r, Fval); hold on
%                         plot( t(ix(1)), nu(1), 'r*' );
% 
%                         pause
                        Fgain = nu - b(7);
                    end
                    t = t(ix(1));
                else
                    x = eval(['z' num2str(k,'%i') '(:,[i j])']);
                    
                    if ord == 4
                    
                        g4000 = (x(:,1).^3)'*x(:,1)/length(x) - 3*(x(:,1)'*x(:,1))^2/length(x)^2;
                        g3100 = (x(:,1).^3)'*x(:,2)/length(x) - 3*(x(:,1)'*x(:,1))*(x(:,1)'*x(:,2))/length(x)^2;
                        g2200 = (x(:,1).^2)'*x(:,2).^2/length(x) - (x(:,1)'*x(:,1))*(x(:,2)'*x(:,2))/length(x)^2 - 2*(x(:,1)'*x(:,2))^2/length(x)^2;
                        g1300 = (x(:,2).^3)'*x(:,1)/length(x) - 3*(x(:,2)'*x(:,2))*(x(:,2)'*x(:,1))/length(x)^2;
                        g0400 = (x(:,2).^3)'*x(:,2)/length(x) - 3*(x(:,2)'*x(:,2))^2/length(x)^2;

                        c = [4*(18*g2200^2 + 16*g3100*g1300 + g4000*g0400); ...
                            8*(6*g2200*(g1300-g3100) + g0400*g3100 - g4000*g1300); ...
                            4*( 3*g2200*(g4000 + g0400) + 4*(g3100^2 + g1300^2)); ...
                            8*(g0400*g1300 - g4000*g3100); ...
                            g4000^2 + g0400^2];
    
                        pol = [-c(4); 8*c(5) - 4*c(3); 3*(c(4) - c(2)); 4*(6*c(5) - c(1)); 4*(3*c(4) + c(2))];
                        while abs(pol(1)) < 1e3*min(eps(abs(pol(2))),eps(abs(pol(end))))
                            pol = pol(2:end);
                        end
                        
%                         pol
                        ksi = real(analyticroots(pol));%[-c(4); 8*c(5) - 4*c(3); 3*(c(4) - c(2)); 4*(6*c(5) - c(1)); 4*(3*c(4) + c(2))]));
%                         ksi
                        [nouse,ix] = max((c(5)*ksi.^4 + c(4)*ksi.^3 + (4*c(5) + c(3))*ksi.^2 + (3*c(4) + c(2))*ksi + 2*(c(5) + c(3)) + c(1))./(4+ksi.^2).^2);
                        ksi = ksi(ix(1));
                    
                        Fgain = nouse - c(5);
                    else
                        g3000 = (x(:,1).^2)'*x(:,1)/length(x);
                        g2100 = (x(:,1).^2)'*x(:,2)/length(x);
                        g1200 = x(:,1)'*x(:,2).^2/length(x);
                        g0300 = (x(:,2).^2)'*x(:,2)/length(x);
                        
                        c = [6*(g3000*g2100-g0300*g1200); ...
                            2*(3*(g3000^2 + g0300^2) - 3*( 2*( g3000*g1200 + g0300*g2100 ) + 3*(g2100^2 + g1200^2) )); ...
                            -24*(g3000*g2100-g0300*g1200)];
                        
                        ksi = (-c(2) + [1; -1]*sqrt(c(2)^2 - 4*c(1)*c(3)))/2/c(1);
                        [nouse,ix] = max( (c(1)*(ksi.^2+1) + c(2)*ksi + c(3))./(ksi.^2+4) );
                        ksi = ksi(ix(1));
                        Fgain = nouse - c(3)/4;
                        
                    end
                    
                    t = [ksi/2 + sqrt(ksi^2+4)/2; ksi/2 - sqrt(ksi^2+4)/2];
                    t = t(t<=1 & t> -1);
                    try 
                        t = t(1);
                    catch
                        t = 0;
%                         fprintf('A possible error occurred\n')
                    end
                    
                    
%                     r = -10:.01:10;
%                     fval = (c(5)*r.^4 + c(4)*r.^3 + (4*c(5) + c(3))*r.^2 + (3*c(4) + c(2))*r + 2*(c(5) + c(3)) + c(1))./(4+r.^2).^2;
%                     figure, plot( r, fval), hold on, plot( ksi, nouse(1), '+r'), hold off
%                     pause
                    
                end
                
                cumt(iter) = max(cumt(iter),Fgain);
                F(iter) = F(iter) + Fgain;
                
                %                 r = -10:.01:10;
                %                 fval = (b(1)*r.^8 + b(2)*r.^7 + b(3)*r.^6 + b(4)*r.^5 + b(5)*r.^4 + b(6)*r.^3 + b(7)*r.^2 + b(8)*r.^1 + b(9))./(1+r.^2).^4;
                %                 figure, plot( r, fval), hold on, plot( t, nu, '+r'), hold off
                %                 pause
                
                q = [1 -t; t 1]/sqrt(1+t^2);
                if k == 1
                    Q(:,[i j]) =  Q(:,[i j])*q;
                    z1(:,[i j]) = z1(:,[i j])*q;
                else
                    Q(:,[i j]+sz(1)) =  Q(:,[i j]+sz(1))*q;
                    z2(:,[i j]) = z2(:,[i j])*q;
                end
            end
        end
    end
%     cumt(iter)
    iter = iter + 1;
end

% figure, plot( F)

y = [z1 z2];
if transp, y = y'; else Q = Q'; end