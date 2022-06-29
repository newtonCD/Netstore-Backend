using System;
using System.Diagnostics.CodeAnalysis;

namespace Template.Application.Common.Security;

/// <summary>
/// Especifica a classe à qual este atributo é aplicado requer autorização.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="AuthorizeAttribute"/>.
    /// </summary>
    public AuthorizeAttribute()
    {
    }

    /// <summary>
    /// Obtém ou define uma lista delimitada por vírgulas de roles que têm permissão para acessar o recurso.
    /// </summary>
    public string Roles { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o nome da política que determina o acesso ao recurso.
    /// </summary>
    public string Policy { get; set; } = string.Empty;
}