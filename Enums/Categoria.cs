using System.ComponentModel;

namespace Workshop.Enums
{
    public enum Categoria
    {
        [Description("Qualidade")]
        Qualidade,
        [Description("DevOps")]
        DevOps,
        [Description("Desenvolvimento")]
        Desenvolvimento,
        [Description("Segurança")]
        Seguranca,
        [Description("Governança de TI")]
        GovernancaDeTI
    }
}
