Sub fill()

' Go through all of the rows
going = True
i = 2

Do While True
    r = Cells(i, 1).Value
    c = Cells(i, 2).Value
    v = Cells(i, 3).Value
    
    If (v = "") Then
        Exit Do
    End If
    Cells(r + 1, c + 4).Value = v
    i = i + 1
Loop

End Sub


Sub generate()

counter = 2

For i = 2 To 11
    For j = 2 To 11
        Value = Cells(i, j).Value
        
        If (Value <> "") Then
            Cells(counter, 13).Value = "con con" & CStr(i - 1) & CStr(j - 1) & CStr(Value) & _
                            ":x[" & CStr(i - 1) & "," & CStr(j - 1) & "," & CStr(Value) & "] = 1;"
            
            counter = counter + 1
        End If
    Next j
Next i
End Sub

Sub generate_sas_code()
code = "proc optmodel;" _
    & vbCrLf _
    & "/* Define grid */" _
    & vbCrLf _
    & "set col = {1, 2, 3, 4, 5, 6, 7, 8, 9};" _
    & vbCrLf _
    & "set row = {1, 2, 3, 4, 5, 6, 7, 8, 9};" _
    & vbCrLf _
    & "set val = {1, 2, 3, 4, 5, 6, 7, 8, 9};" _
 _
    & vbCrLf _
    & vbCrLf _
 _
    & "set block_vals = {1,4,7};" _
    & vbCrLf _
    & "set o_vals = {0,1,2};" _
    & vbCrLf _
 _
    & vbCrLf _
    & "/* Define variables */" _
    & vbCrLf _
    & "var x{row,col,val} binary;" _
    & vbCrLf _
    & vbCrLf _
    & "/* Define constraints */"

code = code & _
    vbCrLf _
    & "/* Objective */" _
    & vbCrLf _
    & "max stuff = sum{ i in col, j in row, k in val}x[i,j,k];" _
    & vbCrLf _
 _
    & vbCrLf _
    & "/* Constraints */" _
    & "con con1{j in row, k in val}: sum{i in col}x[i,j,k] = 1;" _
    & "con con2{i in col, k in val}: sum{j in row}x[i,j,k] = 1;" _
    & "con con3{i in block_vals, j in block_vals, k in val}:sum{r in o_vals, q in o_vals} x[i+q,j+r,k] = 1;" _
    & "con con4{i in col, j in row}: sum{k in val}x[i,j,k] = 1;"
    
    ' Add the constraints
    For i = 2 To 11
        For j = 2 To 11
            Value = Cells(i, j).Value
            
            If (Value <> "") Then
                code = code & "con con" & CStr(i - 1) & CStr(j - 1) & CStr(Value) & _
                                ":x[" & CStr(i - 1) & "," & CStr(j - 1) & "," & CStr(Value) & "] = 1;"
                
                counter = counter + 1
            End If
        Next j
    Next i
    
    code = code & _
    vbCrLf _
    & "solve;" _
    & vbCrLf _
    & "/*print x;*/" _
    & vbCrLf _
    & "create data solution from [c r v]={col,row,val} o=x;" _
    & "quit;" _
    & "proc sql;create table solution as select c,r,v,round(o) as o from solution ; quit;" _
    & "proc sql;create table o as select c,r,v from solution where o <> 0;quit; /* Export as csv */ proc Export replace Data = o " _
    & "outfile="
    
    file = "\\Mathspool1\mathsh\asd\My Documents\LP Notes\Sudoku\result.csv"
    
    sasfile = "\\Mathspool1\mathsh\asd\My Documents\LP Notes\Sudoku\sudoku.sas"
    code = code & Chr(34) & file & Chr(34) & "; run; "
    
    
    ' export sas file
    Dim fso As Object
    Set fso = CreateObject("Scripting.FileSystemObject")

    Dim Fileout As Object
    Set Fileout = fso.CreateTextFile(sasfile, True, True)
    Fileout.Write code
    Fileout.Close
End Sub


Sub read_solution()
' Sub that reads the solution csv file and writes it out

' Solution file path
file = "\\Mathspool1\mathsh\asd\My Documents\LP Notes\Sudoku\result.csv"

Open file For Input As #1

row_number = 0

Do Until EOF(1)
    Line Input #1, linefromfile
    
    lineitems = Split(linefromfile, ",")
    r = lineitems(0)
    c = lineitems(1)
    v = lineitems(2)
    If (row_number > 0) Then
        Cells(r + 1, c + 12).Value = v
    End If
    
    row_number = row_number + 1
Loop

Close #1

Kill "\\Mathspool1\mathsh\asd\My Documents\LP Notes\Sudoku\sudoku.sas"
Kill "\\Mathspool1\mathsh\asd\My Documents\LP Notes\Sudoku\result.csv"
End Sub

Sub call_sas()
saspath = "C:\Program Files\SAS94\SASFoundation\9.4\sas.exe"
filepath = "\\Mathspool1\mathsh\asd\My Documents\LP Notes\Sudoku\sudoku.sas"
    Shell Chr(34) & saspath & Chr(34) & " " & Chr(34) & filepath & Chr(34), vbNormalFocus
End Sub


Sub do_everythin()
generate_sas_code
call_sas
read_solution
End Sub


