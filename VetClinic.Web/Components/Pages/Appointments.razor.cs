@page "/appointments"
@inject VetClinic.Web.Services.ApiClient Api
@rendermode InteractiveServer
@using VetClinic.Web.Models

<h3>📅 Citas</h3>

<div class="row">
    <div class="col-md-5">
        <h5>Agendar Cita</h5>
        <EditForm Model="model" OnValidSubmit="Schedule">
            <select class="form-select mb-2" @bind="selectedOwnerId">
                <option value="0">-- Propietario --</option>
                @foreach (var o in owners)
                {
                    <option value="@o.Id">@o.FullName</option>
                }
            </select>

            <select class="form-select mb-2" @bind="model.PetId">
                <option value="0">-- Mascota --</option>
                @foreach (var p in pets)
                {
                    <option value="@p.Id">@p.Name</option>
                }
            </select>

            <select class="form-select mb-2" @bind="model.ServiceAreaId">
                <option value="0">-- Área de servicio --</option>
                @foreach (var a in areas)
                {
                    <option value="@a.Id">@a.Name</option>
                }
            </select>

            <select class="form-select mb-2" @bind="model.SpecialistId">
                <option value="0">-- Especialista --</option>
                @foreach (var s in specialists)
                {
                    <option value="@s.Id">@s.FullName (@s.SpecialtyName)</option>
                }
            </select>

            <input type="date" class="form-control mb-2" @bind="dateStr" />
            <div class="d-flex gap-2 mb-2">
                <input type="time" class="form-control" @bind="startStr" />
                <input type="time" class="form-control" @bind="endStr" />
            </div>

            <button class="btn btn-primary w-100">Agendar</button>
        </EditForm>

        @if (msg != null) { <div class="alert alert-info mt-2">@msg</div> }
    </div>

    <div class="col-md-7">
        <h5>Citas del día</h5>
        <input type="date" class="form-control mb-2" style="width:200px" @bind="filterDateStr" @bind:after="LoadAppointments" />
        <table class="table table-sm table-striped">
            <thead><tr><th>Mascota</th><th>Especialista</th><th>Área</th><th>Hora</th><th>Estado</th></tr></thead>
            <tbody>
                @foreach (var a in appointments)
                {
                    <tr>
                        <td>@a.PetName</td>
                        <td>@a.SpecialistName</td>
                        <td>@a.ServiceAreaName</td>
                        <td>@a.StartTime - @a.EndTime</td>
                        <td><span class="badge bg-warning">@a.Status</span></td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
</div>

@code {
    private ScheduleAppointmentRequest model = new();
    private List<OwnerDto> owners = new();
    private List<PetDto> pets = new();
    private List<ServiceAreaDto> areas = new();
    private List<SpecialistDto> specialists = new();
    private List<AppointmentDto> appointments = new();

    private int selectedOwnerId;
    private string dateStr = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
    private string startStr = "10:00";
    private string endStr = "10:30";
    private string filterDateStr = DateTime.Today.ToString("yyyy-MM-dd");
    private string? msg;

    protected override async Task OnInitializedAsync()
    {
        owners = await Api.GetOwnersAsync();
        areas = await Api.GetServiceAreasAsync();
        specialists = await Api.GetSpecialistsAsync();
        await LoadAppointments();
    }

    private async Task LoadAppointments()
    {
        var date = DateOnly.Parse(filterDateStr);
        appointments = await Api.GetAppointmentsByDateAsync(date);
    }

    private async Task Schedule()
    {
        if (model.PetId == 0 || model.SpecialistId == 0 || model.ServiceAreaId == 0)
        {
            msg = "❌ Completa todos los campos";
            return;
        }

        model.AppointmentDate = DateOnly.Parse(dateStr);
        model.StartTime = TimeOnly.Parse(startStr);
        model.EndTime = TimeOnly.Parse(endStr);

        var (success, error) = await Api.ScheduleAppointmentAsync(model);
        msg = success ? "✅ Cita agendada" : $"❌ {error}";

        if (success)
        {
            model = new();
            await LoadAppointments();
        }
    }
}