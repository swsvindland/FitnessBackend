namespace FitnessServices.Models;

public sealed class Dashboard
{
    public bool AddWeight { get; set; }
    public bool WeightAdded { get; set; }
    public bool AddBodyMeasurements { get; set; }
    public bool BodyMeasurementsAdded { get; set; }
    public bool AddHeight { get; set; }
    public bool HeightAdded { get; set; }
    public bool AddBloodPressure { get; set; }
    public bool BloodPressureAdded { get; set; }
    public bool DoWorkout { get; set; }
    public bool WorkoutAdded { get; set; }
    public bool TrackMacros { get; set; }
    public bool MacrosAdded { get; set; }
    public bool AddSupplements { get; set; }
    public bool SupplementsAdded { get; set; }
    public bool TrackSupplements { get; set; }
    public bool SupplementsTracked { get; set; }
}