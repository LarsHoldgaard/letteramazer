ALTER TABLE [Orders] DROP CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [OrderItems] DROP CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [OrderItems] DROP CONSTRAINT [FK_OrderItem_Letter]
GO
ALTER TABLE [Letters] DROP CONSTRAINT [FK_Letter_Customer]
GO
/****** Object:  Index [IX_FK_Letter_Customer]    Script Date: 1/3/2014 4:22:21 PM ******/
DROP INDEX [IX_FK_Letter_Customer] ON [Letters]
GO
/****** Object:  Table [Orders]    Script Date: 1/3/2014 4:22:21 PM ******/
DROP TABLE [Orders]
GO
/****** Object:  Table [OrderItems]    Script Date: 1/3/2014 4:22:21 PM ******/
DROP TABLE [OrderItems]
GO
/****** Object:  Table [Letters]    Script Date: 1/3/2014 4:22:21 PM ******/
DROP TABLE [Letters]
GO
/****** Object:  Table [Customers]    Script Date: 1/3/2014 4:22:21 PM ******/
DROP TABLE [Customers]
GO
/****** Object:  Table [Coupons]    Script Date: 1/3/2014 4:22:21 PM ******/
DROP TABLE [Coupons]
GO
ALTER DATABASE [LetterAmazer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LetterAmazer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LetterAmazer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LetterAmazer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LetterAmazer] SET ARITHABORT OFF 
GO
ALTER DATABASE [LetterAmazer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LetterAmazer] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [LetterAmazer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LetterAmazer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LetterAmazer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LetterAmazer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LetterAmazer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LetterAmazer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LetterAmazer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LetterAmazer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LetterAmazer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LetterAmazer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LetterAmazer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LetterAmazer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LetterAmazer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LetterAmazer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LetterAmazer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LetterAmazer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LetterAmazer] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LetterAmazer] SET  MULTI_USER 
GO
ALTER DATABASE [LetterAmazer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LetterAmazer] SET DB_CHAINING OFF 
GO
/****** Object:  Table [Coupons]    Script Date: 1/3/2014 4:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	[CouponStatus] [int] NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Customers]    Script Date: 1/3/2014 4:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerInfo_Address] [nvarchar](150) NULL,
	[CustomerInfo_Address2] [nvarchar](150) NULL,
	[CustomerInfo_AttPerson] [nvarchar](150) NULL,
	[CustomerInfo_City] [nvarchar](150) NULL,
	[CustomerInfo_CompanyName] [nvarchar](150) NULL,
	[CustomerInfo_Country] [nvarchar](150) NULL,
	[CustomerInfo_CountryCode] [nvarchar](10) NULL,
	[CustomerInfo_FirstName] [nvarchar](150) NULL,
	[CustomerInfo_LastName] [nvarchar](150) NULL,
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
/****** Object:  Table [Letters]    Script Date: 1/3/2014 4:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Letters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[FromAddress_Address] [nvarchar](150) NULL,
	[FromAddress_Address2] [nvarchar](150) NULL,
	[FromAddress_AttPerson] [nvarchar](150) NULL,
	[FromAddress_City] [nvarchar](150) NULL,
	[FromAddress_CompanyName] [nvarchar](150) NULL,
	[FromAddress_Country] [nvarchar](150) NULL,
	[FromAddress_CountryCode] [nvarchar](10) NULL,
	[FromAddress_FirstName] [nvarchar](150) NULL,
	[FromAddress_LastName] [nvarchar](150) NULL,
	[FromAddress_Postal] [nvarchar](150) NULL,
	[FromAddress_VatNr] [nvarchar](159) NULL,
	[ToAddress_Address] [nvarchar](150) NULL,
	[ToAddress_Address2] [nvarchar](150) NULL,
	[ToAddress_AttPerson] [nvarchar](150) NULL,
	[ToAddress_City] [nvarchar](150) NULL,
	[ToAddress_CompanyName] [nvarchar](150) NULL,
	[ToAddress_Country] [nvarchar](150) NULL,
	[ToAddress_CountryCode] [nvarchar](10) NULL,
	[ToAddress_FirstName] [nvarchar](150) NULL,
	[ToAddress_LastName] [nvarchar](150) NULL,
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
	[LetterDetail_Size] [int] NOT NULL,
 CONSTRAINT [PK_Letters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [OrderItems]    Script Date: 1/3/2014 4:22:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [OrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](18, 5) NOT NULL,
	[OrderId] [int] NULL,
	[LetterId] [int] NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [Orders]    Script Date: 1/3/2014 4:22:21 PM ******/
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
	[OrderCode] [nvarchar](20) NOT NULL,
	[TransactionCode] [nvarchar](20) NULL,
	[Cost] [decimal](18, 5) NOT NULL,
	[CouponCode] [nvarchar](20) NULL,
	[Discount] [decimal](18, 5) NOT NULL,
	[Price] [decimal](18, 5) NOT NULL,
	[PaymentMethod] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](50) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_FK_Letter_Customer]    Script Date: 1/3/2014 4:22:21 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Letter_Customer] ON [Letters]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [Letters]  WITH CHECK ADD  CONSTRAINT [FK_Letter_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers] ([Id])
GO
ALTER TABLE [Letters] CHECK CONSTRAINT [FK_Letter_Customer]
GO
ALTER TABLE [OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Letter] FOREIGN KEY([LetterId])
REFERENCES [Letters] ([Id])
GO
ALTER TABLE [OrderItems] CHECK CONSTRAINT [FK_OrderItem_Letter]
GO
ALTER TABLE [OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [Orders] ([Id])
GO
ALTER TABLE [OrderItems] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [Orders]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [Customers] ([Id])
GO
ALTER TABLE [Orders] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER DATABASE [LetterAmazer] SET  READ_WRITE 
GO
