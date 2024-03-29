
/****** Object:  StoredProcedure [dbo].[Compra]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Compra]
@compra as char(100), @descripcion as char(200),@cantidad smallint ,@fecha as char(60), @total as smallmoney, @isv as smallmoney
AS
INSERT INTO Compras(compra,descripcion,cantidad,fecha,total,isv) VALUES(@compra,@descripcion,@cantidad,@fecha,@total,@isv)

GO
/****** Object:  StoredProcedure [dbo].[Envio]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Envio]
@repartidor char(100) , @movimiento char(50), @cantidad tinyint, @fecha as char(60),@dinero as money
AS
INSERT INTO Envios(repartidor,movimiento,cantidad,dinero,fecha) VALUES(@repartidor,@movimiento,@cantidad,@dinero,@fecha)

GO
/****** Object:  StoredProcedure [dbo].[Gasto]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Gasto]
@gasto as char(100), @descripcion as char(200), @fecha as char(60), @total as smallmoney
AS
INSERT INTO Gastos(gasto,descripcion,fecha,total) VALUES(@gasto,@descripcion,@fecha,@total)

GO
/****** Object:  StoredProcedure [dbo].[InhrsX]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InhrsX]
@id varchar(15), @fch date, @cantidad int, @tipo int
as
declare @salario as money = (select SalarioEmp from Empleados where IDemp = @id)
declare @horas as int = (select Horastrabajadas from Empleados where IDemp = @id)
declare @total as money

begin
if (@tipo = 1)
set @total = ((@salario/30)/@horas) * @cantidad * 1.25
else
set @total = ((@salario/30)/@horas) * @cantidad * 1.50
end

insert into HrsExtra
values (@id,@fch,@cantidad,@tipo,@total)




GO
/****** Object:  StoredProcedure [dbo].[InModEmpleados]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InModEmpleados]
@id varchar(15), @nombre varchar(100), @nacimiento date, @genero varchar(1), @fechainicio date, @estado bit,
@domicilio varchar(100), @salario money, @entrada varchar(5), @salida varchar(5), @puesto varchar(25),
@comentario varchar(100), @duracion date, @horas int
as

begin

    if exists (select 1 from Empleados where IDemp = @id )
	update Empleados
	set  NombreEmp = @nombre, FchNacimientoEmp = @nacimiento, GenEmp = @genero, FchInicioEmp = @fechainicio ,EstadoEmp = @estado, DomicilioEmp = @domicilio
	,SalarioEmp = @salario, HoraEntrada = @entrada, HoraSalida = @salida , PuestoEmp = @puesto , ComentarioEmp = @comentario
	,DurContratoEmp = @duracion, Horastrabajadas = @horas
	where IDemp = @id 

	else

	insert into Empleados
	values (@id,@nombre,@nacimiento,@genero,@fechainicio,@estado,@domicilio,@salario,@entrada,@salida,@puesto,@comentario,@duracion,@horas)

end

GO
/****** Object:  StoredProcedure [dbo].[InOtrosInEg]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[InOtrosInEg]
@id varchar(15), @descripcion varchar(60) , @tipo bit,  @total money, @fch date
as
insert into OtrosIngresos_Egresos
values (@id,@descripcion,@tipo,@total,@fch)

GO
/****** Object:  StoredProcedure [dbo].[InPlanillaMes]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InPlanillaMes]
@ID varchar(15), @desde as date, @hasta as date
as



declare @Salario as money = (select SalarioEmp from Empleados where IDemp = @ID)
declare @salariodia as money = @salario / 30
declare @hrsextra as money = (select IIF( (select Sum(TotalHrx) from HrsExtra where IdEmp = @ID and FchHrx between @desde and @hasta ) is null , 0  , (select Sum(TotalHrx) from HrsExtra where IdEmp = @ID and FchHrx between @desde and @hasta)   ))
declare @otrosing as money = (select IIF( (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 1 and FchInEg between @desde and @hasta) is null , 0 , (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 1 and FchInEg between @desde and @hasta) ))
declare @ihss as money

begin
if((select SalarioEmp from Empleados where IDemp = @ID) > 8920.80)

	set	@ihss = 133.812
else
	set @ihss = ((select SalarioEmp from Empleados where IDemp = @ID) * 0.015) 
end

declare @rap as money

begin
if((select SalarioEmp from Empleados where IDemp = @ID) > 8920.80)

	set	@rap = 89.208
else
	set @rap = ((select SalarioEmp from Empleados where IDemp = @ID) * 0.01) 
end

declare @Isr as money = 0
declare @otroseg as money = (select IIF( (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 0 and FchInEg between @desde and @hasta) is null ,0 , (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 0 and FchInEg between @desde and @hasta)  ))
declare @total as money = ( (@salario) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )


declare @fechacompletainicio as date = (select FchInicioEmp from Empleados where IDemp = @ID)
declare @fechacompletafinal as date = (select DurContratoEmp from Empleados where IDemp = @ID)

declare @anioactual as integer = year(@hasta)
declare @mesactual as integer = Month(@hasta)
declare @diaactual as integer = day(@hasta)
	
declare @anioempleado as integer = (select year(DurContratoEmp) from Empleados where IDemp = @id)
declare @mesempleado as integer = (select month(DurContratoEmp) from Empleados where IDemp = @id)
declare @diaempleado as integer = (select day(DurContratoEmp) from Empleados where IDemp = @id)

declare @inianioempleado as integer = (select year(FchInicioEmp) from Empleados where IDemp = @id)
declare @inimesempleado as integer = (select month(FchInicioEmp) from Empleados where IDemp = @id)
declare @inidiaempleado as integer = (select day(FchInicioEmp) from Empleados where IDemp = @id)




declare @fechaplussdesde as date = (select DATEADD(MONTH , 1 , @fechacompletainicio ) from Empleados where IDemp = @ID)
declare @diasdesde as int = (select Day(@fechaplussdesde) - 1)
set @fechaplussdesde = (select DATEADD(DAY , -@diasdesde  , @fechaplussdesde))

declare @fechaplusshasta as date = (select DATEADD(MONTH , -1 , @fechacompletafinal ) from Empleados where IDemp = @ID)
declare @diashasta as int = (select Day(@fechaplusshasta) - 1)
set @fechaplusshasta = (select DATEADD(DAY , -@diashasta  , @fechaplusshasta))

if (Exists(select * from PlanillaMes where IDemp = @ID and Fecha between @desde and @hasta))
begin
select 1
end
else
begin


if ((select DurContratoEmp from Empleados where IDemp = @ID) is not NULL)
begin

		--Condicion para mes de corte

		if( exists(select durContratoemp from empleados where IDemp = @ID AND DurContratoEmp between @desde and @hasta) )
		begin

		
			if(@diaactual != @diaempleado)
			begin
		
			
			
			declare @salariomescorte as money = (@salariodia * @diaempleado)
			

				begin
				if(@salariomescorte > 8920.80)

					set	@ihss = 133.812
				else
					set @ihss = (@salariomescorte * 0.015) 
				end



				begin
				if(@salariomescorte > 8920.80)

					set	@rap = 89.208
				else
					set @rap = (@salariomescorte * 0.01) 
				end


			declare @totalmescorte as money = ( (@salariomescorte) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )


			Insert into PlanillaMes
			values (@ID,@salariomescorte,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@totalmescorte,@desde)

			update Empleados
			set EstadoEmp = 0
			where IDemp = @ID
	
			end

			else

			Insert into PlanillaMes
			values (@ID,@Salario,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)


		end

		-- Fin de condicion para mes de corte


		--Inicio de condicion para mes de inicio

		if( exists(select FchInicioEmp from empleados where IDemp = @ID AND FchInicioEmp between @desde and @hasta) )
		begin
		
			if (@inidiaempleado != 1)
					begin

					declare @diastrabajados as int = (@diaactual - @inidiaempleado) + 1
					declare @salariomesini as money = (@salariodia * @diastrabajados)

					begin
						if(@salariomesini > 8920.80)

							set	@ihss = 133.812
						else
							set @ihss = (@salariomesini * 0.015) 
						end



						begin
						if(@salariomesini > 8920.80)

							set	@rap = 89.208
						else
							set @rap = (@salariomesini * 0.01) 
						end

					declare @totalmesini as money = ( (@salariomesini) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

					Insert into PlanillaMes
					values (@ID,@salariomesini,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@totalmesini,@desde)

					end

					else
			
					Insert into PlanillaMes
					values (@ID,@Salario,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

		end

		--fin de condicion para mes de inicio

		--Inicio de condicion para meses dentro del inicio y final de contrato


		if (@desde > @fechacompletainicio and @hasta < @fechacompletafinal)
		begin
		
			Insert into PlanillaMes
			values (@ID,@Salario,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)
			
		end

		--Final de condicion para meses dentro del inicio y final de contrato




end
	
		
if ((select DurContratoEmp from Empleados where IDemp = @ID) is NULL)
begin

			--Inicio de condicion para mes de inicio
				
			if( exists(select FchInicioEmp from empleados where IDemp = @ID AND FchInicioEmp between @desde and @hasta) )
		begin

			if (@inidiaempleado != 1)
					begin

					declare @diastrabajadosN as int = (@diaactual - @inidiaempleado) + 1
					declare @salariomesiniN as money = (@salariodia * @diastrabajadosN)

					begin
						if(@salariomesiniN > 8920.80)

							set	@ihss = 133.812
						else
							set @ihss = (@salariomesiniN * 0.015) 
						end



						begin
						if(@salariomesiniN > 8920.80)

							set	@rap = 89.208
						else
							set @rap = (@salariomesiniN * 0.01) 
						end

					declare @totalmesiniN as money = ( (@salariomesiniN) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

					Insert into PlanillaMes
					values (@ID,@salariomesiniN,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@totalmesiniN,@desde)

					end

					else
			
					Insert into PlanillaMes
					values (@ID,@Salario,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

		end

		--fin de condicion para mes de inicio

		--Inicio de condicion para meses dentro del inicio y final de contrato


		if (@desde > @fechacompletainicio)
		begin

			Insert into PlanillaMes
			values (@ID,@Salario,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)
			
		end

		--Final de condicion para meses dentro del inicio y final de contrato
end


end
GO
/****** Object:  StoredProcedure [dbo].[InPlanillaSem]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InPlanillaSem]
@ID varchar(15), @desde as date, @hasta as date, @iniciomes as date, @finalmes as date
as

declare @diahoras as int = (select Horastrabajadas from Empleados where IDemp = @ID)
declare @horasdiarias as int = (select Horastrabajadas from Empleados where IDemp = @ID)
declare @horas as int = ((select Horastrabajadas from Empleados where IDemp = @ID) * 5) + 4
declare @salariodia as money = ((select SalarioEmp from Empleados where IDemp = @ID)/30)
declare @Salariohora as money = (((select SalarioEmp from Empleados where IDemp = @ID) / 30) / @diahoras  )
declare @hrsextra as money = (select IIF( (select Sum(TotalHrx) from HrsExtra where IdEmp = @ID and FchHrx between @desde and @hasta ) is null , 0  , (select Sum(TotalHrx) from HrsExtra where IdEmp = @ID and FchHrx between @desde and @hasta)   ))
declare @otrosing as money = (select IIF( (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 1 and FchInEg between @desde and @hasta) is null , 0 , (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 1 and FchInEg between @desde and @hasta) ))
declare @ihss as money = 0
declare @rap as money = 0
declare @Isr as money = 0
declare @totalpagarsemana as money
declare @dinero8horas as money = (((select SalarioEmp from Empleados where IDemp = @ID) / 30) / 8  )
declare @otroseg as money = (select IIF( (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 0 and FchInEg between @desde and @hasta) is null ,0 , (select Sum(TotalInEg) from OtrosIngresos_Egresos where IdEmp = @ID and TipoInEg = 0 and FchInEg between @desde and @hasta)  ))
declare @total as money

declare @fechacompletainicio as date = (select FchInicioEmp from Empleados where IDemp = @ID)
declare @fechacompletafinal as date = (select DurContratoEmp from Empleados where IDemp = @ID)

declare @aniodesde as integer = year(@desde)
declare @mesdesde as integer = Month(@desde)
declare @diadesde as integer = day(@desde)

declare @aniohasta as integer = year(@hasta)
declare @meshasta as integer = Month(@hasta)
declare @diahasta as integer = day(@hasta)
	
declare @anioempleado as integer = (select year(DurContratoEmp) from Empleados where IDemp = @id)
declare @mesempleado as integer = (select month(DurContratoEmp) from Empleados where IDemp = @id)
declare @diaempleado as integer = (select day(DurContratoEmp) from Empleados where IDemp = @id)

declare @inianioempleado as integer = (select year(FchInicioEmp) from Empleados where IDemp = @id)
declare @inimesempleado as integer = (select month(FchInicioEmp) from Empleados where IDemp = @id)
declare @inidiaempleado as integer = (select day(FchInicioEmp) from Empleados where IDemp = @id)

declare @diafinalmes as int = day(@finalmes)

declare @aniolunes as int = year(@desde)
declare @meslunes as int = month(@desde)
declare @aniodomingo as int = year(@hasta)
declare @mesdomingo as int = month(@hasta)
declare @diadomingo as int = day(@hasta)

-- parametros de condiciones
 

declare @totalplanillames as money
declare @sumatotales as money = (select IIF( (select Sum(Totalplanillasem) from planillasem where IdEmp = @ID and Fecha between @iniciomes and @finalmes ) is null , 0  , (select Sum(Totalplanillasem) from planillasem where IdEmp = @ID and Fecha between @iniciomes and @finalmes)   ))
declare @diashastacorte as int 
declare @diastrabajados as int
declare @diasinicio as int
declare @diasatrabajar as int 
declare @diasinicioN as int 
declare @diasatrabajarN as int 



if (exists(select * from PlanillaSem where IDemp = @ID and fecha between @desde and @hasta))
begin

select 1

end
else
begin



--Inicio condicion que verifica si es ultima semana del mes

if (@aniolunes < @aniodomingo or @meslunes < @mesdomingo or @diadomingo = @diafinalmes )
begin


--Inicio Condicion que verifica si el contrato del empleado tiene fecha final

	if ((select DurContratoEmp from Empleados where IDemp = @ID) is not NULL)
	begin


	--Condicion para semana de corte

			if( exists(select durContratoemp from empleados where IDemp = @ID AND DurContratoEmp between @desde and @hasta) )
			begin

		
				if(@diahasta != @diaempleado)
				begin
		
				
				begin

				if(@mesdesde = @mesempleado)
				begin
				set @diashastacorte = @diaempleado - @diadesde + 1
					if(@diashastacorte > 5)
					begin
					set @diastrabajados = 5
					end
					else
					set @diastrabajados = @diashastacorte
					
				end
				
				end
				if (@mesdesde != @mesempleado)
				begin
				set @diashastacorte = (@diafinalmes - @diadesde + 1) + @diaempleado 
					if(@diashastacorte > 5)
					begin
					set @diastrabajados = 5
					end
					else
					set @diastrabajados = @diashastacorte
				end
				
			
				

				begin
					if((@fechacompletafinal between @iniciomes and @finalmes) )
					begin
					
					set @totalplanillames  = (@diaempleado) * (@salariodia)
					end
					else

					
					set @totalplanillames  = ( (30) * (@salariodia))

				end


					begin
					if(@totalplanillames > 8920.80)
					begin
						set	@ihss = 133.812

					end
					else
						set @ihss = (@totalplanillames * 0.015) 
					end



					begin
					if(@totalplanillames > 8920.80)
					begin
						set	@rap = 89.208

					end
					else
						set @rap = (@totalplanillames * 0.01) 
					end


					
				if(@diastrabajados >= 5)
				begin
				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  
				set @total = ( @totalpagarsemana + (@salariodia) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

				Insert into PlanillaSem
				values (@ID,(@diastrabajados * @horasdiarias),@Salariohora,@totalpagarsemana, @salariodia,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

				update Empleados
				set EstadoEmp = 0
				where IDemp = @ID

				end
				if(@diastrabajados < 5 )
				begin
				set @totalpagarsemana = ((@diastrabajados * @horasdiarias) * @salariohora)

				set @total = ( @totalpagarsemana + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

				Insert into PlanillaSem
				values (@ID,(@diastrabajados * @horasdiarias),@Salariohora,0, 0,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

				update Empleados
				set EstadoEmp = 0
				where IDemp = @ID


				end
						

				


				

			

				
	
				end

				if(@diahasta = @diaempleado)
				begin

					if(@meshasta = @mesempleado)
					begin
					set @totalplanillames  = (@diaempleado) * (@salariodia)
					end
					if(@meshasta != @mesempleado)
					begin
					set @totalplanillames  = (30) * (@salariodia)
					end


					begin
					if(@totalplanillames > 8920.80)

						set	@ihss = 133.812
					else
						set @ihss = (@totalplanillames * 0.015) 
					end



					begin
					if(@totalplanillames > 8920.80)

						set	@rap = 89.208
					else
						set @rap = (@totalplanillames * 0.01) 
					end

				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  
				set @total = ( @totalpagarsemana + (@salariodia) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

				
				Insert into PlanillaSem
				values (@ID,@horas,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)
				end

			end

			-- Fin de condicion para mes de corte


			--Inicio de condicion para semana de inicio

			if( exists(select FchInicioEmp from empleados where IDemp = @ID AND FchInicioEmp between @desde and @hasta) )
			begin
		
			


							begin

							if(@diadesde != @inidiaempleado)



										if(@mesdesde = @inimesempleado)

											begin

											if(@inidiaempleado = @diadomingo)
											begin

											set @diasinicio = 0
											set @totalpagarsemana = ((@diasinicio * @horasdiarias) * @salariohora)


											end

											if(@inidiaempleado != @diadomingo)
											begin

											set @diasinicio = ((@diafinalmes - @inidiaempleado + 1) + @diadomingo) - 2
											set @totalpagarsemana = ((((@diasinicio * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))

											end

										end
				
										
										if (@mesdesde != @inimesempleado)

											begin

											if(@inidiaempleado = @diadomingo)
											begin

											set @diasinicio = 0
											set @totalpagarsemana = ((@diasinicio * @horasdiarias) * @salariohora)

											end
											if(@inidiaempleado != @diadomingo)
											begin

											set @diasinicio = (@diadomingo - @inidiaempleado + 1) - 2
											set @totalpagarsemana = ((((@diasinicio * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))

											end

										end


						set @total = ( @totalpagarsemana + @Salariohora * @diahoras  + (@hrsextra) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

			

						Insert into PlanillaSem
						values (@ID,(@diasinicio * @horasdiarias) + 4,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@hasta)

						end

						if (@diadesde = @inidiaempleado)
						begin

						
						set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  
						set @total = ( @totalpagarsemana + (@Salariohora * @diahoras) + (@hrsextra) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

			
			
						Insert into PlanillaSem
						values (@ID,@horas,@Salariohora ,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@hasta)
						end

			end

			

			--fin de condicion para semana de inicio

			--Inicio de condicion para meses dentro del inicio y final de contrato


			if (@desde > @fechacompletainicio and @hasta < @fechacompletafinal)
			begin

			begin
			if((@fechacompletainicio between @iniciomes and @finalmes) )
			begin
			set @diasatrabajar = (@diafinalmes - @inidiaempleado) + 1
			set @totalplanillames  = (@diasatrabajar) * (@salariodia)
			end
			if(@mesdesde != @inimesempleado)
			begin
			set @diasatrabajar = 30
			set @totalplanillames  = ( (30) * (@salariodia))
			end

			end

			


					begin
					if(@totalplanillames > 8920.80)

						set	@ihss = 133.812
					else
						set @ihss = (@totalplanillames * 0.015) 
					end



					begin
					if(@totalplanillames > 8920.80)

						set	@rap = 89.208
					else
						set @rap = (@totalplanillames * 0.01) 
					end

				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

				

			
				set @total = ( @totalpagarsemana + (@Salariohora * @diahoras) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@Isr) + (@otroseg) )

		
				Insert into PlanillaSem
				values (@ID,@horas,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)
			
			end

			--Final de condicion para meses dentro del inicio y final de contrato


	end

	--Final Condicion que verifica si el contrato del empleado tiene fecha final


	--Inicio Condicion que verifica si el contrato del empleado es indefinido

	if ((select DurContratoEmp from Empleados where IDemp = @ID) is NULL)
	begin


	--Inicio de condicion para semana de inicio

			if( exists(select FchInicioEmp from empleados where IDemp = @ID AND FchInicioEmp between @desde and @hasta) )
			begin
		
						begin

							if(@diadesde != @inidiaempleado)



										if(@mesdesde = @inimesempleado)

											begin

											if(@inidiaempleado = @diadomingo)
											begin

											set @diasinicio = 0
											set @totalpagarsemana = ((@diasinicio * @horasdiarias) * @salariohora)


											end

											if(@inidiaempleado != @diadomingo)
											begin

											set @diasinicio = ((@diafinalmes - @inidiaempleado + 1) + @diadomingo) - 2
											set @totalpagarsemana = ((((@diasinicio * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))

											end

										end
				
										
										if (@mesdesde != @inimesempleado)

											begin

											if(@inidiaempleado = @diadomingo)
											begin

											set @diasinicio = 0
											set @totalpagarsemana = ((@diasinicio * @horasdiarias) * @salariohora)

											end
											if(@inidiaempleado != @diadomingo)
											begin

											set @diasinicio = (@diadomingo - @inidiaempleado + 1) - 2
											set @totalpagarsemana = ((((@diasinicio * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))

											end

										end


						set @total = ( @totalpagarsemana + @Salariohora * @diahoras  + (@hrsextra) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

			

						Insert into PlanillaSem
						values (@ID,(@diasinicio * @horasdiarias) + 4,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@hasta)

						end

						if (@diadesde = @inidiaempleado)
						begin

						
						set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  
						set @total = ( @totalpagarsemana + (@Salariohora * @diahoras) + (@hrsextra) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

			
			
						Insert into PlanillaSem
						values (@ID,@horas,@Salariohora ,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@hasta)
						end

			end

			

			--fin de condicion para semana de inicio

			--Inicio de condicion para meses dentro del inicio y final de contrato


			if (@desde > @fechacompletainicio)
			begin

						

							if( (@fechacompletainicio between @iniciomes and @finalmes)   )
							begin
			
							set @totalplanillames  = ( (@diafinalmes - @inidiaempleado) + 1 ) * (@salariodia)

							end
							
							if(@inimesempleado != @mesdesde)
							begin
							set @totalplanillames  = ( (30) * (@salariodia))
							end


							begin
							if(@totalplanillames > 8920.80)

								set	@ihss = 133.812
							else
								set @ihss = (@totalplanillames * 0.015) 
							end



							begin
							if(@totalplanillames > 8920.80)

								set	@rap = 89.208
							else
								set @rap = (@totalplanillames * 0.01) 
							end
		

				set @total = ( ((@Salariohora * @horas) * 1.0909) + (@Salariohora * @diahoras) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@Isr) + (@otroseg) )

		
				Insert into PlanillaSem
				values (@ID,@horas,@Salariohora,((@Salariohora * @horas) * 1.0909), @Salariohora * @diahoras ,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)
			
			end

			--Final de condicion para meses dentro del inicio y final de contrato
	



	end

--final Condicion que verifica si el contrato del empleado es indefinido


end


--Ingreso de las mismas condiciones en semana que no sea ultima de mes
if (@meslunes = @mesdomingo and @diadomingo != @diafinalmes )
begin


--Inicio Condicion que verifica si el contrato del empleado tiene fecha final

	if ((select DurContratoEmp from Empleados where IDemp = @ID) is not NULL)
	begin


	--Condicion para semana de corte

			if( exists(select durContratoemp from empleados where IDemp = @ID AND DurContratoEmp between @desde and @hasta) )
			begin

		
				if(@diahasta != @diaempleado)
				begin
		
			
			
				
				set @diashastacorte = @diaempleado - @diadesde + 1
					if(@diashastacorte > 5)
					begin
					set @diastrabajados = 5
					end
					else
					set @diastrabajados = @diashastacorte
			
				set @totalplanillames  = ( @diaempleado * @salariodia )


					begin
					if(@totalplanillames > 8920.80)

						set	@ihss = 133.812
					else
						set @ihss = (@totalplanillames * 0.015) 
					end



					begin
					if(@totalplanillames > 8920.80)

						set	@rap = 89.208
					else
						set @rap = (@totalplanillames * 0.01) 
					end


				if(@diastrabajados >= 5)
				begin
				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  
				set @total = ( @totalpagarsemana + (@salariodia) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

				Insert into PlanillaSem
				values (@ID,(@diastrabajados * @horasdiarias),@Salariohora,@totalpagarsemana, @salariodia,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

				update Empleados
				set EstadoEmp = 0
				where IDemp = @ID

				end
				if(@diastrabajados < 5 )
				begin
				set @totalpagarsemana = ((@diastrabajados * @horasdiarias) * @salariohora)

				set @total = ( @totalpagarsemana + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@isr) + (@otroseg) )

				Insert into PlanillaSem
				values (@ID,(@diastrabajados * @horasdiarias),@Salariohora,0, 0,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

				update Empleados
				set EstadoEmp = 0
				where IDemp = @ID


				end  

		
				end

				if(@diahasta = @diaempleado)
				begin

				set @totalplanillames  = ( @diaempleado * @salariohora )


					begin
					if(@totalplanillames > 8920.80)

						set	@ihss = 133.812
					else
						set @ihss = (@totalplanillames * 0.015) 
					end



					begin
					if(@totalplanillames > 8920.80)

						set	@rap = 89.208
					else
						set @rap = (@totalplanillames * 0.01) 
					end


				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

				

			
				set @total = ( @totalpagarsemana + (@Salariohora * @diahoras) + (@hrsextra) + (@otrosing) ) - (  (@ihss) + (@rap) + (@Isr) + (@otroseg) )
				
				Insert into PlanillaSem
				values (@ID,@horas,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,@ihss,@rap,@Isr,@otroseg,@total,@desde)

				update Empleados
				set EstadoEmp = 0
				where IDemp = @ID
				end

			end

			-- Fin de condicion para mes de corte


			--Inicio de condicion para semana de inicio

			if( exists(select FchInicioEmp from empleados where IDemp = @ID AND FchInicioEmp between @desde and @hasta) )
			begin
		
				if (@diadesde != @inidiaempleado)
						begin

						if((@inidiaempleado - @diadesde) > 5 )
						begin

						set @diasatrabajar = 0
						set @totalpagarsemana = ((((@diasatrabajar * @horasdiarias) * @salariohora)  )*(1.0909))  

						end

						
						if((@inidiaempleado - @diadesde) <= 5 )
						begin


						set @diasinicio  = (@inidiaempleado - @diadesde)
						set @diasatrabajar  = ((@diasinicio - 7)*(-1)) - 2

						set @totalpagarsemana = ((((@diasatrabajar * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

						end

						
						

						set @total = ( (@totalpagarsemana) + (@hrsextra) + (@horasdiarias * @Salariohora) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

			

						Insert into PlanillaSem
						values (@ID,(@diasatrabajar * @horasdiarias) + 4,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@desde)

						end

						
						if (@diadesde = @inidiaempleado)
						begin

						set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

						set @total = ( (@totalpagarsemana) + (@hrsextra) + (@horasdiarias * @Salariohora) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

						

			
			
						Insert into PlanillaSem
						values (@ID,@horas,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@desde)
						end

			end

			--fin de condicion para semana de inicio

			--Inicio de condicion para meses dentro del inicio y final de contrato


			if (@desde > @fechacompletainicio and @hasta < @fechacompletafinal)
			begin


				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

				set @total = ( (@totalpagarsemana) + (@hrsextra) + (@horasdiarias * @Salariohora) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

		
				Insert into PlanillaSem
				values (@ID,@horas,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@desde)
			
			end

			--Final de condicion para meses dentro del inicio y final de contrato


	end

	--Final Condicion que verifica si el contrato del empleado tiene fecha final


	--Inicio Condicion que verifica si el contrato del empleado es indefinido

	if ((select DurContratoEmp from Empleados where IDemp = @ID) is NULL)
	begin


	--Inicio de condicion para semana de inicio

			if( exists(select FchInicioEmp from empleados where IDemp = @ID AND FchInicioEmp between @desde and @hasta) )
			begin
		
				if (@diadesde != @inidiaempleado)
						begin

						set @diasinicio  = (@inidiaempleado - @diadesde)
						set @diasatrabajarN  = ((@diasinicio - 7)*(-1)) - 2
						set @totalpagarsemana = ((((@diasatrabajarN * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

						set @total = ( (@totalpagarsemana) + (@hrsextra) + (@horasdiarias * @Salariohora) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

		

						Insert into PlanillaSem
						values (@ID,(@diasatrabajarN * @horasdiarias) + 4,@Salariohora,@totalpagarsemana, @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@desde)

						end

						if (@diadesde = @inidiaempleado)
						begin
					

						set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

						set @total = ( (@totalpagarsemana) + (@hrsextra) + (@horasdiarias * @Salariohora) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

			
			
						Insert into PlanillaSem
						values (@ID,@horas,@Salariohora,((@Salariohora * @horas) * 1.0909), @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@desde)
						end

			end

			--fin de condicion para semana de inicio

			--Inicio de condicion para meses dentro del inicio y final de contrato


			if (@desde > @fechacompletainicio)
			begin

				set @totalpagarsemana = ((((5 * @horasdiarias) * @salariohora) + (4 * @dinero8horas) )*(1.0909))  

				set @total = ( (@totalpagarsemana) + (@hrsextra) + (@horasdiarias * @Salariohora) + (@otrosing) ) - (  (0) + (0) + (0) + (@otroseg) )

		
				Insert into PlanillaSem
				values (@ID,@horas,@Salariohora,((@Salariohora * @horas) * 1.0909), @Salariohora * @diahoras ,@hrsextra,@otrosing,0,0,0,@otroseg,@total,@desde)
			
			end

			--Final de condicion para meses dentro del inicio y final de contrato
	



	end




--Fin condicion que verifica si es ultima semana del mes

end



end







GO
/****** Object:  StoredProcedure [dbo].[Perdida]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Perdida]
@perdida as char(100), @descripcion as char(200), @fecha as char(60), @total as smallmoney
AS
INSERT INTO Perdidas(perdida,descripcion,fecha,total) VALUES(@perdida,@descripcion,@fecha,@total)

GO
/****** Object:  StoredProcedure [dbo].[SearchEmpleados]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[SearchEmpleados]
@nombre varchar(100)
as
select IDemp, NombreEmp, SalarioEmp, PuestoEmp from Empleados
where NombreEmp Like '%' + @Nombre + '%' and EstadoEmp = 1
GO
/****** Object:  StoredProcedure [dbo].[ShowEmpleados]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ShowEmpleados]
as
select IDemp, NombreEmp, SalarioEmp, PuestoEmp from Empleados
where EstadoEmp = 1
GO
/****** Object:  StoredProcedure [dbo].[Showplanillames]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Showplanillames]
@fecha date
as
select NombreEmp, Salariomes, hrsextra , otrosing, ihss, Rap, Isr, otroseg, Round(totalplanillames,2), Fecha 
from PlanillaMes inner join Empleados on PlanillaMes.IDemp = Empleados.IDemp
where fecha = @fecha
GO
/****** Object:  StoredProcedure [dbo].[Showplanillasem]    Script Date: 12/07/2019 16:25:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Showplanillasem]
@fecha date, @fecha2 date
as
select NombreEmp, horas, Salariohora , devengadoporcentaje, septimodia , hrsextra, otrosing , ihss,rap,Isr , otroseg, totalplanillasem, Fecha 
from PlanillaSem inner join Empleados on PlanillaSem.IDemp = Empleados.IDemp
where fecha between @fecha and @fecha2
 
GO
