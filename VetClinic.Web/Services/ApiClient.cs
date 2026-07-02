using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    // ── Auth ──────────────────────────────────
    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/Auth/login", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }
   
    
    public async Task<(bool Success, int Id, string Error)> RegisterUserAsync(RegisterUserRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Auth/register", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
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

    public async Task<bool> RegisterOwnerAsync(RegisterOwnerRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Owner", request);
        return response.IsSuccessStatusCode;
    }

    // ── Pet ───────────────────────────────────
    public async Task<List<PetDto>> GetPetsByOwnerAsync(int ownerId)
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<PetDto>>($"api/Pet/owner/{ownerId}") ?? new();
    }

    public async Task<bool> RegisterPetAsync(RegisterPetRequest request)
    {
        ApplyAuthHeader();
        var response = await _http.PostAsJsonAsync("api/Pet", request);
        return response.IsSuccessStatusCode;
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
        var error = await response.Content.ReadAsStringAsync();
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
        var error = await response.Content.ReadAsStringAsync();
        return (false, error);
    }

    public async Task<List<VaccinationRecordDto>> GetOverdueVaccinationsAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<VaccinationRecordDto>>("api/Vaccination/overdue") ?? new();
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
    public async Task<List<VaccinationRecordDto>> GetAllVaccinationsAsync()
    {
        ApplyAuthHeader();
        return await _http.GetFromJsonAsync<List<VaccinationRecordDto>>("api/Vaccination/all") ?? new();
    }
}