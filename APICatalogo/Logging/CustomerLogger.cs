namespace APICatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;

        readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public bool IsEnabled(LogLevel level)
        {
            return level == LogLevel.Information;
        }

        // Implementar log personalizado
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        // Método chamado para registrar a mensagem de log
        public void Log<TState>(LogLevel level, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string messagem = $"{level.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverTextoNoArquivo(messagem);
        }

        public void EscreverTextoNoArquivo(string messagem)
        {
            string caminhoArquivoLog = @"C:\log\ApiCatalogo_log.txt";

            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(messagem);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
