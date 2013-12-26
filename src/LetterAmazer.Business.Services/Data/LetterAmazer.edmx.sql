
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/24/2013 08:57:07
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LetterAmazer];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerInfo_Address] nvarchar(150)  NOT NULL,
    [CustomerInfo_Address2] nvarchar(150)  NULL,
    [CustomerInfo_AttPerson] nvarchar(150)  NULL,
    [CustomerInfo_City] nvarchar(150)  NOT NULL,
    [CustomerInfo_CompanyName] nvarchar(150)  NULL,
    [CustomerInfo_Country] nvarchar(150)  NULL,
    [CustomerInfo_CountryCode] nvarchar(10)  NULL,
    [CustomerInfo_FirstName] nvarchar(150)  NOT NULL,
    [CustomerInfo_LastName] nvarchar(150)  NOT NULL,
    [CustomerInfo_Postal] nvarchar(150)  NULL,
    [CustomerInfo_VatNr] nvarchar(159)  NULL,
    [Email] nvarchar(100)  NOT NULL,
    [Password] nvarchar(100)  NOT NULL,
    [Username] nvarchar(100)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NOT NULL
);
GO

-- Creating table 'Letters'
CREATE TABLE [dbo].[Letters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NOT NULL,
    [FromAddress_Address] nvarchar(150)  NOT NULL,
    [FromAddress_Address2] nvarchar(150)  NULL,
    [FromAddress_AttPerson] nvarchar(150)  NULL,
    [FromAddress_City] nvarchar(150)  NOT NULL,
    [FromAddress_CompanyName] nvarchar(150)  NULL,
    [FromAddress_Country] nvarchar(150)  NULL,
    [FromAddress_CountryCode] nvarchar(10)  NULL,
    [FromAddress_FirstName] nvarchar(150)  NOT NULL,
    [FromAddress_LastName] nvarchar(150)  NOT NULL,
    [FromAddress_Postal] nvarchar(150)  NULL,
    [FromAddress_VatNr] nvarchar(159)  NULL,
    [ToAddress_Address] nvarchar(150)  NOT NULL,
    [ToAddress_Address2] nvarchar(150)  NULL,
    [ToAddress_AttPerson] nvarchar(150)  NULL,
    [ToAddress_City] nvarchar(150)  NOT NULL,
    [ToAddress_CompanyName] nvarchar(150)  NULL,
    [ToAddress_Country] nvarchar(150)  NULL,
    [ToAddress_CountryCode] nvarchar(10)  NULL,
    [ToAddress_FirstName] nvarchar(150)  NOT NULL,
    [ToAddress_LastName] nvarchar(150)  NOT NULL,
    [ToAddress_Postal] nvarchar(150)  NULL,
    [ToAddress_VatNr] nvarchar(159)  NULL,
    [LetterContent_Path] nvarchar(250)  NOT NULL,
    [LetterContent_WrittenContent] nvarchar(4000)  NOT NULL,
    [LetterContent_Content] varbinary(max) NULL,
    [LetterStatus] int  NOT NULL,
    [OrderId] int  NOT NULL
);
GO

-- Creating table 'LetterDetails'
CREATE TABLE [dbo].[LetterDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LetterQuantity] int  NOT NULL,
    [Color] int  NOT NULL,
    [LetterStatus] int  NOT NULL,
    [PaperQuality] int  NOT NULL,
    [Size] int  NOT NULL,
    [PrintQuality] int  NOT NULL,
    [LetterId] int  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderStatus] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] nvarchar(max)  NOT NULL,
    [Guid] nvarchar(100)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Letters'
ALTER TABLE [dbo].[Letters]
ADD CONSTRAINT [PK_Letters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LetterDetails'
ALTER TABLE [dbo].[LetterDetails]
ADD CONSTRAINT [PK_LetterDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CustomerId] in table 'Letters'
ALTER TABLE [dbo].[Letters]
ADD CONSTRAINT [FK_Letter_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Letter_Customer'
CREATE INDEX [IX_FK_Letter_Customer]
ON [dbo].[Letters]
    ([CustomerId]);
GO

-- Creating foreign key on [LetterId] in table 'LetterDetails'
ALTER TABLE [dbo].[LetterDetails]
ADD CONSTRAINT [FK_LetterDetail_Letter]
    FOREIGN KEY ([LetterId])
    REFERENCES [dbo].[Letters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LetterDetail_Letter'
CREATE INDEX [IX_FK_LetterDetail_Letter]
ON [dbo].[LetterDetails]
    ([LetterId]);
GO

-- Creating foreign key on [OrderId] in table 'Letters'
ALTER TABLE [dbo].[Letters]
ADD CONSTRAINT [FK_Letter_Order]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Letter_Order'
CREATE INDEX [IX_FK_Letter_Order]
ON [dbo].[Letters]
    ([OrderId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------