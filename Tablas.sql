
/****** Object:  Table [dbo].[Compras]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Compras](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[compra] [char](100) NOT NULL,
	[descripcion] [char](200) NULL,
	[cantidad] [smallint] NOT NULL,
	[fecha] [char](60) NOT NULL,
	[total] [money] NOT NULL,
	[isv] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Empleados]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Empleados](
	[IDemp] [varchar](15) NOT NULL,
	[NombreEmp] [varchar](100) NOT NULL,
	[FchNacimientoEmp] [date] NOT NULL,
	[GenEmp] [varchar](1) NOT NULL,
	[FchInicioEmp] [date] NOT NULL,
	[EstadoEmp] [bit] NOT NULL,
	[DomicilioEmp] [varchar](100) NOT NULL,
	[SalarioEmp] [money] NOT NULL,
	[HoraEntrada] [varchar](5) NOT NULL,
	[HoraSalida] [varchar](5) NOT NULL,
	[PuestoEmp] [varchar](50) NOT NULL,
	[ComentarioEmp] [varchar](100) NULL,
	[DurContratoEmp] [date] NULL,
	[Horastrabajadas] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IDemp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Envios]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Envios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[repartidor] [char](100) NOT NULL,
	[movimiento] [char](50) NULL,
	[cantidad] [tinyint] NULL,
	[dinero] [money] NULL,
	[fecha] [char](60) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Gastos]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Gastos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gasto] [char](100) NOT NULL,
	[descripcion] [char](200) NULL,
	[fecha] [char](60) NOT NULL,
	[total] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HrsExtra]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HrsExtra](
	[CodHrx] [int] IDENTITY(1,1) NOT NULL,
	[IdEmp] [varchar](15) NOT NULL,
	[FchHrx] [date] NOT NULL,
	[CantHrx] [int] NOT NULL,
	[TipoHrx] [int] NOT NULL,
	[TotalHrx] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CodHrx] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OtrosIngresos_Egresos]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OtrosIngresos_Egresos](
	[CodInEg] [int] IDENTITY(1,1) NOT NULL,
	[IdEmp] [varchar](15) NOT NULL,
	[DescInEg] [varchar](60) NOT NULL,
	[TipoInEg] [bit] NOT NULL,
	[TotalInEg] [money] NOT NULL,
	[FchInEg] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CodInEg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Perdidas]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Perdidas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[perdida] [char](100) NOT NULL,
	[descripcion] [char](200) NULL,
	[fecha] [char](60) NOT NULL,
	[total] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PlanillaMes]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PlanillaMes](
	[IdplanillaMes] [int] IDENTITY(1,1) NOT NULL,
	[IDemp] [varchar](15) NOT NULL,
	[Salariomes] [money] NOT NULL,
	[hrsextra] [money] NULL,
	[otrosing] [money] NULL,
	[ihss] [money] NOT NULL,
	[rap] [money] NOT NULL,
	[Isr] [money] NOT NULL,
	[otroseg] [money] NULL,
	[totalplanillames] [money] NOT NULL,
	[Fecha] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdplanillaMes] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PlanillaSem]    Script Date: 12/07/2019 16:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PlanillaSem](
	[Idpanillasem] [int] IDENTITY(1,1) NOT NULL,
	[IDemp] [varchar](15) NOT NULL,
	[horas] [int] NOT NULL,
	[SalarioHora] [money] NOT NULL,
	[Devengadoporcentaje] [money] NOT NULL,
	[septimodia] [money] NOT NULL,
	[hrsextra] [money] NOT NULL,
	[otrosing] [money] NOT NULL,
	[ihss] [money] NOT NULL,
	[rap] [money] NOT NULL,
	[Isr] [money] NOT NULL,
	[otroseg] [money] NOT NULL,
	[totalplanillasem] [money] NOT NULL,
	[fecha] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Idpanillasem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[HrsExtra]  WITH CHECK ADD FOREIGN KEY([IdEmp])
REFERENCES [dbo].[Empleados] ([IDemp])
GO
ALTER TABLE [dbo].[PlanillaSem]  WITH CHECK ADD FOREIGN KEY([IDemp])
REFERENCES [dbo].[Empleados] ([IDemp])
GO
