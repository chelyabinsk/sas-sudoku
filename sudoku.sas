proc optmodel;
/* Define grid */
set col = {1, 2, 3, 4, 5, 6, 7, 8, 9};
set row = {1, 2, 3, 4, 5, 6, 7, 8, 9};
set val = {1, 2, 3, 4, 5, 6, 7, 8, 9};

set block_vals = {1,4,7};
set o_vals = {0,1,2};

/* Define variable */
var x{row,col,val} binary;


/* Objective */
max stuff = sum{ i in col, j in row, k in val}x[i,j,k];

/* Define constraints */
con con1{j in row, k in val}: sum{i in col}x[i,j,k] = 1;
con con2{i in col, k in val}: sum{j in row}x[i,j,k] = 1;
con con3{i in block_vals, j in block_vals, k in val}:sum{r in o_vals, q in o_vals} x[i+q,j+r,k] = 1;
con con4{i in col, j in row}: sum{k in val}x[i,j,k] = 1;

con con118:x[1,1,8] = 1;
con con233:x[2,3,3] = 1;
con con246:x[2,4,6] = 1;
con con327:x[3,2,7] = 1;
con con359:x[3,5,9] = 1;
con con372:x[3,7,2] = 1;
con con425:x[4,2,5] = 1;
con con467:x[4,6,7] = 1;
con con554:x[5,5,4] = 1;
con con565:x[5,6,5] = 1;
con con577:x[5,7,7] = 1;
con con641:x[6,4,1] = 1;
con con683:x[6,8,3] = 1;
con con731:x[7,3,1] = 1;
con con786:x[7,8,6] = 1;
con con798:x[7,9,8] = 1;
con con838:x[8,3,8] = 1;
con con845:x[8,4,5] = 1;
con con881:x[8,8,1] = 1;
con con929:x[9,2,9] = 1;
con con974:x[9,7,4] = 1;


solve;

/*print x;*/

create data solution from [c r v]={col,row,val} o=x;
quit;

proc sql;
create table solution as
select c,r,v,round(o) as o
from solution
;
quit;
proc sql;
create table o as
select c,r,v
from solution
where o <> 0
;
quit;

/* Export as csv */



