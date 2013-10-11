classdef Deriche < handle
	properties(Constant)
		RisingEdge			= 1;
		FallingEdge			= 2;
		RisingFallingEdge	= 3;
	end
	properties(Constant)
		TypeFloat32			= 1;
		TypeInt16			= 2;
	end
	properties(Constant)
		MaxInt16			= hex2dec('7fff');
	end

	methods(Static)
		function result = Smooth(signal, alpha)
            sig         = [zeros(1, 10) signal zeros(1, 10)];
            offDeriche  = 2;
            X           = size(sig, 2);
            limX        = X - offDeriche;
            y1          = zeros(1, X);
            y2          = zeros(1, X);
            
            ke          = exp(-alpha);
            ka          = -(1 - ke)*(1 - ke)/(1 + 2*alpha*ke - ke*ke);

            B1          = -2*ke;
            B2          = ke*ke;

            A0          = ka;
            A1          = ka*(alpha - 1)*ke;
            A2          = A1 - ka*B1;
            A3          = -ka*B2;
            
            for i=offDeriche+1:limX
                y1(i)   = A0*sig(i)        + A1*sig(i - 1)        - B1*y1(i - 1)       - B2*y1(i - 2);
                y2(X - i)= A2*sig(X - i)   + A3*sig(X - i + 1)    - B1*y2(X - i + 1)   - B2*y2(X - i + 2);
            end
            
            r           = -(y1 + y2);
            result      = r(11:10 + size(signal, 2));
		end
		function result = FirstDeriv(signal, alpha)
            sig         = [zeros(1, 10) signal zeros(1, 10)];
            offDeriche  = 2;
            X           = size(sig, 2);
            limX        = X - offDeriche;
            y1          = zeros(1, X);
            y2          = zeros(1, X);
            
            ke          = exp(-alpha);
            ka          = -(1 - ke)*(1 - ke)/ke;

            A           = ka*ke;
            B1          = 2*ke;
            B2          = ke*ke;
            
            for i=offDeriche+1:limX
                y1(i)   = sig(i-1)     + B1*y1(i-1)  - B2*y1(i-2);
                y2(X-i) = sig(X-i+1)   + B1*y2(X-i+1)- B2*y2(X-i+2);
            end
            
            r           = A*(y1 - y2);
            result      = r(11:10 + size(signal, 2));
		end
	end
end
