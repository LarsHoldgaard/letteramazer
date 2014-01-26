
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/26/2014 21:19:14
-- Generated from EDMX file: D:\Projects\letteramazer\source\src\LetterAmazer.Business.Services\Data\LetterAmazer.edmx
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

IF OBJECT_ID(N'[dbo].[FK_Letter_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Letters] DROP CONSTRAINT [FK_Letter_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_Order_Customer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Order_Customer];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderItem_Letter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderItems] DROP CONSTRAINT [FK_OrderItem_Letter];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderItem_Order]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderItems] DROP CONSTRAINT [FK_OrderItem_Order];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Coupons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coupons];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Letters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Letters];
GO
IF OBJECT_ID(N'[dbo].[OrderItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderItems];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerInfo_Address] nchar(150)  NULL,
    [CustomerInfo_Address2] nchar(150)  NULL,
    [CustomerInfo_AttPerson] nchar(150)  NULL,
    [CustomerInfo_City] nchar(150)  NULL,
    [CustomerInfo_CompanyName] nchar(150)  NULL,
    [CustomerInfo_Country] nchar(150)  NULL,
    [CustomerInfo_CountryCode] nchar(10)  NULL,
    [CustomerInfo_FirstName] nchar(150)  NULL,
    [CustomerInfo_LastName] nchar(150)  NULL,
    [CustomerInfo_Postal] nchar(150)  NULL,
    [CustomerInfo_VatNr] nchar(159)  NULL,
    [Email] nvarchar(100)  NOT NULL,
    [Password] nvarchar(100)  NOT NULL,
    [Username] nvarchar(100)  NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NOT NULL,
    [Credits] decimal(18,5)  NULL,
    [ResetPasswordKey] nvarchar(50)  NULL,
    [CreditLimit] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Letters'
CREATE TABLE [dbo].[Letters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CustomerId] int  NULL,
    [FromAddress_Address] nchar(150)  NULL,
    [FromAddress_Address2] nchar(150)  NULL,
    [FromAddress_AttPerson] nchar(150)  NULL,
    [FromAddress_City] nchar(150)  NULL,
    [FromAddress_CompanyName] nchar(150)  NULL,
    [FromAddress_Country] nchar(150)  NULL,
    [FromAddress_CountryCode] nchar(10)  NULL,
    [FromAddress_FirstName] nchar(150)  NULL,
    [FromAddress_LastName] nchar(150)  NULL,
    [FromAddress_Postal] nchar(150)  NULL,
    [FromAddress_VatNr] nchar(159)  NULL,
    [ToAddress_Address] nchar(150)  NULL,
    [ToAddress_Address2] nchar(150)  NULL,
    [ToAddress_AttPerson] nchar(150)  NULL,
    [ToAddress_City] nchar(150)  NULL,
    [ToAddress_CompanyName] nchar(150)  NULL,
    [ToAddress_Country] nchar(150)  NULL,
    [ToAddress_CountryCode] nchar(10)  NULL,
    [ToAddress_FirstName] nchar(150)  NULL,
    [ToAddress_LastName] nchar(150)  NULL,
    [ToAddress_Postal] nchar(150)  NULL,
    [ToAddress_VatNr] nchar(159)  NULL,
    [LetterContent_Path] nchar(250)  NULL,
    [LetterContent_WrittenContent] nchar(4000)  NULL,
    [LetterContent_Content] varbinary(max)  NULL,
    [LetterStatus] int  NOT NULL,
    [LetterDetail_Color] int  NOT NULL,
    [LetterDetail_PrintQuality] int  NOT NULL,
    [LetterDetail_LetterQuality] int  NOT NULL,
    [LetterDetail_PaperQuality] int  NOT NULL,
    [LetterDetail_Size] int  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderStatus] int  NOT NULL,
    [CustomerId] int  NULL,
    [DateCreated] datetime  NOT NULL,
    [DateUpdated] datetime  NOT NULL,
    [Guid] nvarchar(100)  NOT NULL,
    [OrderCode] nvarchar(20)  NOT NULL,
    [TransactionCode] nvarchar(20)  NULL,
    [Cost] decimal(18,5)  NOT NULL,
    [CouponCode] nvarchar(20)  NULL,
    [Discount] decimal(18,5)  NOT NULL,
    [Price] decimal(18,5)  NOT NULL,
    [PaymentMethod] nvarchar(20)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [Phone] nvarchar(50)  NULL,
    [OrderType] int  NOT NULL
);
GO

-- Creating table 'Coupons'
CREATE TABLE [dbo].[Coupons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CouponValue] decimal(18,5)  NOT NULL,
    [DateGiven] datetime  NOT NULL,
    [EarlierCouponRef] int  NULL,
    [RefSource] nvarchar(150)  NULL,
    [RefUserValue] nvarchar(150)  NULL,
    [CouponExpire] datetime  NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [CouponValueLeft] decimal(18,5)  NOT NULL,
    [CouponStatus] int  NOT NULL
);
GO

-- Creating table 'OrderItems'
CREATE TABLE [dbo].[OrderItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Price] decimal(18,5)  NOT NULL,
    [LetterId] int  NULL,
    [OrderId] int  NULL
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

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Coupons'
ALTER TABLE [dbo].[Coupons]
ADD CONSTRAINT [PK_Coupons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderItems'
ALTER TABLE [dbo].[OrderItems]
ADD CONSTRAINT [PK_OrderItems]
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

-- Creating foreign key on [CustomerId] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK_Order_Customer]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Order_Customer'
CREATE INDEX [IX_FK_Order_Customer]
ON [dbo].[Orders]
    ([CustomerId]);
GO

-- Creating foreign key on [LetterId] in table 'OrderItems'
ALTER TABLE [dbo].[OrderItems]
ADD CONSTRAINT [FK_OrderItem_Letter]
    FOREIGN KEY ([LetterId])
    REFERENCES [dbo].[Letters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderItem_Letter'
CREATE INDEX [IX_FK_OrderItem_Letter]
ON [dbo].[OrderItems]
    ([LetterId]);
GO

-- Creating foreign key on [OrderId] in table 'OrderItems'
ALTER TABLE [dbo].[OrderItems]
ADD CONSTRAINT [FK_OrderItem_Order]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderItem_Order'
CREATE INDEX [IX_FK_OrderItem_Order]
ON [dbo].[OrderItems]
    ([OrderId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------