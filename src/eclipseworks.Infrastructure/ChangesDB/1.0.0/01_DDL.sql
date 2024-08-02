/*Script de criação de tabelas eclipseworks*/
/*aasf86 remover comentarios não usados*/

create schema events;

/*######################## <project> ########################*/

create table if not exists project
--create table project
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
    
    insert into events.project (EventUser, Event, Object) 
    values (rowRecord.LastEventByUser, tg_op, objectJson);
	
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