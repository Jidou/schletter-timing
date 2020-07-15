# schletter-timing

1. [schletter-timing](#schletter-timing)
   1. [TODO](#todo)
   2. [Components](#components)
   3. [Roadmap](#roadmap)
      1. [Reading Data](#reading-data)
      2. [Console Frontend](#console-frontend)
      3. [Web Frontend](#web-frontend)
   4. [Testing](#testing)

## TODO

* Web Frontend
  * Read Timing values from device and add to groups
  * Render result as PDF -> call to existing impl?

## Components

* Frontend
* PDF Generator
* Business Logic
* Data Reader(s)
* Repo(s)

## Roadmap

### Reading Data

* [x] Read data from ALGE-Timing Timy3 using USB
  * Output sample: 0005 C0M 22:05:41.08   00
    [0]: Empty
    [1]: Measurement number
    [2]: COM?
    [3]: Time
    [4]: Emtpy
    [5]: Emtpy
    [6]: ?
* [x] Read data from ALGE-Timing Timy3 using RS 232
  * Output sample: ?

### Console Frontend

* [x] Console Frontend Features
  * [x] Add Participant
  * [x] Add Group
  * [x] Add Participant to Group
  * [x] Create new Race
  * [x] Add Group to Race
  * [x] Read timing values from device and assign to Groups
  * [x] Calculate results
  * [x] Generate PDF(s)
* [x] General Features
  * [x] Save Participants into file as JSON
  * [x] Save Groups into file as JSON
  * [x] Save Race into file as JSON
  * [x] Save as often as possible, into different files during read of timing values and assignment process (better save than sorry ;))
  * [x] Add Option to load Participants/Groups/Race

### Web Frontend

* [ ] Web Frontend Features:
  * [x] Create race
  * [x] Add Participants
  * [x] Add Groups
  * [x] Add Participants to Group
  * [x] Add Groups to Race
  * [x] Assign Start Numbers
    * [x] Resulting numbers are __Immutable__ in Frontend, but __Mutable__ on Server
    * [x] Can be triggered multiple times with different results
  * [ ] Add Option to trigger timing value load from device
  * [ ] Add Option to generate PDF(s)
  * [ ] Add Overview of all Participants ever, without groups
  * [ ] Add Overview of all Groups ever, without participants
* [ ] i18n
  * [ ] Basic Setup
  * [ ] English
  * [ ] German
  * [ ] Tirol
* [ ] Data Science Stuff ;)
  * [ ] Add Option to load multiple races for statistics
  * [ ] Add search and display options
    * [ ] Fastest group ever
    * [ ] Time of group over multiple years
    * ...

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