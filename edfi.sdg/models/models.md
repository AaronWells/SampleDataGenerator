#Generating the Models Classes
The models classes are generated from the XSD schema using the Microsoft XML Schema Definition Tool (Xsd.exe).
<http://msdn.microsoft.com/en-us/library/x6c1kb0s(v=vs.110).aspx>

1. Navigate to the directory containing the schema (and extensions)
1. Create a list of all the .xsd files using the short name format for the Xsd files
  * You can find out the short names by typing "`DIR Interchange*.xsd \X > directory.txt`" from a dos prompt and harvesting the filenames from the text file.
  * Do not include `EdFiCore.xsd` or `SchemaAnnotation.xsd` as these files are included by the interchange files.
  * Append all the names together into a space delimited list (no returns)
> `INTERC~1.XSD INTERC~2.XSD INTERC~4.XSD INTERC~3.XSD INA083~1.XSD IND374~1.XSD INB7CF~1.XSD INB43A~1.XSD INE9EA~1.XSD INDB4E~1.XSD IN5A59~1.XSD IN72D9~1.XSD INBA06~1.XSD IN4DB9~1.XSD IN2A6D~1.XSD INAAC8~1.XSD IN8E0E~1.XSD IN68C1~1.XSD IN10D4~1.XSD`

1. Run the XSD schema tool against the list of schema files with these switches
> `xsd INTERC~1.XSD INTERC~2.XSD INTERC~4.XSD INTERC~3.XSD INA083~1.XSD IND374~1.XSD INB7CF~1.XSD INB43A~1.XSD INE9EA~1.XSD INDB4E~1.XSD IN5A59~1.XSD IN72D9~1.XSD INBA06~1.XSD IN4DB9~1.XSD IN2A6D~1.XSD INAAC8~1.XSD IN8E0E~1.XSD IN68C1~1.XSD IN10D4~1.XSD /c /edb /n:edfi.sdg.models`
1. Rename the file to `models.cs`
1. Copy the file into this project (replacing the existing models.cs file)