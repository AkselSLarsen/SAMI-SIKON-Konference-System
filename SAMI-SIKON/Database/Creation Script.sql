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







--CREATE Table Booking (
--	Booking_ID INT IDENTITY(1,1),
--	Seat_ID INT NOT NULL,
--	Event_ID INT NOT NULL,
--	_User_ID INT NOT NULL,
--	Locked BIT NOT NULL,
--	PRIMARY KEY (Booking_ID),
--	FOREIGN KEY (Seat_ID) REFERENCES Seat(Seat_ID),
--	FOREIGN KEY (Event_ID) REFERENCES Seat(Event_ID),
--	FOREIGN KEY (_User_ID) REFERENCES _User(_User_ID)
--);