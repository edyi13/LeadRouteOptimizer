using LeadRouteOptimizer.Application.Interfaces;
using LeadRouteOptimizer.Domain.Entities;
using System.Text.Json;

namespace LeadRouteOptimizer.Application.Uploads.Commands
{
    public class UploadLeadsHandler(
    ILeadCsvParser parser,
    IUploadBatchRepository uploadBatchRepo,
    ILeadRepository leadRepo,
    IUnitOfWork uow)
    {
        public async Task<UploadLeadsResult> HandleAsync(UploadLeadsCommand cmd, CancellationToken ct)
        {
            if (cmd.SourceType is not ("Manager" or "Personal"))
                throw new ArgumentException("SourceType must be 'Manager' or 'Personal'.");

            var parse = await parser.ParseAsync(cmd.CsvStream, ct);

            var batch = new UploadBatch
            {
                SourceType = cmd.SourceType,
                OriginalFileName = cmd.OriginalFileName,
                CreatedAtUtc = DateTime.UtcNow
            };

            await uploadBatchRepo.AddAsync(batch, ct);

            var leads = new List<Lead>(parse.Rows.Count);
            foreach (var r in parse.Rows)
            {
                var (status, err) = ValidateRow(r);

                var lead = new Lead
                {
                    UploadBatch = batch,
                    LeadName = (r.LeadName ?? string.Empty).Trim().Length == 0 ? "(missing)" : r.LeadName!.Trim(),
                    Street = r.Street?.Trim(),
                    City = r.City?.Trim(),
                    State = r.State?.Trim(),
                    Zip = r.Zip?.Trim(),
                    Latitude = r.Latitude ?? 0m,
                    Longitude = r.Longitude ?? 0m,
                    Status = status,
                    ErrorMessage = err,
                    RawRowJson = JsonSerializer.Serialize(new { r.RowNumber, r.Raw }),
                    CreatedAtUtc = DateTime.UtcNow
                };

                leads.Add(lead);
            }

            await leadRepo.AddRangeAsync(leads, ct);
            await uow.SaveChangesAsync(ct);

            var valid = leads.Count(x => x.Status == "Valid");
            var invalid = leads.Count - valid;

            return new UploadLeadsResult(batch.Id, leads.Count, valid, invalid);
        }

        private static (string Status, string? Error) ValidateRow(ParsedLeadRow r)
        {
            if (string.IsNullOrWhiteSpace(r.LeadName))
                return ("Invalid", "LeadName is required.");

            if (r.Latitude is null || r.Longitude is null)
                return ("Invalid", "Latitude and Longitude are required.");

            if (r.Latitude < -90m || r.Latitude > 90m)
                return ("Invalid", "Latitude must be between -90 and 90.");

            if (r.Longitude < -180m || r.Longitude > 180m)
                return ("Invalid", "Longitude must be between -180 and 180.");

            return ("Valid", null);
        }
    }
}
