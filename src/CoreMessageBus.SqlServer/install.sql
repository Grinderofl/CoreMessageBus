USE [ServiceBusQueue]
GO
/****** Object:  Table [dbo].[SqlServerQueue]    Script Date: 15/03/2016 08:35:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SqlServerQueue](
	[Id] [uniqueidentifier] NOT NULL,
	[MessageId] [uniqueidentifier] NOT NULL,
	[ContentType] [nvarchar](255) NOT NULL,
	[Encoding] [nvarchar](255) NOT NULL,
	[Data] [nvarchar](max) NULL,
	[Created] [datetime] NULL,
	[Deferred] [datetime] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](255) NULL,
	[QueueId] [int] NOT NULL,
 CONSTRAINT [PK_SqlServerQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SqlServerQueues]    Script Date: 15/03/2016 08:35:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SqlServerQueues](
	[QueueId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_SqlServerQueues] PRIMARY KEY CLUSTERED 
(
	[QueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SqlServerQueue]  WITH CHECK ADD  CONSTRAINT [FK_SqlServerQueue_SqlServerQueues] FOREIGN KEY([QueueId])
REFERENCES [dbo].[SqlServerQueues] ([QueueId])
GO
ALTER TABLE [dbo].[SqlServerQueue] CHECK CONSTRAINT [FK_SqlServerQueue_SqlServerQueues]
GO
