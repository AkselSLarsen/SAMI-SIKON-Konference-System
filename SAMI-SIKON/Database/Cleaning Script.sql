-- This script will remove everything in the SAMI_SIKON Database and remake clean versions of the tables.

-- Remove all data from the tables.
DELETE FROM Booking;
DELETE FROM Seat;
DELETE FROM _Event;
DELETE FROM Room;
DELETE FROM _User;

-- Delete the tables themselves in case of them being named incorrectly.
DROP TABLE Booking;
DROP TABLE Seat;
DROP TABLE _Event;
DROP TABLE Room;
DROP TABLE _User;

-- Recreate the tables.
CREATE TABLE _User (
	_User_Id INT IDENTITY(1,1),
	Email VARCHAR(120) NOT NULL,
	_Password CHAR(64) NOT NULL,
	Salt CHAR(16) NOT NULL,
	Phone_Number VARCHAR(30),
	_Name VARCHAR(100),
	Administrator BIT NOT NULL,

	PRIMARY KEY (_User_Id)
);

CREATE TABLE Room (
	Room_Id INT IDENTITY (1,1),
	Layout VARCHAR(8000) NOT NULL,

--	Seats INT,
--	The above line should be here according to the ER diagram, but it is calculateable according to the layout, and thus not neccersary.

	PRIMARY KEY (Room_Id)
);

CREATE TABLE _Event (
	Event_Id INT IDENTITY (1,1),
	_Description VARCHAR(500) NOT NULL,
	_Name VARCHAR(100) NOT NULL,

	--	Seats_Taken INT,
	--	The above line should be here according to the ER diagram, but it is calculateable according to the Seat table, and thus not neccersary.
	
	StartTime DateTime NOT NULL,
	Duration INT,
	Room_Id INT NOT NULL,

	PRIMARY KEY (Event_Id),
	FOREIGN KEY (Room_Id) REFERENCES Room(Room_Id)
);

CREATE TABLE Seat (
	Seat_Id INT NOT NULL,
	Event_Id INT NOT NULL,
	Reserved BIT NOT NULL,
	
	PRIMARY KEY (Seat_Id, Event_Id),
	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id)
);

CREATE Table Booking (
	Booking_Id INT IDENTITY(1,1),
	Seat_Id INT NOT NULL,
	Event_Id INT NOT NULL,
	_User_Id INT NOT NULL,
	Locked BIT NOT NULL,
	PRIMARY KEY (Booking_Id),
	FOREIGN KEY (Seat_Id, Event_Id) REFERENCES Seat(Seat_Id, Event_Id),
	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id),
	FOREIGN KEY (_User_Id) REFERENCES _User(_User_Id)
);