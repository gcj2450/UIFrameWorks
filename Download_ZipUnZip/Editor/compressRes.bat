@echo off
set mpath=%1
set bundlePath=%mpath%/exp_local_res
set destResPath=%mpath%/temp
set outPath=%2
set bundleTemp=%mpath%/temp/
set assetPath=asset_map.txt
set indexPath=index.txt
set gbkPath=standalone_project_fzcy_gbk.prefab.bundle
set pck1=base1.pck
set pck2=base2.pck

cd /d %mpath%
if exist temp (
   echo "已经存在文件夹"
) else (
md temp
)

rem 移动到temp路径
cd exp_local_res
if exist asset_map.txt (
move asset_map.txt ../temp/
)
if exist index.txt (
move index.txt ../temp/
)
if exist standalone_project_fzcy_gbk.prefab.bundle (
move standalone_project_fzcy_gbk.prefab.bundle ../temp/
)

rem 压缩
"C:\Program Files\Unity5.6.1\Editor\Data\Tools\7z.exe" a -t7z %outPath%%pck1% %bundleTemp%/*.* -m0=LZMA:a=2:d=24:fb=64:mf=bt4 -ms=off -mmt=2

"C:\Program Files\Unity5.6.1\Editor\Data\Tools\7z.exe" a -t7z %outPath%%pck2% %bundlePath%/*.* -m0=LZMA:a=2:d=24:fb=64:mf=bt4 -ms=off -mmt=2

rem "C:\Program Files (x86)\7-Zip\7z.exe" a -t7z %outPath%%pck1% %mpath%%assetPath% -m0=LZMA:a=2:d=24:fb=64:mf=bt4 -ms=off -mmt=2
rem "C:\Program Files (x86)\7-Zip\7z.exe" a -t7z %outPath%%pck1%	%mpath%%indexPath% -m0=LZMA:a=2:d=24:fb=64:mf=bt4 -ms=off -mmt=2
rem "C:\Program Files (x86)\7-Zip\7z.exe" a -t7z %outPath%%pck1% %mpath%%gbkPath% -m0=LZMA:a=2:d=24:fb=64:mf=bt4 -ms=off -mmt=2

rem for /R %mpath% %%s in (*) do ( 
rem if %%s==%mpath%%assetPath% (echo %%s) else (if %%s==%mpath%%indexPath% (echo %%s) else (if %%s==%mpath%%gbkPath% (echo %%s) else ("C:\Program Files (x86)\7-Zip\7z.exe" a -t7z %outPath%%pck2% %%s -m0=LZMA:a=2:d=24:fb=64:mf=bt4 -ms=off -mmt=2))) 
rem )

rem 移除去的文件再拷贝回来
cd ../temp
copy asset_map.txt "../exp_local_res/"
copy index.txt "../exp_local_res/"
copy standalone_project_fzcy_gbk.prefab.bundle "../exp_local_res/"
