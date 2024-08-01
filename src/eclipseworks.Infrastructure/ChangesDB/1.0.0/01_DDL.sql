/*Script de criação de tabelas eclipseworks*/
/*aasf86 remover comentarios não usados*/

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

---------------
/*
create table events_motorcycle_inserted
(
	id bigserial not null primary key,
    inserted timestamp without time zone default now(),    
    guid uuid, 
    requestid uuid, 
    fromapp varchar(100),
    version varchar(100),
    whenevent timestamp without time zone,
    queue varchar(300),
    mq varchar(300),
    motorcycleid bigserial,
    year integer ,
    model varchar(100),
    plate varchar(7)
);

create table renter
(
	id bigserial not null primary key,
    guid uuid DEFAULT gen_random_uuid(), 
    inserted timestamp without time zone NOT NULL DEFAULT now(),
    updated timestamp without time zone NOT NULL DEFAULT now(),
    name varchar(100) not null,
    dateofbirth timestamp without time zone NOT NULL,
    cnpjcpf varchar(14) not null unique,
    cnh varchar(11) not null unique,
    cnhtype varchar(2) not null,
    cnhimg varchar(500) not null,
    userid text not null unique references "AspNetUsers"("Id")
);

create table filedisk
(
	id bigserial not null primary key,
    guid uuid DEFAULT gen_random_uuid(), 
    inserted timestamp without time zone NOT NULL DEFAULT now(),    
    key varchar(50) not null unique,
    filename varchar(200) not null,
    length numeric not null,
    contenttype varchar(100) not null,
    localpath text not null
);

create index idx_filedisk_key on filedisk(key);

create table rentalplan
(
	id bigserial not null primary key,
    guid uuid DEFAULT gen_random_uuid(), 
    inserted timestamp without time zone NOT NULL DEFAULT now(),
    updated timestamp without time zone NOT NULL DEFAULT now(),

    days int not null unique,
    valueperday decimal not null,
    PercentageOfDailyNotEffectived decimal not null,
    ValuePerDayExceeded decimal not null
);

create table rent
(
	id bigserial not null primary key,
    guid uuid DEFAULT gen_random_uuid(), 
    inserted timestamp without time zone NOT NULL DEFAULT now(),
    updated timestamp without time zone NOT NULL DEFAULT now(),
    
    rentalplanid bigint not null references rentalplan(id),
    motorcycleid bigint not null references motorcycle(id),
    renterid bigint not null references renter(id),
    rentaldays int not null,
    initialdate timestamp without time zone NOT NULL,
    enddate timestamp without time zone NOT NULL,
    endpredictiondate timestamp without time zone NOT NULL
);
*/