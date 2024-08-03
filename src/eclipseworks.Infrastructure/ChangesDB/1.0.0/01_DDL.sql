/*Script de criação de tabelas eclipseworks*/
/*aasf86 remover comentarios não usados*/

create schema events;

/*######################## <project> ########################*/

create table if not exists project
(
	Id bigserial not null primary key,
    Guid uuid DEFAULT gen_random_uuid(), 
    Inserted timestamp without time zone NOT NULL DEFAULT now(),
    Updated timestamp without time zone NOT NULL DEFAULT now(),
    LastEventByUser varchar(100),

    Name varchar(250) not null, /*aasf86 faltou definir o size no pedido, colocar isso na 2ª fase*/
    UserOwner varchar(100)
);
/*Event Sourcing*/
/*----------*/
create table if not exists events.project
(
	Id bigserial not null primary key,
    ProjectId bigint not null,
    TransactionId uuid DEFAULT gen_random_uuid(), 
    Inserted timestamp without time zone NOT NULL DEFAULT now(),
    EventUser varchar(100),
    Event varchar(100),
    Object json    
);
/*----------*/
create or replace function events.fn_tg_project()
returns trigger as $$
declare rowRecord record;
declare objectJson json;
declare objectJsonOld json;
begin

	case
		when tg_op = 'DELETE' then 
			rowRecord := old;
		else		
			rowRecord := new;
	end case;

    objectJson := json_agg(rowRecord)::json->0;

    if tg_op = 'UPDATE' then
        
        objectJsonOld := json_agg(old)::json->0;

        if objectJson::text = objectJsonOld::text then
            return rowRecord;
        end if;

    end if;
    
    insert into events.project (ProjectId, EventUser, Event, Object) 
    values (rowRecord.Id, rowRecord.LastEventByUser, tg_op, objectJson);
	
	return rowRecord;
end;
$$ language plpgsql;
/*----------*/
create trigger tg_pt_project_in
after insert on project
for each row
execute procedure events.fn_tg_project();
/*----------*/
create trigger tg_pt_project_up
after update on project
for each row
execute procedure events.fn_tg_project();
/*----------*/
create trigger tg_pt_project_de
before delete on project
for each row
execute procedure events.fn_tg_project();

/*######################## </project> ########################*/

/*######################## <taske> ########################*/
create table if not exists lv_taske_status
(
    Id int not null primary key,
    Value varchar(250) not null
);

create table if not exists lv_taske_priority
(
    Id int not null primary key,
    Value varchar(250) not null
);

create table if not exists taske
(
	Id bigserial not null primary key,
    Guid uuid DEFAULT gen_random_uuid(), 
    Inserted timestamp without time zone NOT NULL DEFAULT now(),
    Updated timestamp without time zone NOT NULL DEFAULT now(),
    LastEventByUser varchar(100),

    ProjectId bigint not null references project(Id),
    Title varchar(250) not null, /*aasf86 faltou definir o size no pedido, colocar isso na 2ª fase*/
    Description varchar(250) not null,
    Expires timestamp without time zone NOT NULL,
    StatusId int not null references lv_taske_status(Id),
    PriorityId int not null references lv_taske_priority(Id)    
);
/*Event Sourcing*/
/*----------*/
create table if not exists events.taske
(
	Id bigserial not null primary key,
    taskeId bigint not null,
    TransactionId uuid DEFAULT gen_random_uuid(), 
    Inserted timestamp without time zone NOT NULL DEFAULT now(),
    EventUser varchar(100),
    Event varchar(100),

    StatusValue varchar(250),
    PriorityValue varchar(250),
    ProjectLastEventId bigint,
    Object json    
);
/*----------*/
create or replace function events.fn_tg_taske()
returns trigger as $$
declare rowRecord record;
declare objectJson json;
declare objectJsonOld json;
begin

	case
		when tg_op = 'DELETE' then 
			rowRecord := old;
		else		
			rowRecord := new;
	end case;

    objectJson := json_agg(rowRecord)::json->0;

    if tg_op = 'UPDATE' then
        
        objectJsonOld := json_agg(old)::json->0;

        if objectJson::text = objectJsonOld::text then
            return rowRecord;
        end if;

    end if;
    
    insert into events.taske 
    (taskeId, 
     EventUser, 
     Event, 
     StatusValue, 
     PriorityValue, 
     ProjectLastEventId, 
     Object) 
    select 
        rowRecord.Id, 
        rowRecord.LastEventByUser, 
        tg_op, 
        lts.Value, 
        ltp.Value, 
        p.MaxId, 
        objectJson
    from lv_taske_status lts,
    lv_taske_priority ltp,
    lateral (
        select max(pe.Id) MaxId 
        from events.project pe 
        where pe.ProjectId = rowRecord.ProjectId
    ) p
    where ltp.Id = rowRecord.PriorityId
    and lts.Id = rowRecord.StatusId;
	
	return rowRecord;
end;
$$ language plpgsql;
/*----------*/
create trigger tg_pt_taske_in
after insert on taske
for each row
execute procedure events.fn_tg_taske();
/*----------*/
create trigger tg_pt_taske_up
after update on taske
for each row
execute procedure events.fn_tg_taske();
/*----------*/
create trigger tg_pt_taske_de
before delete on taske
for each row
execute procedure events.fn_tg_taske();
/*######################## </taske> ########################*/