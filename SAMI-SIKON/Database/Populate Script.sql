-- This script will populate the SAMI_SIKON database in order to allow some degree of testing with psudoreal data.

--Template of _User table
--INSERT INTO _User (Email, _Password, Salt, Phone_Number, _Name, Administrator)
--VALUES ('Email', 'Password', 'Salt', 'Phone_Number', '_Name', true/false);

INSERT INTO _User (Email, _Password, Salt, Phone_Number, _Name, Administrator)
VALUES ('Mikkel@SAMI.dk', 'p455w0rd', '54l7', '12345678', 'Mikkel', 0);

INSERT INTO _User (Email, _Password, Salt, Phone_Number, _Name, Administrator)
VALUES ('Sebastian@SAMI.dk', '3ncryp7', 'Salt', 'Something', 'Sebastian', 1);

INSERT INTO _User (Email, _Password, Salt, Phone_Number, _Name, Administrator)
VALUES ('Aksel@SAMI.dk', '1234', '4321', 'Money?', 'Aksel', 0);

--___________________________________________________________________________________________________
--___________________________________________________________________________________________________

--Template of Room table
--INSERT INTO Room (Layout)
--VALUES ('Layout');

INSERT INTO Room (Layout)
VALUES ('SSSSS;SSSSS;SSSSS;SSSSS;SSSSS');

INSERT INTO Room (Layout)
VALUES ('SS;SS');

--___________________________________________________________________________________________________
--___________________________________________________________________________________________________

--Template of _Event table
--INSERT INTO _Event (_Description, _Name, StartTime, Duration, Room_Id)
--VALUES ('_Description', '_Name', 'YYYYMMDD hh:mm:ss', DurationInMinutesAsInt, IdOfRoom);

INSERT INTO _Event (_Description, _Name, StartTime, Duration, Room_Id)
VALUES ('To be written', 'Postponing', '20210507 12:00:00', 30, 1);

INSERT INTO _Event (_Description, _Name, StartTime, Duration, Room_Id)
VALUES ('Something about not being afraid of change or something', 'Bravery', '20210507 12:00:00', 45, 2);

INSERT INTO _Event (_Description, _Name, StartTime, Duration, Room_Id)
VALUES ('This is a test event', 'Test', '20210507 12:00:00', 60, 1);

INSERT INTO _Event (_Description, _Name, StartTime, Duration, Room_Id)
VALUES ('Why am I even writing these', 'Questioning', '20210507 12:00:00', 75, 2);

INSERT INTO _Event (_Description, _Name, StartTime, Duration, Room_Id)
VALUES ('Run out of stuff to write', 'Cranial Meltdown', '20210507 12:00:00', 90, 1);

--___________________________________________________________________________________________________
--___________________________________________________________________________________________________

--Template of Speaker table
--INSERT INTO Speaker (Event_Id, _User_Id)
--VALUES (IdOfEvent, IdOfUser);

--___________________________________________________________________________________________________
--___________________________________________________________________________________________________

--Template of Seat table
--INSERT INTO Seat (Seat_Id, Event_Id, Reserved)
--VALUES (IdOfSeat, IdOfEvent, true/false);

--___________________________________________________________________________________________________
--___________________________________________________________________________________________________

--Template of Booking table
--INSERT INTO Booking (Seat_Id, Event_Id, _User_Id, Locked)
--VALUES (IdOfSeat, IdOfEvent, IdOfUser, true/false);

--___________________________________________________________________________________________________
--___________________________________________________________________________________________________
