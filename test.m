fid = fopen('D:\Andriy\��������� ����\������� ������ ����������� ������\Triangulation\TriangulationInNumericalMethods\TriangulationInNumericalMethods\bin\Debug\SolvedResult.txt')%'C:\Users\Andriy\Desktop\read.txt');
x=[];
y=[];
z=[];

% tline = fgetl(fid);
str = fgetl(fid);
%mas = strsplit(str, '\n');
mas=regexp(str,' ','split')

 x(1)=str2double(mas(1));
 y(1)=str2double(mas(2));
 z(1)=str2double(mas(3));
 k=2;
while ~feof(fid)
str = fgetl(fid);
mas=regexp(str,' ','split')
x(k)=str2double(mas(1));
y(k)=str2double(mas(2));
z(k)=str2double(mas(3));
k=k+1;
end
fclose(fid);
x
y
z

x=x';
y=y';
z=z';
%1
% [xq,yq] = meshgrid(min(x):1:max(x), min(y):1:max(y))
% vq = griddata(x,y,z,xq,yq)
% surf(xq,yq,vq)
%2
map = [0, 0, 0.3
    0, 0, 0.4
    0, 0, 0.5
    0, 0, 0.6
    0, 0, 0.8
    0, 0, 1.0];
colormap winter%(map)
 tri = delaunay(x,y); %x,y,z column vectors
 trisurf(tri,x,y,z);
%3
% figure
% z=[x,y,z];
% surf(z); shading('interp');
% xlabel('X'); ylabel('Y'); zlabel('f(X,Y)');