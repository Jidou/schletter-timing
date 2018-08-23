# schletter-timing

## TODO

* Calculated time in groups
* Output sorted and grouped

## Components

* Frontend
* Business Logic
* Data Reader(s)

## Roadmap

### Reading Data

* Read data from ALGE-Timing Timy3 using USB
	- Output sample: 0005 C0M 22:05:41.08   00
		+ [0]: Empty
		+ [1]: Measurement number
		+ [2]: COM?
		+ [3]: Time
		+ [4]: Emtpy
		+ [5]: Emtpy
		+ [6]: ?
* Read data from ALGE-Timing Timy3 using RS 232
	- Output sample: ?


## Testing

t l timing_values_0.json

p l AllParticipants.json
g l AllGroups.json
r l TestSave.json
r l FinalTestSave.json

p s AllParticipants.json
g s AllGroups.json
r s TestSave.json
r s FinalTestSave.json

r c Test TestRace 2018-08-21 Here Someone

g c Group001 1 male
g c Group002 2 female
g c Group003 3 male
g c Group004 4 female
g c Group005 5 male
g c Group006 6 mixed
g c Group007 7 mixed
g c Group008 8 mixed
g c Group009 9 male
g c Group010 10 female

p c Firstname001 Lastname001 87 one
p c Firstname002 Lastname002 87 one
p c Firstname003 Lastname003 87 one
p c Firstname004 Lastname004 87 one
p c Firstname005 Lastname005 87 one
p c Firstname006 Lastname006 87 one
p c Firstname007 Lastname007 87 one
p c Firstname008 Lastname008 87 one
p c Firstname009 Lastname009 87 one
p c Firstname010 Lastname010 87 one
p c Firstname011 Lastname011 87 one
p c Firstname012 Lastname012 87 one
p c Firstname013 Lastname013 87 one
p c Firstname014 Lastname014 87 one
p c Firstname015 Lastname015 87 one
p c Firstname016 Lastname016 87 one
p c Firstname017 Lastname017 87 one
p c Firstname018 Lastname018 87 one
p c Firstname019 Lastname019 87 one
p c Firstname020 Lastname020 87 one

g ap Group001 Firstname001_Lastname001 Firstname002_Lastname002
g ap Group002 Firstname003_Lastname003 Firstname004_Lastname004
g ap Group003 Firstname005_Lastname005 Firstname006_Lastname006
g ap Group004 Firstname007_Lastname007 Firstname008_Lastname008
g ap Group005 Firstname009_Lastname009 Firstname010_Lastname010
g ap Group006 Firstname011_Lastname011 Firstname012_Lastname012
g ap Group007 Firstname013_Lastname013 Firstname014_Lastname014
g ap Group008 Firstname015_Lastname015 Firstname016_Lastname016
g ap Group009 Firstname017_Lastname017 Firstname018_Lastname018
g ap Group010 Firstname019_Lastname019 Firstname020_Lastname020

r ag Group001 Group002 Group003 Group004 Group005 Group006 Group007 Group008 Group009 Group010