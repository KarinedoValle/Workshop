using System.ComponentModel;

namespace Workshop.Enums
{
    public enum Status
    {
        [Description("Novo")]
        Novo,
        [Description("Agendado")]
        Agendado,
        [Description("Em Andamento")]
        EmAndamento,
        [Description("Concluído")]
        Concluido,
        [Description("Cancelado")]
        Cancelado
    }
}
