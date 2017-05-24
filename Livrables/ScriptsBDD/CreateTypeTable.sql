if exists(select 1
			from sys.table_types
			where name = 'TypeWorkTimeTable')
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

if exists(select 1
			from sys.table_types
			where name = 'TypeTaskTable')
drop type TypeTaskTable
go

create type TypeTaskTable as table
(
	TaskId UNIQUEIDENTIFIER,
	Label nvarchar(40) not null,
	IsAnnex bit not null,
	Activity varchar(20) not null,
	Login varchar(20) not null,
	Description nvarchar(100)
)
go

if exists(select 1
			from sys.table_types
			where name = 'TypeTaskProdTable')
drop type TypeTaskProdTable
go

create type TypeTaskProdTable as table
(
	TaskId UNIQUEIDENTIFIER,
	EstimatedRemainingTime float(5) not null,
	PredictedTime float(5) not null,
	Module varchar(20) not null,
	Version float(4) not null,
	Logiciel varchar(20) not null
)
go


if exists(select 1
			from sys.table_types
			where name = 'TypeIdTaskTable')
drop type TypeIdTaskTable
go


create type TypeIdTaskTable as table
(
	TaskId UNIQUEIDENTIFIER
)
go

