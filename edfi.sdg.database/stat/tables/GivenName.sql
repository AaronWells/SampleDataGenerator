CREATE TABLE [stat].[GivenName] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Value]     NVARCHAR (MAX) NOT NULL,
    [Attribute] NVARCHAR (MAX) NOT NULL,
    [Prop100k]  DECIMAL (12, 4) NOT NULL
);
