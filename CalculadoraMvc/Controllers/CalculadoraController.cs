using CalculadoraMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text;

namespace CalculadoraMvc.Controllers
{
  
    public class CalculadoraController : Controller
        {

        private static readonly List<string> _historial = new();


        [HttpGet]
        public IActionResult ExportarCsv()
        {
            // Si el historial está vacío, devolvé un mensaje
            if (_historial == null || !_historial.Any())
                return Content("No hay operaciones para exportar.");

            // Crear cabecera y líneas CSV
            var csv = new StringBuilder();
            csv.AppendLine("FechaHora,Operacion,Numero1,Numero2,Resultado");

            foreach (var linea in _historial)
            {
                // Ejemplo de línea: "15:30:12 → 5 + 8 = 13"
                // Extraer datos básicos para formato CSV simple
                var partes = linea.Split(' ');
                if (partes.Length >= 6)
                {
                    string hora = partes[0];
           
                    string n1 = partes[2];
                    string op = partes[3];
                    string n2 = partes[4];
                    string res = partes[6];
                    csv.AppendLine($"{hora},{op},{n1},{n2},{res}");
                }
            }

            // Retornar archivo CSV descargable
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "historial_calculadora.csv");
        }


        private void AgregarAlHistorial(string operacion, double n1, double n2 , double resultado)

        {

            string simbolo = operacion switch
            {
                "sumar" => "+",
                "restar" => "-",
                "multiplicar" => "x",
                "dividir" => "÷",
                _ => "?"
            };

            string linea = $"{DateTime.Now:HH:mm:ss} -> {n1} {simbolo} {n2} = {resultado}";
            _historial.Insert(0,linea);

            if(_historial.Count > 5)
            {
                _historial.RemoveAt(_historial.Count - 1);
            }

  

        }

        private void CargarHistorial()
        {
          ViewBag.Historial = _historial.ToList();

        }

        // GET: /Calculadora/Index
        [HttpGet]
            public IActionResult Index()
            {
                // Modelo inicial vacío
                return View(new Calculadora());
            }

            // POST: /Calculadora/Calcular
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Calcular(string operacion, double n1, double n2)
            {
                var modelo = new Calculadora { Numero1 = n1, Numero2 = n2 };
                double resultado;

                switch (operacion)
                {
                    case "sumar": resultado = modelo.Sumar(); break;
                    case "restar": resultado = modelo.Restar(); break;
                    case "multiplicar": resultado = modelo.Multiplicar(); break;
                    case "dividir":
                        resultado = modelo.Dividir();
                        if (double.IsNaN(resultado))
                        {
                            ModelState.AddModelError(string.Empty, "No se puede dividir entre cero.");
                            ViewBag.Resultado = null;
                            return View("Index", modelo);
                        }
                        break;
                    default:
                        ModelState.AddModelError(string.Empty, "Operación no válida.");
                        ViewBag.Resultado = null;
                        return View("Index", modelo);
                }

     
              AgregarAlHistorial(operacion, n1, n2,resultado);
              CargarHistorial();


            ViewBag.Resultado = resultado;
                ViewBag.Operacion = operacion;
                return View("Index", modelo);
            }
        }
  



}
