using EFCore.Contacts.Application.Behaviors;
using EFCore.Contacts.Application.Commands.Contact;
using EFCore.Contacts.Application.Queries;
using EFCore.Contacts.Domain;
using EFCore.Contacts.Infrastructure.Context;
using FluentValidation;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var sqLiteConnection = new SqliteConnection("DataSource=:memory:");
sqLiteConnection.Open();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDbContext<ContactContext>(options => options.UseSqlite(sqLiteConnection))
    .AddMediatRDispatcher()
    .AddDomain(new[] { typeof(Contact) })
    .AddRepositories<ContactContext>(binder =>
        binder.Bind<Contact, Guid, Guid>())
    .AddEfCoreInfrastructure(new[] { typeof(Contact) })
    .AddValidatorsFromAssembly(typeof(CreateContactCommand).Assembly)
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ContactContextBehavior<,>))
    .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))    
    .AddMediatR(typeof(CreateContactCommand))
    .AddScoped<IContactQueryService, ContactQueryService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

sqLiteConnection.Close();
