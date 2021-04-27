--Remove "--" in the line below the first time you run this.
--CREATE DATABASE SAMI_SIKON;





--Template for table creation:
--CREATE TABLE <tableName> (
--	<columnName1> <datatype> <constraints>,
--	<columnName2> <datatype> <constraints>,
--	<columnName3> <datatype> <constraints>,
--	
--	PRIMARY KEY (<columnName?>),
--	FOREIGN KEY (<columnName?>) REFERENCES <otherTableName>(<otherTableColumnName?>)
--);


CREATE TABLE _User (
	_User_Id INT IDENTITY(1,1),
	Email VARCHAR(120) NOT NULL,
	Password CHAR(64) NOT NULL,
	Salt CHAR(16) NOT NULL,
	Phone_Number VARCHAR(30),
	_Name VARCHAR(100),
	Administrator BIT NOT NULL,

	PRIMARY KEY (_User_Id)
);




--CREATE Table Booking (
--	Booking_Id INT IDENTITY(1,1),
--	Seat_Id INT NOT NULL,
--	Event_Id INT NOT NULL,
--	_User_Id INT NOT NULL,
--	Locked BIT NOT NULL,
--	PRIMARY KEY (Booking_Id),
--	FOREIGN KEY (Seat_Id) REFERENCES Seat(Seat_Id),
--	FOREIGN KEY (Event_Id) REFERENCES Seat(Event_Id),
--	FOREIGN KEY (_User_Id) REFERENCES _User(_User_Id)
--);