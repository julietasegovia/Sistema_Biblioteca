using System;

namespace Biblioteca_App
{
    public class Program
    {
        static void Main()
        {
            using var biblioteca = new Biblioteca();
            biblioteca.actualizarPrestamosVencidos();

            biblioteca.mostrarLibros();

            // --- Préstamo exitoso ---
            var socioMartin = biblioteca.buscarSocio(1002)!;
            var sapiens = biblioteca.buscarLibros("sapiens")[0];
            biblioteca.prestar(socioMartin, sapiens); // debe registrarse

            // --- Préstamo rechazado: socio inactivo (RN-01) ---
            var socioFederico = biblioteca.buscarSocio(1006)!;
            var cosmos = biblioteca.buscarLibros("cosmos")[0];
            biblioteca.prestar(socioFederico, cosmos); // debe rechazarse

            // --- Préstamo rechazado: sin copias disponibles -> se ofrece reservar (RN-03) ---
            var socioDiego = biblioteca.buscarSocio(1004)!;
            var rayuela = biblioteca.buscarLibros("rayuela")[0];
            biblioteca.prestar(socioDiego, rayuela);   // debe rechazarse (sin copias)
            biblioteca.reservar(socioDiego, rayuela);  // debe registrarse la reserva

            // --- Reserva duplicada (RN-08) ---
            biblioteca.reservar(socioDiego, rayuela); // debe rechazarse, ya tiene una reserva activa

            // --- Devolución con demora -> genera multa (RN-06) y cumple la reserva de Diego (RN-07) ---
            var socioCarolina = biblioteca.buscarSocio(1003)!;
            var prestamoRayuela = biblioteca.prestamosActivosDeSocio(socioCarolina.NroSocio)[0];
            biblioteca.devolver(prestamoRayuela); // debe generar multa y avisar la reserva cumplida

            // --- Detalle de socio ---
            var socioLucia = biblioteca.buscarSocio(1001)!;
            biblioteca.mostrarDetalleSocio(socioLucia);

            // --- Reportes ---
            biblioteca.reporteLibrosMasPrestados();
            biblioteca.reporteSociosConMultas();
            biblioteca.reportePrestamosVencidos();
            biblioteca.reporteDisponibilidad(rayuela);
            biblioteca.reporteHistorialSocio(socioCarolina);
            Console.WriteLine("\nPresione una tecla para salir...");
            Console.ReadKey();
        }
    }
}