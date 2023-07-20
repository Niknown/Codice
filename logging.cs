using System.Reflection;
using log4net;
using log4net.Config;

namespace Gioco
{
    /// <summary>
    /// classe per il logging.
    /// </summary>
    public static class Logger
    {
        private static readonly string logFilePath = "log.txt";
        /// <summary>
        /// Metodo per stampare il messaggio di logging in un file txt.
        /// </summary>
        /// <param name="message"></param>
        public static void Log(string message)
        {
            var logMessage = $"{DateTime.Now} - {message}";
    
            // Aggiungi il log al file
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine(logMessage);
            }
        }
    }

}