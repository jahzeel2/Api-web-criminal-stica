CREATE DATABASE BDCRIMINALISTICA

USE BDCRIMINALISTICA


CREATE TABLE UNIDADSISTEMA
(
	id int NOT NULL IDENTITY PRIMARY KEY,
	nombre varchar(150) NOT NULL
);

CREATE TABLE ROL
(
	id int NOT NULL IDENTITY PRIMARY KEY,
	nombre varchar(50) NOT NULL,
	activo bit NOT NULL
);

CREATE TABLE USUARIOCRIMINALISTICA
(
	id int NOT NULL IDENTITY PRIMARY KEY,
	userCreaRepo int NOT NULL,
	usuarioRepo int,
	fechaAlta datetime NOT NULL,
	persona int NULL,
	civil int NULL,
	norDni int NULL,
	nombre varchar(50) NULL,
	apellido varchar(50) NULL,
	tipoPersona bit NULL,
	fechaBaja datetime NULL,
	usuarioBaja int NULL,
	baja bit NULL,
	sistema int FOREIGN KEY REFERENCES UNIDADSISTEMA(id),
	cifrado varchar(250),
	fechaVinculacion datetime NULL,
	rol int FOREIGN KEY REFERENCES ROL(id),
	activo bit NOT NULL
);

CREATE TABLE ESTADO
(
	id int NOT NULL IDENTITY PRIMARY KEY,
	nombre varchar(50) NOT NULL,
	unidadCreacion int FOREIGN KEY REFERENCES UNIDADSISTEMA(id), 
	activo bit NOT NULL
);

CREATE TABLE PERITOS
(
	id int IDENTITY PRIMARY KEY NOT NULL,
	nombre nvarchar(25) NOT NULL,
	apellido nvarchar(30) NOT NULL,
	dni int NOT NULL,
	tipoPersona nvarchar(25) NOT NULL,
	idPersonalPolicial int NULL,
	idPersonalCivil int NULL,
	tipoPerito nvarchar(25) NULL,
	fechaAlta datetime NOT NULL,
	usuarioAlta int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id) NOT NULL,
	fechaBaja datetime NULL,
	usuarioBaja int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id) NULL,
	unidadCreacion int FOREIGN KEY REFERENCES UNIDADSISTEMA(id) NOT NULL,
	activo bit NOT NULL
);

CREATE TABLE EXPEDIENTE
(
	id int NOT NULL IDENTITY PRIMARY KEY,
	unidadCreacion int NOT NULL,
	fechaExpte datetime NOT NULL,
	nroNota varchar(50) NOT NULL,
	origenExpte varchar(50) NOT NULL,
	extracto varchar(150) NOT NULL,
	nroIntervencion varchar(150) NULL,
	informeTecnico varchar(150) NULL,
	peritoInterviniente int FOREIGN KEY REFERENCES PERITOS(id) NULL,
	tipoPericia varchar(50) NOT NULL,
	estadoExpte int FOREIGN KEY REFERENCES ESTADO(id),
	observacion varchar(150) NULL,
	fechaCreacion datetime NOT NULL,
	usuarioCrea int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id),
	fechaModificacion datetime NULL,
	usuarioModifica int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id),
	fechaBaja datetime NULL,
	usuarioBaja int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id),
	activo bit NOT NULL,
	numerointerno varchar(50) NULL,
	personalInterviniente varchar(150) NULL,
);

CREATE TABLE MOVIMIENTOEXPTE
(
	id int NOT NULL IDENTITY PRIMARY KEY,
	expte int FOREIGN KEY REFERENCES EXPEDIENTE(id),
	destinoPolicial int NULL,
	destinoNoPolicial varchar(150) NULL,
	fechaEnvio datetime NULL,
	usuarioEnvia int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id),
	fechaRecepcion datetime NULL,
	usuarioRecibe int FOREIGN KEY REFERENCES USUARIOCRIMINALISTICA(id),
	tipoMovimiento varchar(50) NULL,
	observaciones varchar(150) NULL,
	activo bit NOT NULL
);