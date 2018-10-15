using System;

public class Paciente
{
    public Guid PacienteId { get; set; }
    public DateTime Timestamp { get; set; }
    public int PressaoSistolica { get; set; }
    public int PressaoDiastolica { get; set; }
    public string PressaoResumida { get; set; }
}