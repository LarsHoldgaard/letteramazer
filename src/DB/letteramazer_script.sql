USE [master]
GO
/****** Object:  Database [LetterAmazer]    Script Date: 15-02-2014 14:21:39 ******/
CREATE DATABASE [LetterAmazer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LetterAmazer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\LetterAmazer.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'LetterAmazer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\LetterAmazer_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [LetterAmazer] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LetterAmazer].[dbo].[sp_fulltext_database] @action = 'enable'
end
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
ALTER DATABASE [LetterAmazer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LetterAmazer] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [LetterAmazer]
GO
/****** Object:  Table [dbo].[DbCountries]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbCountries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [varchar](250) NULL,
	[CurrencyCode] [varchar](50) NULL,
	[FipsCode] [varchar](50) NULL,
	[CountryCode] [varchar](50) NULL,
	[IsoNumeric] [varchar](50) NULL,
	[North] [float] NULL,
	[Capital] [varchar](250) NULL,
	[ContinentName] [varchar](250) NULL,
	[AreaInSqKm] [varchar](50) NULL,
	[Languages] [varchar](250) NULL,
	[IsoAlpha3] [varchar](50) NULL,
	[Continent] [varchar](250) NULL,
	[South] [float] NULL,
	[East] [float] NULL,
	[GeonameId] [int] NULL,
	[West] [float] NULL,
	[Population] [varchar](250) NULL,
	[InsideEu] [bit] NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DbCoupons]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbCoupons](
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
	[DateModified] [datetime] NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbCustomers]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbCustomers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerInfo_Address] [nvarchar](150) NULL,
	[CustomerInfo_Address2] [nvarchar](150) NULL,
	[CustomerInfo_AttPerson] [nvarchar](150) NULL,
	[CustomerInfo_City] [nvarchar](150) NULL,
	[CustomerInfo_CompanyName] [nvarchar](150) NULL,
	[CustomerInfo_Country] [int] NULL,
	[CustomerInfo_FirstName] [nvarchar](150) NULL,
	[CustomerInfo_LastName] [nvarchar](150) NULL,
	[CustomerInfo_Postal] [nvarchar](150) NULL,
	[CustomerInfo_VatNr] [nvarchar](159) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Credits] [decimal](18, 5) NULL,
	[CreditLimit] [decimal](18, 0) NOT NULL,
	[ResetPasswordKey] [nvarchar](50) NULL,
	[Phone] [nvarchar](100) NULL,
	[CustomerInfo_State] [nvarchar](150) NULL,
	[CustomerInfo_Co] [nvarchar](150) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbFulfillmentPartners]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbFulfillmentPartners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[ShopId] [int] NOT NULL,
	[PartnerJob] [int] NOT NULL,
 CONSTRAINT [PK_FulfillmentPartners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DbLetters]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbLetters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[FromAddress_Address] [nvarchar](150) NULL,
	[FromAddress_Address2] [nvarchar](150) NULL,
	[FromAddress_AttPerson] [nvarchar](150) NULL,
	[FromAddress_City] [nvarchar](150) NULL,
	[FromAddress_CompanyName] [nvarchar](150) NULL,
	[FromAddress_Country] [int] NULL,
	[FromAddress_FirstName] [nvarchar](150) NULL,
	[FromAddress_LastName] [nvarchar](150) NULL,
	[FromAddress_Postal] [nvarchar](150) NULL,
	[FromAddress_VatNr] [nvarchar](159) NULL,
	[ToAddress_Address] [nvarchar](150) NOT NULL,
	[ToAddress_Address2] [nvarchar](150) NULL,
	[ToAddress_AttPerson] [nvarchar](150) NULL,
	[ToAddress_City] [nvarchar](150) NOT NULL,
	[ToAddress_CompanyName] [nvarchar](150) NULL,
	[ToAddress_Country] [int] NOT NULL,
	[ToAddress_FirstName] [nvarchar](150) NULL,
	[ToAddress_LastName] [nvarchar](150) NULL,
	[ToAddress_Postal] [nvarchar](150) NOT NULL,
	[ToAddress_VatNr] [nvarchar](159) NULL,
	[LetterContent_Path] [nvarchar](250) NULL,
	[LetterContent_WrittenContent] [nvarchar](4000) NULL,
	[LetterContent_Content] [varbinary](max) NULL,
	[LetterStatus] [int] NOT NULL,
	[OfficeProductId] [int] NOT NULL,
	[ToAddress_State] [nvarchar](150) NULL,
	[ToAddress_Co] [nvarchar](150) NULL,
	[FromAddress_State] [nvarchar](150) NULL,
	[FromAddress_Co] [nvarchar](150) NULL,
	[OrderId] [int] NOT NULL,
	[LetterColor] [int] NOT NULL,
	[LetterPaperWeight] [int] NOT NULL,
	[LetterProcessing] [int] NOT NULL,
	[LetterSize] [int] NOT NULL,
	[LetterType] [int] NOT NULL,
 CONSTRAINT [PK_Letters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DbOfficeProductDetails]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOfficeProductDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LetterColor] [int] NOT NULL,
	[LetterPaperWeight] [int] NOT NULL,
	[LetterProcessing] [int] NOT NULL,
	[LetterSize] [int] NOT NULL,
	[LetterType] [int] NOT NULL,
 CONSTRAINT [PK_OfficeProductDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbOfficeProducts]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOfficeProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OfficeId] [int] NOT NULL,
	[ScopeType] [int] NOT NULL,
	[CountryId] [int] NULL,
	[ContinentId] [int] NULL,
	[ZipId] [int] NULL,
	[ProductDetailsId] [int] NOT NULL,
 CONSTRAINT [PK_OfficeProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbOffices]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbOffices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[CountryId] [int] NOT NULL,
	[PartnerId] [int] NOT NULL,
 CONSTRAINT [PK_Offices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DbOrderItems]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOrderItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NULL,
	[LetterId] [int] NULL,
	[ItemType] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbOrders]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatus] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[OrderCode] [nvarchar](20) NOT NULL,
	[TransactionCode] [nvarchar](20) NULL,
	[Cost] [decimal](18, 5) NOT NULL,
	[CouponCode] [nvarchar](20) NULL,
	[Discount] [decimal](18, 5) NOT NULL,
	[Price] [decimal](18, 5) NOT NULL,
	[PaymentMethod] [nvarchar](20) NOT NULL,
	[DatePaid] [datetime] NULL,
	[DateSent] [datetime] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbPricing]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbPricing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OfficeProductId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
 CONSTRAINT [PK_DbPricing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbProductMatrix]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbProductMatrix](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PriceType] [int] NOT NULL,
	[Span_lower] [int] NOT NULL,
	[Span_upper] [int] NOT NULL,
	[ValueId] [int] NOT NULL,
	[ReferenceType] [int] NOT NULL,
 CONSTRAINT [PK_ProductPriceMatrix] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DbProductMatrixLines]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbProductMatrixLines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](250) NOT NULL,
	[LineType] [int] NOT NULL,
	[BaseCost] [decimal](18, 2) NOT NULL,
	[ProductMatrixId] [int] NOT NULL,
 CONSTRAINT [PK_PriceMatrixOrderLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DbShops]    Script Date: 15-02-2014 14:21:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DbShops](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](250) NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_Shops] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[DbCountries] ON 

INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (1, N'Andorra', N'EUR', N'AN', N'AD', N'020', 42.656044006347656, N'Andorra la Vella', N'Europe', N'468.0', N'ca', N'AND', N'EU', 42.428493499755859, 1.7865427732467651, 3041565, 1.40718674659729, N'84000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (2, N'United Arab Emirates', N'AED', N'AE', N'AE', N'784', 26.084159851074219, N'Abu Dhabi', N'Asia', N'82880.0', N'ar-AE,fa,en,hi,ur', N'ARE', N'AS', 22.633329391479492, 56.381660461425781, 290557, 51.583328247070312, N'4975593', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (3, N'Afghanistan', N'AFN', N'AF', N'AF', N'004', 38.483417510986328, N'Kabul', N'Asia', N'647500.0', N'fa-AF,ps,uz-AF,tk', N'AFG', N'AS', 29.377471923828125, 74.879447937011719, 1149361, 60.478443145751953, N'29121286', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (4, N'Antigua and Barbuda', N'XCD', N'AC', N'AG', N'028', 17.729387283325195, N'St. John''s', N'North America', N'443.0', N'en-AG', N'ATG', N'NA', 16.996978759765625, -61.672420501708984, 3576396, -61.906425476074219, N'86754', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (5, N'Anguilla', N'XCD', N'AV', N'AI', N'660', 18.283424377441406, N'The Valley', N'North America', N'102.0', N'en-AI', N'AIA', N'NA', 18.166814804077148, -62.971359252929688, 3573511, -63.172901153564453, N'13254', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (6, N'Albania', N'ALL', N'AL', N'AL', N'008', 42.665611267089837, N'Tirana', N'Europe', N'28748.0', N'sq,el', N'ALB', N'EU', 39.648361206054688, 21.068471908569336, 783754, 19.293972015380859, N'2986952', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (7, N'Armenia', N'AMD', N'AM', N'AM', N'051', 41.301834106445312, N'Yerevan', N'Asia', N'29800.0', N'hy', N'ARM', N'AS', 38.830528259277344, 46.772434234619141, 174982, 43.449779510498047, N'2968000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (8, N'Angola', N'AOA', N'AO', N'AO', N'024', -4.37682580947876, N'Luanda', N'Africa', N'1246700.0', N'pt-AO', N'AGO', N'AF', -18.042076110839844, 24.082118988037109, 3351879, 11.679219245910645, N'13068161', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (9, N'Antarctica', N'', N'AY', N'AQ', N'010', -60.515533447265625, N'', N'Antarctica', N'1.4E7', N'', N'ATA', N'AN', -89.9999008178711, 179.99989318847656, 6697173, -179.99989318847656, N'0', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (10, N'Argentina', N'ARS', N'AR', N'AR', N'032', -21.781276702880859, N'Buenos Aires', N'South America', N'2766890.0', N'es-AR,en,it,de,fr,gn', N'ARG', N'SA', -55.061313629150391, -53.591835021972656, 3865483, -73.582969665527344, N'41343201', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (11, N'American Samoa', N'USD', N'AQ', N'AS', N'016', -11.049699783325195, N'Pago Pago', N'Oceania', N'199.0', N'en-AS,sm,to', N'ASM', N'OC', -14.38247776031494, -169.41607666015625, 5880801, -171.09188842773438, N'57881', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (12, N'Austria', N'EUR', N'AU', N'AT', N'040', 49.017055511474609, N'Vienna', N'Europe', N'83858.0', N'de-AT,hr,hu,sl', N'AUT', N'EU', 46.378028869628906, 17.162721633911133, 2782113, 9.5359163284301758, N'8205000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (13, N'Australia', N'AUD', N'AS', N'AU', N'036', -10.06280517578125, N'Canberra', N'Oceania', N'7686850.0', N'en-AU', N'AUS', N'OC', -43.643970489501953, 153.63925170898438, 2077456, 112.91105651855467, N'21515754', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (14, N'Aruba', N'AWG', N'AA', N'AW', N'533', 12.62371826171875, N'Oranjestad', N'North America', N'193.0', N'nl-AW,es,en', N'ABW', N'NA', 12.411707878112791, -69.865753173828125, 3577279, -70.0644760131836, N'71566', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (15, N'Åland', N'EUR', N'', N'AX', N'248', 60.488861083984375, N'Mariehamn', N'Europe', N'', N'sv-AX', N'ALA', N'EU', 59.9067497253418, 21.011861801147461, 661882, 19.317693710327148, N'26711', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (16, N'Azerbaijan', N'AZN', N'AJ', N'AZ', N'031', 41.9056396484375, N'Baku', N'Asia', N'86600.0', N'az,ru,hy', N'AZE', N'AS', 38.389152526855469, 50.370082855224609, 587116, 44.774112701416016, N'8303512', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (17, N'Bosnia and Herzegovina', N'BAM', N'BK', N'BA', N'070', 45.239192962646477, N'Sarajevo', N'Europe', N'51129.0', N'bs,hr-BA,sr-BA', N'BIH', N'EU', 42.546112060546875, 19.622222900390625, 3277605, 15.718944549560549, N'4590000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (18, N'Barbados', N'BBD', N'BB', N'BB', N'052', 13.32725715637207, N'Bridgetown', N'North America', N'431.0', N'en-BB', N'BRB', N'NA', 13.039843559265137, -59.420375823974609, 3374084, -59.648921966552734, N'285653', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (19, N'Bangladesh', N'BDT', N'BG', N'BD', N'050', 26.63194465637207, N'Dhaka', N'Asia', N'144000.0', N'bn-BD,en', N'BGD', N'AS', 20.74333381652832, 92.673667907714844, 1210997, 88.028335571289062, N'156118464', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (20, N'Belgium', N'EUR', N'BE', N'BE', N'056', 51.505443572998047, N'Brussels', N'Europe', N'30510.0', N'nl-BE,fr-BE,de-BE', N'BEL', N'EU', 49.493610382080078, 6.4038610458374023, 2802361, 2.5469439029693604, N'10403000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (21, N'Burkina Faso', N'XOF', N'UV', N'BF', N'854', 15.082592964172363, N'Ouagadougou', N'Africa', N'274200.0', N'fr-BF', N'BFA', N'AF', 9.4011077880859375, 2.4053950309753418, 2361809, -5.518916130065918, N'16241811', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (22, N'Bulgaria', N'BGN', N'BU', N'BG', N'100', 44.2176399230957, N'Sofia', N'Europe', N'110910.0', N'bg,tr-BG', N'BGR', N'EU', 41.242084503173821, 28.612167358398441, 732800, 22.371166229248047, N'7148785', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (23, N'Bahrain', N'BHD', N'BA', N'BH', N'048', 26.282583236694336, N'Manama', N'Asia', N'665.0', N'ar-BH,en,fa,ur', N'BHR', N'AS', 25.79686164855957, 50.664470672607422, 290291, 50.454139709472656, N'738004', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (24, N'Burundi', N'BIF', N'BY', N'BI', N'108', -2.3101229667663574, N'Bujumbura', N'Africa', N'27830.0', N'fr-BI,rn', N'BDI', N'AF', -4.4657130241394043, 30.847728729248047, 433561, 28.993061065673828, N'9863117', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (25, N'Benin', N'XOF', N'BN', N'BJ', N'204', 12.418347358703612, N'Porto-Novo', N'Africa', N'112620.0', N'fr-BJ', N'BEN', N'AF', 6.2257480621337891, 3.851701021194458, 2395170, 0.77457499504089355, N'9056010', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (26, N'Saint Barthélemy', N'EUR', N'TB', N'BL', N'652', 17.928808212280273, N'Gustavia', N'North America', N'21.0', N'fr', N'BLM', N'NA', 17.878183364868164, -62.788982391357422, 3578476, -62.8739128112793, N'8450', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (27, N'Bermuda', N'BMD', N'BD', N'BM', N'060', 32.393833160400391, N'Hamilton', N'North America', N'53.0', N'en-BM,pt', N'BMU', N'NA', 32.246639251708984, -64.651992797851562, 3573345, -64.896049499511719, N'65365', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (28, N'Brunei', N'BND', N'BX', N'BN', N'096', 5.04716682434082, N'Bandar Seri Begawan', N'Asia', N'5770.0', N'ms-BN,en-BN', N'BRN', N'AS', 4.0030832290649414, 115.35944366455078, 1820814, 114.07144165039064, N'395027', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (29, N'Bolivia', N'BOB', N'BL', N'BO', N'068', -9.6805667877197266, N'Sucre', N'South America', N'1098580.0', N'es-BO,qu,ay', N'BOL', N'SA', -22.896133422851559, -57.458095550537109, 3923057, -69.640762329101562, N'9947418', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (30, N'Bonaire', N'USD', N'', N'BQ', N'535', 12.304534912109377, N'', N'North America', N'', N'nl,pap,en', N'BES', N'NA', 12.017148971557615, -68.192306518554688, 7626844, -68.416458129882812, N'18012', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (31, N'Brazil', N'BRL', N'BR', N'BR', N'076', 5.2648768424987793, N'Brasília', N'South America', N'8511965.0', N'pt-BR,es,en,fr', N'BRA', N'SA', -33.750705718994141, -32.392997741699219, 3469034, -73.98553466796875, N'201103330', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (32, N'Bahamas', N'BSD', N'BF', N'BS', N'044', 26.919242858886719, N'Nassau', N'North America', N'13940.0', N'en-BS', N'BHS', N'NA', 22.852743148803711, -74.423873901367188, 3572887, -78.99591064453125, N'301790', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (33, N'Bhutan', N'BTN', N'BT', N'BT', N'064', 28.32377815246582, N'Thimphu', N'Asia', N'47000.0', N'dz', N'BTN', N'AS', 26.707639694213867, 92.125190734863281, 1252634, 88.759719848632812, N'699847', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (34, N'Bouvet Island', N'NOK', N'BV', N'BV', N'074', -54.400321960449219, N'', N'Antarctica', N'', N'', N'BVT', N'AN', -54.462383270263672, 3.48797607421875, 3371123, 3.3354990482330318, N'0', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (35, N'Botswana', N'BWP', N'BC', N'BW', N'072', -17.780813217163086, N'Gaborone', N'Africa', N'600370.0', N'en-BW,tn-BW', N'BWA', N'AF', -26.907245635986328, 29.360780715942383, 933860, 19.999534606933594, N'2029307', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (36, N'Belarus', N'BYR', N'BO', N'BY', N'112', 56.165805816650391, N'Minsk', N'Europe', N'207600.0', N'be,ru', N'BLR', N'EU', 51.256416320800781, 32.770805358886719, 630336, 23.176889419555664, N'9685000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (37, N'Belize', N'BZD', N'BH', N'BZ', N'084', 18.496557235717773, N'Belmopan', N'North America', N'22966.0', N'en-BZ,es', N'BLZ', N'NA', 15.889300346374512, -87.776985168457017, 3582678, -89.224815368652358, N'314522', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (38, N'Canada', N'CAD', N'CA', N'CA', N'124', 83.110626220703125, N'Ottawa', N'North America', N'9984670.0', N'en-CA,fr-CA,iu', N'CAN', N'NA', 41.675979614257805, -52.63629150390625, 6251999, -141, N'33679000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (39, N'Cocos [Keeling] Islands', N'AUD', N'CK', N'CC', N'166', -12.072459220886232, N'West Island', N'Asia', N'14.0', N'ms-CC,en', N'CCK', N'AS', -12.208725929260254, 96.929489135742188, 1547376, 96.816940307617188, N'628', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (40, N'Democratic Republic of the Congo', N'CDF', N'CG', N'CD', N'180', 5.3860979080200195, N'Kinshasa', N'Africa', N'2345410.0', N'fr-CD,ln,kg', N'COD', N'AF', -13.45567512512207, 31.305912017822266, 203312, 12.204143524169922, N'70916439', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (41, N'Central African Republic', N'XAF', N'CT', N'CF', N'140', 11.007569313049316, N'Bangui', N'Africa', N'622984.0', N'fr-CF,sg,ln,kg', N'CAF', N'AF', 2.2205140590667725, 27.463420867919918, 239880, 14.420097351074221, N'4844927', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (42, N'Republic of the Congo', N'XAF', N'CF', N'CG', N'178', 3.7030820846557617, N'Brazzaville', N'Africa', N'342000.0', N'fr-CG,kg,ln-CG', N'COG', N'AF', -5.0272231101989746, 18.649839401245117, 2260494, 11.205009460449221, N'3039126', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (43, N'Switzerland', N'CHF', N'SZ', N'CH', N'756', 47.805332183837891, N'Berne', N'Europe', N'41290.0', N'de-CH,fr-CH,it-CH,rm', N'CHE', N'EU', 45.8256950378418, 10.491472244262695, 2658434, 5.95747184753418, N'7581000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (44, N'Ivory Coast', N'XOF', N'IV', N'CI', N'384', 10.736641883850098, N'Yamoussoukro', N'Africa', N'322460.0', N'fr-CI', N'CIV', N'AF', 4.3570671081542969, -2.49489688873291, 2287781, -8.5993022918701172, N'21058798', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (45, N'Cook Islands', N'NZD', N'CW', N'CK', N'184', -10.023114204406738, N'Avarua', N'Oceania', N'240.0', N'en-CK,mi', N'COK', N'OC', -21.944164276123047, -157.3121337890625, 1899402, -161.09365844726565, N'21388', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (46, N'Chile', N'CLP', N'CI', N'CL', N'152', -17.507553100585938, N'Santiago', N'South America', N'756950.0', N'es-CL', N'CHL', N'SA', -55.916347503662109, -66.417556762695312, 3895114, -80.78585052490233, N'16746491', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (47, N'Cameroon', N'XAF', N'CM', N'CM', N'120', 13.078056335449221, N'Yaoundé', N'Africa', N'475440.0', N'en-CM,fr-CM', N'CMR', N'AF', 1.6525479555130005, 16.192115783691406, 2233387, 8.4947633743286133, N'19294149', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (48, N'China', N'CNY', N'CH', N'CN', N'156', 53.560859680175781, N'Beijing', N'Asia', N'9596960.0', N'zh-CN,yue,wuu,dta,ug,za', N'CHN', N'AS', 15.775416374206545, 134.77391052246094, 1814991, 73.557693481445312, N'1330044000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (49, N'Colombia', N'COP', N'CO', N'CO', N'170', 13.380501747131348, N'Bogotá', N'South America', N'1138910.0', N'es-CO', N'COL', N'SA', -4.2258691787719727, -66.869834899902344, 3686110, -81.728111267089844, N'44205293', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (50, N'Costa Rica', N'CRC', N'CS', N'CR', N'188', 11.216818809509276, N'San José', N'North America', N'51100.0', N'es-CR,en', N'CRI', N'NA', 8.0329751968383789, -82.555992126464844, 3624060, -85.95062255859375, N'4516220', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (51, N'Cuba', N'CUP', N'CU', N'CU', N'192', 23.226041793823239, N'Havana', N'North America', N'110860.0', N'es-CU', N'CUB', N'NA', 19.828083038330082, -74.13177490234375, 3562981, -84.957427978515625, N'11423000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (52, N'Cape Verde', N'CVE', N'CV', N'CV', N'132', 17.197177886962891, N'Praia', N'Africa', N'4033.0', N'pt-CV', N'CPV', N'AF', 14.808021545410156, -22.669443130493164, 3374766, -25.358747482299805, N'508659', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (53, N'Curacao', N'ANG', N'UC', N'CW', N'531', 12.385671615600586, N'Willemstad', N'North America', N'', N'nl,pap', N'CUW', N'NA', 12.032745361328123, -68.73394775390625, 7626836, -69.1572036743164, N'141766', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (54, N'Christmas Island', N'AUD', N'KT', N'CX', N'162', -10.412356376647947, N'The Settlement', N'Asia', N'135.0', N'en,zh,ms-CC', N'CXR', N'AS', -10.570483207702637, 105.7126007080078, 2078138, 105.53327941894533, N'1500', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (55, N'Cyprus', N'EUR', N'CY', N'CY', N'196', 35.7015266418457, N'Nicosia', N'Europe', N'9250.0', N'el-CY,tr-CY,en', N'CYP', N'EU', 34.633285522460938, 34.597915649414062, 146669, 32.2730827331543, N'1102677', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (56, N'Czech Republic', N'CZK', N'EZ', N'CZ', N'203', 51.058887481689453, N'Prague', N'Europe', N'78866.0', N'cs,sk', N'CZE', N'EU', 48.542915344238281, 18.860111236572266, 3077311, 12.096194267272947, N'10476000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (57, N'Germany', N'EUR', N'GM', N'DE', N'276', 55.055637359619141, N'Berlin', N'Europe', N'357021.0', N'de', N'DEU', N'EU', 47.275775909423821, 15.039889335632324, 2921044, 5.8656392097473145, N'81802257', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (58, N'Djibouti', N'DJF', N'DJ', N'DJ', N'262', 12.706832885742188, N'Djibouti', N'Africa', N'23000.0', N'fr-DJ,ar,so-DJ,aa', N'DJI', N'AF', 10.909916877746582, 43.416973114013672, 223816, 41.773471832275391, N'740528', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (59, N'Denmark', N'DKK', N'DA', N'DK', N'208', 57.748416900634766, N'Copenhagen', N'Europe', N'43094.0', N'da-DK,en,fo,de-DK', N'DNK', N'EU', 54.5623893737793, 15.158834457397459, 2623032, 8.0756111145019531, N'5484000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (60, N'Dominica', N'XCD', N'DO', N'DM', N'212', 15.631809234619141, N'Roseau', N'North America', N'754.0', N'en-DM', N'DMA', N'NA', 15.201689720153809, -61.2441520690918, 3575830, -61.484107971191406, N'72813', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (61, N'Dominican Republic', N'DOP', N'DR', N'DO', N'214', 19.929859161376953, N'Santo Domingo', N'North America', N'48730.0', N'es-DO', N'DOM', N'NA', 17.543159484863281, -68.319999694824219, 3508796, -72.003486633300781, N'9823821', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (62, N'Algeria', N'DZD', N'AG', N'DZ', N'012', 37.093723297119141, N'Algiers', N'Africa', N'2381740.0', N'ar-DZ', N'DZA', N'AF', 18.960027694702148, 11.979548454284668, 2589581, -8.6738681793212891, N'34586184', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (63, N'Ecuador', N'USD', N'EC', N'EC', N'218', 1.439020037651062, N'Quito', N'South America', N'283560.0', N'es-EC', N'ECU', N'SA', -4.9988231658935547, -75.184585571289062, 3658394, -81.078598022460938, N'14790608', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (64, N'Estonia', N'EUR', N'EN', N'EE', N'233', 59.676223754882812, N'Tallinn', N'Europe', N'45226.0', N'et,ru', N'EST', N'EU', 57.516193389892578, 28.209972381591797, 453733, 21.837583541870117, N'1291170', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (65, N'Egypt', N'EGP', N'EG', N'EG', N'818', 31.667333602905273, N'Cairo', N'Africa', N'1001450.0', N'ar-EG,en,fr', N'EGY', N'AF', 21.72538948059082, 35.79486083984375, 357994, 24.698110580444336, N'80471869', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (66, N'Western Sahara', N'MAD', N'WI', N'EH', N'732', 27.669673919677734, N'El Aaiún', N'Africa', N'266000.0', N'ar,mey', N'ESH', N'AF', 20.774158477783203, -8.6702756881713867, 2461445, -17.103181838989258, N'273008', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (67, N'Eritrea', N'ERN', N'ER', N'ER', N'232', 18.003084182739258, N'Asmara', N'Africa', N'121320.0', N'aa-ER,ar,tig,kun,ti-ER', N'ERI', N'AF', 12.3595552444458, 43.134639739990234, 338010, 36.438777923583984, N'5792984', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (68, N'Spain', N'EUR', N'SP', N'ES', N'724', 43.791721343994141, N'Madrid', N'Europe', N'504782.0', N'es-ES,ca,gl,eu,oc', N'ESP', N'EU', 36.000331878662109, 4.3153891563415527, 2510769, -9.2907781600952148, N'46505963', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (69, N'Ethiopia', N'ETB', N'ET', N'ET', N'231', 14.893750190734863, N'Addis Ababa', N'Africa', N'1127127.0', N'am,en-ET,om-ET,ti-ET,so-ET,sid', N'ETH', N'AF', 3.4024219512939453, 47.986179351806641, 337996, 32.99993896484375, N'88013491', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (70, N'Finland', N'EUR', N'FI', N'FI', N'246', 70.096054077148438, N'Helsinki', N'Europe', N'337030.0', N'fi-FI,sv-FI,smn', N'FIN', N'EU', 59.80877685546875, 31.580944061279297, 660013, 20.556943893432617, N'5244000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (71, N'Fiji', N'FJD', N'FJ', N'FJ', N'242', -12.480111122131348, N'Suva', N'Oceania', N'18270.0', N'en-FJ,fj', N'FJI', N'OC', -20.675970077514648, -178.4244384765625, 2205218, 177.12933349609375, N'875983', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (72, N'Falkland Islands', N'FKP', N'FK', N'FK', N'238', -51.240650177001953, N'Stanley', N'South America', N'12173.0', N'en-FK', N'FLK', N'SA', -52.360511779785149, -57.712486267089837, 3474414, -61.345191955566406, N'2638', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (73, N'Micronesia', N'USD', N'FM', N'FM', N'583', 10.08903980255127, N'Palikir', N'Oceania', N'702.0', N'en-FM,chk,pon,yap,kos,uli,woe,nkr,kpg', N'FSM', N'OC', 1.0262900590896606, 163.03717041015625, 2081918, 137.33648681640625, N'107708', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (74, N'Faroe Islands', N'DKK', N'FO', N'FO', N'234', 62.400749206542969, N'Tórshavn', N'Europe', N'1399.0', N'fo,da-FO', N'FRO', N'EU', 61.394943237304688, -6.3995828628540039, 2622320, -7.4580001831054688, N'48228', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (75, N'France', N'EUR', N'FR', N'FR', N'250', 51.092803955078125, N'Paris', N'Europe', N'547030.0', N'fr-FR,frp,br,co,ca,eu,oc', N'FRA', N'EU', 41.37158203125, 9.5615558624267578, 3017382, -5.1422219276428223, N'64768389', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (76, N'Gabon', N'XAF', N'GB', N'GA', N'266', 2.3226120471954346, N'Libreville', N'Africa', N'267667.0', N'fr-GA', N'GAB', N'AF', -3.9788060188293457, 14.502346992492676, 2400553, 8.6954708099365234, N'1545255', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (77, N'United Kingdom', N'GBP', N'UK', N'GB', N'826', 59.360248565673821, N'London', N'Europe', N'244820.0', N'en-GB,cy-GB,gd', N'GBR', N'EU', 49.906192779541016, 1.7589999437332151, 2635167, -8.6235551834106445, N'62348447', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (78, N'Grenada', N'XCD', N'GJ', N'GD', N'308', 12.318284034729004, N'St. George''s', N'North America', N'344.0', N'en-GD', N'GRD', N'NA', 11.986892700195313, -61.5767707824707, 3580239, -61.802345275878906, N'107818', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (79, N'Georgia', N'GEL', N'GG', N'GE', N'268', 43.586498260498047, N'Tbilisi', N'Asia', N'69700.0', N'ka,ru,hy,az', N'GEO', N'AS', 41.053195953369141, 46.725971221923821, 614540, 40.010139465332031, N'4630000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (80, N'French Guiana', N'EUR', N'FG', N'GF', N'254', 5.7764959335327148, N'Cayenne', N'South America', N'91000.0', N'fr-GF', N'GUF', N'SA', 2.127094030380249, -51.613948822021491, 3381670, -54.542510986328125, N'195506', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (81, N'Guernsey', N'GBP', N'GK', N'GG', N'831', 49.738609313964851, N'St Peter Port', N'Europe', N'78.0', N'en,fr', N'GGY', N'EU', 49.412776947021491, -2.1638889312744141, 3042362, -2.6824719905853271, N'65228', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (82, N'Ghana', N'GHS', N'GH', N'GH', N'288', 11.173300743103027, N'Accra', N'Africa', N'239460.0', N'en-GH,ak,ee,tw', N'GHA', N'AF', 4.7367229461669922, 1.1917810440063477, 2300660, -3.2554199695587158, N'24339838', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (83, N'Gibraltar', N'GIP', N'GI', N'GI', N'292', 36.155437469482422, N'Gibraltar', N'Europe', N'6.5', N'en-GI,es,it,pt', N'GIB', N'EU', 36.109031677246094, -5.338284969329834, 2411586, -5.3662614822387695, N'27884', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (84, N'Greenland', N'DKK', N'GL', N'GL', N'304', 83.627357482910156, N'Nuuk', N'North America', N'2166086.0', N'kl,da-GL,en', N'GRL', N'NA', 59.777400970458984, -11.312318801879885, 3425505, -73.042030334472656, N'56375', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (85, N'Gambia', N'GMD', N'GA', N'GM', N'270', 13.826571464538574, N'Banjul', N'Africa', N'11300.0', N'en-GM,mnk,wof,wo,ff', N'GMB', N'AF', 13.064251899719238, -13.7977933883667, 2413451, -16.8250789642334, N'1593256', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (86, N'Guinea', N'GNF', N'GV', N'GN', N'324', 12.676219940185549, N'Conakry', N'Africa', N'245857.0', N'fr-GN', N'GIN', N'AF', 7.1935529708862305, -7.64107084274292, 2420477, -14.926618576049805, N'10324025', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (87, N'Guadeloupe', N'EUR', N'GP', N'GP', N'312', 16.516847610473633, N'Basse-Terre', N'North America', N'1780.0', N'fr-GP', N'GLP', N'NA', 15.867565155029297, -61, 3579143, -61.544765472412109, N'443000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (88, N'Equatorial Guinea', N'XAF', N'EK', N'GQ', N'226', 2.3469889163970947, N'Malabo', N'Africa', N'28051.0', N'es-GQ,fr', N'GNQ', N'AF', 0.92085999250411987, 11.335723876953123, 2309096, 9.3468647003173828, N'1014999', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (89, N'Greece', N'EUR', N'GR', N'GR', N'300', 41.748500823974609, N'Athens', N'Europe', N'131940.0', N'el-GR,en,fr', N'GRC', N'EU', 34.802066802978516, 28.24708366394043, 390903, 19.373603820800781, N'11000000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (90, N'South Georgia and the South Sandwich Islands', N'GBP', N'SX', N'GS', N'239', -53.970466613769531, N'Grytviken', N'Antarctica', N'3903.0', N'en', N'SGS', N'AN', -59.4792594909668, -26.229326248168945, 3474415, -38.021175384521491, N'30', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (91, N'Guatemala', N'GTQ', N'GT', N'GT', N'320', 17.815219879150391, N'Guatemala City', N'North America', N'108890.0', N'es-GT', N'GTM', N'NA', 13.737301826477053, -88.223197937011719, 3595528, -92.236289978027344, N'13550440', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (92, N'Guam', N'USD', N'GQ', N'GU', N'316', 13.65233325958252, N'Hagåtña', N'Oceania', N'549.0', N'en-GU,ch-GU', N'GUM', N'OC', 13.240611076354981, 144.9539794921875, 4043988, 144.61924743652344, N'159358', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (93, N'Guinea-Bissau', N'XOF', N'PU', N'GW', N'624', 12.680788993835447, N'Bissau', N'Africa', N'36120.0', N'pt-GW,pov', N'GNB', N'AF', 10.924264907836914, -13.63652229309082, 2372248, -16.7175350189209, N'1565126', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (94, N'Guyana', N'GYD', N'GY', N'GY', N'328', 8.55756664276123, N'Georgetown', N'South America', N'214970.0', N'en-GY', N'GUY', N'SA', 1.1750799417495728, -56.480251312255859, 3378535, -61.384761810302734, N'748486', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (95, N'Hong Kong', N'HKD', N'HK', N'HK', N'344', 22.559778213500977, N'Hong Kong', N'Asia', N'1092.0', N'zh-HK,yue,zh,en', N'HKG', N'AS', 22.153249740600582, 114.43475341796876, 1819730, 113.83775329589844, N'6898686', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (96, N'Heard Island and McDonald Islands', N'AUD', N'HM', N'HM', N'334', -52.909416198730469, N'', N'Antarctica', N'412.0', N'', N'HMD', N'AN', -53.192001342773438, 73.859146118164062, 1547314, 72.5965347290039, N'0', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (97, N'Honduras', N'HNL', N'HO', N'HN', N'340', 16.510255813598633, N'Tegucigalpa', N'North America', N'112090.0', N'es-HN', N'HND', N'NA', 12.98241138458252, -83.155403137207017, 3608932, -89.350791931152358, N'7989415', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (98, N'Croatia', N'HRK', N'HR', N'HR', N'191', 46.538749694824219, N'Zagreb', N'Europe', N'56542.0', N'hr-HR,sr', N'HRV', N'EU', 42.435890197753906, 19.427389144897461, 3202326, 13.4932222366333, N'4491000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (99, N'Haiti', N'HTG', N'HA', N'HT', N'332', 20.087820053100582, N'Port-au-Prince', N'North America', N'27750.0', N'ht,fr-HT', N'HTI', N'NA', 18.021032333374023, -71.613357543945312, 3723988, -74.478584289550781, N'9648924', NULL)
GO
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (100, N'Hungary', N'HUF', N'HU', N'HU', N'348', 48.585666656494141, N'Budapest', N'Europe', N'93030.0', N'hu-HU', N'HUN', N'EU', 45.743610382080078, 22.9060001373291, 719819, 16.111888885498047, N'9982000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (101, N'Indonesia', N'IDR', N'ID', N'ID', N'360', 5.9044170379638672, N'Jakarta', N'Asia', N'1919440.0', N'id,en,nl,jv', N'IDN', N'AS', -10.941861152648926, 141.02180480957031, 1643084, 95.009330749511719, N'242968342', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (102, N'Ireland', N'EUR', N'EI', N'IE', N'372', 55.387916564941406, N'Dublin', N'Europe', N'70280.0', N'en-IE,ga-IE', N'IRL', N'EU', 51.451583862304688, -6.0023889541625977, 2963597, -10.478555679321287, N'4622917', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (103, N'Israel', N'ILS', N'IS', N'IL', N'376', 33.340137481689453, N'', N'Asia', N'20770.0', N'he,ar-IL,en-IL,', N'ISR', N'AS', 29.496639251708984, 35.876804351806641, 294640, 34.270278930664062, N'7353985', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (104, N'Isle of Man', N'GBP', N'IM', N'IM', N'833', 54.419723510742195, N'Douglas', N'Europe', N'572.0', N'en,gv', N'IMN', N'EU', 54.055915832519531, -4.311500072479248, 3042225, -4.7987217903137207, N'75049', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (105, N'India', N'INR', N'IN', N'IN', N'356', 35.504222869873047, N'New Delhi', N'Asia', N'3287590.0', N'en-IN,hi,bn,te,mr,ta,ur,gu,kn,ml,or,pa,as,bh,sat,ks,ne,sd,kok,doi,mni,sit,sa,fr,lus,inc', N'IND', N'AS', 6.74713897705078, 97.403305053710938, 1269750, 68.186691284179688, N'1173108018', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (106, N'British Indian Ocean Territory', N'USD', N'IO', N'IO', N'086', -5.2683329582214355, N'', N'Asia', N'60.0', N'en-IO', N'IOT', N'AS', -7.4380278587341309, 72.4931640625, 1282588, 71.259971618652344, N'4000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (107, N'Iraq', N'IQD', N'IZ', N'IQ', N'368', 37.378028869628906, N'Baghdad', N'Asia', N'437072.0', N'ar-IQ,ku,hy', N'IRQ', N'AS', 29.06944465637207, 48.5759162902832, 99237, 38.7958869934082, N'29671605', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (108, N'Iran', N'IRR', N'IR', N'IR', N'364', 39.7772216796875, N'Tehran', N'Asia', N'1648000.0', N'fa-IR,ku', N'IRN', N'AS', 25.064083099365231, 63.317470550537109, 130758, 44.047279357910163, N'76923300', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (109, N'Iceland', N'ISK', N'IC', N'IS', N'352', 66.534629821777344, N'Reykjavik', N'Europe', N'103000.0', N'is,en,de,da,sv,no', N'ISL', N'EU', 63.393253326416016, -13.495815277099608, 2629691, -24.546524047851559, N'308910', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (110, N'Italy', N'EUR', N'IT', N'IT', N'380', 47.095195770263672, N'Rome', N'Europe', N'301230.0', N'it-IT,de-IT,fr-IT,sc,ca,co,sl', N'ITA', N'EU', 36.652778625488281, 18.513444900512695, 3175395, 6.6148891448974609, N'60340328', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (111, N'Jersey', N'GBP', N'JE', N'JE', N'832', 49.265056610107422, N'Saint Helier', N'Europe', N'116.0', N'en,pt', N'JEY', N'EU', 49.169834136962891, -2.022083044052124, 3042142, -2.2600278854370117, N'90812', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (112, N'Jamaica', N'JMD', N'JM', N'JM', N'388', 18.526975631713867, N'Kingston', N'North America', N'10991.0', N'en-JM', N'JAM', N'NA', 17.703554153442383, -76.1803207397461, 3489940, -78.36663818359375, N'2847232', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (113, N'Jordan', N'JOD', N'JO', N'JO', N'400', 33.367668151855469, N'Amman', N'Asia', N'92300.0', N'ar-JO,en', N'JOR', N'AS', 29.185888290405273, 39.301166534423821, 248816, 34.959999084472656, N'6407085', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (114, N'Japan', N'JPY', N'JA', N'JP', N'392', 45.523139953613281, N'Tokyo', N'Asia', N'377835.0', N'ja', N'JPN', N'AS', 24.249471664428711, 145.82089233398438, 1861060, 122.93852996826172, N'127288000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (115, N'Kenya', N'KES', N'KE', N'KE', N'404', 5.0199379920959473, N'Nairobi', N'Africa', N'582650.0', N'en-KE,sw-KE', N'KEN', N'AF', -4.67804718017578, 41.899078369140625, 192950, 33.908859252929688, N'40046566', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (116, N'Kyrgyzstan', N'KGS', N'KG', N'KG', N'417', 43.238224029541016, N'Bishkek', N'Asia', N'198500.0', N'ky,uz,ru', N'KGZ', N'AS', 39.172832489013672, 80.283164978027344, 1527747, 69.276611328125, N'5508626', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (117, N'Cambodia', N'KHR', N'CB', N'KH', N'116', 14.686416625976564, N'Phnom Penh', N'Asia', N'181040.0', N'km,fr,en', N'KHM', N'AS', 10.409083366394045, 107.62772369384766, 1831722, 102.33999633789064, N'14453680', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (118, N'Kiribati', N'AUD', N'KR', N'KI', N'296', 4.7195701599121094, N'Tarawa', N'Oceania', N'811.0', N'en-KI,gil', N'KIR', N'OC', -11.437038421630859, -150.21534729003906, 4030945, 169.55613708496094, N'92533', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (119, N'Comoros', N'KMF', N'CN', N'KM', N'174', -11.362380981445313, N'Moroni', N'Africa', N'2170.0', N'ar,fr-KM', N'COM', N'AF', -12.387857437133787, 44.538223266601562, 921929, 43.215789794921875, N'773407', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (120, N'Saint Kitts and Nevis', N'XCD', N'SC', N'KN', N'659', 17.42011833190918, N'Basseterre', N'North America', N'261.0', N'en-KN', N'KNA', N'NA', 17.0953426361084, -62.543266296386719, 3575174, -62.869560241699219, N'49898', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (121, N'North Korea', N'KPW', N'KN', N'KP', N'408', 43.006053924560547, N'Pyongyang', N'Asia', N'120540.0', N'ko-KP', N'PRK', N'AS', 37.673332214355469, 130.67486572265625, 1873107, 124.31588745117188, N'22912177', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (122, N'South Korea', N'KRW', N'KS', N'KR', N'410', 38.612445831298835, N'Seoul', N'Asia', N'98480.0', N'ko-KR,en', N'KOR', N'AS', 33.190944671630859, 129.58467102050781, 1835841, 125.88710784912108, N'48422644', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (123, N'Kuwait', N'KWD', N'KU', N'KW', N'414', 30.095945358276367, N'Kuwait City', N'Asia', N'17820.0', N'ar-KW,en', N'KWT', N'AS', 28.52461051940918, 48.431472778320312, 285570, 46.555557250976562, N'2789132', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (124, N'Cayman Islands', N'KYD', N'CJ', N'KY', N'136', 19.761699676513672, N'George Town', N'North America', N'262.0', N'en-KY', N'CYM', N'NA', 19.263029098510746, -79.7272720336914, 3580718, -81.432777404785156, N'44270', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (125, N'Kazakhstan', N'KZT', N'KZ', N'KZ', N'398', 55.451194763183594, N'Astana', N'Asia', N'2717300.0', N'kk,ru', N'KAZ', N'AS', 40.936332702636719, 87.312667846679673, 1522867, 46.491859436035149, N'15340000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (126, N'Laos', N'LAK', N'LA', N'LA', N'418', 22.500389099121097, N'Vientiane', N'Asia', N'236800.0', N'lo,fr,en', N'LAO', N'AS', 13.910026550292969, 107.69702911376952, 1655842, 100.09305572509766, N'6368162', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (127, N'Lebanon', N'LBP', N'LE', N'LB', N'422', 34.6914176940918, N'Beirut', N'Asia', N'10400.0', N'ar-LB,fr-LB,en,hy', N'LBN', N'AS', 33.053859710693359, 36.639194488525391, 272103, 35.114276885986328, N'4125247', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (128, N'Saint Lucia', N'XCD', N'ST', N'LC', N'662', 14.103244781494141, N'Castries', N'North America', N'616.0', N'en-LC', N'LCA', N'NA', 13.704777717590332, -60.874202728271491, 3576468, -61.074150085449219, N'160922', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (129, N'Liechtenstein', N'CHF', N'LS', N'LI', N'438', 47.273529052734375, N'Vaduz', N'Europe', N'160.0', N'de-LI', N'LIE', N'EU', 47.055862426757805, 9.6321954727172852, 3042058, 9.4778051376342773, N'35000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (130, N'Sri Lanka', N'LKR', N'CE', N'LK', N'144', 9.8313608169555664, N'Colombo', N'Asia', N'65610.0', N'si,ta,en', N'LKA', N'AS', 5.91683292388916, 81.881278991699219, 1227603, 79.652915954589844, N'21513990', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (131, N'Liberia', N'LRD', N'LI', N'LR', N'430', 8.5517911911010742, N'Monrovia', N'Africa', N'111370.0', N'en-LR', N'LBR', N'AF', 4.3530569076538086, -7.3651127815246573, 2275384, -11.492082595825195, N'3685076', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (132, N'Lesotho', N'LSL', N'LT', N'LS', N'426', -28.572057723999023, N'Maseru', N'Africa', N'30355.0', N'en-LS,st,zu,xh', N'LSO', N'AF', -30.668964385986328, 29.465761184692383, 932692, 27.029067993164062, N'1919552', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (133, N'Lithuania', N'LTL', N'LH', N'LT', N'440', 56.446918487548835, N'Vilnius', N'Europe', N'65200.0', N'lt,ru,pl', N'LTU', N'EU', 53.90130615234375, 26.871944427490231, 597427, 20.9415283203125, N'3565000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (134, N'Luxembourg', N'EUR', N'LU', N'LU', N'442', 50.184944152832031, N'Luxembourg', N'Europe', N'2586.0', N'lb,de-LU,fr-LU', N'LUX', N'EU', 49.446582794189453, 6.5284719467163086, 2960313, 5.7345561981201172, N'497538', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (135, N'Latvia', N'LVL', N'LG', N'LV', N'428', 58.082305908203125, N'Riga', N'Europe', N'64589.0', N'lv,ru,lt', N'LVA', N'EU', 55.668861389160163, 28.241167068481445, 458258, 20.974277496337891, N'2217969', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (136, N'Libya', N'LYD', N'LY', N'LY', N'434', 33.168998718261719, N'Tripoli', N'Africa', N'1759540.0', N'ar-LY,it,en', N'LBY', N'AF', 19.508045196533203, 25.150611877441406, 2215636, 9.3870201110839844, N'6461454', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (137, N'Morocco', N'MAD', N'MO', N'MA', N'504', 35.9224967956543, N'Rabat', N'Africa', N'446550.0', N'ar-MA,fr', N'MAR', N'AF', 27.6621150970459, -0.99175000190734863, 2542007, -13.168585777282717, N'31627428', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (138, N'Monaco', N'EUR', N'MN', N'MC', N'492', 43.751968383789062, N'Monaco', N'Europe', N'1.95', N'fr-MC,en,it', N'MCO', N'EU', 43.724727630615234, 7.4399394989013663, 2993457, 7.4089622497558594, N'32965', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (139, N'Moldova', N'MDL', N'MD', N'MD', N'498', 48.490165710449219, N'Chisinau', N'Europe', N'33843.0', N'ro,ru,gag,tr', N'MDA', N'EU', 45.468887329101562, 30.135444641113281, 617790, 26.61894416809082, N'4324000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (140, N'Montenegro', N'EUR', N'MJ', N'ME', N'499', 43.570137023925781, N'Podgorica', N'Europe', N'14026.0', N'sr,hu,bs,sq,hr,rom', N'MNE', N'EU', 41.850166320800781, 20.358833312988281, 3194884, 18.461305618286133, N'666730', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (141, N'Saint Martin', N'EUR', N'RN', N'MF', N'663', 18.130353927612305, N'Marigot', N'North America', N'53.0', N'fr', N'MAF', N'NA', 18.052230834960938, -63.012992858886719, 3578421, -63.152767181396477, N'35925', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (142, N'Madagascar', N'MGA', N'MA', N'MG', N'450', -11.945432662963867, N'Antananarivo', N'Africa', N'587040.0', N'fr-MG,mg', N'MDG', N'AF', -25.608951568603516, 50.483779907226562, 1062947, 43.224876403808594, N'21281844', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (143, N'Marshall Islands', N'USD', N'RM', N'MH', N'584', 14.619999885559082, N'Majuro', N'Oceania', N'181.3', N'mh,en-MH', N'MHL', N'OC', 5.5876388549804688, 171.93180847167969, 2080185, 165.52491760253906, N'65859', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (144, N'Macedonia', N'MKD', N'MK', N'MK', N'807', 42.3618049621582, N'Skopje', N'Europe', N'25333.0', N'mk,sq,tr,rmm,sr', N'MKD', N'EU', 40.860195159912109, 23.038139343261719, 718075, 20.464694976806641, N'2062294', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (145, N'Mali', N'XOF', N'ML', N'ML', N'466', 25.000001907348633, N'Bamako', N'Africa', N'1240000.0', N'fr-ML,bm', N'MLI', N'AF', 10.159513473510742, 4.2449679374694824, 2453866, -12.242613792419434, N'13796354', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (146, N'Myanmar [Burma]', N'MMK', N'BM', N'MM', N'104', 28.543249130249023, N'Nay Pyi Taw', N'Asia', N'678500.0', N'my', N'MMR', N'AS', 9.78458309173584, 101.17678070068359, 1327865, 92.189277648925781, N'53414374', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (147, N'Mongolia', N'MNT', N'MG', N'MN', N'496', 52.154251098632812, N'Ulan Bator', N'Asia', N'1565000.0', N'mn,ru', N'MNG', N'AS', 41.5676383972168, 119.92430877685548, 2029969, 87.749664306640625, N'3086918', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (148, N'Macao', N'MOP', N'MC', N'MO', N'446', 22.222333908081055, N'Macao', N'Asia', N'254.0', N'zh,zh-MO,pt', N'MAC', N'AS', 22.180389404296875, 113.56583404541016, 1821275, 113.52894592285156, N'449198', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (149, N'Northern Mariana Islands', N'USD', N'CQ', N'MP', N'580', 20.553440093994141, N'Saipan', N'Oceania', N'477.0', N'fil,tl,zh,ch-MP,en-MP', N'MNP', N'OC', 14.110230445861816, 146.06527709960938, 4041468, 144.88626098632813, N'53883', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (150, N'Martinique', N'EUR', N'MB', N'MQ', N'474', 14.878819465637209, N'Fort-de-France', N'North America', N'1100.0', N'fr-MQ', N'MTQ', N'NA', 14.39226245880127, -60.815509796142578, 3570311, -61.230117797851562, N'432900', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (151, N'Mauritania', N'MRO', N'MR', N'MR', N'478', 27.298072814941406, N'Nouakchott', N'Africa', N'1030700.0', N'ar-MR,fuc,snk,fr,mey,wo', N'MRT', N'AF', 14.715546607971191, -4.82767391204834, 2378080, -17.066520690917969, N'3205060', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (152, N'Montserrat', N'XCD', N'MH', N'MS', N'500', 16.824060440063477, N'Plymouth', N'North America', N'102.0', N'en-MS', N'MSR', N'NA', 16.674768447875977, -62.144100189208984, 3578097, -62.241382598876953, N'9341', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (153, N'Malta', N'EUR', N'MT', N'MT', N'470', 36.0791130065918, N'Valletta', N'Europe', N'316.0', N'mt,en-MT', N'MLT', N'EU', 35.810276031494141, 14.577638626098633, 2562770, 14.18437671661377, N'403000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (154, N'Mauritius', N'MUR', N'MP', N'MU', N'480', -10.319254875183106, N'Port Louis', N'Africa', N'2040.0', N'en-MU,bho,fr', N'MUS', N'AF', -20.525716781616211, 63.500179290771491, 934292, 56.512718200683594, N'1294104', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (155, N'Maldives', N'MVR', N'MV', N'MV', N'462', 7.0983610153198242, N'Malé', N'Asia', N'300.0', N'dv,en', N'MDV', N'AS', -0.69269400835037231, 73.637275695800781, 1282028, 72.693222045898438, N'395650', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (156, N'Malawi', N'MWK', N'MI', N'MW', N'454', -9.3675413131713867, N'Lilongwe', N'Africa', N'118480.0', N'ny,yao,tum,swk', N'MWI', N'AF', -17.125, 35.916820526123047, 927384, 32.6739501953125, N'15447500', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (157, N'Mexico', N'MXN', N'MX', N'MX', N'484', 32.716758728027344, N'Mexico City', N'North America', N'1972550.0', N'es-MX', N'MEX', N'NA', 14.532865524291992, -86.7033920288086, 3996063, -118.45394897460938, N'112468855', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (158, N'Malaysia', N'MYR', N'MY', N'MY', N'458', 7.3634171485900879, N'Kuala Lumpur', N'Asia', N'329750.0', N'ms-MY,en,zh,ta,te,ml,pa,th', N'MYS', N'AS', 0.85522198677062988, 119.26750183105467, 1733045, 99.643447875976562, N'28274729', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (159, N'Mozambique', N'MZN', N'MZ', N'MZ', N'508', -10.471882820129396, N'Maputo', N'Africa', N'801590.0', N'pt-MZ,vmw', N'MOZ', N'AF', -26.868684768676761, 40.842994689941406, 1036973, 30.217319488525391, N'22061451', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (160, N'Namibia', N'NAD', N'WA', N'NA', N'516', -16.959894180297852, N'Windhoek', N'Africa', N'825418.0', N'en-NA,af,de,hz,naq', N'NAM', N'AF', -28.9714298248291, 25.25670051574707, 3355338, 11.715629577636721, N'2128471', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (161, N'New Caledonia', N'XPF', N'NC', N'NC', N'540', -19.549777984619141, N'Noumea', N'Oceania', N'19060.0', N'fr-NC', N'NCL', N'OC', -22.697999954223633, 168.12913513183594, 2139685, 163.56466674804688, N'216494', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (162, N'Niger', N'XOF', N'NG', N'NE', N'562', 23.525026321411133, N'Niamey', N'Africa', N'1267000.0', N'fr-NE,ha,kr,dje', N'NER', N'AF', 11.696974754333496, 15.99564266204834, 2440476, 0.16625000536441803, N'15878271', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (163, N'Norfolk Island', N'AUD', N'NF', N'NF', N'574', -28.995170593261719, N'Kingston', N'Oceania', N'34.6', N'en-NF', N'NFK', N'OC', -29.063076019287109, 167.99774169921875, 2155115, 167.91543579101565, N'1828', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (164, N'Nigeria', N'NGN', N'NI', N'NG', N'566', 13.892006874084473, N'Abuja', N'Africa', N'923768.0', N'en-NG,ha,yo,ig,ff', N'NGA', N'AF', 4.2771439552307129, 14.680072784423828, 2328926, 2.6684319972991943, N'154000000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (165, N'Nicaragua', N'NIO', N'NU', N'NI', N'558', 15.025909423828123, N'Managua', N'North America', N'129494.0', N'es-NI,en', N'NIC', N'NA', 10.70754337310791, -82.738288879394531, 3617476, -87.6903076171875, N'5995928', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (166, N'Netherlands', N'EUR', N'NL', N'NL', N'528', 53.5121955871582, N'Amsterdam', N'Europe', N'41526.0', N'nl-NL,fy-NL', N'NLD', N'EU', 50.7539176940918, 7.2279438972473153, 2750405, 3.362555980682373, N'16645000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (167, N'Norway', N'NOK', N'NO', N'NO', N'578', 71.1881103515625, N'Oslo', N'Europe', N'324220.0', N'no,nb,nn,se,fi', N'NOR', N'EU', 57.9779167175293, 31.078052520751953, 3144096, 4.6501669883728027, N'5009150', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (168, N'Nepal', N'NPR', N'NP', N'NP', N'524', 30.433389663696289, N'Kathmandu', N'Asia', N'140800.0', N'ne,en', N'NPL', N'AS', 26.356721878051761, 88.199333190917969, 1282988, 80.0562744140625, N'28951852', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (169, N'Nauru', N'AUD', N'NR', N'NR', N'520', -0.50430601835250854, N'', N'Oceania', N'21.0', N'na,en-NR', N'NRU', N'OC', -0.55233299732208252, 166.94528198242188, 2110425, 166.89903259277344, N'10065', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (170, N'Niue', N'NZD', N'NE', N'NU', N'570', -18.951068878173828, N'Alofi', N'Oceania', N'260.0', N'niu,en-NU', N'NIU', N'OC', -19.152193069458008, -169.77517700195313, 4036232, -169.95100402832031, N'2166', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (171, N'New Zealand', N'NZD', N'NZ', N'NZ', N'554', -34.389667510986328, N'Wellington', N'Oceania', N'268680.0', N'en-NZ,mi', N'NZL', N'OC', -47.286026000976562, -180, 2186224, 166.71549987792969, N'4252277', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (172, N'Oman', N'OMR', N'MU', N'OM', N'512', 26.387971878051761, N'Muscat', N'Asia', N'212460.0', N'ar-OM,en,bal,ur', N'OMN', N'AS', 16.645750045776367, 59.836582183837891, 286963, 51.881999969482422, N'2967717', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (173, N'Panama', N'PAB', N'PM', N'PA', N'591', 9.6375141143798828, N'Panama City', N'North America', N'78200.0', N'es-PA,en', N'PAN', N'NA', 7.1979060173034668, -77.17411041259767, 3703430, -83.051445007324219, N'3410676', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (174, N'Peru', N'PEN', N'PE', N'PE', N'604', -0.012977000325918198, N'Lima', N'South America', N'1285220.0', N'es-PE,qu,ay', N'PER', N'SA', -18.349727630615231, -68.677986145019531, 3932488, -81.326744079589844, N'29907003', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (175, N'French Polynesia', N'XPF', N'FP', N'PF', N'258', -7.9035730361938477, N'Papeete', N'Oceania', N'4167.0', N'fr-PF,ty', N'PYF', N'OC', -27.653572082519531, -134.92982482910156, 4030656, -152.87716674804688, N'270485', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (176, N'Papua New Guinea', N'PGK', N'PP', N'PG', N'598', -1.3186390399932859, N'Port Moresby', N'Oceania', N'462840.0', N'en-PG,ho,meu,tpi', N'PNG', N'OC', -11.65786075592041, 155.96343994140625, 2088628, 140.84286499023438, N'6064515', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (177, N'Philippines', N'PHP', N'RP', N'PH', N'608', 21.1206111907959, N'Manila', N'Asia', N'300000.0', N'tl,en-PH,fil', N'PHL', N'AS', 4.643305778503418, 126.60152435302734, 1694008, 116.93155670166016, N'99900177', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (178, N'Pakistan', N'PKR', N'PK', N'PK', N'586', 37.097000122070312, N'Islamabad', N'Asia', N'803940.0', N'ur-PK,en-PK,pa,sd,ps,brh', N'PAK', N'AS', 23.786722183227539, 77.8409194946289, 1168579, 60.878612518310547, N'184404791', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (179, N'Poland', N'PLN', N'PL', N'PL', N'616', 54.839138031005859, N'Warsaw', N'Europe', N'312685.0', N'pl', N'POL', N'EU', 49.006362915039062, 24.150749206542969, 798544, 14.123000144958496, N'38500000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (180, N'Saint Pierre and Miquelon', N'EUR', N'SB', N'PM', N'666', 47.146286010742195, N'Saint-Pierre', N'North America', N'242.0', N'fr-PM', N'SPM', N'NA', 46.786041259765625, -56.25299072265625, 3424932, -56.420658111572266, N'7012', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (181, N'Pitcairn Islands', N'NZD', N'PC', N'PN', N'612', -24.315864562988281, N'Adamstown', N'Oceania', N'47.0', N'en-PN', N'PCN', N'OC', -24.672565460205082, -124.77285003662108, 4030699, -128.346435546875, N'46', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (182, N'Puerto Rico', N'USD', N'RQ', N'PR', N'630', 18.520166397094727, N'San Juan', N'North America', N'9104.0', N'en-PR,es-PR', N'PRI', N'NA', 17.92640495300293, -65.24273681640625, 4566966, -67.9427261352539, N'3916632', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (183, N'Palestine', N'ILS', N'WE', N'PS', N'275', 32.54638671875, N'', N'Asia', N'5970.0', N'ar-PS', N'PSE', N'AS', 31.216541290283203, 35.573295593261719, 6254930, 34.216659545898438, N'3800000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (184, N'Portugal', N'EUR', N'PO', N'PT', N'620', 42.145637512207031, N'Lisbon', N'Europe', N'92391.0', N'pt-PT,mwl', N'PRT', N'EU', 36.961250305175781, -6.1826939582824707, 2264397, -9.4959440231323242, N'10676000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (185, N'Palau', N'USD', N'PS', N'PW', N'585', 8.4696598052978516, N'Melekeok - Palau State Capital', N'Oceania', N'458.0', N'pau,sov,en-PW,tox,ja,fil,zh', N'PLW', N'OC', 2.8036000728607178, 134.72306823730469, 1559582, 131.11787414550781, N'19907', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (186, N'Paraguay', N'PYG', N'PA', N'PY', N'600', -19.294040679931641, N'Asunción', N'South America', N'406750.0', N'es-PY,gn', N'PRY', N'SA', -27.608737945556641, -54.259353637695312, 3437598, -62.647075653076179, N'6375830', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (187, N'Qatar', N'QAR', N'QA', N'QA', N'634', 26.154722213745117, N'Doha', N'Asia', N'11437.0', N'ar-QA,es', N'QAT', N'AS', 24.482944488525391, 51.636638641357422, 289688, 50.757221221923821, N'840926', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (188, N'Réunion', N'EUR', N'RE', N'RE', N'638', -20.856855392456055, N'Saint-Denis', N'Africa', N'2517.0', N'fr-RE', N'REU', N'AF', -21.372211456298828, 55.845039367675781, 935317, 55.219085693359375, N'776948', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (189, N'Romania', N'RON', N'RO', N'RO', N'642', 48.266944885253906, N'Bucharest', N'Europe', N'237500.0', N'ro,hu,rom', N'ROU', N'EU', 43.627304077148438, 29.691055297851559, 798549, 20.26997184753418, N'21959278', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (190, N'Serbia', N'RSD', N'RI', N'RS', N'688', 46.181388854980469, N'Belgrade', N'Europe', N'88361.0', N'sr,hu,bs,rom', N'SRB', N'EU', 42.232215881347656, 23.004997253417969, 6290252, 18.817020416259769, N'7344847', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (191, N'Russia', N'RUB', N'RS', N'RU', N'643', 81.85736083984375, N'Moscow', N'Europe', N'1.71E7', N'ru,tt,xal,cau,ady,kv,ce,tyv,cv,udm,tut,mns,bua,myv,mdf,chm,ba,inh,tut,kbd,krc,ava,sah,nog', N'RUS', N'EU', 41.188861846923821, -169.05000305175781, 2017370, 19.25, N'140702000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (192, N'Rwanda', N'RWF', N'RW', N'RW', N'646', -1.0534809827804563, N'Kigali', N'Africa', N'26338.0', N'rw,en-RW,fr-RW,sw', N'RWA', N'AF', -2.8406789302825928, 30.895957946777344, 49518, 28.856794357299805, N'11055976', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (193, N'Saudi Arabia', N'SAR', N'SA', N'SA', N'682', 32.158332824707031, N'Riyadh', N'Asia', N'1960582.0', N'ar-SA', N'SAU', N'AS', 15.614250183105469, 55.666584014892578, 102358, 34.495693206787109, N'25731776', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (194, N'Solomon Islands', N'SBD', N'BP', N'SB', N'090', -6.5896110534667969, N'Honiara', N'Oceania', N'28450.0', N'en-SB,tpi', N'SLB', N'OC', -11.850555419921877, 166.98086547851565, 2103350, 155.50860595703125, N'559198', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (195, N'Seychelles', N'SCR', N'SE', N'SC', N'690', -4.283717155456543, N'Victoria', N'Africa', N'455.0', N'en-SC,fr-SC', N'SYC', N'AF', -9.7538671493530273, 56.279506683349609, 241170, 46.204769134521491, N'88340', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (196, N'Sudan', N'SDG', N'SU', N'SD', N'729', 22.232219696044918, N'Khartoum', N'Africa', N'1861484.0', N'ar-SD,en,fia', N'SDN', N'AF', 8.6847209930419922, 38.607498168945312, 366755, 21.827774047851559, N'35000000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (197, N'Sweden', N'SEK', N'SW', N'SE', N'752', 69.0625, N'Stockholm', N'Europe', N'449964.0', N'sv-SE,se,sma,fi-SE', N'SWE', N'EU', 55.337112426757805, 24.156291961669918, 2661886, 11.118694305419922, N'9555893', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (198, N'Singapore', N'SGD', N'SN', N'SG', N'702', 1.4712779521942141, N'Singapore', N'Asia', N'692.7', N'cmn,en-SG,ms-SG,ta-SG,zh-SG', N'SGP', N'AS', 1.2585560083389282, 104.00746917724608, 1880251, 103.63827514648438, N'4701069', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (199, N'Saint Helena', N'SHP', N'SH', N'SH', N'654', -7.887814998626709, N'Jamestown', N'Africa', N'410.0', N'en-SH', N'SHN', N'AF', -16.019542694091797, -5.6387529373168945, 3370751, -14.421230316162108, N'7460', NULL)
GO
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (200, N'Slovenia', N'EUR', N'SI', N'SI', N'705', 46.8779182434082, N'Ljubljana', N'Europe', N'20273.0', N'sl,sh', N'SVN', N'EU', 45.413139343261719, 16.565999984741211, 3190538, 13.383083343505859, N'2007000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (201, N'Svalbard and Jan Mayen', N'NOK', N'SV', N'SJ', N'744', 80.7620849609375, N'Longyearbyen', N'Europe', N'62049.0', N'no,ru', N'SJM', N'EU', 79.220306396484375, 33.287334442138672, 607072, 17.69938850402832, N'2550', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (202, N'Slovakia', N'EUR', N'LO', N'SK', N'703', 49.603168487548835, N'Bratislava', N'Europe', N'48845.0', N'sk,hu', N'SVK', N'EU', 47.728111267089837, 22.570444107055664, 3057568, 16.847749710083008, N'5455000', 1)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (203, N'Sierra Leone', N'SLL', N'SL', N'SL', N'694', 10, N'Freetown', N'Africa', N'71740.0', N'en-SL,men,tem', N'SLE', N'AF', 6.9296112060546875, -10.2842378616333, 2403846, -13.30763053894043, N'5245695', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (204, N'San Marino', N'EUR', N'SM', N'SM', N'674', 43.992237091064453, N'San Marino', N'Europe', N'61.2', N'it-SM', N'SMR', N'EU', 43.893711090087891, 12.516531944274902, 3168068, 12.403538703918455, N'31477', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (205, N'Senegal', N'XOF', N'SG', N'SN', N'686', 16.691633224487305, N'Dakar', N'Africa', N'196190.0', N'fr-SN,wo,fuc,mnk', N'SEN', N'AF', 12.30727481842041, -11.355887413024902, 2245662, -17.535236358642578, N'12323252', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (206, N'Somalia', N'SOS', N'SO', N'SO', N'706', 11.979166030883787, N'Mogadishu', N'Africa', N'637657.0', N'so-SO,ar-SO,it,en-SO', N'SOM', N'AF', -1.6748679876327517, 51.412635803222656, 51537, 40.986595153808594, N'10112453', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (207, N'Suriname', N'SRD', N'NS', N'SR', N'740', 6.0045461654663086, N'Paramaribo', N'South America', N'163270.0', N'nl-SR,en,srn,hns,jv', N'SUR', N'SA', 1.8311450481414795, -53.977493286132812, 3382998, -58.086563110351562, N'492829', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (208, N'South Sudan', N'SSP', N'OD', N'SS', N'728', 12.219148635864258, N'Juba', N'Africa', N'644329.0', N'en', N'SSD', N'AF', 3.4933943748474121, 35.9405517578125, 7909807, 24.140274047851559, N'8260490', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (209, N'São Tomé and Príncipe', N'STD', N'TP', N'ST', N'678', 1.7013230323791504, N'São Tomé', N'Africa', N'1001.0', N'pt-ST', N'STP', N'AF', 0.024765999987721443, 7.4663739204406738, 2410758, 6.4701700210571289, N'175808', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (210, N'El Salvador', N'USD', N'ES', N'SV', N'222', 14.445067405700684, N'San Salvador', N'North America', N'21040.0', N'es-SV', N'SLV', N'NA', 13.148678779602053, -87.6921615600586, 3585968, -90.128662109375, N'6052064', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (211, N'Sint Maarten', N'ANG', N'NN', N'SX', N'534', 18.070247650146484, N'Philipsburg', N'North America', N'', N'nl,en', N'SXM', N'NA', 18.011692047119141, -63.012992858886719, 7609695, -63.144039154052734, N'37429', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (212, N'Syria', N'SYP', N'SY', N'SY', N'760', 37.319137573242195, N'Damascus', N'Asia', N'185180.0', N'ar-SY,ku,hy,arc,fr,en', N'SYR', N'AS', 32.310665130615234, 42.385028839111328, 163843, 35.727222442626953, N'22198110', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (213, N'Swaziland', N'SZL', N'WZ', N'SZ', N'748', -25.719648361206055, N'Mbabane', N'Africa', N'17363.0', N'en-SZ,ss-SZ', N'SWZ', N'AF', -27.317100524902344, 32.137260437011719, 934841, 30.794107437133789, N'1354051', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (214, N'Turks and Caicos Islands', N'USD', N'TK', N'TC', N'796', 21.961877822875977, N'Cockburn Town', N'North America', N'430.0', N'en-TC', N'TCA', N'NA', 21.422626495361328, -71.123641967773438, 3576916, -72.483871459960938, N'20556', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (215, N'Chad', N'XAF', N'CD', N'TD', N'148', 23.450368881225582, N'N''Djamena', N'Africa', N'1284000.0', N'fr-TD,ar-TD,sre', N'TCD', N'AF', 7.441068172454834, 24.002660751342773, 2434508, 13.473475456237791, N'10543464', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (216, N'French Southern Territories', N'EUR', N'FS', N'TF', N'260', -37.790721893310547, N'Port-aux-Français', N'Antarctica', N'7829.0', N'fr', N'ATF', N'AN', -49.735183715820312, 77.598808288574219, 1546748, 50.170257568359375, N'140', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (217, N'Togo', N'XOF', N'TO', N'TG', N'768', 11.13897705078125, N'Lomé', N'Africa', N'56785.0', N'fr-TG,ee,hna,kbp,dag,ha', N'TGO', N'AF', 6.1044168472290039, 1.8066929578781128, 2363686, -0.14732399582862854, N'6587239', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (218, N'Thailand', N'THB', N'TH', N'TH', N'764', 20.463193893432617, N'Bangkok', N'Asia', N'514000.0', N'th,en', N'THA', N'AS', 5.6100001335144043, 105.63938903808594, 1605651, 97.345642089843764, N'67089500', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (219, N'Tajikistan', N'TJS', N'TI', N'TJ', N'762', 41.042251586914062, N'Dushanbe', N'Asia', N'143100.0', N'tg,ru', N'TJK', N'AS', 36.674137115478509, 75.137222290039062, 1220409, 67.387138366699219, N'7487489', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (220, N'Tokelau', N'NZD', N'TL', N'TK', N'772', -8.5536136627197266, N'', N'Oceania', N'10.0', N'tkl,en-TK', N'TKL', N'OC', -9.3811111450195312, -171.21142578125, 4031074, -172.50033569335938, N'1466', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (221, N'East Timor', N'USD', N'TT', N'TL', N'626', -8.135833740234375, N'Dili', N'Oceania', N'15007.0', N'tet,pt-TL,id,en', N'TLS', N'OC', -9.4636268615722656, 127.30859375, 1966436, 124.0460968017578, N'1154625', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (222, N'Turkmenistan', N'TMT', N'TX', N'TM', N'795', 42.795555114746094, N'Ashgabat', N'Asia', N'488100.0', N'tk,ru,uz', N'TKM', N'AS', 35.141082763671875, 66.6843032836914, 1218197, 52.441444396972656, N'4940916', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (223, N'Tunisia', N'TND', N'TS', N'TN', N'788', 37.543914794921875, N'Tunis', N'Africa', N'163610.0', N'ar-TN,fr', N'TUN', N'AF', 30.24041748046875, 11.598278045654297, 2464461, 7.5248332023620605, N'10589025', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (224, N'Tonga', N'TOP', N'TN', N'TO', N'776', -15.56298828125, N'Nuku''alofa', N'Oceania', N'748.0', N'to,en-TO', N'TON', N'OC', -21.455057144165039, -173.90757751464844, 4032283, -175.68226623535156, N'122580', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (225, N'Turkey', N'TRY', N'TU', N'TR', N'792', 42.107612609863281, N'Ankara', N'Asia', N'780580.0', N'tr-TR,ku,diq,az,av', N'TUR', N'AS', 35.8154182434082, 44.834999084472656, 298795, 25.668500900268555, N'77804122', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (226, N'Trinidad and Tobago', N'TTD', N'TD', N'TT', N'780', 11.33834171295166, N'Port of Spain', N'North America', N'5128.0', N'en-TT,hns,fr,es,zh', N'TTO', N'NA', 10.036105155944824, -60.5179328918457, 3573591, -61.923770904541016, N'1228691', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (227, N'Tuvalu', N'AUD', N'TV', N'TV', N'798', -5.6419720649719238, N'Funafuti', N'Oceania', N'26.0', N'tvl,en,sm,gil', N'TUV', N'OC', -10.801169395446776, 179.86328125, 2110297, 176.06486511230469, N'10472', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (228, N'Taiwan', N'TWD', N'TW', N'TW', N'158', 25.298250198364254, N'Taipei', N'Asia', N'35980.0', N'zh-TW,zh,nan,hak', N'TWN', N'AS', 21.901805877685547, 122.0004425048828, 1668284, 119.53469085693359, N'22894384', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (229, N'Tanzania', N'TZS', N'TZ', N'TZ', N'834', -0.9907360076904298, N'Dodoma', N'Africa', N'945087.0', N'sw-TZ,en,ar', N'TZA', N'AF', -11.74569606781006, 40.443222045898438, 149590, 29.327167510986328, N'41892895', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (230, N'Ukraine', N'UAH', N'UP', N'UA', N'804', 52.369361877441406, N'Kyiv', N'Europe', N'603700.0', N'uk,ru-UA,rom,pl,hu', N'UKR', N'EU', 44.390415191650391, 40.207389831542969, 690791, 22.128889083862305, N'45415596', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (231, N'Uganda', N'UGX', N'UG', N'UG', N'800', 4.2144269943237305, N'Kampala', N'Africa', N'236040.0', N'en-UG,lg,sw,ar', N'UGA', N'AF', -1.4840500354766846, 35.036048889160156, 226074, 29.573251724243164, N'33398682', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (232, N'U.S. Minor Outlying Islands', N'USD', N'', N'UM', N'581', 28.219814300537109, N'', N'Oceania', N'0.0', N'en-UM', N'UMI', N'OC', -0.38900598883628851, 166.65452575683594, 5854968, -177.39202880859375, N'0', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (233, N'United States', N'USD', N'US', N'US', N'840', 49.38861083984375, N'Washington', N'North America', N'9629091.0', N'en-US,es-US,haw,fr', N'USA', N'NA', 24.544244766235352, -66.9548110961914, 6252001, -124.73325347900392, N'310232863', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (234, N'Uruguay', N'UYU', N'UY', N'UY', N'858', -30.082223892211911, N'Montevideo', N'South America', N'176220.0', N'es-UY', N'URY', N'SA', -34.980815887451172, -53.073932647705078, 3439705, -58.442722320556641, N'3477000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (235, N'Uzbekistan', N'UZS', N'UZ', N'UZ', N'860', 45.575000762939453, N'Tashkent', N'Asia', N'447400.0', N'uz,ru,tg', N'UZB', N'AS', 37.184444427490234, 73.132278442382812, 1512440, 55.996639251708984, N'27865738', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (236, N'Vatican City', N'EUR', N'VT', N'VA', N'336', 41.907440185546875, N'Vatican', N'Europe', N'0.44', N'la,it,fr', N'VAT', N'EU', 41.9002799987793, 12.458375930786133, 3164670, 12.445706367492676, N'921', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (237, N'Saint Vincent and the Grenadines', N'XCD', N'VC', N'VC', N'670', 13.377834320068359, N'Kingstown', N'North America', N'389.0', N'en-VC,fr', N'VCT', N'NA', 12.583984375, -61.1138801574707, 3577815, -61.460903167724609, N'104217', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (238, N'Venezuela', N'VEF', N'VE', N'VE', N'862', 12.201903343200684, N'Caracas', N'South America', N'912050.0', N'es-VE', N'VEN', N'SA', 0.62631100416183472, -59.803779602050781, 3625428, -73.354072570800781, N'27223228', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (239, N'British Virgin Islands', N'USD', N'VI', N'VG', N'092', 18.757221221923828, N'Road Town', N'North America', N'153.0', N'en-VG', N'VGB', N'NA', 18.389980316162109, -64.268768310546875, 3577718, -64.715362548828125, N'21730', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (240, N'U.S. Virgin Islands', N'USD', N'VQ', N'VI', N'850', 18.391746520996097, N'Charlotte Amalie', N'North America', N'352.0', N'en-VI', N'VIR', N'NA', 17.681724548339844, -64.565177917480469, 4796775, -65.0382308959961, N'108708', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (241, N'Vietnam', N'VND', N'VM', N'VN', N'704', 23.388833999633789, N'Hanoi', N'Asia', N'329560.0', N'vi,en,fr,zh,km', N'VNM', N'AS', 8.5596113204956055, 109.46463775634766, 1562822, 102.14822387695313, N'89571130', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (242, N'Vanuatu', N'VUV', N'NH', N'VU', N'548', -13.073444366455078, N'Port Vila', N'Oceania', N'12200.0', N'bi,en-VU,fr-VU', N'VUT', N'OC', -20.248945236206055, 169.90478515625, 2134431, 166.52497863769531, N'221552', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (243, N'Wallis and Futuna', N'XPF', N'WF', N'WF', N'876', -13.216757774353027, N'Mata-Utu', N'Oceania', N'274.0', N'wls,fud,fr-WF', N'WLF', N'OC', -14.314559936523438, -176.1617431640625, 4034749, -178.184814453125, N'16025', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (244, N'Samoa', N'WST', N'WS', N'WS', N'882', -13.432207107543944, N'Apia', N'Oceania', N'2944.0', N'sm,en-WS', N'WSM', N'OC', -14.040939331054688, -171.41574096679688, 4034894, -172.79859924316406, N'192001', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (245, N'Kosovo', N'EUR', N'KV', N'XK', N'0', 43.26824951171875, N'Pristina', N'Europe', N'', N'sq,sr', N'XKX', N'EU', 41.856369018554688, 21.8033504486084, 831053, 19.977481842041016, N'1800000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (246, N'Yemen', N'YER', N'YM', N'YE', N'887', 18.999998092651367, N'Sanaa', N'Asia', N'527970.0', N'ar-YE', N'YEM', N'AS', 12.111090660095217, 54.530540466308594, 69543, 42.532539367675781, N'23495361', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (247, N'Mayotte', N'EUR', N'MF', N'YT', N'175', -12.648891448974608, N'Mamoutzou', N'Africa', N'374.0', N'fr-YT', N'MYT', N'AF', -13.000131607055664, 45.292949676513672, 1024031, 45.037960052490234, N'159042', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (248, N'South Africa', N'ZAR', N'SF', N'ZA', N'710', -22.126611709594727, N'Pretoria', N'Africa', N'1219912.0', N'zu,xh,af,nso,en-ZA,tn,st,ts,ss,ve,nr', N'ZAF', N'AF', -34.839828491210938, 32.895973205566406, 953987, 16.45802116394043, N'49000000', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (249, N'Zambia', N'ZMK', N'ZA', N'ZM', N'894', -8.224360466003418, N'Lusaka', N'Africa', N'752614.0', N'en-ZM,bem,loz,lun,lue,ny,toi', N'ZMB', N'AF', -18.0794734954834, 33.705703735351562, 895949, 21.999370574951172, N'13460305', NULL)
INSERT [dbo].[DbCountries] ([Id], [CountryName], [CurrencyCode], [FipsCode], [CountryCode], [IsoNumeric], [North], [Capital], [ContinentName], [AreaInSqKm], [Languages], [IsoAlpha3], [Continent], [South], [East], [GeonameId], [West], [Population], [InsideEu]) VALUES (250, N'Zimbabwe', N'ZWL', N'ZI', N'ZW', N'716', -15.608835220336914, N'Harare', N'Africa', N'390580.0', N'en-ZW,sn,nr,nd', N'ZWE', N'AF', -22.41773796081543, 33.056304931640625, 878675, 25.237028121948239, N'11651858', NULL)
SET IDENTITY_INSERT [dbo].[DbCountries] OFF
SET IDENTITY_INSERT [dbo].[DbCustomers] ON 

INSERT [dbo].[DbCustomers] ([Id], [CustomerInfo_Address], [CustomerInfo_Address2], [CustomerInfo_AttPerson], [CustomerInfo_City], [CustomerInfo_CompanyName], [CustomerInfo_Country], [CustomerInfo_FirstName], [CustomerInfo_LastName], [CustomerInfo_Postal], [CustomerInfo_VatNr], [Email], [Password], [DateCreated], [DateUpdated], [Credits], [CreditLimit], [ResetPasswordKey], [Phone], [CustomerInfo_State], [CustomerInfo_Co]) VALUES (7, NULL, NULL, NULL, NULL, N'LetterAmazer', NULL, N'Lars', N'Holdgaard', NULL, NULL, N'mcoroklo@gmail.com', N'70734c95aab7aa72699dd97e7211fe6df82e4690', CAST(0x0000A2CF0145D743 AS DateTime), CAST(0x0000A2CF0145D743 AS DateTime), CAST(0.00000 AS Decimal(18, 5)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, NULL)
INSERT [dbo].[DbCustomers] ([Id], [CustomerInfo_Address], [CustomerInfo_Address2], [CustomerInfo_AttPerson], [CustomerInfo_City], [CustomerInfo_CompanyName], [CustomerInfo_Country], [CustomerInfo_FirstName], [CustomerInfo_LastName], [CustomerInfo_Postal], [CustomerInfo_VatNr], [Email], [Password], [DateCreated], [DateUpdated], [Credits], [CreditLimit], [ResetPasswordKey], [Phone], [CustomerInfo_State], [CustomerInfo_Co]) VALUES (8, NULL, NULL, NULL, NULL, N'Jysk', NULL, N'Lars', N'Larsen', NULL, NULL, N'jysk@jysk.dk', N'70734c95aab7aa72699dd97e7211fe6df82e4690', CAST(0x0000A2D0015109DD AS DateTime), CAST(0x0000A2D0015109DD AS DateTime), CAST(0.00000 AS Decimal(18, 5)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, NULL)
INSERT [dbo].[DbCustomers] ([Id], [CustomerInfo_Address], [CustomerInfo_Address2], [CustomerInfo_AttPerson], [CustomerInfo_City], [CustomerInfo_CompanyName], [CustomerInfo_Country], [CustomerInfo_FirstName], [CustomerInfo_LastName], [CustomerInfo_Postal], [CustomerInfo_VatNr], [Email], [Password], [DateCreated], [DateUpdated], [Credits], [CreditLimit], [ResetPasswordKey], [Phone], [CustomerInfo_State], [CustomerInfo_Co]) VALUES (9, NULL, NULL, NULL, NULL, N'dfsdff', NULL, N'asdad', N'fdsdf', NULL, NULL, N'dfsdf@dsfd.dk', N'70734c95aab7aa72699dd97e7211fe6df82e4690', CAST(0x0000A2D0015188BD AS DateTime), CAST(0x0000A2D0015188BD AS DateTime), CAST(0.00000 AS Decimal(18, 5)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, NULL)
INSERT [dbo].[DbCustomers] ([Id], [CustomerInfo_Address], [CustomerInfo_Address2], [CustomerInfo_AttPerson], [CustomerInfo_City], [CustomerInfo_CompanyName], [CustomerInfo_Country], [CustomerInfo_FirstName], [CustomerInfo_LastName], [CustomerInfo_Postal], [CustomerInfo_VatNr], [Email], [Password], [DateCreated], [DateUpdated], [Credits], [CreditLimit], [ResetPasswordKey], [Phone], [CustomerInfo_State], [CustomerInfo_Co]) VALUES (10, NULL, NULL, NULL, NULL, N'fdssdf', NULL, N'dfdaff', N'fdsfdf', NULL, NULL, N'dfsdf@dfsdfds.dk', N'89d7d41ce817a015aadc86f6a018953e069553c2', CAST(0x0000A2D00153C8D1 AS DateTime), CAST(0x0000A2D00153C8D1 AS DateTime), CAST(0.00000 AS Decimal(18, 5)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, NULL)
INSERT [dbo].[DbCustomers] ([Id], [CustomerInfo_Address], [CustomerInfo_Address2], [CustomerInfo_AttPerson], [CustomerInfo_City], [CustomerInfo_CompanyName], [CustomerInfo_Country], [CustomerInfo_FirstName], [CustomerInfo_LastName], [CustomerInfo_Postal], [CustomerInfo_VatNr], [Email], [Password], [DateCreated], [DateUpdated], [Credits], [CreditLimit], [ResetPasswordKey], [Phone], [CustomerInfo_State], [CustomerInfo_Co]) VALUES (11, NULL, NULL, NULL, NULL, N'dsdd', NULL, N'sasd', N'sf', NULL, NULL, N'fdsf@dfsd.dk', N'70734c95aab7aa72699dd97e7211fe6df82e4690', CAST(0x0000A2D0015C6723 AS DateTime), CAST(0x0000A2D0015C6723 AS DateTime), CAST(0.00000 AS Decimal(18, 5)), CAST(0 AS Decimal(18, 0)), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[DbCustomers] OFF
SET IDENTITY_INSERT [dbo].[DbFulfillmentPartners] ON 

INSERT [dbo].[DbFulfillmentPartners] ([Id], [Name], [ShopId], [PartnerJob]) VALUES (1, N'Jupiter', 1, 0)
SET IDENTITY_INSERT [dbo].[DbFulfillmentPartners] OFF
SET IDENTITY_INSERT [dbo].[DbOffices] ON 

INSERT [dbo].[DbOffices] ([Id], [Name], [CountryId], [PartnerId]) VALUES (1, N'Netherlands_jupiter', 166, 1)
SET IDENTITY_INSERT [dbo].[DbOffices] OFF
SET IDENTITY_INSERT [dbo].[DbOrders] ON 

INSERT [dbo].[DbOrders] ([Id], [OrderStatus], [CustomerId], [DateCreated], [DateUpdated], [Guid], [OrderCode], [TransactionCode], [Cost], [CouponCode], [Discount], [Price], [PaymentMethod], [DatePaid], [DateSent]) VALUES (1, 0, NULL, CAST(0x0000A2D10171739F AS DateTime), CAST(0x0000A2D1017173C5 AS DateTime), N'8cefd37f-ebd1-4a64-91b9-f99367fd6db4', N'LA-1653903876', NULL, CAST(0.00000 AS Decimal(18, 5)), NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'', NULL, NULL)
INSERT [dbo].[DbOrders] ([Id], [OrderStatus], [CustomerId], [DateCreated], [DateUpdated], [Guid], [OrderCode], [TransactionCode], [Cost], [CouponCode], [Discount], [Price], [PaymentMethod], [DatePaid], [DateSent]) VALUES (2, 0, NULL, CAST(0x0000A2D1017284AC AS DateTime), CAST(0x0000A2D1017284AC AS DateTime), N'a443a940-6eab-4c62-bc4f-a4afe7e63119', N'LA677819258', NULL, CAST(0.00000 AS Decimal(18, 5)), NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'', NULL, NULL)
INSERT [dbo].[DbOrders] ([Id], [OrderStatus], [CustomerId], [DateCreated], [DateUpdated], [Guid], [OrderCode], [TransactionCode], [Cost], [CouponCode], [Discount], [Price], [PaymentMethod], [DatePaid], [DateSent]) VALUES (3, 0, NULL, CAST(0x0000A2D10172A06B AS DateTime), CAST(0x0000A2D10172A06B AS DateTime), N'494033fb-213a-4c60-8472-70de19c3a68c', N'LA638083737', NULL, CAST(0.00000 AS Decimal(18, 5)), NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'', NULL, NULL)
INSERT [dbo].[DbOrders] ([Id], [OrderStatus], [CustomerId], [DateCreated], [DateUpdated], [Guid], [OrderCode], [TransactionCode], [Cost], [CouponCode], [Discount], [Price], [PaymentMethod], [DatePaid], [DateSent]) VALUES (4, 0, NULL, CAST(0x0000A2D1017307EF AS DateTime), CAST(0x0000A2D1017307EF AS DateTime), N'7504c7d8-ac21-422a-a89a-5a6bfd4858c8', N'LA1806360684', NULL, CAST(0.00000 AS Decimal(18, 5)), NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'', NULL, NULL)
INSERT [dbo].[DbOrders] ([Id], [OrderStatus], [CustomerId], [DateCreated], [DateUpdated], [Guid], [OrderCode], [TransactionCode], [Cost], [CouponCode], [Discount], [Price], [PaymentMethod], [DatePaid], [DateSent]) VALUES (5, 0, NULL, CAST(0x0000A2D10173A6E8 AS DateTime), CAST(0x0000A2D10173A6E8 AS DateTime), N'a3b16406-d002-4bef-af1b-284d75feb23f', N'LA-1132099177', NULL, CAST(0.00000 AS Decimal(18, 5)), NULL, CAST(0.00000 AS Decimal(18, 5)), CAST(0.00000 AS Decimal(18, 5)), N'', NULL, NULL)
SET IDENTITY_INSERT [dbo].[DbOrders] OFF
SET IDENTITY_INSERT [dbo].[DbShops] ON 

INSERT [dbo].[DbShops] ([Id], [Name], [CountryId]) VALUES (1, N'LetterAmazer IvS', 59)
SET IDENTITY_INSERT [dbo].[DbShops] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_DbCountries]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbCountries] ON [dbo].[DbCountries]
(
	[CountryCode] ASC,
	[ContinentName] ASC,
	[Continent] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_DbCoupons]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbCoupons] ON [dbo].[DbCoupons]
(
	[Code] ASC,
	[CouponStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_DbCustomers]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbCustomers] ON [dbo].[DbCustomers]
(
	[Email] ASC,
	[Password] ASC,
	[ResetPasswordKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DbFulfillmentPartners]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbFulfillmentPartners] ON [dbo].[DbFulfillmentPartners]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Letter_Customer]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_FK_Letter_Customer] ON [dbo].[DbLetters]
(
	[CustomerId] ASC,
	[LetterStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DbOfficeProducts]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbOfficeProducts] ON [dbo].[DbOfficeProducts]
(
	[CountryId] ASC,
	[ScopeType] ASC,
	[ContinentId] ASC,
	[ProductDetailsId] ASC,
	[OfficeId] ASC,
	[ZipId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DbOffices]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbOffices] ON [dbo].[DbOffices]
(
	[PartnerId] ASC,
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DbOrderItems]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbOrderItems] ON [dbo].[DbOrderItems]
(
	[OrderId] ASC,
	[LetterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_DbOrders]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbOrders] ON [dbo].[DbOrders]
(
	[OrderStatus] ASC,
	[DateCreated] ASC,
	[CustomerId] ASC,
	[OrderCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DbProductMatrix]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbProductMatrix] ON [dbo].[DbProductMatrix]
(
	[ReferenceType] ASC,
	[ValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DbProductMatrixLines]    Script Date: 15-02-2014 14:21:39 ******/
CREATE NONCLUSTERED INDEX [IX_DbProductMatrixLines] ON [dbo].[DbProductMatrixLines]
(
	[ProductMatrixId] ASC,
	[LineType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DbCustomers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_Countries] FOREIGN KEY([CustomerInfo_Country])
REFERENCES [dbo].[DbCountries] ([Id])
GO
ALTER TABLE [dbo].[DbCustomers] CHECK CONSTRAINT [FK_Customers_Countries]
GO
ALTER TABLE [dbo].[DbFulfillmentPartners]  WITH CHECK ADD  CONSTRAINT [FK_FulfillmentPartners_Shops] FOREIGN KEY([ShopId])
REFERENCES [dbo].[DbShops] ([Id])
GO
ALTER TABLE [dbo].[DbFulfillmentPartners] CHECK CONSTRAINT [FK_FulfillmentPartners_Shops]
GO
ALTER TABLE [dbo].[DbLetters]  WITH CHECK ADD  CONSTRAINT [FK_Letter_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[DbCustomers] ([Id])
GO
ALTER TABLE [dbo].[DbLetters] CHECK CONSTRAINT [FK_Letter_Customer]
GO
ALTER TABLE [dbo].[DbLetters]  WITH CHECK ADD  CONSTRAINT [FK_Letters_FromCountry] FOREIGN KEY([FromAddress_Country])
REFERENCES [dbo].[DbCountries] ([Id])
GO
ALTER TABLE [dbo].[DbLetters] CHECK CONSTRAINT [FK_Letters_FromCountry]
GO
ALTER TABLE [dbo].[DbLetters]  WITH CHECK ADD  CONSTRAINT [FK_Letters_OfficeProducts] FOREIGN KEY([OfficeProductId])
REFERENCES [dbo].[DbOfficeProducts] ([Id])
GO
ALTER TABLE [dbo].[DbLetters] CHECK CONSTRAINT [FK_Letters_OfficeProducts]
GO
ALTER TABLE [dbo].[DbLetters]  WITH CHECK ADD  CONSTRAINT [FK_Letters_ToCountry] FOREIGN KEY([ToAddress_Country])
REFERENCES [dbo].[DbCountries] ([Id])
GO
ALTER TABLE [dbo].[DbLetters] CHECK CONSTRAINT [FK_Letters_ToCountry]
GO
ALTER TABLE [dbo].[DbOfficeProducts]  WITH CHECK ADD  CONSTRAINT [FK_OfficeProducts_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[DbCountries] ([Id])
GO
ALTER TABLE [dbo].[DbOfficeProducts] CHECK CONSTRAINT [FK_OfficeProducts_Countries]
GO
ALTER TABLE [dbo].[DbOfficeProducts]  WITH CHECK ADD  CONSTRAINT [FK_OfficeProducts_OfficeProductDetails] FOREIGN KEY([ProductDetailsId])
REFERENCES [dbo].[DbOfficeProductDetails] ([Id])
GO
ALTER TABLE [dbo].[DbOfficeProducts] CHECK CONSTRAINT [FK_OfficeProducts_OfficeProductDetails]
GO
ALTER TABLE [dbo].[DbOfficeProducts]  WITH CHECK ADD  CONSTRAINT [FK_OfficeProducts_Offices] FOREIGN KEY([OfficeId])
REFERENCES [dbo].[DbOffices] ([Id])
GO
ALTER TABLE [dbo].[DbOfficeProducts] CHECK CONSTRAINT [FK_OfficeProducts_Offices]
GO
ALTER TABLE [dbo].[DbOffices]  WITH CHECK ADD  CONSTRAINT [FK_Offices_Countries] FOREIGN KEY([Id])
REFERENCES [dbo].[DbCountries] ([Id])
GO
ALTER TABLE [dbo].[DbOffices] CHECK CONSTRAINT [FK_Offices_Countries]
GO
ALTER TABLE [dbo].[DbOffices]  WITH CHECK ADD  CONSTRAINT [FK_Offices_FulfillmentPartners] FOREIGN KEY([PartnerId])
REFERENCES [dbo].[DbFulfillmentPartners] ([Id])
GO
ALTER TABLE [dbo].[DbOffices] CHECK CONSTRAINT [FK_Offices_FulfillmentPartners]
GO
ALTER TABLE [dbo].[DbOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Letter] FOREIGN KEY([LetterId])
REFERENCES [dbo].[DbLetters] ([Id])
GO
ALTER TABLE [dbo].[DbOrderItems] CHECK CONSTRAINT [FK_OrderItem_Letter]
GO
ALTER TABLE [dbo].[DbOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[DbOrders] ([Id])
GO
ALTER TABLE [dbo].[DbOrderItems] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [dbo].[DbOrders]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[DbCustomers] ([Id])
GO
ALTER TABLE [dbo].[DbOrders] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[DbProductMatrixLines]  WITH CHECK ADD  CONSTRAINT [FK_ProductMatrixLines_ProductMatrix] FOREIGN KEY([ProductMatrixId])
REFERENCES [dbo].[DbProductMatrix] ([Id])
GO
ALTER TABLE [dbo].[DbProductMatrixLines] CHECK CONSTRAINT [FK_ProductMatrixLines_ProductMatrix]
GO
ALTER TABLE [dbo].[DbShops]  WITH CHECK ADD  CONSTRAINT [FK_Shops_Countries] FOREIGN KEY([Id])
REFERENCES [dbo].[DbCountries] ([Id])
GO
ALTER TABLE [dbo].[DbShops] CHECK CONSTRAINT [FK_Shops_Countries]
GO
USE [master]
GO
ALTER DATABASE [LetterAmazer] SET  READ_WRITE 
GO
