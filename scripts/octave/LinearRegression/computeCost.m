function J = computeCost(X, y, theta)


sum = 0;
m = length(y);
for i=1:m
  h = theta(1) + theta(2)*X(i,2);
  p = h - y(i,:);
  sum += p^2;
end;

J = sum / (2*m);
  
