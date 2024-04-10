﻿namespace APICatalogo.Logging
{
    public class CustomLoggerProviderConfiguration
    {
        // Define o nível mínimo de log a ser registrado
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        
        //Define o ID do evento de log
        public int EventId { get; set; } = 0;
    }
}
