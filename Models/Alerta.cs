public class Alerta
{
    public string Mensagem { get; set; } = "Alerta de ressecamento!";
    public double Temperatura { get; set; }
    public double Umidade { get; set; }
    public DateTime DataHora { get; set; } = DateTime.Now;
}