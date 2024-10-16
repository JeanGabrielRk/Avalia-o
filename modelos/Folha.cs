public class Folha
{
    public int folhaId { get; set; }
    public double valor { get; set; }
    public int quantidade { get; set; }
    public int mes { get; set; }
    public int ano { get; set; }
    public int funcionarioId { get; set; }

    public double salarioBruto { get; set; }
    public double descontoIrrf { get; set; }
    public double descontoInss { get; set; }
    public double fgts { get; set; }
    public double salarioLiquido { get; set; }
    
}