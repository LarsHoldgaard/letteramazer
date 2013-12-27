DROP TABLE [OrderItems]
GO
DROP TABLE [Orders]
GO
DROP TABLE [Letters]
GO
DROP TABLE [Customers]
GO
DROP TABLE [Coupons]
GO
CREATE TABLE [Coupons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CouponValue] [decimal](18, 5) NOT NULL,
	[DateGiven] [datetime] NOT NULL,
	[EarlierCouponRef] [int] NULL,
	[RefSource] [nvarchar](150) NULL,
	[RefUserValue] [nvarchar](150) NULL,
	[CouponExpire] [datetime] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[CouponValueLeft] [decimal](18, 5) NOT NULL,
	[CouponStatus] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Customers](
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
	[Credits] [decimal](18, 5) NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Letters](
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
	[LetterDetail_Color] [int] NOT NULL,
	[LetterDetail_PrintQuality] [int] NOT NULL,
	[LetterDetail_PaperQuality] [int] NOT NULL,
	[LetterDetail_LetterQuality] [int] NOT NULL,
	[LetterDetail_Size] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](18, 5) NOT NULL,
	[OrderId] [int] NULL,
	[LetterId] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Orders](
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
	[Phone] [nvarchar](50) NULL
) ON [PRIMARY]

GO

