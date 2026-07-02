@page "/vaccination"
@inject VetClinic.Web.Services.ApiClient Api
@rendermode InteractiveServer
@using VetClinic.Web.Models

<h3>💉 Vacunación</h3>

<div class="row">
    <div class="col-md-5">
        <h5>Agregar vacuna al catálogo</h5>
        <EditForm Model="vaccineModel" OnValidSubmit="AddVaccine">
            <InputText class="form-control mb-2" placeholder="Nombre" @bind-Value="vaccineModel.Name" />
            <InputText class="form-control mb-2" placeholder="Especie" @bind-Value="vaccineModel.Species" />
            <InputNumber class="form-control mb-2" placeholder="Edad mínima (días)" @bind-Value="vaccineModel.MinAgeDays" />
            <InputNumber class="form-control mb-2" placeholder="Intervalo refuerzo (días)" @bind-Value="vaccineModel.BoosterIntervalDays" />
            <button class="btn btn-primary w-100">Agregar</button>
        </EditForm>
        @if (vaccineMsg != null) { <div class="alert alert-info mt-2">@vaccineMsg</div> }

        <hr />

        <h5>Aplicar vacuna</h5>
        <EditForm Model="recordModel" OnValidSubmit="RegisterVaccination">
            <InputNumber class="form-control mb-2" placeholder="Id Mascota" @bind-Value="recordModel.PetId" />

            <select class="form-select mb-2" @bind="recordModel.VaccineId">
                <option value="0">-- Vacuna --</option>
                @foreach (var v in catalog)
                {
                    <option value="@v.Id">@v.Name (@v.Species)</option>
                }
            </select>

            <InputNumber class="form-control mb-2" placeholder="Id Veterinario" @bind-Value="recordModel.VeterinarianId" />
            <InputText class="form-control mb-2" placeholder="Lote" @bind-Value="recordModel.BatchNumber" />
            <label class="form-label small">Fecha de aplicación</label>
            <input type="date" class="form-control mb-2" @bind="applicationDateStr" />
            <label class="form-label small">Próximo refuerzo</label>
            <input type="date" class="form-control mb-2" @bind="boosterDateStr" />
            <button class="btn btn-success w-100">Aplicar vacuna</button>
        </EditForm>
        @if (recordMsg != null) { <div class="alert alert-info mt-2">@recordMsg</div> }
    </div>

    <div class="col-md-7">
        <h5>Vacunaciones vencidas</h5>
        <button class="btn btn-sm btn-outline-secondary mb-2" @onclick="LoadOverdue">🔄 Actualizar</button>
        <table class="table table-sm table-striped">
            <thead><tr><th>Mascota</th><th>Vacuna</th><th>Próximo refuerzo</th><th>Días vencida</th></tr></thead>
            <tbody>
                @foreach (var r in overdue)
                {
                    <tr class="@(r.DaysOverdue > 30 ? "table-danger" : "table-warning")">
                        <td>@r.PetName</td>
                        <td>@r.VaccineName</td>
                        <td>@r.NextBoosterDate</td>
                        <td>@r.DaysOverdue</td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
</div>

@code {
    private AddVaccineCatalogRequest vaccineModel = new();
    private RegisterVaccinationRequest recordModel = new();
    private List<VaccineDto> catalog = new();
    private List<VaccinationRecordDto> overdue = new();
    private string applicationDateStr = DateTime.Today.ToString("yyyy-MM-dd");
    private string boosterDateStr = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");
    private string? vaccineMsg;
    private string? recordMsg;

    protected override async Task OnInitializedAsync()
    {
        catalog = await Api.GetVaccineCatalogAsync();
        await LoadOverdue();
    }

    private async Task LoadOverdue() => overdue = await Api.GetOverdueVaccinationsAsync();

    private async Task AddVaccine()
    {
        var success = await Api.AddVaccineCatalogAsync(vaccineModel);
        vaccineMsg = success ? "✅ Vacuna agregada" : "❌ Error al agregar";
        if (success)
        {
            vaccineModel = new();
            catalog = await Api.GetVaccineCatalogAsync();
        }
    }

    private async Task RegisterVaccination()
    {
        recordModel.ApplicationDate = DateOnly.Parse(applicationDateStr);
        recordModel.NextBoosterDate = DateOnly.Parse(boosterDateStr);

        var (success, error) = await Api.RegisterVaccinationAsync(recordModel);
        recordMsg = success ? "✅ Vacunación registrada" : $"❌ {error}";
        if (success)
        {
            recordModel = new();
            await LoadOverdue();
        }
    }
}