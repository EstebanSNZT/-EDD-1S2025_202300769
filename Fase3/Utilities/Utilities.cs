using System.Diagnostics;

namespace Utilities
{
    public static class Utility
    {
        public static void GenerateDotFile(string name, string content)
        {
            try
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
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

                string rutaArchivo = Path.Combine(folder, name);
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
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
                string dotFile = Path.Combine(folder, reportName);

                if (string.IsNullOrEmpty(dotFile))
                {
                    Console.WriteLine("El archivo no tiene una ruta válida.");
                    return;
                }

                if (!File.Exists(dotFile))
                {
                    Console.WriteLine($"El archivo {reportName} no existe en la carpeta 'Reportes'.");
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

                Process proceso = Process.Start(processStartInfo);

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

        public static void GenerateJsonFile(string fileName, string jsonContent, string folderName)
        {
            try
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                
                fileName += ".json";
                string filePath = Path.Combine(folder, fileName);

                File.WriteAllText(filePath, jsonContent);

                Console.WriteLine($"Archivo {fileName} creado con éxito en: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el archivo JSON: {ex.Message}");
            }
        }

        public static string LoadJsonFile(string fileName, string folderName)
        {
            try
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string filePath = Path.Combine(folder, $"{fileName}.json");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"El archivo {fileName}.json no existe en la carpeta {folderName}.");
                    return string.Empty;
                }

                string jsonContent = File.ReadAllText(filePath);
                return jsonContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el archivo JSON: {ex.Message}");
                return string.Empty;
            }
        }
    }
}