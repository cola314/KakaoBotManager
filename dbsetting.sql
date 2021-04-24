CREATE DATABASE kakaomanager;
USE kakaomanager;

CREATE TABLE user
(
	id VARCHAR(30) PRIMARY KEY,
	is_temp_password BOOL,
	password_hash VARCHAR(50) NOT NULL,
	account_type INT,
	guid VARCHAR(50) NOT NULL,
	session VARCHAR(50) 
);

CREATE TABLE application
(
    name VARCHAR(30),
    guid VARCHAR(50) PRIMARY KEY,
    owner_guid VARCHAR(50) NOT NULL,
    icon MEDIUMBLOB,
    is_use_callback BOOL
);

CREATE TABLE callbackoption 
(
    application_guid VARCHAR(50),
    http_method INT,
    url VARCHAR(50)
);