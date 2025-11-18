using CalculadoraMvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace CalculadoraMvc.Controllers
{
  
    public class CalculadoraController : Controller
        {



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

     


            ViewBag.Resultado = resultado;
                ViewBag.Operacion = operacion;
                return View("Index", modelo);
            }
        }
  



}
