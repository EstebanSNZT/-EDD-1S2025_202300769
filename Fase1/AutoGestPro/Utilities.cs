using System.Diagnostics;

namespace Utilities
{
    public static class Utility
    {
        public static void GenerateDotFile(string name, string content)
        {
            try
            {
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "reports");
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("El nombre del archivo no puede ser nulo o vacío.");
                    return;
                }

                if (!name.EndsWith(".dot"))
                {
                    name += ".dot";
                }

                string rutaArchivo = Path.Combine(carpeta, name);
                File.WriteAllText(rutaArchivo, content);

                Console.WriteLine($"Archivo generado con éxito en: {rutaArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el archivo: {ex.Message}");
            }
        }

        public static void ConvertDotToImage(string reportName)
        {
            try
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "reports");
                string dotFile = Path.Combine(folder, reportName);

                if (string.IsNullOrEmpty(dotFile))
                {
                    Console.WriteLine("El archivo no tiene una ruta válida.");
                    return;
                }

                if (!File.Exists(dotFile))
                {
                    Console.WriteLine($"El archivo {reportName} no existe en la carpeta 'reports'.");
                    return;
                }

                string imageFile = Path.ChangeExtension(dotFile, ".png");

                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-Tpng \"{dotFile}\" -o \"{imageFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process? proceso = Process.Start(processStartInfo);

                if (proceso == null)
                {
                    Console.WriteLine("No se pudo iniciar el proceso para convertir el archivo .dot.");
                    return;
                }

                proceso.WaitForExit();

                if (proceso.ExitCode == 0)
                {
                    Console.WriteLine($"Conversión exitosa. La imagen se guardó en: {imageFile}");
                }
                else
                {
                    Console.WriteLine("Hubo un error al intentar convertir el archivo .dot a imagen.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir el archivo .dot a imagen: {ex.Message}");
            }
        }
    }
}