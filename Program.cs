
using API.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

app.MapGet("/", () => "AVALIAÇÃO DEV VISUAL");

app.MapPost("/gabriel/funcionario/cadastrar", ([FromBody] Funcionario funcionario, [FromServices] AppDataContext ctx) => 
{
    ctx.Funcionarios.Add(funcionario);
    ctx.SaveChanges();
    return Results.Created("", funcionario);
});

app.MapPost("/gabriel/folha/cadastrar", ([FromBody] Folha folha, [FromServices] AppDataContext ctx) => 
{
   if(ctx.Folha.Any()){

    double salarioBruto = folha.quantidade * folha.valor;
    double impostoIrrf = CalculoIrrf(salarioBruto);
    double impostoInss = CalculoInns(salarioBruto);
    double impostoFgts = salarioBruto * 0.08;
    double salarioLiquido = salarioBruto - impostoIrrf - impostoInss;

    folha.salarioBruto = salarioBruto;
    folha.descontoIrrf = impostoIrrf;
    folha.descontoInss = impostoInss;
    folha.fgts = impostoFgts;
    folha.salarioLiquido = salarioLiquido;

    ctx.Folha.Add(folha);
    ctx.SaveChangesAsync();
    return Results.Created("", folha);
   }    

   return Results.NotFound();
});

double CalculoIrrf(double salarioBruto)
{

    if (salarioBruto <= 1903.98)
        return 0;

    if (salarioBruto <= 2826.65)
        return salarioBruto * 0.075 - 142.80;

    if (salarioBruto <= 3751.05)
        return salarioBruto * 0.15 - 354.80;

    if (salarioBruto <= 4664.68)
        return salarioBruto * 0.225 - 636.13;

    return salarioBruto * 0.275 - 869.36;
}

double CalculoInns(double salarioBruto)
{

    if (salarioBruto <= 1693.72)
        return salarioBruto * 0.08;

    if (salarioBruto <= 2822.90)
        return salarioBruto * 0.09;

    if (salarioBruto <= 5645.80)
        return salarioBruto * 0.11;

    return 621.03; 
}

app.MapGet("/gabriel/folha/listar", ([FromServices] AppDataContext ctx) => 
{
    if(ctx.Folha.Any())
    {
        return Results.Ok(ctx.Folha.ToList());
    }
    return Results.NotFound();
});

app.MapGet("/gabriel/funcionario/listar", ([FromServices] AppDataContext ctx) => 
{
    if(ctx.Funcionarios.Any())
    {
        return Results.Ok(ctx.Folha.ToList());
    }
    return Results.NotFound();
});

app.MapGet("/gabriel/folha/buscar/{cpf}/{mes}/{ano}", ([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano, [FromServices] AppDataContext ctx) => 
{
    Folha? folha = ctx.Folha.Find(cpf);
    if(folha is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(folha);
});

app.Run();

