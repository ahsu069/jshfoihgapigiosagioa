ALTER TABLE SIGAPUser
ADD
    username NVARCHAR(20) NULL,
    password NVARCHAR(100) NULL,
    refresh_token NVARCHAR(100) NULL;