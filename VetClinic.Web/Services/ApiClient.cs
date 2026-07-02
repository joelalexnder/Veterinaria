using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using VetClinic.Web.Models;

namespace VetClinic.Web.Services;

public class ApiClient
{
    private readonly HttpClient _http;
    private readonly AuthStateService _authState;

    public ApiClient(HttpClient http, AuthStateService authState)
    {
        _http = http;
        _authState = authState;
    }

    private void ApplyAuthHeader()
    {
        _http.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(_authState.Token)
            ? null
            : new AuthenticationHeaderValue("Bearer", _authState.Token);
    }

    // Parsea tanto el formato ProblemDetails de ASP.NET (errores de validación como
    // [Required]/[RegularExpression]) como el formato { status, message, details } de
    // tu ErrorHandlingMiddleware, y devuelve siempre un mensaje legible.
    private static async Task<string> ExtractErrorMessage(HttpResponseMessage response)
    {
        var raw = await response.Content.ReadAsStringAsync();
        try
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out var errors))
            {
                var messages = new List<string>();
                foreach (var field in errors.EnumerateObject())
                    foreach (var msg in field.Value.EnumerateArray())
                        messages.Add(msg.GetString() ?? "");
                if (messages.Count > 0) return string.Join(" ", messages);
            }

            if (root.TryGetProperty("details", out var details) && details.GetString() is string d && !string.IsNullOrWhiteSpace(d))
                return d;

            if (root.TryGetProperty("message", out var message) && message.GetString() is string m && !string.IsNullOrWhiteSpace(m))
                return m;
        }
        catch { /* no era JSON válido, devolvemos el texto crudo */ }

        return string.IsNullOrWhiteSpace(raw) ? "Ocurrió un error inesperado" : raw;
    }

    // ── Auth ──────────────────────────────────
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/login", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        ApplyAuthHeader();
        var all = new List<UserDto>();
        for (int roleId = 1; roleId <= 4; roleId++)
        {
            var users = await _http.GetFromJsonAsync<List<UserDto>>($"api/Auth/users?roleId={roleId}") ?? new();
            all.AddRange(users);
        }
        return all;
    }

    public async Task<(bool Success, string Error)> DeleteUserAsync(int userId)
    {
        ApplyAuthHeader();
        var response = await _http.DeleteAsync($"api/Auth/users/{userId}");
        if (response.IsSuccessStatusCode) return (true, "");
        var error = await ExtractErrorMessage(response);
        return (false, error);
    }

    public async Task<bool> AssignRoleAsync(AssignRoleRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PutAsJsonAsync("api/Auth/update-role", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<(bool Success, int Id, string Error)> RegisterUserAsync(RegisterUserRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Auth/register", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await ExtractErrorMessage(response);
            return (false, 0, error);
        }
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var id = json != null && json.TryGetValue("id", out var idVal) ? Convert.ToInt32(idVal.ToString()) : 0;
        return (true, id, "");
    }

    public async Task<bool> RegisterSpecialistAsync(RegisterSpecialistRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Specialist", request);
        return response.IsSuccessStatusCode;
    }

    // ── Owner ─────────────────────────────────
    public async Task<List<OwnerDto>> GetOwnersAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<OwnerDto>>("api/Owner") ?? new();
    }

    public async Task<(bool Success, string Error)> RegisterOwnerAsync(RegisterOwnerRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Owner", request);
        if (response.IsSuccessStatusCode) return (true, "");
        var error = await ExtractErrorMessage(response);
        return (false, error);
    }

    public async Task<(bool Success, string Error)> DeleteOwnerAsync(int id)
    {
        ApplyAuthHeader();
        var response = await _http.DeleteAsync($"api/Owner/{id}");
        if (response.IsSuccessStatusCode) return (true, "");
        var error = await ExtractErrorMessage(response);
        return (false, error);
    }

    // ── Pet ───────────────────────────────────
    public async Task<List<PetDto>> GetPetsByOwnerAsync(int ownerId)
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<PetDto>>($"api/Pet/owner/{ownerId}") ?? new();
    }

    public async Task<(bool Success, string Error)> RegisterPetAsync(RegisterPetRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Pet", request);
        if (response.IsSuccessStatusCode) return (true, "");
        var error = await ExtractErrorMessage(response);
        return (false, error);
    }

    // ── Pet search ─────────────────────────────
    public async Task<List<PetDto>> SearchPetsAsync(string? name = null, string? species = null, int? ownerId = null)
    {
        ApplyAuthHeader();

        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(name)) query.Add($"name={Uri.EscapeDataString(name)}");
        if (!string.IsNullOrWhiteSpace(species)) query.Add($"species={Uri.EscapeDataString(species)}");
        if (ownerId.HasValue) query.Add($"ownerId={ownerId}");

        var url = "api/Pet/search" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
        return await _http.GetFromJsonAsync<List<PetDto>>(url) ?? new();
    }

    // ── ServiceArea / Specialist ──────────────
    public async Task<List<ServiceAreaDto>> GetServiceAreasAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<ServiceAreaDto>>("api/ServiceArea") ?? new();
    }

    public async Task<bool> RegisterServiceAreaAsync(RegisterServiceAreaRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/ServiceArea", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<SpecialistDto>> GetSpecialistsAsync(int? serviceAreaId = null)
    {
        ApplyAuthHeader();
        var url = serviceAreaId.HasValue ? $"api/Specialist?serviceAreaId={serviceAreaId}" : "api/Specialist";
        return await _http.GetFromJsonAsync<List<SpecialistDto>>(url) ?? new();
    }

    // ── Appointment ───────────────────────────
    public async Task<List<AppointmentDto>> GetAppointmentsByDateAsync(DateOnly date)
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<AppointmentDto>>($"api/Appointment/by-date?date={date:yyyy-MM-dd}") ?? new();
    }

    public async Task<(bool Success, string Error)> ScheduleAppointmentAsync(ScheduleAppointmentRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Appointment", request);
        if (response.IsSuccessStatusCode) return (true, "");
        var error = await ExtractErrorMessage(response);
        return (false, error);
    }

    // ── Vaccination ───────────────────────────
    public async Task<List<VaccineDto>> GetVaccineCatalogAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<VaccineDto>>("api/Vaccination/catalog") ?? new();
    }

    public async Task<bool> AddVaccineCatalogAsync(AddVaccineCatalogRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Vaccination/catalog", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<(bool Success, string Error)> RegisterVaccinationAsync(RegisterVaccinationRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Vaccination", request);
        if (response.IsSuccessStatusCode) return (true, "");
        var error = await ExtractErrorMessage(response);
        return (false, error);
    }

    public async Task<List<VaccinationRecordDto>> GetOverdueVaccinationsAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<VaccinationRecordDto>>("api/Vaccination/overdue") ?? new();
    }

    public async Task<List<VaccinationRecordDto>> GetAllVaccinationsAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<VaccinationRecordDto>>("api/Vaccination/all") ?? new();
    }

    // ── Reports ───────────────────────────────
    public async Task<byte[]?> GetVaccinationsReportAsync()
    {
        ApplyAuthHeader();
        var response = await _http.GetAsync("api/Report/vaccinations");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<byte[]?> GetAppointmentsReportAsync(DateOnly? date = null)
    {
        ApplyAuthHeader();
        var url = date.HasValue
            ? $"api/Report/appointments?date={date:yyyy-MM-dd}"
            : "api/Report/appointments";
        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadAsByteArrayAsync();
    }

    // ── AI ────────────────────────────────────
    public async Task<string?> GetPetHealthSummaryAsync(int petId)
    {
        ApplyAuthHeader();
        var response = await _http.GetAsync($"api/Pet/{petId}/health-summary-ai");
        if (!response.IsSuccessStatusCode) return null;
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return json != null && json.TryGetValue("summary", out var summary) ? summary : null;
    }
}