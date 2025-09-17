CREATE TABLE Users(
UserId SERIAL primary key,
Avatar varchar(256) default '/def.png',
Name varchar(256) not null,
Surname varchar(256),
Patronymic varchar(256),
Email varchar(256) not null unique,
TelephonNumber varchar(50) not null,
PasswordHash varchar(512) not null,
Role varchar(50) not null CHECK (Role in ('Admin', 'Teacher', 'Student')),
IsActive boolean default True,
CreatedAt timestamp default now()
);

CREATE TABLE portfolio(
UserId int not null,
achievement varchar(256) not null,
AddedAt timestamp default now(),
foreign key (UserId) references Users (UserId),
PRIMARY KEY (UserId,achievement)
);

CREATE TABLE Cities (
CityId SERIAL PRIMARY KEY,
CityName varchar(100) not null unique,
PostalCode varchar(20),
Country varchar(100)
);

CREATE TABLE Institution(
InstitutionId SERIAL PRIMARY KEY,
InstitutionName varchar (256) not null,
CityId int,
Street varchar (256) not null,
Phone varchar (50),
Website varchar (256),
foreign key (CityId) references Cities (CityId)
);


CREATE TABLE Rooms(
RoomId SERIAL PRIMARY KEY,
RoomNumber varchar(50) not null,
InstitutionId int not null,
UNIQUE (RoomNumber, InstitutionId),
foreign key (InstitutionId) references Institution (InstitutionId)
);

CREATE TABLE Room_equipment(
RoomId int not null,
equipment varchar(256) not null,
foreign key (RoomId) references Rooms (RoomId) on delete cascade,
PRIMARY KEY (RoomId, equipment) 
);


CREATE TABLE Groups(
GroupId SERIAL primary key,
GroupName varchar(256) not null,
Course int CHECK (Course between 1 and 4),
CuratorId int not null,
Specialty varchar(256), 
InstitutionId int not null,
foreign key (InstitutionId) references Institution (InstitutionId) on delete cascade on update cascade,
foreign key (CuratorId) references Users (UserId) on delete cascade on update cascade
);

CREATE TABLE Students_Groups(
UserId int not null,
GroupId int not null,
enrolledAt date default current_date,
foreign key (UserId) references Users (UserId) on delete cascade on update cascade,
foreign key (GroupId) references Groups (GroupId) on delete cascade on update cascade
);


CREATE TABLE Lectures(
LectureId SERIAL PRIMARY KEY,
LectureName varchar(256) not null,
Description Text,
StartTime time not null,
EndTime time not null,
TeacherId int not null,
RoomId int,
IsActive boolean default True,
CreatedAt timestamp default now(),
foreign key (TeacherId) references Users (UserId),
foreign key (RoomId) references Rooms (RoomId)
);

CREATE TABLE Attendance (
AttendanceId SERIAL PRIMARY KEY,
LectureId int not null,
UserId int not null,
IsPresent boolean not null,
Note varchar(500),
RecordedAt timestamp default now(),
foreign key (LectureId) references Lectures(LectureId) on delete cascade,
foreign key (UserId) references Users(UserId) on delete cascade,
unique (LectureId, UserId)
);

CREATE TABLE Notifications(
NotificationId SERIAL PRIMARY KEY,
UserId int not null,
CreatedAt timestamp default now(),
IsRead boolean default false,
Note text not null,
foreign key (UserId) references Users (UserId)
);


CREATE TABLE Lectures_Groups(
LectureId int not null,
GroupId int not null,
primary key (GroupId, LectureId),
foreign key (LectureId) references Lectures (LectureId),
foreign key (GroupId) references Groups (GroupId)
);
