using System;
using System.Collections.Generic;

namespace ConectionWithS71200.Contexto;

public partial class Lectura
{
    public int Id { get; set; }

    public string? Out1 { get; set; }

    public string? Out2 { get; set; }

    public string? Out3 { get; set; }

    public string? Ia1 { get; set; }

    public string? Fecha { get; set; }
}
