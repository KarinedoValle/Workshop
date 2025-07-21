using System.ComponentModel;

namespace Workshop.Enums
{
    public enum Perfil
    {
        [Description("Administrador")]
        Administrador,
        [Description("Instrutor")]
        Instrutor,
        [Description("Leitor")]
        Leitor
    }
}
