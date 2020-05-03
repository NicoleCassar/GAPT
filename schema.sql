USE [master]
GO
/****** Object:  Database [fypallocation]    Script Date: 19/04/2020 14:53:24 ******/
CREATE DATABASE [fypallocation]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'fypallocation', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER2\MSSQL\DATA\fypallocation.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'fypallocation_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER2\MSSQL\DATA\fypallocation_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [fypallocation] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [fypallocation].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [fypallocation] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [fypallocation] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [fypallocation] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [fypallocation] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [fypallocation] SET ARITHABORT OFF 
GO
ALTER DATABASE [fypallocation] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [fypallocation] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [fypallocation] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [fypallocation] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [fypallocation] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [fypallocation] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [fypallocation] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [fypallocation] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [fypallocation] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [fypallocation] SET  DISABLE_BROKER 
GO
ALTER DATABASE [fypallocation] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [fypallocation] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [fypallocation] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [fypallocation] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [fypallocation] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [fypallocation] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [fypallocation] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [fypallocation] SET RECOVERY FULL 
GO
ALTER DATABASE [fypallocation] SET  MULTI_USER 
GO
ALTER DATABASE [fypallocation] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [fypallocation] SET DB_CHAINING OFF 
GO
ALTER DATABASE [fypallocation] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [fypallocation] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [fypallocation] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'fypallocation', N'ON'
GO
ALTER DATABASE [fypallocation] SET QUERY_STORE = OFF
GO
USE [fypallocation]
GO
/****** Object:  Table [dbo].[allocation]    Script Date: 19/04/2020 14:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[allocation](
	[allocation_id] [int] NOT NULL,
	[student_id] [varchar](10) NOT NULL,
	[supervisor_id] [varchar](10) NOT NULL,
 CONSTRAINT [PK_allocation] PRIMARY KEY CLUSTERED 
(
	[allocation_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student]    Script Date: 19/04/2020 14:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[student](
	[student_id] [varchar](10) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[surname] [varchar](50) NOT NULL,
	[email] [text] NOT NULL,
	[average_mark] [float] NOT NULL,
 CONSTRAINT [PK_student] PRIMARY KEY CLUSTERED 
(
	[student_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[student_preference]    Script Date: 19/04/2020 14:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[student_preference](
	[preference_id] [int] NOT NULL,
	[student_id] [varchar](10) NOT NULL,
	[area_id] [int] NOT NULL,
	[time_submitted] [datetime] NOT NULL,
 CONSTRAINT [PK_student_preference] PRIMARY KEY CLUSTERED 
(
	[preference_id] ASC,
	[student_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[supervisor]    Script Date: 19/04/2020 14:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[supervisor](
	[supervisor_id] [varchar](10) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[surname] [varchar](50) NOT NULL,
	[email] [text] NOT NULL,
	[quota] [int] NOT NULL,
 CONSTRAINT [PK_supervisor] PRIMARY KEY CLUSTERED 
(
	[supervisor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[supervisor_area]    Script Date: 19/04/2020 14:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[supervisor_area](
	[area_id] [int] NOT NULL,
	[supervisor_id] [varchar](10) NOT NULL,
	[title] [varchar](100) NOT NULL,
	[description] [text] NOT NULL,
	[available] [bit] NOT NULL,
	[area_quota] [int] NOT NULL,
 CONSTRAINT [PK_supervisor_areanew] PRIMARY KEY CLUSTERED 
(
	[area_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[allocation]  WITH CHECK ADD  CONSTRAINT [FK_allocation_student] FOREIGN KEY([student_id])
REFERENCES [dbo].[student] ([student_id])
GO
ALTER TABLE [dbo].[allocation] CHECK CONSTRAINT [FK_allocation_student]
GO
ALTER TABLE [dbo].[allocation]  WITH CHECK ADD  CONSTRAINT [FK_allocation_supervisor] FOREIGN KEY([supervisor_id])
REFERENCES [dbo].[supervisor] ([supervisor_id])
GO
ALTER TABLE [dbo].[allocation] CHECK CONSTRAINT [FK_allocation_supervisor]
GO
ALTER TABLE [dbo].[student_preference]  WITH CHECK ADD  CONSTRAINT [FK_student_preference_student_preference] FOREIGN KEY([preference_id], [student_id])
REFERENCES [dbo].[student_preference] ([preference_id], [student_id])
GO
ALTER TABLE [dbo].[student_preference] CHECK CONSTRAINT [FK_student_preference_student_preference]
GO
ALTER TABLE [dbo].[supervisor_area]  WITH CHECK ADD  CONSTRAINT [FK_supervisor_area_supervisor] FOREIGN KEY([supervisor_id])
REFERENCES [dbo].[supervisor] ([supervisor_id])
GO
ALTER TABLE [dbo].[supervisor_area] CHECK CONSTRAINT [FK_supervisor_area_supervisor]
GO
USE [master]
GO
ALTER DATABASE [fypallocation] SET  READ_WRITE 
GO
