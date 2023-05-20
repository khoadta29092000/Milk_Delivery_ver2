

namespace DataAccess.DTO.Station

{
    public partial class PutStationDTO
    {
        public string Id { get; set; } = null!;
        public string StationAddress { get; set; } = null!;
        public string StationName { get; set; } = null!;
        public string StationDescription { get; set; } = null!;

    }
}
