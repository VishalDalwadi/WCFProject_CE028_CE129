USE [ChessDb];

CREATE TABLE Users (
    _id BIGINT PRIMARY KEY IDENTITY,
    username VARCHAR(20) NOT NULL UNIQUE,
    email_id VARCHAR(100) NOT NULL UNIQUE,
    hashed_password BINARY(64) NOT NULL,
    salt BINARY(32) NOT NULL
);
