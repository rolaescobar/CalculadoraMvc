namespace CalculadoraMvc.Models
{
    public class Calculadora
    {
        public double Numero1 { get; set; }
        public double Numero2 { get; set; }

        public double Sumar() => Numero1 + Numero2;
        public double Restar() => Numero1 - Numero2;
        public double Multiplicar() => Numero1 * Numero2;
        public double Dividir() => Numero2 == 0 ? double.NaN : Numero1 / Numero2;
    }
}
