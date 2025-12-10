CREATE PROCEDURE dbo.GetProductsByDepartment
    @DepartmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.SKU, p.Name, p.Description, p.Price, p.Weight, p.DepartmentID
    FROM Product AS p
    WHERE p.DepartmentID = @DepartmentId;
END