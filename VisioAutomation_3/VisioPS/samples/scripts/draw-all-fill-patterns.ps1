import-module ./VisioPS.dll

new-visioapplication
new-drawing

$numcols = 6
$cellwidth = 0.5
$cellsep = 1.0

$d = $cellwidth + $cellsep
for ($i=0;$i -le 40;$i++) 
{
    $x = $i % $numcols 
    $y = [math]::floor($i / $numcols )
    $left = $x*$d
    $bottom = $y*$d
    $right = $left + $cellwidth
    $top = $bottom + $cellwidth
    $s1 = Draw-Rectangle $left $bottom $right $top
    Set-Formula "FillForegnd" "rgb(0,128,195)"
    Set-Formula "FillBkgnd" "rgb(255,255,255)"
    Set-Formula "FillPattern" $i
    $s2 = Draw-Rectangle ($left-$cellwidth) $bottom ($right-$cellwidth) $top
    set-text $i
}
