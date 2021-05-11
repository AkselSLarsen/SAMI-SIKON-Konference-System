--Remove "--" in the line below the first time you run this.
--CREATE DATABASE SAMI_SIKON;

--Remove the "--" if your database lacks the "_User" table
--CREATE TABLE _User (
--	_User_Id INT IDENTITY(1,1),
--	Email VARCHAR(120) NOT NULL,
--	_Password CHAR(64) NOT NULL,
--	Salt CHAR(16) NOT NULL,
--	Phone_Number VARCHAR(30),
--	_Name VARCHAR(100),
--	Administrator BIT NOT NULL,

--	PRIMARY KEY (_User_Id)
--);

--Remove the FIRST "--" in each line below if your database lacks the "Room" table
--CREATE TABLE Room (
--	Room_Id INT IDENTITY (1,1),
--	Layout VARCHAR(8000) NOT NULL,

----	Seats INT,
----	The above line should be here according to the ER diagram, but it is calculateable according to the layout, and thus not neccersary.

--	PRIMARY KEY (Room_Id)
--);

--Remove the "--" if your database lacks the "_Event" table
--CREATE TABLE _Event (
--	Event_Id INT IDENTITY (1,1),
--	_Description VARCHAR(500) NOT NULL,
--	_Name VARCHAR(100) NOT NULL,

	--	Seats_Taken INT,
	--	The above line should be here according to the ER diagram, but it is calculateable according to the Seat table, and thus not neccersary.--	StartTime DateTime NOT NULL,

--	StartTime DateTime NOT NULL,
--	Duration INT,
--	Room_Id INT NOT NULL,

--	PRIMARY KEY (Event_Id),
--	FOREIGN KEY (Room_Id) REFERENCES Room(Room_Id)
--);

--Remove the "--" if your database lacks the "Speaker" table
--CREATE TABLE Speaker (
--	Event_Id INT NOT NULL,
--	_User_Id INT NOT NULL,

--	PRIMARY KEY (Event_Id, _User_Id),
--	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id) ON DELETE CASCADE,
--	FOREIGN KEY (_User_Id) REFERENCES _User(_User_Id) ON DELETE CASCADE
--);

--Remove the "--" if your database lacks the "Seat" table
--CREATE TABLE Seat (
--	Seat_Id INT NOT NULL,
--	Event_Id INT NOT NULL,
--	Reserved BIT NOT NULL,
	
--	PRIMARY KEY (Seat_Id, Event_Id),
--	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id) ON DELETE CASCADE
--);

--Remove the "--" if your database lacks the "Booking" table
--CREATE Table Booking (
--	Booking_Id INT IDENTITY(1,1),
--	Seat_Id INT NOT NULL,
--	Event_Id INT NOT NULL,
--	_User_Id INT NOT NULL,
--	Locked BIT NOT NULL,
--	PRIMARY KEY (Booking_Id),
--	FOREIGN KEY (Seat_Id, Event_Id) REFERENCES Seat(Seat_Id, Event_Id),
--	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id) ON DELETE CASCADE,
--	FOREIGN KEY (_User_Id) REFERENCES _User(_User_Id) ON DELETE CASCADE
--);