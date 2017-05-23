drop type TypeWorkTimeTable
go

create type TypeWorkTimeTable as table
(
	TaskId UNIQUEIDENTIFIER,
	WorkingDate Date not null,
	Hours float(5) not null,
	Productivity float(5) not null
)
go

drop type TypeTaskTable
go

create type TypeTaskTable as table
(
	TaskId UNIQUEIDENTIFIER primary key,
	Label nvarchar(40) not null,
	IsAnnex bit not null,
	Activity varchar(20) not null,
	Login varchar(20) not null,
	Description nvarchar(100)
)
go

drop type TypeTaskProdTable
go

create type TypeTaskProdTable as table
(
	TaskId UNIQUEIDENTIFIER primary key,
	EstimatedRemainingTime float(5) not null,
	PredictedTime float(5) not null,
	Module varchar(20) not null,
	Version float(4) not null,
	Logiciel varchar(20) not null
)
go


drop type TypeIdTaskTable
go

create type TypeIdTaskTable as table
(
	TaskId UNIQUEIDENTIFIER primary key
)
go

