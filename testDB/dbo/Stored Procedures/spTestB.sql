CREATE PROCEDURE spTestB (
	@param1 int
)AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select getdate() as [date], SYSDATETIME() as [dateTime2]
END