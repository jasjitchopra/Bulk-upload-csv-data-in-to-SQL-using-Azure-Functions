CREATE TABLE [dbo].[Sales](
	[Region] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[Item_Type] [nvarchar](50) NOT NULL,
	[Sales_Channel] [nvarchar](50) NOT NULL,
	[Order_Priority] [nvarchar](50) NOT NULL,
	[Order_Date] [datetime2](7) NOT NULL,
	[Order_ID] [int] NOT NULL,
	[Ship_Date] [datetime2](7) NOT NULL,
	[Units_Sold] [int] NOT NULL,
	[Unit_Price] [float] NOT NULL,
	[Unit_Cost] [float] NOT NULL,
	[Total_Revenue] [float] NOT NULL,
	[Total_Cost] [float] NOT NULL,
	[Total_Profit] [float] NOT NULL
) ON [PRIMARY]
GO
