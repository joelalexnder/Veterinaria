@page "/login"
@page "/"
@inject VetClinic.Web.Services.ApiClient Api
@inject VetClinic.Web.Services.AuthStateService AuthState
@inject NavigationManager Nav
    @rendermode InteractiveServer
    @using VetClinic.Web.Models

    <PageTitle>Login - VetClinic</PageTitle>

    <div class="d-flex justify-content-center align-items-center" style="height:80vh;">
    <div class="card p-4" style="width:350px;">
    <h3 class="mb-3">🐾 VetClinic Pro</h3>

    <EditForm Model="model" OnValidSubmit="HandleLogin">
    <div class="mb-2">
    <label>Email</label>
    <InputText class="form-control" @bind-Value="model.Email" />
    </div>
    <div class="mb-3">
    <label>Contraseña</label>
    <InputText type="password" class="form-control" @bind-Value="model.Password" />
    </div>

    @if (!string.IsNullOrEmpty(error))
{
    <div class="alert alert-danger py-2">@error</div>
}

<button type="submit" class="btn btn-primary w-100" disabled="@loading">
    @(loading ? "Ingresando..." : "Ingresar")
    </button>
    </EditForm>
    </div>
    </div>

    @code {
private LoginRequest model = new();
private string? error;
private bool loading;

private async Task HandleLogin()
{
    loading = true;
    error = null;

    var result = await Api.LoginAsync(model);

    if (result is null)
    {
        error = "Credenciales inválidas";
        loading = false;
        return;
    }

    AuthState.SetSession(result.Token, result.FullName, result.Role);
    Nav.NavigateTo("/");
    loading = false;
}
}