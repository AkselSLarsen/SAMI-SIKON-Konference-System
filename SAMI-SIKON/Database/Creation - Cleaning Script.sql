--Remove "--" in the line below the first time you run this.
--CREATE DATABASE SAMI_SIKON;

-- This script will remove everything in the SAMI_SIKON Database and remake clean versions of the tables.

-- Remove all data from the tables.
--DELETE FROM Booking;
--DELETE FROM Speaker;
--DELETE FROM _Event;
--DELETE FROM Room;
--DELETE FROM _User;

-- Delete the tables themselves in case of them having been created incorrectly.
DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS Speaker;
DROP TABLE IF EXISTS _Event;
DROP TABLE IF EXISTS Room;
DROP TABLE IF EXISTS _User;

-- Recreate the tables.
CREATE TABLE _User (
	_User_Id INT IDENTITY(1,1),
	Email VARCHAR(120) NOT NULL,
	_Password CHAR(64) NOT NULL,
	Salt CHAR(24) NOT NULL,
	Phone_Number VARCHAR(30),
	_Name VARCHAR(100),
	Administrator BIT NOT NULL,

	PRIMARY KEY (_User_Id)
);

CREATE TABLE Room (
	Room_Id INT IDENTITY (1,1),
	Layout NVARCHAR(MAX) NOT NULL,
	_Name VARCHAR(50),

	PRIMARY KEY (Room_Id)
);

CREATE TABLE _Event (
	Event_Id INT IDENTITY (1,1),
	_Description VARCHAR(500) NOT NULL,
	_Name VARCHAR(100) NOT NULL,
	StartTime DateTime NOT NULL,
	Duration INT,
	Room_Id INT NOT NULL,

	PRIMARY KEY (Event_Id),
	FOREIGN KEY (Room_Id) REFERENCES Room(Room_Id)
);

CREATE TABLE Speaker (
	Event_Id INT NOT NULL,
	_User_Id INT NOT NULL,

	PRIMARY KEY (Event_Id, _User_Id),
	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id) ON DELETE CASCADE,
	FOREIGN KEY (_User_Id) REFERENCES _User(_User_Id) ON DELETE CASCADE
);

CREATE Table Booking (
	Booking_Id INT IDENTITY(1,1),
	Seat_Nr INT,
	Event_Id INT NOT NULL,
	_User_Id INT NOT NULL,
	Locked BIT NOT NULL,
	PRIMARY KEY (Booking_Id),
	FOREIGN KEY (Event_Id) REFERENCES _Event(Event_Id) ON DELETE CASCADE,
	FOREIGN KEY (_User_Id) REFERENCES _User(_User_Id) ON DELETE CASCADE
);