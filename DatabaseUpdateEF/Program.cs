using Dapper;
using DatabaseUpdateEF;
using DatabaseUpdateEF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapPut("update-salary-by-company-id", async (Guid companyId, ApplicationDbContext dbContext) =>
{
    var Companies = await dbContext.Set<Company>()
                    .Include(c => c.Employees)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(c => c.Id == companyId);

    if (Companies is null) return Results.NotFound($"Not found company with id: {companyId}");

    foreach (var employee in Companies.Employees)
    {
        employee.Salary *= 1.1m;
    }

    Companies.UpdateSalaryDateUTC = DateTime.UtcNow;

    await dbContext.SaveChangesAsync();

    return Results.Ok("Update salary has been successfully!");

});

app.MapPut("update-salary-by-company-id-sql", async (Guid companyId, ApplicationDbContext dbContext) =>
{
    var Companies = await dbContext.Set<Company>()
                    .FirstOrDefaultAsync(c => c.Id == companyId);

    if (Companies is null) return Results.NotFound($"Not found company with id: {companyId}");

    FormattableString sqlQuery = $"UPDATE EMPLOYEE SET [SALARY] = [SALARY] * 1.1 WHERE [COMPANYID] = {companyId}";

    await dbContext.Database.BeginTransactionAsync();

    await dbContext.Database
        .ExecuteSqlInterpolatedAsync(sqlQuery);

    Companies.UpdateSalaryDateUTC = DateTime.UtcNow;

    await dbContext.SaveChangesAsync();

    await dbContext.Database.CommitTransactionAsync();

    return Results.Ok("Update salary has been successfully!");

});

app.MapPut("update-salary-by-company-id-dapper", async (Guid companyId, ApplicationDbContext dbContext) =>
{
    var Companies = await dbContext.Set<Company>()
                    .FirstOrDefaultAsync(c => c.Id == companyId);

    if (Companies is null) return Results.NotFound($"Not found company with id: {companyId}");

    var sqlQuery = $"UPDATE EMPLOYEE SET [SALARY] = [SALARY] * 1.1 WHERE [COMPANYID] = @CompanyId";

    var transaction = await dbContext.Database.BeginTransactionAsync();

    await dbContext.Database.GetDbConnection()
        .ExecuteAsync(sqlQuery, new { CompanyId = companyId },transaction.GetDbTransaction());

    Companies.UpdateSalaryDateUTC = DateTime.UtcNow;

    await dbContext.SaveChangesAsync();

    await dbContext.Database.CommitTransactionAsync();

    return Results.Ok("Update salary has been successfully!");

});

app.MapControllers();

app.Run();
