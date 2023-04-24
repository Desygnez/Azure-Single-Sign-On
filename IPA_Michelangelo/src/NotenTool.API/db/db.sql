USE Notentool

DROP DATABASE IF EXISTS Notentool
go

CREATE DATABASE Notentool
go
USE Notentool
go

create table School
(
    Id     uniqueidentifier not null primary key,
    School varchar(255)     not null
)
go

create table Subject
(
    Id      uniqueidentifier not null
        primary key,
    Subject varchar(45)      not null,
)
go

create table Semester
(
    Id       uniqueidentifier not null primary key,
    Semester int              not null
)

create table Grade
(
    Id          uniqueidentifier not null
        primary key,
    Grade       float            not null,
    Date        datetime         not null,
    Comment     varchar(255)     not null,
    Weight      float            not null,
    Subject_id  uniqueidentifier not null
        references Subject on update cascade on delete cascade,
    Semester_id uniqueidentifier not null
        references Semester (id)
            on update cascade on delete cascade
)
go

create table UserInformation
(
    Id         uniqueidentifier not null
        primary key,
    Firstname  varchar(45)      not null,
    Lastname   varchar(45)      not null,
    Email      varchar(45)      not null,
    Username   varchar(255)     not null,
)
go

create table AllGrades
(
    Id          uniqueidentifier not null
        primary key,
    UserInfo_id uniqueidentifier not null
        references UserInformation (id)
            on update cascade on delete cascade,
    Grade_id    uniqueidentifier not null
        references Grade (id)
            on update cascade on delete cascade
)
go

create table Subject_has_user
(
    Id          uniqueidentifier not null
        primary key,
    Subject_id  uniqueidentifier
        references Subject on update cascade on delete cascade,
    UserInfo_id uniqueidentifier
        references UserInformation (id)
            on update cascade on delete cascade,
    School_id   uniqueidentifier
        references School (id)
            on update cascade on delete cascade,
    Semester_id uniqueidentifier
        references Semester (id)
            on update cascade on delete cascade
)
go

create table User_has_Semester
(
    Id          uniqueidentifier not null primary key,
    UserInfo_id uniqueidentifier
        references UserInformation (id)
            on update cascade on delete cascade,
    Semester_id uniqueidentifier
        references Semester (id)
            on update cascade on delete cascade,
)

CREATE table TrainerApprentice
(
    Id           uniqueidentifier not null primary key,
    TrainerId    uniqueidentifier not null
        references UserInformation,
    ApprenticeId uniqueidentifier not null
        references UserInformation
)
CREATE TABLE VocationalTrainer
(
    Id uniqueidentifier not null primary key references UserInformation
)


insert into UserInformation (Id, Firstname, Lastname, Email, Username)
values (N'8aa59663-3302-467e-945b-e8ae949ffed9', N'Shansai', N'Muraleetharan', N'smuraleetharan@kpmg.com',
        N'smuraleetharan')
insert into Semester (Id, Semester)
values (N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F', 1)
insert into School (Id, School)
values (N'6F468269-AA40-409C-946E-7A8E1CD038BB', N'BBW')

insert into Subject (Id, Subject)
values (N'ebe686ef-25e6-4029-a190-cd297fe9ba46', N'Mathematik'),
       (N'2675daa3-8b97-4791-bd37-fdefc52c0076', N'test');

insert into Grade (Id, Grade, Date, Comment, Weight, Subject_id, Semester_id)
values (N'6dfa5ac2-4320-4b55-a869-3ed16d9ced0d', 2, N'2022-11-30 14:09:45.023', N'No Comment',
        1, N'ebe686ef-25e6-4029-a190-cd297fe9ba46',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'400796db-ce03-4d7e-9e2d-a965d0bcd318', 3, N'2022-11-30 14:09:36.297', N'No Comment',
        1, N'ebe686ef-25e6-4029-a190-cd297fe9ba46',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'4871c6dc-44f2-4b72-86f7-baf650925e3e', 5, N'2022-11-30 13:08:22.617', N'No Comment',
        1, N'ebe686ef-25e6-4029-a190-cd297fe9ba46',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'a796d3b0-d133-4426-900c-ce5e15632663', 5, N'2022-11-30 13:48:43.983', N'No Comment',
        1, N'ebe686ef-25e6-4029-a190-cd297fe9ba46',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'3828d89d-73c8-4bf6-a9c4-e68876479e6a', 6, N'2022-11-30 14:10:35.677', N'No Comment',
        1, N'ebe686ef-25e6-4029-a190-cd297fe9ba46',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F');

insert into AllGrades (Id, UserInfo_id, Grade_id)
values (N'ed89bdf2-3d80-4d7c-bf6e-7f57a72db93c', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'a796d3b0-d133-4426-900c-ce5e15632663'),
       (N'502b3c1b-d685-4554-9192-abb25e0a8c53', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'3828d89d-73c8-4bf6-a9c4-e68876479e6a'),
       (N'f32e6164-8838-4bec-ae1d-c14010aebe17', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'400796db-ce03-4d7e-9e2d-a965d0bcd318'),
       (N'7151c801-8ffa-472b-8e8d-d631bb6a5c9c', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'6dfa5ac2-4320-4b55-a869-3ed16d9ced0d'),
       (N'c6d81455-6f4c-4df2-9ba8-f5c9e98e45fa', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'4871c6dc-44f2-4b72-86f7-baf650925e3e')

insert into Subject_has_user (Id, Subject_id, UserInfo_id, School_id, Semester_id)
values (N'da72591f-4d7e-45be-a5a5-3bc60bf80732', N'2675daa3-8b97-4791-bd37-fdefc52c0076',
        N'8aa59663-3302-467e-945b-e8ae949ffed9', N'6F468269-AA40-409C-946E-7A8E1CD038BB',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'5fd9b0e5-e951-4c17-aefb-5914e051d21b', N'2675daa3-8b97-4791-bd37-fdefc52c0076',
        N'ccda8817-c6c2-414c-942f-c5b888e2dcbf', N'6F468269-AA40-409C-946E-7A8E1CD038BB',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'891c0633-fcce-4469-8725-c98432eeaf47', N'ebe686ef-25e6-4029-a190-cd297fe9ba46',
        N'ccda8817-c6c2-414c-942f-c5b888e2dcbf', N'6F468269-AA40-409C-946E-7A8E1CD038BB',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F')
insert into User_has_Semester (Id, UserInfo_id, Semester_id)
values (N'c33608cb-200f-4b63-8111-df836883f638', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'bc8daa82-b0c4-4865-a8b3-7cf4968ed030', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F'),
       (N'3c587266-23fe-4f45-aa97-fdc596c2baf7', N'ccda8817-c6c2-414c-942f-c5b888e2dcbf',
        N'7E99FC57-27D9-4F3A-9351-BAA17B839C9F')

