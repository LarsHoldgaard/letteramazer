USE [LetterAmazer]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 12/26/2013 2:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CouponValue] [decimal](18, 5) NOT NULL,
	[DateGiven] [datetime] NOT NULL,
	[EarlierCouponRef] [int] NULL,
	[RefSource] [nvarchar](150) NULL,
	[RefUserValue] [nvarchar](150) NULL,
	[CouponExpire] [datetime] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[CouponValueLeft] [decimal](18, 5) NOT NULL,
	[CouponStatus] [int] NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customers]    Script Date: 12/26/2013 2:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerInfo_Address] [nvarchar](150) NOT NULL,
	[CustomerInfo_Address2] [nvarchar](150) NULL,
	[CustomerInfo_AttPerson] [nvarchar](150) NULL,
	[CustomerInfo_City] [nvarchar](150) NOT NULL,
	[CustomerInfo_CompanyName] [nvarchar](150) NULL,
	[CustomerInfo_Country] [nvarchar](150) NULL,
	[CustomerInfo_CountryCode] [nvarchar](10) NULL,
	[CustomerInfo_FirstName] [nvarchar](150) NOT NULL,
	[CustomerInfo_LastName] [nvarchar](150) NOT NULL,
	[CustomerInfo_Postal] [nvarchar](150) NULL,
	[CustomerInfo_VatNr] [nvarchar](159) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Credits] [decimal](18, 5) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LetterDetails]    Script Date: 12/26/2013 2:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LetterDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Color] [int] NOT NULL,
	[LetterQuality] [int] NOT NULL,
	[PaperQuality] [int] NOT NULL,
	[PrintQuality] [int] NOT NULL,
	[LetterStatus] [int] NOT NULL,
	[Size] [int] NOT NULL,
	[LetterId] [int] NOT NULL,
 CONSTRAINT [PK_LetterDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Letters]    Script Date: 12/26/2013 2:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Letters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[FromAddress_Address] [nvarchar](150) NOT NULL,
	[FromAddress_Address2] [nvarchar](150) NULL,
	[FromAddress_AttPerson] [nvarchar](150) NULL,
	[FromAddress_City] [nvarchar](150) NOT NULL,
	[FromAddress_CompanyName] [nvarchar](150) NULL,
	[FromAddress_Country] [nvarchar](150) NULL,
	[FromAddress_CountryCode] [nvarchar](10) NULL,
	[FromAddress_FirstName] [nvarchar](150) NOT NULL,
	[FromAddress_LastName] [nvarchar](150) NOT NULL,
	[FromAddress_Postal] [nvarchar](150) NULL,
	[FromAddress_VatNr] [nvarchar](159) NULL,
	[ToAddress_Address] [nvarchar](150) NOT NULL,
	[ToAddress_Address2] [nvarchar](150) NULL,
	[ToAddress_AttPerson] [nvarchar](150) NULL,
	[ToAddress_City] [nvarchar](150) NOT NULL,
	[ToAddress_CompanyName] [nvarchar](150) NULL,
	[ToAddress_Country] [nvarchar](150) NULL,
	[ToAddress_CountryCode] [nvarchar](10) NULL,
	[ToAddress_FirstName] [nvarchar](150) NOT NULL,
	[ToAddress_LastName] [nvarchar](150) NOT NULL,
	[ToAddress_Postal] [nvarchar](150) NULL,
	[ToAddress_VatNr] [nvarchar](159) NULL,
	[LetterContent_Path] [nvarchar](250) NULL,
	[LetterContent_WrittenContent] [nvarchar](4000) NULL,
	[LetterContent_Content] [varbinary](max) NULL,
	[LetterStatus] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_Letters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 12/26/2013 2:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatus] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Guid] [nvarchar](100) NOT NULL,
	[OrderCode] [nvarchar](20) NULL,
	[TransactionCode] [nvarchar](20) NOT NULL,
	[Cost] [decimal](18, 5) NOT NULL,
	[CouponCode] [nvarchar](20) NULL,
	[Discount] [decimal](18, 5) NOT NULL,
	[Price] [decimal](18, 5) NOT NULL,
	[PaymentMethod] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](50) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_FK_LetterDetail_Letter]    Script Date: 12/26/2013 2:27:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_LetterDetail_Letter] ON [dbo].[LetterDetails]
(
	[LetterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Letter_Customer]    Script Date: 12/26/2013 2:27:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Letter_Customer] ON [dbo].[Letters]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Letter_Order]    Script Date: 12/26/2013 2:27:58 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Letter_Order] ON [dbo].[Letters]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LetterDetails]  WITH CHECK ADD  CONSTRAINT [FK_LetterDetail_Letter] FOREIGN KEY([LetterId])
REFERENCES [dbo].[Letters] ([Id])
GO
ALTER TABLE [dbo].[LetterDetails] CHECK CONSTRAINT [FK_LetterDetail_Letter]
GO
ALTER TABLE [dbo].[Letters]  WITH CHECK ADD  CONSTRAINT [FK_Letter_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[Letters] CHECK CONSTRAINT [FK_Letter_Customer]
GO
ALTER TABLE [dbo].[Letters]  WITH CHECK ADD  CONSTRAINT [FK_Letter_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
GO
ALTER TABLE [dbo].[Letters] CHECK CONSTRAINT [FK_Letter_Order]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Order_Customer]
GO

