CREATE TABLE [dbo].[Product](
	[ProductID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[ProductNumber] [nvarchar](50) NULL,
	[Color] [nvarchar](50) NULL,
	[StandardCost] [float] NULL,
	[ListPrice] [float] NULL,
	[Size] [nvarchar](50) NULL,
	[Weight] [float] NULL,
	[ProductCategoryID] [int] NULL,
	[ProductModelID] [int] NULL
) ON [PRIMARY]
GO
