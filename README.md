# schletter-timing

1. [schletter-timing](#schletter-timing)
   1. [How to start Timy](#how-to-start-timy)
   2. [How to send data to application](#how-to-send-data-to-application)
   3. [TODO](#todo)
   4. [Components](#components)
   5. [Roadmap](#roadmap)
      1. [Reading Data](#reading-data)
      2. [Console Frontend](#console-frontend)
      3. [Web Frontend](#web-frontend)
         1. [2.0](#20)
         2. [3.0](#30)
         3. [Possible future features](#possible-future-features)
   6. [Testing](#testing)

## How to start Timy

* Press Larger Green Button `Start ON`
* Wait for Message: `Wirklich einschalten? Druecken Sie die Gruene OK Taste`
* Press Round Green Button `OK`
* Wait for Menu
* Select `Backup`
* Here you can Delete the old Values or keep them
* Insert Date and Time, continue with `OK` Button
* Press `Start ON` to send Sync Puls

## How to send data to application

* Make sure, Timy is connected to Laptop
* In Application:
  * Load the Race in the application
  * Navigate to Race -> Timing
  * Press `Get Times`
* On Timy
  * Press the Button with the List icon (next to `2nd`)
  * Navigate to `Interface`
  * Select `RS-232`
  * Select `Send Memory`
* You should now see two Lists in the Application
  * All Groups with Starting numbers on the left hand side
  * All Times with Start Number `0` on the right hand side
* Now you have to match up the Times with the start numbers
  * Additional Times can be ignored (set Start Number to 0)
* After this, press the `Assign` Button

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

* [x] Console Frontend Features (1.0 Release)
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

#### 2.0

* [x] 2.0 Release
  * [x] Create race
  * [x] Add Participants
  * [x] Add Groups
  * [x] Add Participants to Group
  * [x] Add Groups to Race
  * [x] Assign Start Numbers
    * [ ] ~~Resulting numbers are __Immutable__ in Frontend, but __Mutable__ on Server~~
    * [x] Can be triggered multiple times with different results
  * [x] Add Option to trigger timing value load from device
    * [x] Test load from device with Web Frontend
  * [x] Add Option to generate PDF(s)
    * [x] Implement PDF generation in javascript
    * [x] Add different options (by Group, by Class, ...)
  * [x] Add Overview of all Participants ever, without groups
  * [x] Add Overview of all Groups ever, with latest Participants
  * [x] Add Overview of all Races
  * [x] Add Option to load old Races
  * [x] Add Option to create new Races
    * [x] Add new CRU(D) functionality to Race detail page
  * [x] Add Overview of Current Groups in loaded Race (old or new)
  * [x] Remove `CurrentContext` to enable easier live reload
  * [x] (**IMPORTANT**) Test everything done up to this point
    * [x] Categories
      * [x] Create
      * [x] Read
      * [x] Update
    * [x] Classes
      * [x] Create
      * [x] Read
      * [x] Update
    * [x] Participants
      * [x] Create
      * [x] Read
      * [x] Update
    * [x] Groups
      * [x] Create
      * [x] Read
      * [x] Update
      * [x] Participant assignment
    * [x] Race
      * [x] Create new Race
      * [x] Load old Race
      * [x] Add Groups
      * [x] Manipulate Groups in Race only
        * [x] Add implementation of `onBlur`
      * [x] Assign Start numbers
      * [x] Load Timing Values from Device
      * [x] Assign Timing Values to Groups
      * [x] Results
        * [x] Show overall results
        * [x] Toggle Classes grouping
          * [x] Unselect all Groups in Grouping
        * [x] Toggle Custom result
          * [x] Custom Result title is used for PDF
          * [x] Unselect is working
  * [x] Release Preparations

#### 3.0

* [ ] next Release
  * [ ] Overhaul workflow
    * [x] Remove global Group DB (To many changes between the years, to be of use)
    * [ ] Add Option to Add Groups via the Race Overview page
  * [x] CSV Importer
  * [x] Fix race load error
  * [ ] Rethink Pagination for global pages
    * [ ] Participants
  * [x] Check if `Model.Group.Groupnumber` is still needed
    * [x] No, removed it
  * [x] Make `FinishTime` in Group nullable
  * [ ] New feature multiple start times (Groups 1-10 with starting time 1, then 11-20 with starting time 2, ...)
    * [ ] Enable selection of multiple starting times
    * [ ] Add Grouping features
    * [ ] Assign starting time to Grouping
  * [x] Autofocus newly created element on large pages
  * [ ] Sortable [table](https://react-bootstrap-table.github.io/react-bootstrap-table2/docs/basic-sort.html)
  * [ ] Add Delete Option
    * [ ] Group.js
    * [ ] Participant.js
    * [ ] RaceGroups.js
    * [ ] ~~RaceParticipants.js~~
    * [ ] Categories.js
    * [ ] Classes.js
  * [ ] `Autosuggestion` might cause performance issues on larger pages (Group and Participant Overview), look for alternative or improve it
  * [ ] i18n
    * [ ] Basic Setup
    * [ ] English
    * [ ] Tirol
    * [ ] German
  * [ ] Results
    * [ ] Multiselect/unselect (Whole group)
    * [ ] Try to fetch name of list for name of generated file

#### Possible future features

* [ ] future Release
  * [ ] UX
    * [ ] Improve Add functionality
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