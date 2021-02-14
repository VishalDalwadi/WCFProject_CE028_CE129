USE [ChessDb];

CREATE TABLE SavedGames (
    _id BIGINT PRIMARY KEY IDENTITY,
    user_id BIGINT FOREIGN KEY REFERENCES Users(_id),
    game_string VARCHAR(MAX) NOT NULL,
    played_as CHAR(1) NOT NULL CHECK (played_as IN ('W', 'B'))
);
