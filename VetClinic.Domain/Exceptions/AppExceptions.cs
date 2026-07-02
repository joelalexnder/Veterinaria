namespace VetClinic.Domain.Exceptions;

/// <summary>Recurso no encontrado → HTTP 404</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>Conflicto de estado/regla de negocio → HTTP 409</summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}

/// <summary>Credenciales inválidas o token roto → HTTP 401</summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}

/// <summary>Autenticado pero sin permiso / cuenta inactiva → HTTP 403</summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}